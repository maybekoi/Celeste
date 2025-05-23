// Decompiled with JetBrains decompiler
// Type: Celeste.SoundEmitter
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    public class SoundEmitter : Entity
    {
      public static SoundEmitter Play(string sfx)
      {
        SoundEmitter soundEmitter = new SoundEmitter(sfx);
        Engine.Scene.Add((Entity) soundEmitter);
        return soundEmitter;
      }

      public static SoundEmitter Play(string sfx, Entity follow, Vector2? offset = null)
      {
        SoundEmitter soundEmitter = new SoundEmitter(sfx, follow, offset.HasValue ? offset.Value : Vector2.Zero);
        Engine.Scene.Add((Entity) soundEmitter);
        return soundEmitter;
      }

      public SoundSource Source { get; private set; }

      private SoundEmitter(string sfx)
      {
        this.Add((Component) (this.Source = new SoundSource()));
        this.Source.Play(sfx);
        this.Source.DisposeOnTransition = false;
        this.Tag = (int) Tags.Persistent | (int) Tags.TransitionUpdate;
        this.Add((Component) new LevelEndingHook(new Action(this.OnLevelEnding)));
      }

      private SoundEmitter(string sfx, Entity follow, Vector2 offset)
      {
        this.Add((Component) (this.Source = new SoundSource()));
        this.Position = follow.Position + offset;
        this.Source.Play(sfx);
        this.Source.DisposeOnTransition = false;
        this.Tag = (int) Tags.Persistent | (int) Tags.TransitionUpdate;
        this.Add((Component) new LevelEndingHook(new Action(this.OnLevelEnding)));
      }

      public override void Update()
      {
        base.Update();
        if (this.Source.Playing)
          return;
        this.RemoveSelf();
      }

      private void OnLevelEnding() => this.Source.Stop();
    }
}
