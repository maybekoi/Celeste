// Decompiled with JetBrains decompiler
// Type: Celeste.PostUpdateHook
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Monocle;
using System;

namespace Celeste
{

    [Tracked(false)]
    public class PostUpdateHook : Component
    {
      public Action OnPostUpdate;

      public PostUpdateHook(Action onPostUpdate)
        : base(false, false)
      {
        this.OnPostUpdate = onPostUpdate;
      }
    }
}
