// Decompiled with JetBrains decompiler
// Type: FMOD.MODE
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System;

namespace FMOD
{

    [Flags]
    public enum MODE : uint
    {
      DEFAULT = 0,
      LOOP_OFF = 1,
      LOOP_NORMAL = 2,
      LOOP_BIDI = 4,
      _2D = 8,
      _3D = 16, // 0x00000010
      CREATESTREAM = 128, // 0x00000080
      CREATESAMPLE = 256, // 0x00000100
      CREATECOMPRESSEDSAMPLE = 512, // 0x00000200
      OPENUSER = 1024, // 0x00000400
      OPENMEMORY = 2048, // 0x00000800
      OPENMEMORY_POINT = 268435456, // 0x10000000
      OPENRAW = 4096, // 0x00001000
      OPENONLY = 8192, // 0x00002000
      ACCURATETIME = 16384, // 0x00004000
      MPEGSEARCH = 32768, // 0x00008000
      NONBLOCKING = 65536, // 0x00010000
      UNIQUE = 131072, // 0x00020000
      _3D_HEADRELATIVE = 262144, // 0x00040000
      _3D_WORLDRELATIVE = 524288, // 0x00080000
      _3D_INVERSEROLLOFF = 1048576, // 0x00100000
      _3D_LINEARROLLOFF = 2097152, // 0x00200000
      _3D_LINEARSQUAREROLLOFF = 4194304, // 0x00400000
      _3D_INVERSETAPEREDROLLOFF = 8388608, // 0x00800000
      _3D_CUSTOMROLLOFF = 67108864, // 0x04000000
      _3D_IGNOREGEOMETRY = 1073741824, // 0x40000000
      IGNORETAGS = 33554432, // 0x02000000
      LOWMEM = 134217728, // 0x08000000
      LOADSECONDARYRAM = 536870912, // 0x20000000
      VIRTUAL_PLAYFROMSTART = 2147483648, // 0x80000000
    }
}
