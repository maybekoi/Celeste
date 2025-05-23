﻿// Decompiled with JetBrains decompiler
// Type: Celeste.CS07_Credits
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Celeste {

    public class CS07_Credits : CutsceneEntity
    {
        public const float CameraXOffset = 70f;
        public const float CameraYOffset = -24f;
        public static CS07_Credits Instance;
        public string Event;
        private MTexture gradient = GFX.Gui["creditsgradient"].GetSubtexture(0, 1, 1920, 1);
        private Credits credits;
        private Player player;
        private bool autoWalk = true;
        private bool autoUpdateCamera = true;
        private BadelineDummy badeline;
        private bool badelineAutoFloat = true;
        private bool badelineAutoWalk;
        private float badelineWalkApproach;
        private Vector2 badelineWalkApproachFrom;
        private float walkOffset;
        private bool wasDashAssistOn;
        private CS07_Credits.Fill fillbg;
        private float fade = 1f;
        private HiresSnow snow;
        private bool gotoEpilogue;

        public CS07_Credits()
          : base()
        {
            MInput.Disabled = true;
            CS07_Credits.Instance = this;
            this.Tag = (int)Tags.Global | (int)Tags.HUD;
            this.wasDashAssistOn = SaveData.Instance.Assists.DashAssist;
            SaveData.Instance.Assists.DashAssist = false;
        }

        public override void OnBegin(Level level)
        {
            Audio.BusMuted("bus:/gameplay_sfx", new bool?(true));
            this.gotoEpilogue = level.Session.OldStats.Modes[0].Completed;
            this.gotoEpilogue = true;
            this.Add((Component)new Coroutine(this.Routine()));
            this.Add((Component)new PostUpdateHook(new Action(this.PostUpdate)));
        }

        public override void Added(Scene scene)
        {
            base.Added(scene);
            (this.Scene as Level).InCredits = true;
        }

        private IEnumerator Routine()
        {
            CS07_Credits cs07Credits1 = this;
            cs07Credits1.Level.Background.Backdrops.Add((Backdrop)(cs07Credits1.fillbg = new CS07_Credits.Fill()));
            cs07Credits1.Level.Completed = true;
            cs07Credits1.Level.Entities.FindFirst<SpeedrunTimerDisplay>()?.RemoveSelf();
            cs07Credits1.Level.Entities.FindFirst<TotalStrawberriesDisplay>()?.RemoveSelf();
            cs07Credits1.Level.Entities.FindFirst<GameplayStats>()?.RemoveSelf();
            yield return (object)null;
            cs07Credits1.Level.Wipe.Cancel();
            yield return (object)0.5f;
            float alignment = 1f;
            if (SaveData.Instance != null && SaveData.Instance.Assists.MirrorMode)
                alignment = 0.0f;
            cs07Credits1.credits = new Credits(alignment, 0.6f, false, true);
            cs07Credits1.credits.AllowInput = false;
            yield return (object)3f;
            cs07Credits1.SetBgFade(0.0f);
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            yield return (object)cs07Credits1.SetupLevel();
            yield return (object)cs07Credits1.WaitForPlayer();
            yield return (object)cs07Credits1.FadeTo(1f);
            yield return (object)1f;
            cs07Credits1.SetBgFade(0.1f);
            yield return (object)cs07Credits1.NextLevel("credits-dashes");
            yield return (object)cs07Credits1.SetupLevel();
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            yield return (object)cs07Credits1.WaitForPlayer();
            yield return (object)cs07Credits1.FadeTo(1f);
            yield return (object)1f;
            cs07Credits1.SetBgFade(0.2f);
            yield return (object)cs07Credits1.NextLevel("credits-walking");
            yield return (object)cs07Credits1.SetupLevel();
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            yield return (object)5.8f;
            cs07Credits1.badelineAutoFloat = false;
            yield return (object)0.5f;
            cs07Credits1.badeline.Sprite.Scale.X = 1f;
            yield return (object)0.5f;
            cs07Credits1.autoWalk = false;
            cs07Credits1.player.Speed = Vector2.Zero;
            cs07Credits1.player.Facing = Facings.Right;
            yield return (object)1.5f;
            cs07Credits1.badeline.Sprite.Scale.X = -1f;
            yield return (object)1f;
            cs07Credits1.badeline.Sprite.Scale.X = -1f;
            cs07Credits1.badelineAutoWalk = true;
            cs07Credits1.badelineWalkApproachFrom = cs07Credits1.badeline.Position;
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.BadelineApproachWalking()));
            yield return (object)0.7f;
            cs07Credits1.autoWalk = true;
            cs07Credits1.player.Facing = Facings.Left;
            yield return (object)cs07Credits1.WaitForPlayer();
            yield return (object)cs07Credits1.FadeTo(1f);
            yield return (object)1f;
            cs07Credits1.SetBgFade(0.3f);
            yield return (object)cs07Credits1.NextLevel("credits-tree");
            yield return (object)cs07Credits1.SetupLevel();
            Petals petals = new Petals();
            cs07Credits1.Level.Foreground.Backdrops.Add((Backdrop)petals);
            cs07Credits1.autoUpdateCamera = false;
            Vector2 target1 = cs07Credits1.Level.Camera.Position + new Vector2(-220f, 32f);
            cs07Credits1.Level.Camera.Position += new Vector2(-100f, 0.0f);
            cs07Credits1.badelineWalkApproach = 1f;
            cs07Credits1.badelineAutoFloat = false;
            cs07Credits1.badelineAutoWalk = true;
            cs07Credits1.badeline.Floatness = 0.0f;
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            cs07Credits1.Add((Component)new Coroutine(CutsceneEntity.CameraTo(target1, 12f, Ease.Linear)));
            yield return (object)3.5f;
            cs07Credits1.badeline.Sprite.Play("idle");
            cs07Credits1.badelineAutoWalk = false;
            yield return (object)0.25f;
            cs07Credits1.autoWalk = false;
            cs07Credits1.player.Sprite.Play("idle");
            cs07Credits1.player.Speed = Vector2.Zero;
            cs07Credits1.player.DummyAutoAnimate = false;
            cs07Credits1.player.Facing = Facings.Right;
            yield return (object)0.5f;
            cs07Credits1.player.Sprite.Play("sitDown");
            yield return (object)4f;
            cs07Credits1.badeline.Sprite.Play("laugh");
            yield return (object)1.75f;
            yield return (object)cs07Credits1.FadeTo(1f);
            cs07Credits1.Level.Foreground.Backdrops.Remove((Backdrop)petals);
            petals = (Petals)null;
            yield return (object)1f;
            cs07Credits1.SetBgFade(0.4f);
            yield return (object)cs07Credits1.NextLevel("credits-clouds");
            yield return (object)cs07Credits1.SetupLevel();
            cs07Credits1.autoWalk = false;
            cs07Credits1.player.Speed = Vector2.Zero;
            cs07Credits1.autoUpdateCamera = false;
            cs07Credits1.player.ForceCameraUpdate = false;
            cs07Credits1.badeline.Visible = false;
            Player other = (Player)null;
            foreach (CreditsTrigger entity in cs07Credits1.Scene.Tracker.GetEntities<CreditsTrigger>())
            {
                if (entity.Event == "BadelineOffset")
                {
                    other = new Player(entity.Position, PlayerSpriteMode.Badeline);
                    other.OverrideHairColor = new Color?(BadelineOldsite.HairColor);
                    yield return (object)null;
                    other.StateMachine.State = 11;
                    other.Facing = Facings.Left;
                    cs07Credits1.Scene.Add((Entity)other);
                }
            }
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            cs07Credits1.Level.Camera.Position += new Vector2(0.0f, -100f);
            Vector2 target2 = cs07Credits1.Level.Camera.Position + new Vector2(0.0f, 160f);
            cs07Credits1.Add((Component)new Coroutine(CutsceneEntity.CameraTo(target2, 12f, Ease.Linear)));
            float playerHighJump = 0.0f;
            float baddyHighJump = 0.0f;
            for (float p = 0.0f; (double)p < 10.0; p += Engine.DeltaTime)
            {
                if (((double)p > 3.0 && (double)p < 6.0 || (double)p > 9.0) && (double)cs07Credits1.player.Speed.Y < 0.0 && cs07Credits1.player.OnGround(4))
                    playerHighJump = 0.25f;
                if ((double)p > 5.0 && (double)p < 8.0 && (double)other.Speed.Y < 0.0 && other.OnGround(4))
                    baddyHighJump = 0.25f;
                if ((double)playerHighJump > 0.0)
                {
                    playerHighJump -= Engine.DeltaTime;
                    cs07Credits1.player.Speed.Y = -200f;
                }
                if ((double)baddyHighJump > 0.0)
                {
                    baddyHighJump -= Engine.DeltaTime;
                    other.Speed.Y = -200f;
                }
                yield return (object)null;
            }
            yield return (object)cs07Credits1.FadeTo(1f);
            other = (Player)null;
            yield return (object)1f;
            CS07_Credits cs07Credits = cs07Credits1;
            cs07Credits1.SetBgFade(0.5f);
            yield return (object)cs07Credits1.NextLevel("credits-resort");
            yield return (object)cs07Credits1.SetupLevel();
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            cs07Credits1.badelineWalkApproach = 1f;
            cs07Credits1.badelineAutoFloat = false;
            cs07Credits1.badelineAutoWalk = true;
            cs07Credits1.badeline.Floatness = 0.0f;
            Vector2 vector2 = Vector2.Zero;
            foreach (CreditsTrigger creditsTrigger in cs07Credits1.Scene.Entities.FindAll<CreditsTrigger>())
            {
                if (creditsTrigger.Event == "Oshiro")
                    vector2 = creditsTrigger.Position;
            }
            NPC oshiro = new NPC(vector2 + new Vector2(0.0f, 4f));
            oshiro.Add((Component)(oshiro.Sprite = (Sprite)new OshiroSprite(1)));
            oshiro.MoveAnim = "sweeping";
            oshiro.IdleAnim = "sweeping";
            oshiro.Sprite.Play("sweeping");
            oshiro.Maxspeed = 10f;
            oshiro.Depth = -60;
            cs07Credits1.Scene.Add((Entity)oshiro);
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.DustyRoutine((Entity)oshiro)));
            yield return (object)4.8f;
            Vector2 oshiroTarget = oshiro.Position + new Vector2(116f, 0.0f);
            Coroutine oshiroRoutine = new Coroutine(oshiro.MoveTo(oshiroTarget));
            cs07Credits1.Add((Component)oshiroRoutine);
            yield return (object)2f;
            cs07Credits1.autoUpdateCamera = false;
            yield return (object)CutsceneEntity.CameraTo(new Vector2((float)(cs07Credits1.Level.Bounds.Left + 64 /*0x40*/), (float)cs07Credits1.Level.Bounds.Top), 2f);
            yield return (object)5f;
            BirdNPC bird = new BirdNPC(oshiro.Position + new Vector2(280f, -160f), BirdNPC.Modes.None);
            bird.Depth = 10010;
            bird.Light.Visible = false;
            cs07Credits1.Scene.Add((Entity)bird);
            bird.Facing = Facings.Left;
            bird.Sprite.Play("fall");
            Vector2 from = bird.Position;
            Vector2 to = oshiroTarget + new Vector2(50f, -12f);
            baddyHighJump = 0.0f;
            while ((double)baddyHighJump < 1.0)
            {
                bird.Position = from + (to - from) * Ease.QuadOut(baddyHighJump);
                if ((double)baddyHighJump > 0.5)
                {
                    bird.Sprite.Play("fly");
                    bird.Depth = -1000000;
                    bird.Light.Visible = true;
                }
                baddyHighJump += Engine.DeltaTime * 0.5f;
                yield return (object)null;
            }
            bird.Position = to;
            oshiroRoutine.RemoveSelf();
            oshiro.Sprite.Play("putBroomAway");
            oshiro.Sprite.OnFrameChange = (Action<string>)(anim =>
            {
                if (oshiro.Sprite.CurrentAnimationFrame != 10)
                    return;
                Entity entity = new Entity(oshiro.Position);
                entity.Depth = oshiro.Depth + 1;
                cs07Credits.Scene.Add(entity);
                entity.Add((Component)new Monocle.Image(GFX.Game["characters/oshiro/broom"])
                {
                    Origin = oshiro.Sprite.Origin
                });
                oshiro.Sprite.OnFrameChange = (Action<string>)null;
            });
            bird.Sprite.Play("idle");
            yield return (object)0.5f;
            bird.Sprite.Play("croak");
            yield return (object)0.6f;
            from = new Vector2();
            to = new Vector2();
            oshiro.Maxspeed = 40f;
            oshiro.MoveAnim = "move";
            oshiro.IdleAnim = "idle";
            yield return (object)oshiro.MoveTo(oshiroTarget + new Vector2(14f, 0.0f));
            yield return (object)2f;
            cs07Credits1.Add((Component)new Coroutine(bird.StartleAndFlyAway()));
            yield return (object)0.75f;
            bird.Light.Visible = false;
            bird.Depth = 10010;
            oshiro.Sprite.Scale.X = -1f;
            yield return (object)cs07Credits1.FadeTo(1f);
            oshiroTarget = new Vector2();
            oshiroRoutine = (Coroutine)null;
            bird = (BirdNPC)null;
            yield return (object)1f;
            cs07Credits1.SetBgFade(0.6f);
            yield return (object)cs07Credits1.NextLevel("credits-wallslide");
            yield return (object)cs07Credits1.SetupLevel();
            cs07Credits1.badelineAutoFloat = false;
            cs07Credits1.badeline.Floatness = 0.0f;
            cs07Credits1.badeline.Sprite.Play("idle");
            cs07Credits1.badeline.Sprite.Scale.X = 1f;
            foreach (CreditsTrigger entity in cs07Credits1.Scene.Tracker.GetEntities<CreditsTrigger>())
            {
                if (entity.Event == "BadelineOffset")
                    cs07Credits1.badeline.Position = entity.Position + new Vector2(8f, 16f);
            }
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.WaitForPlayer()));
            while ((double)cs07Credits1.player.X > (double)cs07Credits1.badeline.X - 16.0)
                yield return (object)null;
            cs07Credits1.badeline.Sprite.Scale.X = -1f;
            yield return (object)0.1f;
            cs07Credits1.badelineAutoWalk = true;
            cs07Credits1.badelineWalkApproachFrom = cs07Credits1.badeline.Position;
            cs07Credits1.badelineWalkApproach = 0.0f;
            cs07Credits1.badeline.Sprite.Play("walk");
            while ((double)cs07Credits1.badelineWalkApproach != 1.0)
            {
                cs07Credits1.badelineWalkApproach = Calc.Approach(cs07Credits1.badelineWalkApproach, 1f, Engine.DeltaTime * 4f);
                yield return (object)null;
            }
            while ((double)cs07Credits1.player.X > (double)(cs07Credits1.Level.Bounds.X + 160 /*0xA0*/))
                yield return (object)null;
            yield return (object)cs07Credits1.FadeTo(1f);
            yield return (object)1f;
            cs07Credits1.SetBgFade(0.7f);
            yield return (object)cs07Credits1.NextLevel("credits-payphone");
            yield return (object)cs07Credits1.SetupLevel();
            cs07Credits1.player.Speed = Vector2.Zero;
            cs07Credits1.player.Facing = Facings.Left;
            cs07Credits1.autoWalk = false;
            cs07Credits1.badeline.Sprite.Play("idle");
            cs07Credits1.badeline.Floatness = 0.0f;
            cs07Credits1.badeline.Y = cs07Credits1.player.Y;
            cs07Credits1.badeline.Sprite.Scale.X = 1f;
            cs07Credits1.badelineAutoFloat = false;
            cs07Credits1.autoUpdateCamera = false;
            cs07Credits1.Level.Camera.X += 100f;
            Vector2 target3 = cs07Credits1.Level.Camera.Position + new Vector2(-200f, 0.0f);
            cs07Credits1.Add((Component)new Coroutine(CutsceneEntity.CameraTo(target3, 14f, Ease.Linear)));
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            yield return (object)1.5f;
            cs07Credits1.badeline.Sprite.Scale.X = -1f;
            yield return (object)0.5f;
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.badeline.FloatTo(cs07Credits1.badeline.Position + new Vector2(16f, -12f), new int?(-1), false)));
            yield return (object)0.5f;
            cs07Credits1.player.Facing = Facings.Right;
            yield return (object)1.5f;
            oshiroTarget = cs07Credits1.badeline.Position;
            to = cs07Credits1.player.Center;
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.BadelineAround(oshiroTarget, to, cs07Credits1.badeline)));
            yield return (object)0.5f;
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.BadelineAround(oshiroTarget, to)));
            yield return (object)0.5f;
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.BadelineAround(oshiroTarget, to)));
            yield return (object)3f;
            cs07Credits1.badeline.Sprite.Play("laugh");
            yield return (object)0.5f;
            cs07Credits1.player.Facing = Facings.Left;
            yield return (object)0.5f;
            cs07Credits1.player.DummyAutoAnimate = false;
            cs07Credits1.player.Sprite.Play("sitDown");
            yield return (object)3f;
            yield return (object)cs07Credits1.FadeTo(1f);
            oshiroTarget = new Vector2();
            to = new Vector2();
            yield return (object)1f;
            cs07Credits1.SetBgFade(0.8f);
            yield return (object)cs07Credits1.NextLevel("credits-city");
            yield return (object)cs07Credits1.SetupLevel();
            BirdNPC first = cs07Credits1.Scene.Entities.FindFirst<BirdNPC>();
            if (first != null)
                first.Facing = Facings.Right;
            cs07Credits1.badelineWalkApproach = 1f;
            cs07Credits1.badelineAutoFloat = false;
            cs07Credits1.badelineAutoWalk = true;
            cs07Credits1.badeline.Floatness = 0.0f;
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            yield return (object)cs07Credits1.WaitForPlayer();
            yield return (object)cs07Credits1.FadeTo(1f);
            yield return (object)1f;
            cs07Credits1.SetBgFade(0.0f);
            yield return (object)cs07Credits1.NextLevel("credits-prologue");
            yield return (object)cs07Credits1.SetupLevel();
            cs07Credits1.badelineWalkApproach = 1f;
            cs07Credits1.badelineAutoFloat = false;
            cs07Credits1.badelineAutoWalk = true;
            cs07Credits1.badeline.Floatness = 0.0f;
            cs07Credits1.Add((Component)new Coroutine(cs07Credits1.FadeTo(0.0f)));
            yield return (object)cs07Credits1.WaitForPlayer();
            yield return (object)cs07Credits1.FadeTo(1f);
            while ((double)cs07Credits1.credits.BottomTimer < 2.0)
                yield return (object)null;
            if (!cs07Credits1.gotoEpilogue)
            {
                cs07Credits1.snow = new HiresSnow();
                cs07Credits1.snow.Alpha = 0.0f;
                // ISSUE: reference to a compiler-generated method
                this.snow.AttachAlphaTo = new FadeWipe(this.Level, false, delegate ()
                {
                    base.EndCutscene(this.Level, true);
                });
                this.Level.Add(this.Level.HiresSnow = this.snow);
            }
            else
            {
                new FadeWipe(this.Level, false, delegate ()
                {
                    base.EndCutscene(this.Level, true);
                });
            }
            yield break;
            yield break;
        }

        private IEnumerator SetupLevel()
        {
            CS07_Credits cs07Credits = this;
            cs07Credits.Level.SnapColorGrade("credits");
            cs07Credits.player = (Player)null;
            while ((cs07Credits.player = cs07Credits.Scene.Tracker.GetEntity<Player>()) == null)
                yield return (object)null;
            cs07Credits.Level.Add((Entity)(cs07Credits.badeline = new BadelineDummy(cs07Credits.player.Position + new Vector2(16f, -16f))));
            cs07Credits.badeline.Floatness = 4f;
            cs07Credits.badelineAutoFloat = true;
            cs07Credits.badelineAutoWalk = false;
            cs07Credits.badelineWalkApproach = 0.0f;
            cs07Credits.Level.Session.Inventory.Dashes = 1;
            cs07Credits.player.Dashes = 1;
            cs07Credits.player.StateMachine.State = 11;
            cs07Credits.player.DummyFriction = false;
            cs07Credits.player.DummyMaxspeed = false;
            cs07Credits.player.Facing = Facings.Left;
            cs07Credits.autoWalk = true;
            cs07Credits.autoUpdateCamera = true;
            cs07Credits.Level.CameraOffset.X = 70f;
            cs07Credits.Level.CameraOffset.Y = -24f;
            cs07Credits.Level.Camera.Position = cs07Credits.player.CameraTarget;
        }

        private IEnumerator WaitForPlayer()
        {
            CS07_Credits cs07Credits = this;
            while ((double)cs07Credits.player.X > (double)(cs07Credits.Level.Bounds.X + 160 /*0xA0*/))
            {
                if (cs07Credits.Event != null)
                    yield return (object)cs07Credits.DoEvent(cs07Credits.Event);
                cs07Credits.Event = (string)null;
                yield return (object)null;
            }
        }

        private IEnumerator NextLevel(string name)
        {
            CS07_Credits cs07Credits = this;
            if (cs07Credits.player != null)
                cs07Credits.player.RemoveSelf();
            cs07Credits.player = (Player)null;
            cs07Credits.Level.OnEndOfFrame += (Action)(() =>
            {
                this.Level.UnloadLevel();
                this.Level.Session.Level = name;
                Session session = this.Level.Session;
                Level level = this.Level;
                Rectangle bounds = this.Level.Bounds;
                double left = (double)bounds.Left;
                bounds = this.Level.Bounds;
                double top = (double)bounds.Top;
                Vector2 from = new Vector2((float)left, (float)top);
                Vector2? nullable = new Vector2?(level.GetSpawnPoint(from));
                session.RespawnPoint = nullable;
                this.Level.LoadLevel(Player.IntroTypes.None);
                this.Level.Wipe.Cancel();
            });
            yield return (object)null;
            yield return (object)null;
        }

        private IEnumerator FadeTo(float value)
        {
            while ((double)(this.fade = Calc.Approach(this.fade, value, Engine.DeltaTime * 0.5f)) != (double)value)
                yield return (object)null;
            this.fade = value;
        }

        private IEnumerator BadelineApproachWalking()
        {
            while ((double)this.badelineWalkApproach < 1.0)
            {
                this.badeline.Floatness = Calc.Approach(this.badeline.Floatness, 0.0f, Engine.DeltaTime * 8f);
                this.badelineWalkApproach = Calc.Approach(this.badelineWalkApproach, 1f, Engine.DeltaTime * 0.6f);
                yield return (object)null;
            }
        }

        private IEnumerator DustyRoutine(Entity oshiro)
        {
            CS07_Credits cs07Credits = this;
            List<Entity> dusty = new List<Entity>();
            float timer = 0.0f;
            Vector2 offset = oshiro.Position + new Vector2(220f, -24f);
            Vector2 start = offset;
            for (int index = 0; index < 3; ++index)
            {
                Entity entity = new Entity(offset + new Vector2((float)(index * 24), 0.0f))
                {
                    Depth = -50
                };
                entity.Add((Component)new DustGraphic(true, autoExpandDust: true));
                Monocle.Image image = new Monocle.Image(GFX.Game["decals/3-resort/brokenbox_" + ((char)(97 + index)).ToString()]);
                image.JustifyOrigin(0.5f, 1f);
                image.Position = new Vector2(0.0f, -4f);
                entity.Add((Component)image);
                cs07Credits.Scene.Add(entity);
                dusty.Add(entity);
            }
            yield return (object)3.8f;
            while (true)
            {
                for (int index = 0; index < dusty.Count; ++index)
                {
                    Entity entity = dusty[index];
                    entity.X = offset.X + (float)(index * 24);
                    entity.Y = offset.Y + (float)Math.Sin((double)timer * 4.0 + (double)index * 0.800000011920929) * 4f;
                }
                if ((double)offset.X < (double)(cs07Credits.Level.Bounds.Left + 120))
                    offset.Y = Calc.Approach(offset.Y, start.Y + 16f, Engine.DeltaTime * 16f);
                offset.X -= 26f * Engine.DeltaTime;
                timer += Engine.DeltaTime;
                yield return (object)null;
            }
        }

        private IEnumerator BadelineAround(Vector2 start, Vector2 around, BadelineDummy badeline = null)
        {
            CS07_Credits cs07Credits = this;
            bool removeAtEnd = badeline == null;
            if (badeline == null)
                cs07Credits.Scene.Add((Entity)(badeline = new BadelineDummy(start)));
            badeline.Sprite.Play("fallSlow");
            float angle = Calc.Angle(around, start);
            float dist = (around - start).Length();
            float duration = 3f;
            for (float p = 0.0f; (double)p < 1.0; p += Engine.DeltaTime / duration)
            {
                badeline.Position = around + Calc.AngleToVector(angle - p * 2f * 6.28318548f, (float)((double)dist + (double)Calc.YoYo(p) * 16.0 + Math.Sin((double)p * 6.2831854820251465 * 4.0) * 5.0));
                badeline.Sprite.Scale.X = (float)Math.Sign(around.X - badeline.X);
                if (!removeAtEnd)
                    cs07Credits.player.Facing = (Facings)Math.Sign(badeline.X - cs07Credits.player.X);
                if (cs07Credits.Scene.OnInterval(0.1f))
                    TrailManager.Add((Entity)badeline, Player.NormalHairColor);
                yield return (object)null;
            }
            if (removeAtEnd)
                badeline.Vanish();
            else
                badeline.Sprite.Play("laugh");
        }

        private IEnumerator DoEvent(string e)
        {
            switch (e)
            {
                case "WaitJumpDash":
                    yield return (object)this.EventWaitJumpDash();
                    break;
                case "WaitJumpDoubleDash":
                    yield return (object)this.EventWaitJumpDoubleDash();
                    break;
                case "ClimbDown":
                    yield return (object)this.EventClimbDown();
                    break;
                case "Wait":
                    yield return (object)this.EventWait();
                    break;
            }
        }

        private IEnumerator EventWaitJumpDash()
        {
            this.autoWalk = false;
            this.player.DummyFriction = true;
            yield return (object)0.1f;
            this.PlayerJump(-1);
            yield return (object)0.2f;
            this.player.OverrideDashDirection = new Vector2?(new Vector2(-1f, -1f));
            this.player.StateMachine.State = this.player.StartDash();
            yield return (object)0.6f;
            this.player.OverrideDashDirection = new Vector2?();
            this.player.StateMachine.State = 11;
            this.autoWalk = true;
        }

        private IEnumerator EventWaitJumpDoubleDash()
        {
            CS07_Credits cs07Credits = this;
            cs07Credits.autoWalk = false;
            cs07Credits.player.DummyFriction = true;
            yield return (object)0.1f;
            cs07Credits.player.Facing = Facings.Right;
            yield return (object)0.25f;
            yield return (object)cs07Credits.BadelineCombine();
            cs07Credits.player.Dashes = 2;
            yield return (object)0.5f;
            cs07Credits.player.Facing = Facings.Left;
            yield return (object)0.7f;
            cs07Credits.PlayerJump(-1);
            yield return (object)0.4f;
            cs07Credits.player.OverrideDashDirection = new Vector2?(new Vector2(-1f, -1f));
            cs07Credits.player.StateMachine.State = cs07Credits.player.StartDash();
            yield return (object)0.6f;
            cs07Credits.player.OverrideDashDirection = new Vector2?(new Vector2(-1f, 0.0f));
            cs07Credits.player.StateMachine.State = cs07Credits.player.StartDash();
            yield return (object)0.6f;
            cs07Credits.player.OverrideDashDirection = new Vector2?();
            cs07Credits.player.StateMachine.State = 11;
            cs07Credits.autoWalk = true;
            while (!cs07Credits.player.OnGround())
                yield return (object)null;
            cs07Credits.autoWalk = false;
            cs07Credits.player.DummyFriction = true;
            cs07Credits.player.Dashes = 2;
            yield return (object)0.5f;
            cs07Credits.player.Facing = Facings.Right;
            yield return (object)1f;
            cs07Credits.Level.Displacement.AddBurst(cs07Credits.player.Position, 0.4f, 8f, 32f, 0.5f);
            cs07Credits.badeline.Position = cs07Credits.player.Position;
            cs07Credits.badeline.Visible = true;
            cs07Credits.badelineAutoFloat = true;
            cs07Credits.player.Dashes = 1;
            yield return (object)0.8f;
            cs07Credits.player.Facing = Facings.Left;
            cs07Credits.autoWalk = true;
            cs07Credits.player.DummyFriction = false;
        }

        private IEnumerator EventClimbDown()
        {
            this.autoWalk = false;
            this.player.DummyFriction = true;
            yield return (object)0.1f;
            this.PlayerJump(-1);
            yield return (object)0.4f;
            while (!this.player.CollideCheck<Solid>(this.player.Position + new Vector2(-1f, 0.0f)))
                yield return (object)null;
            this.player.DummyAutoAnimate = false;
            this.player.Sprite.Play("wallslide");
            while (this.player.CollideCheck<Solid>(this.player.Position + new Vector2(-1f, 32f)))
            {
                this.player.CreateWallSlideParticles(-1);
                this.player.Speed.Y = Math.Min(this.player.Speed.Y, 40f);
                yield return (object)null;
            }
            this.PlayerJump(1);
            yield return (object)0.4f;
            while (!this.player.CollideCheck<Solid>(this.player.Position + new Vector2(1f, 0.0f)))
                yield return (object)null;
            this.player.DummyAutoAnimate = false;
            this.player.Sprite.Play("wallslide");
            while (!this.player.CollideCheck<Solid>(this.player.Position + new Vector2(0.0f, 32f)))
            {
                this.player.CreateWallSlideParticles(1);
                this.player.Speed.Y = Math.Min(this.player.Speed.Y, 40f);
                yield return (object)null;
            }
            this.PlayerJump(-1);
            yield return (object)0.4f;
            this.autoWalk = true;
        }

        private IEnumerator EventWait()
        {
            CS07_Credits cs07Credits = this;
            cs07Credits.badeline.Sprite.Play("idle");
            cs07Credits.badelineAutoWalk = false;
            cs07Credits.autoWalk = false;
            cs07Credits.player.DummyFriction = true;
            yield return (object)0.1f;
            cs07Credits.player.DummyAutoAnimate = false;
            cs07Credits.player.Speed = Vector2.Zero;
            yield return (object)0.5f;
            cs07Credits.player.Sprite.Play("lookUp");
            yield return (object)2f;
            BirdNPC first = cs07Credits.Scene.Entities.FindFirst<BirdNPC>();
            if (first != null)
                first.AutoFly = true;
            yield return (object)0.1f;
            cs07Credits.player.Sprite.Play("idle");
            yield return (object)1f;
            cs07Credits.autoWalk = true;
            cs07Credits.player.DummyFriction = false;
            cs07Credits.player.DummyAutoAnimate = true;
            cs07Credits.badelineAutoWalk = true;
            cs07Credits.badelineWalkApproach = 0.0f;
            cs07Credits.badelineWalkApproachFrom = cs07Credits.badeline.Position;
            cs07Credits.badeline.Sprite.Play("walk");
            while ((double)cs07Credits.badelineWalkApproach < 1.0)
            {
                cs07Credits.badelineWalkApproach += Engine.DeltaTime * 4f;
                yield return (object)null;
            }
        }

        private IEnumerator BadelineCombine()
        {
            CS07_Credits cs07Credits = this;
            Vector2 from = cs07Credits.badeline.Position;
            cs07Credits.badelineAutoFloat = false;
            for (float p = 0.0f; (double)p < 1.0; p += Engine.DeltaTime / 0.25f)
            {
                cs07Credits.badeline.Position = Vector2.Lerp(from, cs07Credits.player.Position, Ease.CubeIn(p));
                yield return (object)null;
            }
            cs07Credits.badeline.Visible = false;
            cs07Credits.Level.Displacement.AddBurst(cs07Credits.player.Position, 0.4f, 8f, 32f, 0.5f);
        }

        private void PlayerJump(int direction)
        {
            this.player.Facing = (Facings)direction;
            this.player.DummyFriction = false;
            this.player.DummyAutoAnimate = true;
            this.player.Speed.X = (float)(direction * 120);
            this.player.Jump();
            this.player.AutoJump = true;
            this.player.AutoJumpTimer = 2f;
        }

        private void SetBgFade(float alpha) => this.fillbg.Color = Color.Black * alpha;

        public override void Update()
        {
            MInput.Disabled = false;
            if (this.Level.CanPause && (Input.Pause.Pressed || Input.ESC.Pressed))
            {
                Input.Pause.ConsumeBuffer();
                Input.ESC.ConsumeBuffer();
                this.Level.Pause(minimal: true);
            }
            MInput.Disabled = true;
            if (this.player != null && this.player.Scene != null)
            {
                if (this.player.OverrideDashDirection.HasValue)
                {
                    Input.MoveX.Value = (int)this.player.OverrideDashDirection.Value.X;
                    Input.MoveY.Value = (int)this.player.OverrideDashDirection.Value.Y;
                }
                if (this.autoWalk)
                {
                    if (this.player.OnGround())
                    {
                        this.player.Speed.X = -44.8f;
                        bool flag1 = this.player.CollideCheck<Solid>(this.player.Position + new Vector2(-20f, 0.0f));
                        bool flag2 = !this.player.CollideCheck<Solid>(this.player.Position + new Vector2(-8f, 1f)) && !this.player.CollideCheck<Solid>(this.player.Position + new Vector2(-8f, 32f));
                        if (flag1 | flag2)
                        {
                            this.player.Jump();
                            this.player.AutoJump = true;
                            this.player.AutoJumpTimer = flag1 ? 0.6f : 2f;
                        }
                    }
                    else
                        this.player.Speed.X = -64f;
                }
                if (this.badeline != null && this.badelineAutoFloat)
                {
                    Vector2 position = this.badeline.Position;
                    Vector2 vector2 = this.player.Position + new Vector2(16f, -16f);
                    this.badeline.Position = position + (vector2 - position) * (1f - (float)Math.Pow(0.0099999997764825821, (double)Engine.DeltaTime));
                    this.badeline.Sprite.Scale.X = -1f;
                }
                if (this.badeline != null && this.badelineAutoWalk)
                {
                    Player.ChaserState chaseState;
                    this.player.GetChasePosition(this.Scene.TimeActive, (float)(0.34999999403953552 + Math.Sin((double)this.walkOffset) * 0.10000000149011612), out chaseState);
                    if (chaseState.OnGround)
                        this.walkOffset += Engine.DeltaTime;
                    if ((double)this.badelineWalkApproach >= 1.0)
                    {
                        this.badeline.Position = chaseState.Position;
                        if (this.badeline.Sprite.Has(chaseState.Animation))
                            this.badeline.Sprite.Play(chaseState.Animation);
                        this.badeline.Sprite.Scale.X = (float)chaseState.Facing;
                    }
                    else
                        this.badeline.Position = Vector2.Lerp(this.badelineWalkApproachFrom, chaseState.Position, this.badelineWalkApproach);
                }
                if ((double)Math.Abs(this.player.Speed.X) > 90.0)
                    this.player.Speed.X = Calc.Approach(this.player.Speed.X, 90f * (float)Math.Sign(this.player.Speed.X), 1000f * Engine.DeltaTime);
            }
            if (this.credits != null)
                this.credits.Update();
            base.Update();
        }

        public void PostUpdate()
        {
            if (this.player == null || this.player.Scene == null || !this.autoUpdateCamera)
                return;
            Vector2 position = this.Level.Camera.Position;
            Vector2 cameraTarget = this.player.CameraTarget;
            if (!this.player.OnGround())
                cameraTarget.Y = (float)(((double)this.Level.Camera.Y * 2.0 + (double)cameraTarget.Y) / 3.0);
            this.Level.Camera.Position = position + (cameraTarget - position) * (1f - (float)Math.Pow(0.0099999997764825821, (double)Engine.DeltaTime));
            this.Level.Camera.X = (float)(int)cameraTarget.X;
        }

        public override void Render()
        {
            bool flag = SaveData.Instance != null && SaveData.Instance.Assists.MirrorMode;
            if (!this.Level.Paused)
            {
                if (flag)
                    this.gradient.Draw(new Vector2(1720f, -10f), Vector2.Zero, Color.White * 0.6f, new Vector2(-1f, 1100f));
                else
                    this.gradient.Draw(new Vector2(200f, -10f), Vector2.Zero, Color.White * 0.6f, new Vector2(1f, 1100f));
            }
            if ((double)this.fade > 0.0)
                Draw.Rect(-10f, -10f, 1940f, 1100f, Color.Black * Ease.CubeInOut(this.fade));
            if (this.credits != null && !this.Level.Paused)
                this.credits.Render(new Vector2(flag ? 100f : 1820f, 0.0f));
            base.Render();
        }

        public override void OnEnd(Level level)
        {
            SaveData.Instance.Assists.DashAssist = this.wasDashAssistOn;
            Audio.BusMuted("bus:/gameplay_sfx", new bool?(false));
            CS07_Credits.Instance = (CS07_Credits)null;
            MInput.Disabled = false;
            if (!this.gotoEpilogue)
                Engine.Scene = (Scene)new OverworldLoader(Overworld.StartMode.AreaComplete, this.snow);
            else
                LevelEnter.Go(new Session(new AreaKey(8)), false);
        }

        private class Fill : Backdrop
        {
            public override void Render(Scene scene) => Draw.Rect(-10f, -10f, 340f, 200f, this.Color);
        }
    }
}