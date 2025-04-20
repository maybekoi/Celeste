// Decompiled with JetBrains decompiler
// Type: FMOD.REVERB_PROPERTIES
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

#nullable disable
namespace FMOD;

public struct REVERB_PROPERTIES(
  float decayTime,
  float earlyDelay,
  float lateDelay,
  float hfReference,
  float hfDecayRatio,
  float diffusion,
  float density,
  float lowShelfFrequency,
  float lowShelfGain,
  float highCut,
  float earlyLateMix,
  float wetLevel)
{
  public float DecayTime = decayTime;
  public float EarlyDelay = earlyDelay;
  public float LateDelay = lateDelay;
  public float HFReference = hfReference;
  public float HFDecayRatio = hfDecayRatio;
  public float Diffusion = diffusion;
  public float Density = density;
  public float LowShelfFrequency = lowShelfFrequency;
  public float LowShelfGain = lowShelfGain;
  public float HighCut = highCut;
  public float EarlyLateMix = earlyLateMix;
  public float WetLevel = wetLevel;
}
