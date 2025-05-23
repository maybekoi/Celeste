﻿// Decompiled with JetBrains decompiler
// Type: Celeste.CS04_Granny
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;

namespace Celeste {

    public class CS04_Granny : CutsceneEntity
    {
        public const string Flag = "granny_1";
        private NPC04_Granny granny;
        private Player player;

        public CS04_Granny(NPC04_Granny granny, Player player)
          : base()
        {
            this.granny = granny;
            this.player = player;
        }

        public override void OnBegin(Level level)
        {
            this.Add((Component)new Coroutine(this.Cutscene(level)));
        }

        private IEnumerator Cutscene(Level level)
        {
            CS04_Granny cs04Granny = this;
            cs04Granny.player.StateMachine.State = 11;
            cs04Granny.player.StateMachine.Locked = true;
            cs04Granny.player.ForceCameraUpdate = true;
            yield return (object)cs04Granny.player.DummyWalkTo(cs04Granny.granny.X - 30f);
            cs04Granny.player.Facing = Facings.Right;
            yield return (object)Textbox.Say("CH4_GRANNY_1", new Func<IEnumerator>(cs04Granny.Laughs), new Func<IEnumerator>(cs04Granny.StopLaughing), new Func<IEnumerator>(cs04Granny.WaitABeat), new Func<IEnumerator>(cs04Granny.ZoomIn), new Func<IEnumerator>(cs04Granny.MaddyTurnsAround), new Func<IEnumerator>(cs04Granny.MaddyApproaches), new Func<IEnumerator>(cs04Granny.MaddyWalksPastGranny));
            yield return (object)cs04Granny.Level.ZoomBack(0.5f);
            cs04Granny.EndCutscene(level);
        }

        private IEnumerator Laughs()
        {
            this.granny.Sprite.Play("laugh");
            yield return (object)1f;
        }

        private IEnumerator StopLaughing()
        {
            this.granny.Sprite.Play("idle");
            yield return (object)0.25f;
        }

        private IEnumerator WaitABeat()
        {
            yield return (object)1.2f;
        }

        private IEnumerator ZoomIn()
        {
            /*
            // ISSUE: reference to a compiler-generated field
            int num = this.\u003C\u003E1__state;
            CS04_Granny cs04Granny = this;
            if (num != 0)
            {
                if (num != 1)
                    return false;
                // ISSUE: reference to a compiler-generated field
                this.\u003C\u003E1__state = -1;
                return false;
            }
            // ISSUE: reference to a compiler-generated field
            this.\u003C\u003E1__state = -1;
            // ISSUE: reference to a compiler-generated field
            this.\u003C\u003E2__current = (object)cs04Granny.Level.ZoomTo(new Vector2(123f, 116f), 2f, 0.5f);
            // ISSUE: reference to a compiler-generated field
            this.\u003C\u003E1__state = 1;
            return true;
            */
            yield return this.Level.ZoomTo(new Vector2(123f, 116f), 2f, 0.5f);
            yield break;
        }

        private IEnumerator MaddyTurnsAround()
        {
            yield return (object)0.2f;
            this.player.Facing = Facings.Left;
            yield return (object)0.1f;
        }

        private IEnumerator MaddyApproaches()
        {
            yield return (object)this.player.DummyWalkTo(this.granny.X - 20f);
        }

        private IEnumerator MaddyWalksPastGranny()
        {
            yield return (object)this.player.DummyWalkToExact((int)this.granny.X + 30);
        }

        public override void OnEnd(Level level)
        {
            this.player.X = this.granny.X + 30f;
            this.player.StateMachine.Locked = false;
            this.player.StateMachine.State = 0;
            this.player.ForceCameraUpdate = false;
            if (this.WasSkipped)
                level.Camera.Position = this.player.CameraTarget;
            this.granny.Sprite.Play("laugh");
            level.Session.SetFlag("granny_1");
        }
    }
}