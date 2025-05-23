// Decompiled with JetBrains decompiler
// Type: Celeste.NegaBlock
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{
    [Tracked]
    public class NegaBlock : Solid
    {
        public NegaBlock(Vector2 position, float width, float height)
            : base(position, width, height, false)
        {
        }

        public NegaBlock(EntityData data, Vector2 offset)
            : this(data.Position + offset, data.Width, data.Height)
        {
        }

        public override void Render()
        {
            base.Render();
            Draw.Rect(Collider, Color.Red);
        }
    }
}