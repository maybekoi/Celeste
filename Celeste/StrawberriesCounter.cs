// Decompiled with JetBrains decompiler
// Type: Celeste.StrawberriesCounter
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste
{

    public class StrawberriesCounter : Component
    {
      public static readonly Color FlashColor = Calc.HexToColor("FF5E76");
      private const int IconWidth = 60;
      public bool Golden;
      public Vector2 Position;
      public bool CenteredX;
      public bool CanWiggle = true;
      public float Scale = 1f;
      public float Stroke = 2f;
      public float Rotation;
      public Color Color = Color.White;
      public Color OutOfColor = Color.LightGray;
      public bool OverworldSfx;
      private int amount;
      private int outOf = -1;
      private Wiggler wiggler;
      private float flashTimer;
      private string sAmount;
      private string sOutOf;
      private MTexture x;
      private bool showOutOf;

      public StrawberriesCounter(bool centeredX, int amount, int outOf = 0, bool showOutOf = false)
        : base(true, true)
      {
        this.CenteredX = centeredX;
        this.amount = amount;
        this.outOf = outOf;
        this.showOutOf = showOutOf;
        this.UpdateStrings();
        this.wiggler = Wiggler.Create(0.5f, 3f);
        this.wiggler.StartZero = true;
        this.wiggler.UseRawDeltaTime = true;
        this.x = GFX.Gui[nameof (x)];
      }

      public int Amount
      {
        get => this.amount;
        set
        {
          if (this.amount == value)
            return;
          this.amount = value;
          this.UpdateStrings();
          if (!this.CanWiggle)
            return;
          if (this.OverworldSfx)
            Audio.Play(this.Golden ? "event:/ui/postgame/goldberry_count" : "event:/ui/postgame/strawberry_count");
          else
            Audio.Play("event:/ui/game/increment_strawberry");
          this.wiggler.Start();
          this.flashTimer = 0.5f;
        }
      }

      public int OutOf
      {
        get => this.outOf;
        set
        {
          this.outOf = value;
          this.UpdateStrings();
        }
      }

      public bool ShowOutOf
      {
        get => this.showOutOf;
        set
        {
          if (this.showOutOf == value)
            return;
          this.showOutOf = value;
          this.UpdateStrings();
        }
      }

      public float FullHeight
      {
        get => Math.Max(ActiveFont.LineHeight, (float) GFX.Gui["collectables/strawberry"].Height);
      }

      private void UpdateStrings()
      {
        this.sAmount = this.amount.ToString();
        if (this.outOf > -1)
          this.sOutOf = "/" + this.outOf.ToString();
        else
          this.sOutOf = "";
      }

      public void Wiggle()
      {
        this.wiggler.Start();
        this.flashTimer = 0.5f;
      }

      public override void Update()
      {
        base.Update();
        if (this.wiggler.Active)
          this.wiggler.Update();
        if ((double) this.flashTimer <= 0.0)
          return;
        this.flashTimer -= Engine.RawDeltaTime;
      }

      public override void Render()
      {
        Vector2 renderPosition = this.RenderPosition;
        Vector2 vector = Calc.AngleToVector(this.Rotation, 1f);
        Vector2 vector2 = new Vector2(-vector.Y, vector.X);
        string sOutOf = this.showOutOf ? this.sOutOf : "";
        float x1 = ActiveFont.Measure(this.sAmount).X;
        float x2 = ActiveFont.Measure(sOutOf).X;
        float num = (float) (62.0 + (double) this.x.Width + 2.0) + x1 + x2;
        Color color = this.Color;
        if ((double) this.flashTimer > 0.0 && this.Scene != null && this.Scene.BetweenRawInterval(0.05f))
          color = StrawberriesCounter.FlashColor;
        if (this.CenteredX)
          renderPosition -= vector * (num / 2f) * this.Scale;
        string id = this.Golden ? "collectables/goldberry" : "collectables/strawberry";
        GFX.Gui[id].DrawCentered(renderPosition + vector * 60f * 0.5f * this.Scale, Color.White, this.Scale);
        this.x.DrawCentered(renderPosition + vector * (float) (62.0 + (double) this.x.Width * 0.5) * this.Scale + vector2 * 2f * this.Scale, color, this.Scale);
        ActiveFont.DrawOutline(this.sAmount, renderPosition + vector * (float) ((double) num - (double) x2 - (double) x1 * 0.5) * this.Scale + vector2 * (this.wiggler.Value * 18f) * this.Scale, new Vector2(0.5f, 0.5f), Vector2.One * this.Scale, color, this.Stroke, Color.Black);
        if (!(sOutOf != ""))
          return;
        ActiveFont.DrawOutline(sOutOf, renderPosition + vector * (num - x2 / 2f) * this.Scale, new Vector2(0.5f, 0.5f), Vector2.One * this.Scale, this.OutOfColor, this.Stroke, Color.Black);
      }

      public Vector2 RenderPosition
      {
        get => ((this.Entity != null ? this.Entity.Position : Vector2.Zero) + this.Position).Round();
      }
    }
}
