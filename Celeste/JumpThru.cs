// Decompiled with JetBrains decompiler
// Type: Celeste.JumpThru
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste
{

    [Tracked(true)]
    public class JumpThru : Platform
    {
      public JumpThru(Vector2 position, int width, bool safe)
        : base(position, safe)
      {
        this.Collider = (Collider) new Hitbox((float) width, 5f);
      }

      public override void Awake(Scene scene)
      {
        base.Awake(scene);
        foreach (StaticMover component in scene.Tracker.GetComponents<StaticMover>())
        {
          if (component.IsRiding(this) && component.Platform == null)
          {
            this.staticMovers.Add(component);
            component.Platform = (Platform) this;
            if (component.OnAttach != null)
              component.OnAttach((Platform) this);
          }
        }
      }

      public bool HasRider()
      {
        foreach (Actor entity in this.Scene.Tracker.GetEntities<Actor>())
        {
          if (entity.IsRiding(this))
            return true;
        }
        return false;
      }

      public bool HasPlayerRider()
      {
        foreach (Actor entity in this.Scene.Tracker.GetEntities<Player>())
        {
          if (entity.IsRiding(this))
            return true;
        }
        return false;
      }

      public Player GetPlayerRider()
      {
        foreach (Player entity in this.Scene.Tracker.GetEntities<Player>())
        {
          if (entity.IsRiding(this))
            return entity;
        }
        return (Player) null;
      }

      public override void MoveHExact(int move)
      {
        if (this.Collidable)
        {
          foreach (Actor entity in this.Scene.Tracker.GetEntities<Actor>())
          {
            if (entity.IsRiding(this))
            {
              if (entity.TreatNaive)
                entity.NaiveMove(Vector2.UnitX * (float) move);
              else
                entity.MoveHExact(move);
            }
          }
        }
        this.X += (float) move;
        this.MoveStaticMovers(Vector2.UnitX * (float) move);
      }

      public override void MoveVExact(int move)
      {
        if (this.Collidable)
        {
          if (move < 0)
          {
            foreach (Actor entity in this.Scene.Tracker.GetEntities<Actor>())
            {
              if (entity.IsRiding(this))
              {
                this.Collidable = false;
                if (entity.TreatNaive)
                  entity.NaiveMove(Vector2.UnitY * (float) move);
                else
                  entity.MoveVExact(move);
                entity.LiftSpeed = this.LiftSpeed;
                this.Collidable = true;
              }
              else if (!entity.TreatNaive && this.CollideCheck((Entity) entity, this.Position + Vector2.UnitY * (float) move) && !this.CollideCheck((Entity) entity))
              {
                this.Collidable = false;
                entity.MoveVExact((int) ((double) this.Top + (double) move - (double) entity.Bottom));
                entity.LiftSpeed = this.LiftSpeed;
                this.Collidable = true;
              }
            }
          }
          else
          {
            foreach (Actor entity in this.Scene.Tracker.GetEntities<Actor>())
            {
              if (entity.IsRiding(this))
              {
                this.Collidable = false;
                if (entity.TreatNaive)
                  entity.NaiveMove(Vector2.UnitY * (float) move);
                else
                  entity.MoveVExact(move);
                entity.LiftSpeed = this.LiftSpeed;
                this.Collidable = true;
              }
            }
          }
        }
        this.Y += (float) move;
        this.MoveStaticMovers(Vector2.UnitY * (float) move);
      }
    }
}
