// Decompiled with JetBrains decompiler
// Type: Celeste.TempleMirror
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    public class TempleMirror : Entity
    {
      private readonly Color color = Calc.HexToColor("05070e");
      private readonly Vector2 size;
      private MTexture[,] frame = new MTexture[3, 3];
      private MirrorSurface surface;

      public TempleMirror(EntityData e, Vector2 offset)
        : base(e.Position + offset)
      {
        this.size = new Vector2((float) e.Width, (float) e.Height);
        this.Depth = 9500;
        this.Collider = (Collider) new Hitbox((float) e.Width, (float) e.Height);
        this.Add((Component) (this.surface = new MirrorSurface()));
        this.surface.ReflectionOffset = new Vector2(e.Float("reflectX"), e.Float("reflectY"));
        this.surface.OnRender = (Action) (() => Draw.Rect(this.X + 2f, this.Y + 2f, this.size.X - 4f, this.size.Y - 4f, this.surface.ReflectionColor));
        MTexture mtexture = GFX.Game["scenery/templemirror"];
        for (int index1 = 0; index1 < mtexture.Width / 8; ++index1)
        {
          for (int index2 = 0; index2 < mtexture.Height / 8; ++index2)
            this.frame[index1, index2] = mtexture.GetSubtexture(new Rectangle(index1 * 8, index2 * 8, 8, 8));
        }
      }

      public override void Added(Scene scene)
      {
        base.Added(scene);
        scene.Add((Entity) new TempleMirror.Frame(this));
      }

      public override void Render()
      {
        Draw.Rect(this.X + 3f, this.Y + 3f, this.size.X - 6f, this.size.Y - 6f, this.color);
      }

      private class Frame : Entity
      {
        private TempleMirror mirror;

        public Frame(TempleMirror mirror)
        {
          this.mirror = mirror;
          this.Depth = 8995;
        }

        public override void Render()
        {
          this.Position = this.mirror.Position;
          MTexture[,] frame = this.mirror.frame;
          Vector2 size = this.mirror.size;
          frame[0, 0].Draw(this.Position + new Vector2(0.0f, 0.0f));
          frame[2, 0].Draw(this.Position + new Vector2(size.X - 8f, 0.0f));
          frame[0, 2].Draw(this.Position + new Vector2(0.0f, size.Y - 8f));
          frame[2, 2].Draw(this.Position + new Vector2(size.X - 8f, size.Y - 8f));
          for (int index = 1; (double) index < (double) size.X / 8.0 - 1.0; ++index)
          {
            frame[1, 0].Draw(this.Position + new Vector2((float) (index * 8), 0.0f));
            frame[1, 2].Draw(this.Position + new Vector2((float) (index * 8), size.Y - 8f));
          }
          for (int index = 1; (double) index < (double) size.Y / 8.0 - 1.0; ++index)
          {
            frame[0, 1].Draw(this.Position + new Vector2(0.0f, (float) (index * 8)));
            frame[2, 1].Draw(this.Position + new Vector2(size.X - 8f, (float) (index * 8)));
          }
        }
      }
    }
}
