// Decompiled with JetBrains decompiler
// Type: Monocle.Image
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;

namespace Monocle
{

    public class Image : GraphicsComponent
    {
      public MTexture Texture;
      public bool TEST;

      public Image(MTexture texture)
        : base(false)
      {
        this.Texture = texture;
      }

      internal Image(MTexture texture, bool active)
        : base(active)
      {
        this.Texture = texture;
      }

      public override void Render()
      {
        if (this.Texture == null)
          return;
        this.Texture.Draw(this.RenderPosition, this.Origin, this.Color, this.Scale, this.Rotation, this.Effects);
      }

      public virtual float Width => (float) this.Texture.Width;

      public virtual float Height => (float) this.Texture.Height;

      public Image SetOrigin(float x, float y)
      {
        this.Origin.X = x;
        this.Origin.Y = y;
        return this;
      }

      public Image CenterOrigin()
      {
        this.Origin.X = this.Width / 2f;
        this.Origin.Y = this.Height / 2f;
        return this;
      }

      public Image JustifyOrigin(Vector2 at)
      {
        this.Origin.X = this.Width * at.X;
        this.Origin.Y = this.Height * at.Y;
        return this;
      }

      public Image JustifyOrigin(float x, float y)
      {
        this.Origin.X = this.Width * x;
        this.Origin.Y = this.Height * y;
        return this;
      }

      public Image SetColor(Color color)
      {
        this.Color = color;
        return this;
      }
    }
}
