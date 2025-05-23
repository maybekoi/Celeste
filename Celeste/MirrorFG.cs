// Decompiled with JetBrains decompiler
// Type: Celeste.MirrorFG
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    public class MirrorFG : Backdrop
    {
      private static readonly Color[] colors = new Color[1]
      {
        Color.Red
      };
      private MirrorFG.Particle[] particles = new MirrorFG.Particle[50];
      private float fade;

      public MirrorFG()
      {
        for (int i = 0; i < this.particles.Length; ++i)
          this.Reset(i, Calc.Random.NextFloat());
      }

      private void Reset(int i, float p)
      {
        this.particles[i].Percent = p;
        this.particles[i].Position = new Vector2((float) Calc.Random.Range(0, 320), (float) Calc.Random.Range(0, 180));
        this.particles[i].Speed = (float) Calc.Random.Range(4, 14);
        this.particles[i].Spin = Calc.Random.Range(0.25f, 18.849556f);
        this.particles[i].Duration = Calc.Random.Range(1f, 4f);
        this.particles[i].Direction = Calc.AngleToVector(Calc.Random.NextFloat(6.28318548f), 1f);
        this.particles[i].Color = Calc.Random.Next(MirrorFG.colors.Length);
      }

      public override void Update(Scene scene)
      {
        base.Update(scene);
        for (int i = 0; i < this.particles.Length; ++i)
        {
          if ((double) this.particles[i].Percent >= 1.0)
            this.Reset(i, 0.0f);
          this.particles[i].Percent += Engine.DeltaTime / this.particles[i].Duration;
          this.particles[i].Position += this.particles[i].Direction * this.particles[i].Speed * Engine.DeltaTime;
          this.particles[i].Direction.Rotate(this.particles[i].Spin * Engine.DeltaTime);
        }
        this.fade = Calc.Approach(this.fade, this.Visible ? 1f : 0.0f, Engine.DeltaTime);
      }

      public override void Render(Scene level)
      {
        if ((double) this.fade <= 0.0)
          return;
        Camera camera = (level as Level).Camera;
        for (int index = 0; index < this.particles.Length; ++index)
        {
          Vector2 position = new Vector2()
          {
            X = this.Mod(this.particles[index].Position.X - camera.X, 320f),
            Y = this.Mod(this.particles[index].Position.Y - camera.Y, 180f)
          };
          float percent = this.particles[index].Percent;
          float num = (double) percent >= 0.699999988079071 ? Calc.ClampedMap(percent, 0.7f, 1f, 1f, 0.0f) : Calc.ClampedMap(percent, 0.0f, 0.3f);
          Color color = MirrorFG.colors[this.particles[index].Color] * (this.fade * num);
          Draw.Rect(position, 1f, 1f, color);
        }
      }

      private float Mod(float x, float m) => (x % m + m) % m;

      private struct Particle
      {
        public Vector2 Position;
        public float Percent;
        public float Duration;
        public Vector2 Direction;
        public float Speed;
        public float Spin;
        public int Color;
      }
    }
}
