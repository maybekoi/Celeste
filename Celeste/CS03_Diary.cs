// Decompiled with JetBrains decompiler
// Type: Celeste.CS03_Diary
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Monocle;
using System.Collections;

namespace Celeste
{

    public class CS03_Diary : CutsceneEntity
    {
      private Player player;

      public CS03_Diary(Player player)
        : base()
      {
        this.player = player;
      }

      public override void OnBegin(Level level) => this.Add((Component) new Coroutine(this.Routine()));

      private IEnumerator Routine()
      {
        CS03_Diary cs03Diary = this;
        cs03Diary.player.StateMachine.State = 11;
        cs03Diary.player.StateMachine.Locked = true;
        yield return (object) Textbox.Say("CH3_DIARY");
        yield return (object) 0.1f;
        cs03Diary.EndCutscene(cs03Diary.Level);
      }

      public override void OnEnd(Level level)
      {
        this.player.StateMachine.Locked = false;
        this.player.StateMachine.State = 0;
      }
    }
}
