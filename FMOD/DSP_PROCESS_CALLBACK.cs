// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_PROCESS_CALLBACK
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

namespace FMOD
{

    public delegate RESULT DSP_PROCESS_CALLBACK(
      ref DSP_STATE dsp_state,
      uint length,
      ref DSP_BUFFER_ARRAY inbufferarray,
      ref DSP_BUFFER_ARRAY outbufferarray,
      bool inputsidle,
      DSP_PROCESS_OPERATION op);
}
