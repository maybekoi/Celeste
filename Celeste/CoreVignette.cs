// Decompiled with JetBrains decompiler
// Type: Celeste.CoreVignette
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;
using System.Collections;

namespace Celeste
{

    public class CoreVignette : Scene
    {
      private Session session;
      private Coroutine textCoroutine;
      private FancyText.Text text;
      private int textStart;
      private float textAlpha;
      private HiresSnow snow;
      private HudRenderer hud;
      private TextMenu menu;
      private float fade;
      private float pauseFade;
      private bool started;
      private bool exiting;

      public bool CanPause => this.menu == null;

      public CoreVignette(Session session, HiresSnow snow = null)
      {
        this.session = session;
        if (snow == null)
          snow = new HiresSnow();
        this.Add((Monocle.Renderer) (this.hud = new HudRenderer()));
        this.Add((Monocle.Renderer) (this.snow = snow));
        this.RendererList.UpdateLists();
        this.text = FancyText.Parse(Dialog.Get("APP_INTRO"), 960, 8, 0.0f);
        this.textCoroutine = new Coroutine(this.TextSequence());
      }

      private IEnumerator TextSequence()
      {
        yield return (object) 1f;
        while (this.textStart < this.text.Count)
        {
          this.textAlpha = 1f;
          float fadeTimePerCharacter = 1f / (float) this.text.GetCharactersOnPage(this.textStart);
          for (int i = this.textStart; i < this.text.Count && !(this.text[i] is FancyText.NewPage); ++i)
          {
            if (this.text[i] is FancyText.Char c)
            {
              while ((double) (c.Fade += Engine.DeltaTime / fadeTimePerCharacter) < 1.0)
                yield return (object) null;
              c.Fade = 1f;
              c = (FancyText.Char) null;
            }
          }
          yield return (object) 2.5f;
          while ((double) this.textAlpha > 0.0)
          {
            this.textAlpha -= 1f * Engine.DeltaTime;
            yield return (object) null;
          }
          this.textAlpha = 0.0f;
          this.textStart = this.text.GetNextPageStart(this.textStart);
          yield return (object) 0.5f;
        }
        if (!this.started)
          this.StartGame();
        this.textStart = int.MaxValue;
      }

      public override void Update()
      {
        if (this.menu == null)
        {
          base.Update();
          if (!this.exiting)
          {
            if (this.textCoroutine != null && this.textCoroutine.Active)
              this.textCoroutine.Update();
            if (this.menu == null && (Input.Pause.Pressed || Input.ESC.Pressed))
            {
              Input.Pause.ConsumeBuffer();
              Input.ESC.ConsumeBuffer();
              this.OpenMenu();
            }
          }
        }
        else if (!this.exiting)
          this.menu.Update();
        this.pauseFade = Calc.Approach(this.pauseFade, this.menu != null ? 1f : 0.0f, Engine.DeltaTime * 8f);
        this.hud.BackgroundFade = Calc.Approach(this.hud.BackgroundFade, this.menu != null ? 0.6f : 0.0f, Engine.DeltaTime * 3f);
        this.fade = Calc.Approach(this.fade, 0.0f, Engine.DeltaTime);
      }

      public void OpenMenu()
      {
        Audio.Play("event:/ui/game/pause");
        this.Add((Entity) (this.menu = new TextMenu()));
        this.menu.Add(new TextMenu.Button(Dialog.Clean("intro_vignette_resume")).Pressed(new Action(this.CloseMenu)));
        this.menu.Add(new TextMenu.Button(Dialog.Clean("intro_vignette_skip")).Pressed(new Action(this.StartGame)));
        this.menu.Add(new TextMenu.Button(Dialog.Clean("intro_vignette_quit")).Pressed(new Action(this.ReturnToMap)));
        this.menu.OnCancel = this.menu.OnESC = this.menu.OnPause = new Action(this.CloseMenu);
      }

      private void CloseMenu()
      {
        Audio.Play("event:/ui/game/unpause");
        if (this.menu != null)
          this.menu.RemoveSelf();
        this.menu = (TextMenu) null;
      }

      private void StartGame()
      {
        this.textCoroutine = (Coroutine) null;
        if (this.menu != null)
        {
          this.menu.RemoveSelf();
          this.menu = (TextMenu) null;
        }
        new FadeWipe((Scene) this, false, (Action) (() => Engine.Scene = (Scene) new LevelLoader(this.session))).OnUpdate = (Action<float>) (f => this.textAlpha = Math.Min(this.textAlpha, 1f - f));
        this.started = true;
        this.exiting = true;
      }

      private void ReturnToMap()
      {
        this.menu.RemoveSelf();
        this.menu = (TextMenu) null;
        this.exiting = true;
        bool toAreaQuit = SaveData.Instance.Areas[0].Modes[0].Completed && Celeste.PlayMode != Celeste.PlayModes.Event;
        new FadeWipe((Scene) this, false, (Action) (() =>
        {
          if (toAreaQuit)
            Engine.Scene = (Scene) new OverworldLoader(Overworld.StartMode.AreaQuit, this.snow);
          else
            Engine.Scene = (Scene) new OverworldLoader(Overworld.StartMode.Titlescreen, this.snow);
        })).OnUpdate = (Action<float>) (f => this.textAlpha = Math.Min(this.textAlpha, 1f - f));
        this.RendererList.UpdateLists();
        this.RendererList.MoveToFront((Monocle.Renderer) this.snow);
      }

      public override void Render()
      {
        base.Render();
        if ((double) this.fade <= 0.0 && (double) this.textAlpha <= 0.0)
          return;
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, (DepthStencilState) null, RasterizerState.CullNone, (Effect) null, Engine.ScreenMatrix);
        if ((double) this.fade > 0.0)
          Draw.Rect(-1f, -1f, 1922f, 1082f, Color.Black * this.fade);
        if (this.textStart < this.text.Nodes.Count && (double) this.textAlpha > 0.0)
          this.text.Draw(new Vector2(1920f, 1080f) * 0.5f, new Vector2(0.5f, 0.5f), Vector2.One, this.textAlpha * (1f - this.pauseFade), this.textStart);
        Draw.SpriteBatch.End();
      }
    }
}
