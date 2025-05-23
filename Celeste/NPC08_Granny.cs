// Decompiled with JetBrains decompiler
// Type: Celeste.NPC08_Granny
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    public class NPC08_Granny : NPC
    {
      public NPC08_Granny(EntityData data, Vector2 position)
        : base(data.Position + position)
      {
        this.Add((Component) (this.Sprite = GFX.SpriteBank.Create("granny")));
        this.Sprite.Scale.X = -1f;
        this.Sprite.Play("idle");
        this.IdleAnim = "idle";
        this.MoveAnim = "walk";
        this.Maxspeed = 30f;
        this.Depth = -10;
      }
    }
}
