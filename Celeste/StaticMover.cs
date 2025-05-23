// Decompiled with JetBrains decompiler
// Type: Celeste.StaticMover
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    [Tracked(false)]
    public class StaticMover : Component
    {
      public Func<Solid, bool> SolidChecker;
      public Func<JumpThru, bool> JumpThruChecker;
      public Action<Vector2> OnMove;
      public Action<Vector2> OnShake;
      public Action<Platform> OnAttach;
      public Action OnDestroy;
      public Action OnDisable;
      public Action OnEnable;
      public Platform Platform;

      public StaticMover()
        : base(false, false)
      {
      }

      public void Destroy()
      {
        if (this.OnDestroy != null)
          this.OnDestroy();
        else
          this.Entity.RemoveSelf();
      }

      public void Shake(Vector2 amount)
      {
        if (this.OnShake == null)
          return;
        this.OnShake(amount);
      }

      public void Move(Vector2 amount)
      {
        if (this.OnMove != null)
          this.OnMove(amount);
        else
          this.Entity.Position += amount;
      }

      public bool IsRiding(Solid solid) => this.SolidChecker != null && this.SolidChecker(solid);

      public bool IsRiding(JumpThru jumpThru)
      {
        return this.JumpThruChecker != null && this.JumpThruChecker(jumpThru);
      }

      public void TriggerPlatform()
      {
        if (this.Platform == null)
          return;
        this.Platform.OnStaticMoverTrigger(this);
      }

      public void Disable()
      {
        if (this.OnDisable != null)
          this.OnDisable();
        else
          this.Entity.Active = this.Entity.Visible = this.Entity.Collidable = false;
      }

      public void Enable()
      {
        if (this.OnEnable != null)
          this.OnEnable();
        else
          this.Entity.Active = this.Entity.Visible = this.Entity.Collidable = true;
      }
    }
}
