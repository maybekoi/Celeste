// Decompiled with JetBrains decompiler
// Type: Celeste.CoreModeListener
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Monocle;
using System;

namespace Celeste
{

    [Tracked(false)]
    public class CoreModeListener : Component
    {
      public Action<Session.CoreModes> OnChange;

      public CoreModeListener(Action<Session.CoreModes> onChange)
        : base(false, false)
      {
        this.OnChange = onChange;
      }
    }
}
