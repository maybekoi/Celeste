// Decompiled with JetBrains decompiler
// Type: Celeste.CoreMessage
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    public class CoreMessage : Entity
    {
      private string text;
      private float alpha;

      public CoreMessage(EntityData data, Vector2 offset)
        : base(data.Position + offset)
      {
        this.Tag = (int) Tags.HUD;
        this.text = Dialog.Clean("app_ending").Split(new char[2]
        {
          '\n',
          '\r'
        }, StringSplitOptions.RemoveEmptyEntries)[data.Int("line")];
      }

      public override void Update()
      {
        Player entity = this.Scene.Tracker.GetEntity<Player>();
        if (entity != null)
          this.alpha = Ease.CubeInOut(Calc.ClampedMap(Math.Abs(this.X - entity.X), 0.0f, 128f, 1f, 0.0f));
        base.Update();
      }

      public override void Render()
      {
        Vector2 position1 = (this.Scene as Level).Camera.Position;
        Vector2 vector2 = position1 + new Vector2(160f, 90f);
        Vector2 position2 = (this.Position - position1 + (this.Position - vector2) * 0.2f) * 6f;
        if (SaveData.Instance != null && SaveData.Instance.Assists.MirrorMode)
          position2.X = 1920f - position2.X;
        ActiveFont.Draw(this.text, position2, new Vector2(0.5f, 0.5f), Vector2.One * 1.25f, Color.White * this.alpha);
      }
    }
}
