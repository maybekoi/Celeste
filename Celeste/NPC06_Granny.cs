// Decompiled with JetBrains decompiler
// Type: Celeste.NPC06_Granny
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    public class NPC06_Granny : NPC
    {
      public Hahaha Hahaha;
      private int cutsceneIndex;

      public NPC06_Granny(EntityData data, Vector2 position)
        : base(data.Position + position)
      {
        this.Add((Component) (this.Sprite = GFX.SpriteBank.Create("granny")));
        this.Sprite.Scale.X = -1f;
        this.Sprite.Play("idle");
        this.Add((Component) new GrannyLaughSfx(this.Sprite));
      }

      public override void Added(Scene scene)
      {
        base.Added(scene);
        scene.Add((Entity) (this.Hahaha = new Hahaha(this.Position + new Vector2(8f, -4f))));
        this.Hahaha.Enabled = false;
        while (this.Session.GetFlag("granny_" + (object) this.cutsceneIndex))
          ++this.cutsceneIndex;
        this.Add((Component) (this.Talker = new TalkComponent(new Rectangle(-20, -8, 30, 8), new Vector2(0.0f, -24f), new Action<Player>(this.OnTalk))));
        this.Talker.Enabled = this.cutsceneIndex > 0 && this.cutsceneIndex < 3;
      }

      public override void Update()
      {
        if (this.cutsceneIndex == 0)
        {
          Player entity = this.Level.Tracker.GetEntity<Player>();
          if (entity != null && (double) entity.X > (double) this.X - 60.0)
            this.OnTalk(entity);
        }
        this.Hahaha.Enabled = this.Sprite.CurrentAnimationID == "laugh";
        base.Update();
      }

      private void OnTalk(Player player)
      {
        this.Scene.Add((Entity) new CS06_Granny(this, player, this.cutsceneIndex));
        ++this.cutsceneIndex;
        this.Talker.Enabled = this.cutsceneIndex > 0 && this.cutsceneIndex < 3;
      }
    }
}
