// Decompiled with JetBrains decompiler
// Type: Celeste.ClutterDoor
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;

namespace Celeste
{

    public class ClutterDoor : Solid
    {
      public ClutterBlock.Colors Color;
      private Sprite sprite;
      private Wiggler wiggler;

      public ClutterDoor(EntityData data, Vector2 offset, Session session)
        : base(data.Position + offset, (float) data.Width, (float) data.Height, false)
      {
        this.Color = data.Enum<ClutterBlock.Colors>("type", ClutterBlock.Colors.Green);
        this.SurfaceSoundIndex = 20;
        this.Tag = (int) Tags.TransitionUpdate;
        this.Add((Component) (this.sprite = GFX.SpriteBank.Create("ghost_door")));
        this.sprite.Position = new Vector2(this.Width, this.Height) / 2f;
        this.sprite.Play("idle");
        this.OnDashCollide = new DashCollision(this.OnDashed);
        this.Add((Component) (this.wiggler = Wiggler.Create(0.6f, 3f, (Action<float>) (f => this.sprite.Scale = Vector2.One * (float) (1.0 - (double) f * 0.20000000298023224)))));
        if (this.IsLocked(session))
          return;
        this.InstantUnlock();
      }

      public override void Update()
      {
        Level scene = this.Scene as Level;
        if (scene.Transitioning && this.CollideCheck<Player>())
        {
          this.Visible = false;
          this.Collidable = false;
        }
        else if (!this.Collidable && this.IsLocked(scene.Session) && !this.CollideCheck<Player>())
        {
          this.Visible = true;
          this.Collidable = true;
          this.wiggler.Start();
          Audio.Play("event:/game/03_resort/forcefield_bump", this.Position);
        }
        base.Update();
      }

      public bool IsLocked(Session session)
      {
        return !session.GetFlag("oshiro_clutter_door_open") || this.IsComplete(session);
      }

      public bool IsComplete(Session session)
      {
        return session.GetFlag("oshiro_clutter_cleared_" + (object) (int) this.Color);
      }

      public IEnumerator UnlockRoutine()
      {
        ClutterDoor clutterDoor = this;
        Camera camera = clutterDoor.SceneAs<Level>().Camera;
        Vector2 from = camera.Position;
        Vector2 to = clutterDoor.CameraTarget();
        float p;
        if ((double) (from - to).Length() > 8.0)
        {
          for (p = 0.0f; (double) p < 1.0; p += Engine.DeltaTime)
          {
            camera.Position = from + (to - from) * Ease.CubeInOut(p);
            yield return (object) null;
          }
        }
        else
          yield return (object) 0.2f;
        Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
        Audio.Play("event:/game/03_resort/forcefield_vanish", clutterDoor.Position);
        clutterDoor.sprite.Play("open");
        clutterDoor.Collidable = false;
        for (p = 0.0f; (double) p < 0.40000000596046448; p += Engine.DeltaTime)
        {
          camera.Position = clutterDoor.CameraTarget();
          yield return (object) null;
        }
      }

      public void InstantUnlock() => this.Visible = this.Collidable = false;

      private Vector2 CameraTarget()
      {
        Level level = this.SceneAs<Level>();
        Vector2 vector2 = this.Position - new Vector2(320f, 180f) / 2f;
        ref Vector2 local1 = ref vector2;
        double x = (double) vector2.X;
        Rectangle bounds1 = level.Bounds;
        double left = (double) bounds1.Left;
        bounds1 = level.Bounds;
        double max1 = (double) (bounds1.Right - 320);
        double num1 = (double) MathHelper.Clamp((float) x, (float) left, (float) max1);
        local1.X = (float) num1;
        ref Vector2 local2 = ref vector2;
        double y = (double) vector2.Y;
        Rectangle bounds2 = level.Bounds;
        double top = (double) bounds2.Top;
        bounds2 = level.Bounds;
        double max2 = (double) (bounds2.Bottom - 180);
        double num2 = (double) MathHelper.Clamp((float) y, (float) top, (float) max2);
        local2.Y = (float) num2;
        return vector2;
      }

      private DashCollisionResults OnDashed(Player player, Vector2 direction)
      {
        this.wiggler.Start();
        Audio.Play("event:/game/03_resort/forcefield_bump", this.Position);
        return DashCollisionResults.Bounce;
      }
    }
}
