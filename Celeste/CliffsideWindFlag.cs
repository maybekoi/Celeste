// Decompiled with JetBrains decompiler
// Type: Celeste.CliffsideWindFlag
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    public class CliffsideWindFlag : Entity
    {
      private CliffsideWindFlag.Segment[] segments;
      private float sine;
      private float random;
      private int sign;

      public CliffsideWindFlag(EntityData data, Vector2 offset)
        : base(data.Position + offset)
      {
        MTexture atlasSubtexturesAt = GFX.Game.GetAtlasSubtexturesAt("scenery/cliffside/flag", data.Int("index"));
        this.segments = new CliffsideWindFlag.Segment[atlasSubtexturesAt.Width];
        for (int x = 0; x < this.segments.Length; ++x)
          this.segments[x] = new CliffsideWindFlag.Segment()
          {
            Texture = atlasSubtexturesAt.GetSubtexture(x, 0, 1, atlasSubtexturesAt.Height),
            Offset = new Vector2((float) x, 0.0f)
          };
        this.sine = Calc.Random.NextFloat(6.28318548f);
        this.random = Calc.Random.NextFloat();
        this.Depth = 8999;
        this.Tag = (int) Tags.TransitionUpdate;
      }

      private float wind => Calc.ClampedMap(Math.Abs((this.Scene as Level).Wind.X), 0.0f, 800f);

      public override void Added(Scene scene)
      {
        base.Added(scene);
        this.sign = 1;
        if ((double) this.wind != 0.0)
          this.sign = Math.Sign((this.Scene as Level).Wind.X);
        for (int i = 0; i < this.segments.Length; ++i)
          this.SetFlagSegmentPosition(i, true);
      }

      public override void Update()
      {
        base.Update();
        if ((double) this.wind != 0.0)
          this.sign = Math.Sign((this.Scene as Level).Wind.X);
        this.sine += (float) ((double) Engine.DeltaTime * (4.0 + (double) this.wind * 4.0) * (0.800000011920929 + (double) this.random * 0.20000000298023224));
        for (int i = 0; i < this.segments.Length; ++i)
          this.SetFlagSegmentPosition(i, false);
      }

      private float Sin(float timer) => (float) Math.Sin(-(double) timer);

      private void SetFlagSegmentPosition(int i, bool snap)
      {
        CliffsideWindFlag.Segment segment = this.segments[i];
        float num = (float) ((double) (i * this.sign) * (0.20000000298023224 + (double) this.wind * 0.800000011920929 * (0.800000011920929 + (double) this.random * 0.20000000298023224)) * (0.89999997615814209 + (double) this.Sin(this.sine) * 0.10000000149011612));
        float target1 = Calc.LerpClamp((float) ((double) this.Sin((float) ((double) this.sine * 0.5 - (double) i * 0.10000000149011612)) * ((double) i / (double) this.segments.Length) * (double) i * 0.20000000298023224), num, (float) Math.Ceiling((double) this.wind));
        float target2 = (float) ((double) i / (double) this.segments.Length * (double) Math.Max(0.1f, 1f - this.wind) * 16.0);
        if (!snap)
        {
          segment.Offset.X = Calc.Approach(segment.Offset.X, target1, Engine.DeltaTime * 40f);
          segment.Offset.Y = Calc.Approach(segment.Offset.Y, target2, Engine.DeltaTime * 40f);
        }
        else
        {
          segment.Offset.X = target1;
          segment.Offset.Y = target2;
        }
      }

      public override void Render()
      {
        base.Render();
        for (int index = 0; index < this.segments.Length; ++index)
        {
          CliffsideWindFlag.Segment segment = this.segments[index];
          float num = (float) ((double) index / (double) this.segments.Length * (double) this.Sin((float) -index * 0.1f + this.sine) * 2.0);
          segment.Texture.Draw(this.Position + segment.Offset + Vector2.UnitY * num);
        }
      }

      private class Segment
      {
        public MTexture Texture;
        public Vector2 Offset;
      }
    }
}
