// Decompiled with JetBrains decompiler
// Type: FMOD.TAG
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System;
using System.Runtime.InteropServices;

namespace FMOD
{

    public struct TAG
    {
      public TAGTYPE type;
      public TAGDATATYPE datatype;
      private IntPtr name_internal;
      public IntPtr data;
      public uint datalen;
      public bool updated;

      public string name => Marshal.PtrToStringAnsi(this.name_internal);
    }
}
