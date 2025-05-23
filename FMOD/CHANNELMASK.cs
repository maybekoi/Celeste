// Decompiled with JetBrains decompiler
// Type: FMOD.CHANNELMASK
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System;

namespace FMOD
{

    [Flags]
    public enum CHANNELMASK : uint
    {
      FRONT_LEFT = 1,
      FRONT_RIGHT = 2,
      FRONT_CENTER = 4,
      LOW_FREQUENCY = 8,
      SURROUND_LEFT = 16, // 0x00000010
      SURROUND_RIGHT = 32, // 0x00000020
      BACK_LEFT = 64, // 0x00000040
      BACK_RIGHT = 128, // 0x00000080
      BACK_CENTER = 256, // 0x00000100
      MONO = FRONT_LEFT, // 0x00000001
      STEREO = MONO | FRONT_RIGHT, // 0x00000003
      LRC = STEREO | FRONT_CENTER, // 0x00000007
      QUAD = STEREO | SURROUND_RIGHT | SURROUND_LEFT, // 0x00000033
      SURROUND = QUAD | FRONT_CENTER, // 0x00000037
      _5POINT1 = SURROUND | LOW_FREQUENCY, // 0x0000003F
      _5POINT1_REARS = LRC | BACK_RIGHT | BACK_LEFT | LOW_FREQUENCY, // 0x000000CF
      _7POINT0 = SURROUND | BACK_RIGHT | BACK_LEFT, // 0x000000F7
      _7POINT1 = _7POINT0 | LOW_FREQUENCY, // 0x000000FF
    }
}
