// Decompiled with JetBrains decompiler
// Type: Celeste.JumpthruPlatform
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    public class JumpthruPlatform : JumpThru
    {
      private int columns;
      private string overrideTexture;
      private int overrideSoundIndex = -1;

      public JumpthruPlatform(
        Vector2 position,
        int width,
        string overrideTexture,
        int overrideSoundIndex = -1)
        : base(position, width, true)
      {
        this.columns = width / 8;
        this.Depth = -60;
        this.overrideTexture = overrideTexture;
        this.overrideSoundIndex = overrideSoundIndex;
      }

      public JumpthruPlatform(EntityData data, Vector2 offset)
        : this(data.Position + offset, data.Width, data.Attr("texture", "default"), data.Int("surfaceIndex", -1))
      {
      }

      public override void Awake(Scene scene)
      {
        string str = AreaData.Get(scene).Jumpthru;
        if (!string.IsNullOrEmpty(this.overrideTexture) && !this.overrideTexture.Equals("default"))
          str = this.overrideTexture;
        if (this.overrideSoundIndex > 0)
        {
          this.SurfaceSoundIndex = this.overrideSoundIndex;
        }
        else
        {
          switch (str.ToLower())
          {
            case "dream":
              this.SurfaceSoundIndex = 32 /*0x20*/;
              break;
            case "temple":
            case "templeb":
              this.SurfaceSoundIndex = 8;
              break;
            case "core":
              this.SurfaceSoundIndex = 3;
              break;
            default:
              this.SurfaceSoundIndex = 5;
              break;
          }
        }
        MTexture mtexture = GFX.Game["objects/jumpthru/" + str];
        int num1 = mtexture.Width / 8;
        for (int index = 0; index < this.columns; ++index)
        {
          int num2;
          int num3;
          if (index == 0)
          {
            num2 = 0;
            num3 = this.CollideCheck<Solid, SwapBlock, ExitBlock>(this.Position + new Vector2(-1f, 0.0f)) ? 0 : 1;
          }
          else if (index == this.columns - 1)
          {
            num2 = num1 - 1;
            num3 = this.CollideCheck<Solid, SwapBlock, ExitBlock>(this.Position + new Vector2(1f, 0.0f)) ? 0 : 1;
          }
          else
          {
            num2 = 1 + Calc.Random.Next(num1 - 2);
            num3 = Calc.Random.Choose<int>(0, 1);
          }
          Monocle.Image image = new Monocle.Image(mtexture.GetSubtexture(num2 * 8, num3 * 8, 8, 8));
          image.X = (float) (index * 8);
          this.Add((Component) image);
        }
      }
    }
}
