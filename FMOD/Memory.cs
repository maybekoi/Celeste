// Decompiled with JetBrains decompiler
// Type: FMOD.Memory
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System;
using System.Runtime.InteropServices;

namespace FMOD
{

    public class Memory
    {
      public static RESULT Initialize(
        IntPtr poolmem,
        int poollen,
        MEMORY_ALLOC_CALLBACK useralloc,
        MEMORY_REALLOC_CALLBACK userrealloc,
        MEMORY_FREE_CALLBACK userfree,
        MEMORY_TYPE memtypeflags = MEMORY_TYPE.ALL)
      {
        return Memory.FMOD_Memory_Initialize(poolmem, poollen, useralloc, userrealloc, userfree, memtypeflags);
      }

      public static RESULT GetStats(out int currentalloced, out int maxalloced)
      {
        return Memory.GetStats(out currentalloced, out maxalloced, false);
      }

      public static RESULT GetStats(out int currentalloced, out int maxalloced, bool blocking)
      {
        return Memory.FMOD_Memory_GetStats(out currentalloced, out maxalloced, blocking);
      }

      [DllImport("fmod")]
      private static extern RESULT FMOD_Memory_Initialize(
        IntPtr poolmem,
        int poollen,
        MEMORY_ALLOC_CALLBACK useralloc,
        MEMORY_REALLOC_CALLBACK userrealloc,
        MEMORY_FREE_CALLBACK userfree,
        MEMORY_TYPE memtypeflags);

      [DllImport("fmod")]
      private static extern RESULT FMOD_Memory_GetStats(
        out int currentalloced,
        out int maxalloced,
        bool blocking);
    }
}
