// Decompiled with JetBrains decompiler
// Type: Celeste.NPC06_Theo_Plateau
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    public class NPC06_Theo_Plateau : NPC
    {
      private float speedY;

      public NPC06_Theo_Plateau(EntityData data, Vector2 offset)
        : base(data.Position + offset)
      {
        this.Add((Component) (this.Sprite = GFX.SpriteBank.Create("theo")));
        this.IdleAnim = "idle";
        this.MoveAnim = "walk";
        this.Maxspeed = 48f;
        this.MoveY = false;
      }

      public override void Awake(Scene scene)
      {
        base.Awake(scene);
        this.Scene.Add((Entity) new CS06_Campfire((NPC) this, this.Scene.Tracker.GetEntity<Player>()));
        this.Add((Component) (this.Light = new VertexLight(new Vector2(0.0f, -6f), Color.White, 1f, 16 /*0x10*/, 48 /*0x30*/)));
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
