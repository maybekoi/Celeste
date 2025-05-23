// Decompiled with JetBrains decompiler
// Type: Celeste.NPC00_Granny
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    public class NPC00_Granny : NPC
    {
      public Hahaha Hahaha;
      public GrannyLaughSfx LaughSfx;
      private bool talking;

      public NPC00_Granny(Vector2 position)
        : base(position)
      {
        this.Add((Component) (this.Sprite = GFX.SpriteBank.Create("granny")));
        this.Sprite.Play("idle");
        this.Add((Component) (this.LaughSfx = new GrannyLaughSfx(this.Sprite)));
      }

      public override void Added(Scene scene)
      {
        base.Added(scene);
        if ((scene as Level).Session.GetFlag("granny"))
          this.Sprite.Play("laugh");
        scene.Add((Entity) (this.Hahaha = new Hahaha(this.Position + new Vector2(8f, -4f))));
        this.Hahaha.Enabled = false;
      }

      public override void Update()
      {
        Player entity = this.Level.Tracker.GetEntity<Player>();
        if (entity != null && !this.Session.GetFlag("granny") && !this.talking)
        {
          int num = this.Level.Bounds.Left + 96 /*0x60*/;
          if (entity.OnGround() && (double) entity.X >= (double) num && (double) entity.X <= (double) this.X + 16.0 && (double) Math.Abs(entity.Y - this.Y) < 4.0 && entity.Facing == (Facings) Math.Sign(this.X - entity.X))
          {
            this.talking = true;
            this.Scene.Add((Entity) new CS00_Granny(this, entity));
          }
        }
        this.Hahaha.Enabled = this.Sprite.CurrentAnimationID == "laugh";
        base.Update();
      }
    }
}
