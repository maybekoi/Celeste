// Decompiled with JetBrains decompiler
// Type: Celeste.NPC06_Theo_Ending
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    public class NPC06_Theo_Ending : NPC
    {
      private float speedY;

      public NPC06_Theo_Ending(EntityData data, Vector2 offset)
        : base(data.Position + offset)
      {
        this.Add((Component) (this.Sprite = GFX.SpriteBank.Create("theo")));
        this.IdleAnim = "idle";
        this.MoveAnim = "run";
        this.Maxspeed = 72f;
        this.MoveY = false;
        this.Visible = false;
        this.Add((Component) (this.Light = new VertexLight(new Vector2(0.0f, -8f), Color.White, 1f, 16 /*0x10*/, 32 /*0x20*/)));
        this.SetupTheoSpriteSounds();
      }

      public override void Update()
      {
        base.Update();
        if (!this.CollideCheck<Solid>(this.Position + new Vector2(0.0f, 1f)))
        {
          this.speedY += 400f * Engine.DeltaTime;
          this.Position.Y += this.speedY * Engine.DeltaTime;
        }
        else
          this.speedY = 0.0f;
      }
    }
}
