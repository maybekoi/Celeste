// Decompiled with JetBrains decompiler
// Type: FMOD.FILE_SEEKCALLBACK
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System;

namespace FMOD
{

    public delegate RESULT FILE_SEEKCALLBACK(IntPtr handle, uint pos, IntPtr userdata);
}
