// Decompiled with JetBrains decompiler
// Type: Celeste.DashListener
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    [Tracked(false)]
    public class DashListener : Component
    {
      public Action<Vector2> OnDash;
      public Action OnSet;

      public DashListener()
        : base(false, false)
      {
      }
    }
}
