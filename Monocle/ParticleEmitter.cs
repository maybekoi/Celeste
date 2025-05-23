// Decompiled with JetBrains decompiler
// Type: Monocle.ParticleEmitter
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;

namespace Monocle
{

    public class ParticleEmitter : Component
    {
      public ParticleSystem System;
      public ParticleType Type;
      public Entity Track;
      public float Interval;
      public Vector2 Position;
      public Vector2 Range;
      public int Amount;
      public float? Direction;
      private float timer;

      public ParticleEmitter(
        ParticleSystem system,
        ParticleType type,
        Vector2 position,
        Vector2 range,
        int amount,
        float interval)
        : base(true, false)
      {
        this.System = system;
        this.Type = type;
        this.Position = position;
        this.Range = range;
        this.Amount = amount;
        this.Interval = interval;
      }

      public ParticleEmitter(
        ParticleSystem system,
        ParticleType type,
        Vector2 position,
        Vector2 range,
        float direction,
        int amount,
        float interval)
        : this(system, type, position, range, amount, interval)
      {
        this.Direction = new float?(direction);
      }

      public ParticleEmitter(
        ParticleSystem system,
        ParticleType type,
        Entity track,
        Vector2 position,
        Vector2 range,
        float direction,
        int amount,
        float interval)
        : this(system, type, position, range, amount, interval)
      {
        this.Direction = new float?(direction);
        this.Track = track;
      }

      public void SimulateCycle() => this.Simulate(this.Type.LifeMax);

      public void Simulate(float duration)
      {
        float num = duration / this.Interval;
        for (int index1 = 0; (double) index1 < (double) num; ++index1)
        {
          for (int index2 = 0; index2 < this.Amount; ++index2)
          {
            Particle particle = new Particle();
            Vector2 position = this.Entity.Position + this.Position + Calc.Random.Range(-this.Range, this.Range);
            if (!this.Direction.HasValue)
            {
                particle = this.Type.Create(ref particle, position);
            }
            else
            {
                particle = this.Type.Create(ref particle, position, this.Direction.Value);
            }
            particle.Track = this.Track;
            float duration1 = duration - this.Interval * (float) index1;
            if (particle.SimulateFor(duration1))
              this.System.Add(particle);
          }
        }
      }

      public void Emit()
      {
        if (this.Direction.HasValue)
          this.System.Emit(this.Type, this.Amount, this.Entity.Position + this.Position, this.Range, this.Direction.Value);
        else
          this.System.Emit(this.Type, this.Amount, this.Entity.Position + this.Position, this.Range);
      }

      public override void Update()
      {
        this.timer -= Engine.DeltaTime;
        if ((double) this.timer > 0.0)
          return;
        this.timer = this.Interval;
        this.Emit();
      }
    }
}
