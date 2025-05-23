﻿// Decompiled with JetBrains decompiler
// Type: Celeste.CS03_Ending
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using FMOD.Studio;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;

namespace Celeste {

    public class CS03_Ending : CutsceneEntity
    {
        public const string Flag = "oshiroEnding";
        private ResortRoofEnding roof;
        private AngryOshiro angryOshiro;
        private Player player;
        private Entity oshiro;
        private Sprite oshiroSprite;
        private EventInstance smashSfx;
        private bool smashRumble;

        public CS03_Ending(ResortRoofEnding roof, Player player)
          : base(false, true)
        {
            this.roof = roof;
            this.player = player;
            this.Depth = -1000000;
        }

        public override void OnBegin(Level level)
        {
            level.RegisterAreaComplete();
            this.Add((Component)new Coroutine(this.Cutscene(level)));
        }

        public override void Update()
        {
            base.Update();
            if (!this.smashRumble)
                return;
            Input.Rumble(RumbleStrength.Light, RumbleLength.Short);
        }

        private IEnumerator Cutscene(Level level)
        {
            CS03_Ending cs03Ending1 = this;
            cs03Ending1.player.StateMachine.State = 11;
            cs03Ending1.player.StateMachine.Locked = true;
            cs03Ending1.player.ForceCameraUpdate = false;
            cs03Ending1.Add((Component)new Coroutine(cs03Ending1.player.DummyRunTo((float)((double)cs03Ending1.roof.X + (double)cs03Ending1.roof.Width - 32.0), true)));
            yield return (object)null;
            cs03Ending1.player.DummyAutoAnimate = false;
            yield return (object)0.5f;
            cs03Ending1.angryOshiro = cs03Ending1.Scene.Entities.FindFirst<AngryOshiro>();
            cs03Ending1.Add((Component)new Coroutine(cs03Ending1.MoveGhostTo(new Vector2(cs03Ending1.roof.X + 40f, cs03Ending1.roof.Y - 12f))));
            yield return (object)1f;
            cs03Ending1.player.DummyAutoAnimate = true;
            yield return (object)level.ZoomTo(new Vector2(130f, 60f), 2f, 0.5f);
            cs03Ending1.player.Facing = Facings.Left;
            yield return (object)0.5f;
            yield return (object)Textbox.Say("CH3_OSHIRO_CHASE_END", new Func<IEnumerator>(cs03Ending1.GhostSmash));
            yield return (object)cs03Ending1.GhostSmash(0.5f, true);
            Audio.SetMusic((string)null);
            cs03Ending1.oshiroSprite = (Sprite)null;
            CS03_Ending.BgFlash bgFlash = new CS03_Ending.BgFlash();
            bgFlash.Alpha = 1f;
            level.Add((Entity)bgFlash);
            Distort.GameRate = 0.0f;
            Sprite sprite = GFX.SpriteBank.Create("oshiro_boss_lightning");
            sprite.Position = cs03Ending1.angryOshiro.Position + new Vector2(140f, -100f);
            sprite.Rotation = Calc.Angle(sprite.Position, cs03Ending1.angryOshiro.Position + new Vector2(0.0f, 10f));
            sprite.Play("once");
            cs03Ending1.Add((Component)sprite);
            yield return (object)null;
            Celeste.Freeze(0.3f);
            yield return (object)null;
            level.Shake();
            Input.Rumble(RumbleStrength.Strong, RumbleLength.Long);
            cs03Ending1.smashRumble = false;
            yield return (object)0.2f;
            Distort.GameRate = 1f;
            level.Flash(Color.White);
            cs03Ending1.player.DummyGravity = false;
            cs03Ending1.angryOshiro.Sprite.Play("transformBack");
            cs03Ending1.player.Sprite.Play("fall");
            cs03Ending1.roof.BeginFalling = true;
            yield return (object)null;
            Engine.TimeRate = 0.01f;
            cs03Ending1.player.Sprite.Play("fallFast");
            cs03Ending1.player.DummyGravity = true;
            cs03Ending1.player.Speed.Y = -200f;
            cs03Ending1.player.Speed.X = 300f;
            Vector2 oshiroFallSpeed = new Vector2(-100f, -250f);
            Tween tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.SineInOut, 1.5f, true);
            tween.OnUpdate = delegate (Tween tw)
            {
                this.angryOshiro.Sprite.Rotation = tw.Eased * -100f * 0.017453292f;
            };
            cs03Ending1.Add(tween);
            float t;
            for (t = 0.0f; (double)t < 2.0; t += Engine.DeltaTime)
            {
                oshiroFallSpeed.X = Calc.Approach(oshiroFallSpeed.X, 0.0f, Engine.DeltaTime * 400f);
                oshiroFallSpeed.Y += Engine.DeltaTime * 800f;
                AngryOshiro angryOshiro = cs03Ending1.angryOshiro;
                angryOshiro.Position = angryOshiro.Position + oshiroFallSpeed * Engine.DeltaTime;
                bgFlash.Alpha = Calc.Approach(bgFlash.Alpha, 0.0f, Engine.RawDeltaTime);
                Engine.TimeRate = Calc.Approach(Engine.TimeRate, 1f, Engine.RawDeltaTime * 0.6f);
                yield return (object)null;
            }
            level.DirectionalShake(new Vector2(0.0f, -1f), 0.5f);
            Input.Rumble(RumbleStrength.Medium, RumbleLength.Long);
            yield return (object)1f;
            bgFlash = (CS03_Ending.BgFlash)null;
            oshiroFallSpeed = new Vector2();
            while (!cs03Ending1.player.OnGround())
                cs03Ending1.player.MoveV(1f);
            cs03Ending1.player.DummyAutoAnimate = false;
            cs03Ending1.player.Sprite.Play("tired");
            cs03Ending1.angryOshiro.RemoveSelf();
            Scene scene = cs03Ending1.Scene;
            CS03_Ending cs03Ending2 = cs03Ending1;
            Rectangle bounds = level.Bounds;
            Entity entity1;
            Entity entity2 = entity1 = new Entity(new Vector2((float)(bounds.Left + 110), cs03Ending1.player.Y));
            cs03Ending2.oshiro = entity1;
            Entity entity3 = entity2;
            scene.Add(entity3);
            cs03Ending1.oshiro.Add((Component)(cs03Ending1.oshiroSprite = GFX.SpriteBank.Create("oshiro")));
            cs03Ending1.oshiroSprite.Play("fall");
            cs03Ending1.oshiroSprite.Scale.X = 1f;
            cs03Ending1.oshiro.Collider = (Collider)new Hitbox(8f, 8f, -4f, -8f);
            cs03Ending1.oshiro.Add((Component)new VertexLight(new Vector2(0.0f, -8f), Color.White, 1f, 16 /*0x10*/, 32 /*0x20*/));
            yield return (object)CutsceneEntity.CameraTo(cs03Ending1.player.CameraTarget + new Vector2(0.0f, 40f), 1f, Ease.CubeOut);
            yield return (object)1.5f;
            Audio.SetMusic("event:/music/lvl3/intro");
            yield return (object)3f;
            Audio.Play("event:/char/oshiro/chat_get_up", cs03Ending1.oshiro.Position);
            cs03Ending1.oshiroSprite.Play("recover");
            float target = cs03Ending1.oshiro.Y + 4f;
            while ((double)cs03Ending1.oshiro.Y != (double)target)
            {
                cs03Ending1.oshiro.Y = Calc.Approach(cs03Ending1.oshiro.Y, target, 6f * Engine.DeltaTime);
                yield return (object)null;
            }
            yield return (object)0.6f;
            yield return (object)Textbox.Say("CH3_ENDING", new Func<IEnumerator>(cs03Ending1.OshiroTurns));
            cs03Ending1.Add((Component)new Coroutine(CutsceneEntity.CameraTo(level.Camera.Position + new Vector2(-80f, 0.0f), 3f)));
            yield return (object)0.5f;
            cs03Ending1.oshiroSprite.Scale.X = -1f;
            yield return (object)0.2f;
            t = 0.0f;
            cs03Ending1.oshiro.Add((Component)new SoundSource("event:/char/oshiro/move_08_roof07_exit"));
            while ((double)cs03Ending1.oshiro.X > (double)(level.Bounds.Left - 16 /*0x10*/))
            {
                cs03Ending1.oshiro.X -= 40f * Engine.DeltaTime;
                cs03Ending1.oshiroSprite.Y = (float)Math.Sin((double)(t += Engine.DeltaTime * 2f)) * 2f;
                cs03Ending1.oshiro.CollideFirst<Door>()?.Open(cs03Ending1.oshiro.X);
                yield return (object)null;
            }
            cs03Ending1.Add((Component)new Coroutine(CutsceneEntity.CameraTo(level.Camera.Position + new Vector2(80f, 0.0f), 2f)));
            yield return (object)1.2f;
            cs03Ending1.player.DummyAutoAnimate = true;
            yield return (object)cs03Ending1.player.DummyWalkTo(cs03Ending1.player.X - 16f);
            yield return (object)2f;
            cs03Ending1.player.Facing = Facings.Right;
            yield return (object)1f;
            cs03Ending1.player.ForceCameraUpdate = false;
            cs03Ending1.player.Add((Component)new Coroutine(cs03Ending1.RunPlayerRight()));
            cs03Ending1.EndCutscene(level);
        }

        private IEnumerator OshiroTurns()
        {
            yield return (object)1f;
            this.oshiroSprite.Scale.X = -1f;
            yield return (object)0.2f;
        }

        private IEnumerator MoveGhostTo(Vector2 target)
        {
            if (this.angryOshiro != null)
            {
                target.Y -= this.angryOshiro.Height / 2f;
                this.angryOshiro.EnterDummyMode();
                this.angryOshiro.Collidable = false;
                while (this.angryOshiro.Position != target)
                {
                    this.angryOshiro.Position = Calc.Approach(this.angryOshiro.Position, target, 64f * Engine.DeltaTime);
                    yield return (object)null;
                }
            }
        }

        private IEnumerator GhostSmash()
        {
            yield return (object)this.GhostSmash(0.0f, false);
        }

        private IEnumerator GhostSmash(float topDelay, bool final)
        {
            CS03_Ending cs03Ending = this;
            if (cs03Ending.angryOshiro != null)
            {
                cs03Ending.smashSfx = !final ? Audio.Play("event:/char/oshiro/boss_slam_first", cs03Ending.angryOshiro.Position) : Audio.Play("event:/char/oshiro/boss_slam_final", cs03Ending.angryOshiro.Position);
                float from = cs03Ending.angryOshiro.Y;
                float to = cs03Ending.angryOshiro.Y - 32f;
                float p;
                for (p = 0.0f; (double)p < 1.0; p += Engine.DeltaTime * 2f)
                {
                    cs03Ending.angryOshiro.Y = MathHelper.Lerp(from, to, Ease.CubeOut(p));
                    yield return (object)null;
                }
                yield return (object)topDelay;
                float ground = from + 20f;
                for (p = 0.0f; (double)p < 1.0; p += Engine.DeltaTime * 8f)
                {
                    cs03Ending.angryOshiro.Y = MathHelper.Lerp(to, ground, Ease.CubeOut(p));
                    yield return (object)null;
                }
                cs03Ending.angryOshiro.Squish();
                cs03Ending.Level.Shake(0.5f);
                Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);
                cs03Ending.smashRumble = true;
                cs03Ending.roof.StartShaking(0.5f);
                if (!final)
                {
                    for (p = 0.0f; (double)p < 1.0; p += Engine.DeltaTime * 16f)
                    {
                        cs03Ending.angryOshiro.Y = MathHelper.Lerp(ground, from, Ease.CubeOut(p));
                        yield return (object)null;
                    }
                }
                else
                    cs03Ending.angryOshiro.Y = (float)(((double)ground + (double)from) / 2.0);
                if (cs03Ending.angryOshiro != null)
                {
                    cs03Ending.player.DummyAutoAnimate = false;
                    cs03Ending.player.Sprite.Play("shaking");
                    cs03Ending.roof.Wobble(cs03Ending.angryOshiro, final);
                    if (!final)
                        yield return (object)0.5f;
                }
            }
        }

        private IEnumerator RunPlayerRight()
        {
            yield return (object)0.75f;
            yield return (object)this.player.DummyRunTo(this.player.X + 128f);
        }

        public override void OnEnd(Level level)
        {
            Audio.SetMusic("event:/music/lvl3/intro");
            Audio.Stop(this.smashSfx);
            level.CompleteArea();
            SpotlightWipe.FocusPoint = new Vector2(192f, 120f);
        }

        private class BgFlash : Entity
        {
            public float Alpha;

            public BgFlash() => this.Depth = 10100;

            public override void Render()
            {
                Camera camera = (this.Scene as Level).Camera;
                Draw.Rect(camera.X - 10f, camera.Y - 10f, 340f, 200f, Color.Black * this.Alpha);
            }
        }
    }
}