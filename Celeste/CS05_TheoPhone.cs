// Decompiled with JetBrains decompiler
// Type: Celeste.CS05_TheoPhone
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;

namespace Celeste
{

    public class CS05_TheoPhone : CutsceneEntity
    {
      private Player player;
      private float targetX;

      public CS05_TheoPhone(Player player, float targetX)
        : base()
      {
        this.player = player;
        this.targetX = targetX;
      }

      public override void OnBegin(Level level) => this.Add((Component) new Coroutine(this.Routine()));

      private IEnumerator Routine()
      {
        CS05_TheoPhone cs05TheoPhone = this;
        cs05TheoPhone.player.StateMachine.State = 11;
        if ((double) cs05TheoPhone.player.X != (double) cs05TheoPhone.targetX)
          cs05TheoPhone.player.Facing = (Facings) Math.Sign(cs05TheoPhone.targetX - cs05TheoPhone.player.X);
        yield return (object) 0.5f;
        yield return (object) cs05TheoPhone.Level.ZoomTo(new Vector2(80f, 60f), 2f, 0.5f);
        yield return (object) Textbox.Say("CH5_PHONE", new Func<IEnumerator>(cs05TheoPhone.WalkToPhone), new Func<IEnumerator>(cs05TheoPhone.StandBackUp));
        yield return (object) cs05TheoPhone.Level.ZoomBack(0.5f);
        cs05TheoPhone.EndCutscene(cs05TheoPhone.Level);
      }

      private IEnumerator WalkToPhone()
      {
        yield return (object) 0.25f;
        yield return (object) this.player.DummyWalkToExact((int) this.targetX);
        this.player.Facing = Facings.Left;
        yield return (object) 0.5f;
        this.player.DummyAutoAnimate = false;
        this.player.Sprite.Play("duck");
        yield return (object) 0.5f;
      }

      private IEnumerator StandBackUp()
      {
        this.RemovePhone();
        yield return (object) 0.6f;
        this.player.Sprite.Play("idle");
        yield return (object) 0.2f;
      }

      public override void OnEnd(Level level)
      {
        this.RemovePhone();
        this.player.StateMachine.State = 0;
      }

      private void RemovePhone() => this.Scene.Entities.FindFirst<TheoPhone>()?.RemoveSelf();
    }
}
