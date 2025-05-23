// Decompiled with JetBrains decompiler
// Type: Celeste.CS02_Mirror
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Celeste
{

    public class CS02_Mirror : CutsceneEntity
    {
      private Player player;
      private DreamMirror mirror;
      private float playerEndX;
      private int direction = 1;
      private SoundSource sfx;

      public CS02_Mirror(Player player, DreamMirror mirror)
        : base()
      {
        this.player = player;
        this.mirror = mirror;
      }

      public override void OnBegin(Level level)
      {
        this.Add((Component) new Coroutine(this.Cutscene(level)));
      }

      private IEnumerator Cutscene(Level level)
      {
        CS02_Mirror cs02Mirror = this;
        cs02Mirror.Add((Component) (cs02Mirror.sfx = new SoundSource()));
        cs02Mirror.sfx.Position = cs02Mirror.mirror.Center;
        cs02Mirror.sfx.Play("event:/music/lvl2/dreamblock_sting_pt1");
        cs02Mirror.direction = Math.Sign(cs02Mirror.player.X - cs02Mirror.mirror.X);
        cs02Mirror.player.StateMachine.State = 11;
        cs02Mirror.playerEndX = (float) (8 * cs02Mirror.direction);
        yield return (object) 1f;
        cs02Mirror.player.Facing = (Facings)(-(int)cs02Mirror.direction); 
        yield return (object) 0.4f;
        yield return (object) cs02Mirror.player.DummyRunTo(cs02Mirror.mirror.X + cs02Mirror.playerEndX);
        yield return (object) 0.5f;
        yield return (object) level.ZoomTo(cs02Mirror.mirror.Position - level.Camera.Position - Vector2.UnitY * 24f, 2f, 1f);
        yield return (object) 0.5f;
        yield return (object) cs02Mirror.mirror.BreakRoutine(cs02Mirror.direction);
        cs02Mirror.player.DummyAutoAnimate = false;
        cs02Mirror.player.Sprite.Play("lookUp");
        Vector2 from = level.Camera.Position;
        Vector2 to = level.Camera.Position + new Vector2(0.0f, -80f);
        for (float ease = 0.0f; (double) ease < 1.0; ease += Engine.DeltaTime * 1.2f)
        {
          level.Camera.Position = from + (to - from) * Ease.CubeInOut(ease);
          yield return (object) null;
        }
        cs02Mirror.Add((Component) new Coroutine(cs02Mirror.ZoomBack()));
        List<Entity>.Enumerator enumerator = cs02Mirror.Scene.Tracker.GetEntities<DreamBlock>().GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
            yield return (object) ((DreamBlock) enumerator.Current).Activate();
        }
        finally
        {
          enumerator.Dispose();
        }
        enumerator = new List<Entity>.Enumerator();
        from = new Vector2();
        to = new Vector2();
        yield return (object) 0.5f;
        cs02Mirror.EndCutscene(level);
      }

      private IEnumerator ZoomBack()
      {
        CS02_Mirror cs02Mirror = this;
        yield return (object) 1.2f;
        yield return (object) cs02Mirror.Level.ZoomBack(3f);
      }

      public override void OnEnd(Level level)
      {
        this.mirror.Broken(this.WasSkipped);
        if (this.WasSkipped)
          this.SceneAs<Level>().ParticlesFG.Clear();
        Player entity1 = this.Scene.Tracker.GetEntity<Player>();
        if (entity1 != null)
        {
          entity1.StateMachine.State = 0;
          entity1.DummyAutoAnimate = true;
          entity1.Speed = Vector2.Zero;
          entity1.X = this.mirror.X + this.playerEndX;
          entity1.Facing = this.direction == 0 ? Facings.Right : (Facings)(-(int)(this.direction));
        }
        foreach (DreamBlock entity2 in this.Scene.Tracker.GetEntities<DreamBlock>())
          entity2.ActivateNoRoutine();
        level.ResetZoom();
        level.Session.Inventory.DreamDash = true;
        level.Session.Audio.Music.Event = "event:/music/lvl2/mirror";
        level.Session.Audio.Apply();
      }
    }
}
