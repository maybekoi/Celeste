// Decompiled with JetBrains decompiler
// Type: Celeste.Trigger
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    [Tracked(true)]
    public abstract class Trigger : Entity
    {
      public bool Triggered;

      public bool PlayerIsInside { get; private set; }

      public Trigger(EntityData data, Vector2 offset)
        : base(data.Position + offset)
      {
        this.Collider = (Collider) new Hitbox((float) data.Width, (float) data.Height);
        this.Visible = false;
      }

      public virtual void OnEnter(Player player) => this.PlayerIsInside = true;

      public virtual void OnStay(Player player)
      {
      }

      public virtual void OnLeave(Player player) => this.PlayerIsInside = false;

      protected float GetPositionLerp(Player player, Trigger.PositionModes mode)
      {
        switch (mode)
        {
          case Trigger.PositionModes.HorizontalCenter:
            return Math.Min(Calc.ClampedMap(player.CenterX, this.Left, this.CenterX), Calc.ClampedMap(player.CenterX, this.Right, this.CenterX));
          case Trigger.PositionModes.VerticalCenter:
            return Math.Min(Calc.ClampedMap(player.CenterY, this.Top, this.CenterY), Calc.ClampedMap(player.CenterY, this.Bottom, this.CenterY));
          case Trigger.PositionModes.TopToBottom:
            return Calc.ClampedMap(player.CenterY, this.Top, this.Bottom);
          case Trigger.PositionModes.BottomToTop:
            return Calc.ClampedMap(player.CenterY, this.Bottom, this.Top);
          case Trigger.PositionModes.LeftToRight:
            return Calc.ClampedMap(player.CenterX, this.Left, this.Right);
          case Trigger.PositionModes.RightToLeft:
            return Calc.ClampedMap(player.CenterX, this.Right, this.Left);
          default:
            return 1f;
        }
      }

      public enum PositionModes
      {
        NoEffect,
        HorizontalCenter,
        VerticalCenter,
        TopToBottom,
        BottomToTop,
        LeftToRight,
        RightToLeft,
      }
    }
}
