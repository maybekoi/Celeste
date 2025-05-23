﻿// Decompiled with JetBrains decompiler
// Type: Celeste.CS08_EnterDoor
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;

namespace Celeste {

    public class CS08_EnterDoor : CutsceneEntity
    {
        private Player player;
        private float targetX;

        public CS08_EnterDoor(Player player, float targetX)
          : base()
        {
            this.player = player;
            this.targetX = targetX;
        }

        public override void OnBegin(Level level)
        {
            this.Add((Component)new Coroutine(this.Cutscene(level)));
        }

        private IEnumerator Cutscene(Level level)
        {
            /*
            // ISSUE: reference to a compiler-generated field
            int num = this.\u003C\u003E1__state;
            CS08_EnterDoor cs08EnterDoor = this;
            if (num != 0)
            {
                if (num != 1)
                    return false;
                // ISSUE: reference to a compiler-generated field
                this.\u003C\u003E1__state = -1;
                cs08EnterDoor.EndCutscene(level);
                return false;
            }
            // ISSUE: reference to a compiler-generated field
            this.\u003C\u003E1__state = -1;
            */
            CS08_EnterDoor cs08EnterDoor = this;
            cs08EnterDoor.player.StateMachine.State = 11;
            cs08EnterDoor.Add((Component)new Coroutine(cs08EnterDoor.player.DummyWalkToExact((int)cs08EnterDoor.targetX, speedMultiplier: 0.7f)));
            cs08EnterDoor.Add((Component)new Coroutine(level.ZoomTo(new Vector2(cs08EnterDoor.targetX - level.Camera.X, 90f), 2f, 2f)));
            yield return new FadeWipe(level, false, null)
            {
                Duration = 2f
            }.Wait();
            base.EndCutscene(level, true);
            yield break;
        }

        public override void OnEnd(Level level)
        {
            level.OnEndOfFrame += (Action)(() =>
            {
                level.Remove((Entity)this.player);
                level.UnloadLevel();
                level.Session.Level = "inside";
                Session session = level.Session;
                Level level1 = level;
                Rectangle bounds = level.Bounds;
                double left = (double)bounds.Left;
                bounds = level.Bounds;
                double top = (double)bounds.Top;
                Vector2 from = new Vector2((float)left, (float)top);
                Vector2? nullable = new Vector2?(level1.GetSpawnPoint(from));
                session.RespawnPoint = nullable;
                level.LoadLevel(Player.IntroTypes.None);
                level.Add((Entity)new CS08_Ending());
            });
        }
    }
}