// Decompiled with JetBrains decompiler
// Type: Celeste.StarRotateSpinner
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    public class StarRotateSpinner : RotateSpinner
    {
      public Sprite Sprite;
      private int colorID;

      public StarRotateSpinner(EntityData data, Vector2 offset)
        : base(data, offset)
      {
        this.Add((Component) (this.Sprite = GFX.SpriteBank.Create("moonBlade")));
        this.colorID = Calc.Random.Choose<int>(0, 1, 2);
        this.Sprite.Play("idle" + (object) this.colorID);
        this.Depth = -50;
        this.Add((Component) new MirrorReflection());
      }

      public override void Update()
      {
        base.Update();
        if (this.Moving && this.Scene.OnInterval(0.03f))
          this.SceneAs<Level>().ParticlesBG.Emit(StarTrackSpinner.P_Trail[this.colorID], 1, this.Position, Vector2.One * 3f);
        if (!this.Scene.OnInterval(0.8f))
          return;
        ++this.colorID;
        this.colorID %= 3;
        this.Sprite.Play("spin" + (object) this.colorID);
      }
    }
}
