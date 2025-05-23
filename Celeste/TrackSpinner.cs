// Decompiled with JetBrains decompiler
// Type: Celeste.TrackSpinner
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    public class TrackSpinner : Entity
    {
      public static readonly float[] PauseTimes = new float[3]
      {
        0.3f,
        0.2f,
        0.6f
      };
      public static readonly float[] MoveTimes = new float[3]
      {
        0.9f,
        0.4f,
        0.3f
      };
      public bool Up = true;
      public float PauseTimer;
      public TrackSpinner.Speeds Speed;
      public bool Moving = true;
      public float Angle;

      public Vector2 Start { get; private set; }

      public Vector2 End { get; private set; }

      public float Percent { get; private set; }

      public TrackSpinner(EntityData data, Vector2 offset)
      {
        this.Collider = (Collider) new ColliderList(new Collider[2]
        {
          (Collider) new Monocle.Circle(6f),
          (Collider) new Hitbox(16f, 4f, -8f, -3f)
        });
        this.Add((Component) new PlayerCollider(new Action<Player>(this.OnPlayer)));
        this.Start = data.Position + offset;
        this.End = data.Nodes[0] + offset;
        this.Speed = data.Enum<TrackSpinner.Speeds>("speed", TrackSpinner.Speeds.Normal);
        this.Angle = (this.Start - this.End).Angle();
        this.Percent = data.Bool("startCenter") ? 0.5f : 0.0f;
        if ((double) this.Percent == 1.0)
          this.Up = false;
        this.UpdatePosition();
      }

      public void UpdatePosition()
      {
        this.Position = Vector2.Lerp(this.Start, this.End, Ease.SineInOut(this.Percent));
      }

      public override void Awake(Scene scene)
      {
        base.Awake(scene);
        this.OnTrackStart();
      }

      public override void Update()
      {
        base.Update();
        if (!this.Moving)
          return;
        if ((double) this.PauseTimer > 0.0)
        {
          this.PauseTimer -= Engine.DeltaTime;
          if ((double) this.PauseTimer > 0.0)
            return;
          this.OnTrackStart();
        }
        else
        {
          this.Percent = Calc.Approach(this.Percent, this.Up ? 1f : 0.0f, Engine.DeltaTime / TrackSpinner.MoveTimes[(int) this.Speed]);
          this.UpdatePosition();
          if ((!this.Up || (double) this.Percent != 1.0) && (this.Up || (double) this.Percent != 0.0))
            return;
          this.Up = !this.Up;
          this.PauseTimer = TrackSpinner.PauseTimes[(int) this.Speed];
          this.OnTrackEnd();
        }
      }

      public virtual void OnPlayer(Player player)
      {
        if (player.Die((player.Position - this.Position).SafeNormalize()) == null)
          return;
        this.Moving = false;
      }

      public virtual void OnTrackStart()
      {
      }

      public virtual void OnTrackEnd()
      {
      }

      public enum Speeds
      {
        Slow,
        Normal,
        Fast,
      }
    }
}
