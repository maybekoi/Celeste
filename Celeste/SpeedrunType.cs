// Decompiled with JetBrains decompiler
// Type: Celeste.SpeedrunType
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System.Xml.Serialization;

namespace Celeste
{

    public enum SpeedrunType
    {
      [XmlEnum("false")] Off,
      [XmlEnum("true")] Chapter,
      File,
    }
}
