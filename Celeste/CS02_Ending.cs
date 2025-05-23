// Decompiled with JetBrains decompiler
// Type: Celeste.CS02_Ending
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Monocle;
using System.Collections;

namespace Celeste
{

    public class CS02_Ending : CutsceneEntity
    {
      private Player player;
      private Payphone payphone;
      private SoundSource phoneSfx;

      public CS02_Ending(Player player)
        : base(false, true)
      {
        this.player = player;
        this.Add((Component) (this.phoneSfx = new SoundSource()));
      }

      public override void OnBegin(Level level)
      {
        level.RegisterAreaComplete();
        this.payphone = this.Scene.Tracker.GetEntity<Payphone>();
        this.Add((Component) new Coroutine(this.Cutscene(level)));
      }

      private IEnumerator Cutscene(Level level)
      {
        CS02_Ending cs02Ending = this;
        cs02Ending.player.StateMachine.State = 11;
        cs02Ending.player.Dashes = 1;
        while ((double) cs02Ending.player.Light.Alpha > 0.0)
        {
          cs02Ending.player.Light.Alpha -= Engine.DeltaTime * 1.25f;
          yield return (object) null;
        }
        yield return (object) 1f;
        yield return (object) cs02Ending.player.DummyWalkTo(cs02Ending.payphone.X - 4f);
        yield return (object) 0.2f;
        cs02Ending.player.Facing = Facings.Right;
        yield return (object) 0.5f;
        cs02Ending.player.Visible = false;
        Audio.Play("event:/game/02_old_site/sequence_phone_pickup", cs02Ending.player.Position);
        yield return (object) cs02Ending.payphone.Sprite.PlayRoutine("pickUp");
        yield return (object) 0.25f;
        cs02Ending.phoneSfx.Position = cs02Ending.player.Position;
        cs02Ending.phoneSfx.Play("event:/game/02_old_site/sequence_phone_ringtone_loop");
        yield return (object) 6f;
        cs02Ending.phoneSfx.Stop();
        cs02Ending.payphone.Sprite.Play("talkPhone");
        yield return (object) Textbox.Say("CH2_END_PHONECALL");
        yield return (object) 0.3f;
        cs02Ending.EndCutscene(level);
      }

      public override void OnEnd(Level level) => level.CompleteArea();
    }
}
