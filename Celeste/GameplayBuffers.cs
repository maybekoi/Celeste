// Decompiled with JetBrains decompiler
// Type: Celeste.GameplayBuffers
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Monocle;
using System.Collections.Generic;

namespace Celeste
{

    public static class GameplayBuffers
    {
      public static VirtualRenderTarget Gameplay;
      public static VirtualRenderTarget Level;
      public static VirtualRenderTarget ResortDust;
      public static VirtualRenderTarget LightBuffer;
      public static VirtualRenderTarget Light;
      public static VirtualRenderTarget Displacement;
      public static VirtualRenderTarget MirrorSources;
      public static VirtualRenderTarget MirrorMasks;
      public static VirtualRenderTarget SpeedRings;
      public static VirtualRenderTarget Lightning;
      public static VirtualRenderTarget TempA;
      public static VirtualRenderTarget TempB;
      private static List<VirtualRenderTarget> all = new List<VirtualRenderTarget>();

      public static void Create()
      {
        GameplayBuffers.Unload();
        GameplayBuffers.Gameplay = GameplayBuffers.Create(320, 180);
        GameplayBuffers.Level = GameplayBuffers.Create(320, 180);
        GameplayBuffers.ResortDust = GameplayBuffers.Create(320, 180);
        GameplayBuffers.Light = GameplayBuffers.Create(320, 180);
        GameplayBuffers.Displacement = GameplayBuffers.Create(320, 180);
        GameplayBuffers.LightBuffer = GameplayBuffers.Create(1024 /*0x0400*/, 1024 /*0x0400*/);
        GameplayBuffers.MirrorSources = GameplayBuffers.Create(384, 244);
        GameplayBuffers.MirrorMasks = GameplayBuffers.Create(384, 244);
        GameplayBuffers.SpeedRings = GameplayBuffers.Create(512 /*0x0200*/, 512 /*0x0200*/);
        GameplayBuffers.Lightning = GameplayBuffers.Create(160 /*0xA0*/, 160 /*0xA0*/);
        GameplayBuffers.TempA = GameplayBuffers.Create(320, 180);
        GameplayBuffers.TempB = GameplayBuffers.Create(320, 180);
      }

      private static VirtualRenderTarget Create(int width, int height)
      {
        VirtualRenderTarget renderTarget = VirtualContent.CreateRenderTarget("gameplay-buffer-" + (object) GameplayBuffers.all.Count, width, height);
        GameplayBuffers.all.Add(renderTarget);
        return renderTarget;
      }

      public static void Unload()
      {
        foreach (VirtualAsset virtualAsset in GameplayBuffers.all)
          virtualAsset.Dispose();
        GameplayBuffers.all.Clear();
      }
    }
}
