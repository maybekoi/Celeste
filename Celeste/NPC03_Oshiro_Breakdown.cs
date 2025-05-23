// Decompiled with JetBrains decompiler
// Type: Celeste.NPC03_Oshiro_Breakdown
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    public class NPC03_Oshiro_Breakdown : NPC
    {
      private bool talked;

      public NPC03_Oshiro_Breakdown(Vector2 position)
        : base(position)
      {
        this.Add((Component) (this.Sprite = (Sprite) new OshiroSprite(1)));
        this.Add((Component) (this.Light = new VertexLight(-Vector2.UnitY * 16f, Color.White, 1f, 32 /*0x20*/, 64 /*0x40*/)));
        this.MoveAnim = "move";
        this.IdleAnim = "idle";
      }

      public override void Added(Scene scene)
      {
        base.Added(scene);
        if (!this.Session.GetFlag("oshiro_breakdown"))
          return;
        this.RemoveSelf();
      }

      public override void Update()
      {
        base.Update();
        Player entity = this.Scene.Tracker.GetEntity<Player>();
        if (this.talked || entity == null)
          return;
        double x1 = (double) entity.X;
        Rectangle bounds = this.Level.Bounds;
        double num1 = (double) (bounds.Left + 370);
        if (x1 <= num1 && entity.OnSafeGround)
        {
          double y1 = (double) entity.Y;
          bounds = this.Level.Bounds;
          double y2 = (double) bounds.Center.Y;
          if (y1 < y2)
            goto label_4;
        }
        double x2 = (double) entity.X;
        bounds = this.Level.Bounds;
        double num2 = (double) (bounds.Left + 320);
        if (x2 > num2)
          return;
    label_4:
        this.Scene.Add((Entity) new CS03_OshiroBreakdown(entity, (NPC) this));
        this.talked = true;
      }
    }
}
