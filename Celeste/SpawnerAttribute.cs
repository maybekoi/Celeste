// Decompiled with JetBrains decompiler
// Type: Celeste.SpawnerAttribute
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System;

namespace Celeste
{

    public class SpawnerAttribute : Attribute
    {
      public string Name;

      public SpawnerAttribute(string name = null) => this.Name = name;
    }
}
