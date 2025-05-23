﻿// Decompiled with JetBrains decompiler
// Type: Celeste.BloomFadeTrigger
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    public class BloomFadeTrigger : Trigger
    {
        public float BloomAddFrom;
        public float BloomAddTo;
        public Trigger.PositionModes PositionMode;

        public BloomFadeTrigger(EntityData data, Vector2 offset)
          : base(data, offset)
        {
            this.BloomAddFrom = data.Float("bloomAddFrom");
            this.BloomAddTo = data.Float("bloomAddTo");
            this.PositionMode = data.Enum<Trigger.PositionModes>("positionMode");
        }

        public override void OnStay(Player player)
        {
            Level scene = this.Scene as Level;
            Session session = scene.Session;
            float num1 = this.BloomAddFrom + (this.BloomAddTo - this.BloomAddFrom) * MathHelper.Clamp(this.GetPositionLerp(player, this.PositionMode), 0.0f, 1f);
            double num2 = (double)num1;
            session.BloomBaseAdd = (float)num2;
            scene.Bloom.Base = AreaData.Get((Scene)scene).BloomBase + num1;
        }
    }
}