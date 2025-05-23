﻿// Decompiled with JetBrains decompiler
// Type: Celeste.OuiMainMenu
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Celeste.Pico8;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Celeste {

    public class OuiMainMenu : Oui
    {
        private static readonly Vector2 TargetPosition = new Vector2(160f, 160f);
        private static readonly Vector2 TweenFrom = new Vector2(-500f, 160f);
        private static readonly Color UnselectedColor = Color.White;
        private static readonly Color SelectedColorA = TextMenu.HighlightColorA;
        private static readonly Color SelectedColorB = TextMenu.HighlightColorB;
        private const float IconWidth = 64f;
        private const float IconSpacing = 20f;
        private float ease;
        private MainMenuClimb climbButton;
        private List<MenuButton> buttons;
        private bool startOnOptions;
        private bool mountainStartFront;

        public OuiMainMenu() => this.buttons = new List<MenuButton>();

        public override void Added(Scene scene)
        {
            base.Added(scene);
            this.Position = OuiMainMenu.TweenFrom;
            this.CreateButtons();
        }

        public void CreateButtons()
        {
            foreach (Entity button in this.buttons)
                button.RemoveSelf();
            this.buttons.Clear();
            Vector2 targetPosition = new Vector2(320f, 160f);
            Vector2 vector2 = new Vector2(-640f, 0.0f);
            this.climbButton = new MainMenuClimb((Oui)this, targetPosition, targetPosition + vector2, new Action(this.OnBegin));
            if (!this.startOnOptions)
                this.climbButton.StartSelected();
            this.buttons.Add((MenuButton)this.climbButton);
            targetPosition += Vector2.UnitY * this.climbButton.ButtonHeight;
            targetPosition.X -= 140f;
            if (Celeste.PlayMode == Celeste.PlayModes.Debug)
            {
                MainMenuSmallButton mainMenuSmallButton = new MainMenuSmallButton("menu_debug", "menu/options", (Oui)this, targetPosition, targetPosition + vector2, new Action(this.OnDebug));
                this.buttons.Add((MenuButton)mainMenuSmallButton);
                targetPosition += Vector2.UnitY * mainMenuSmallButton.ButtonHeight;
            }
            if (Settings.Instance.Pico8OnMainMenu || Celeste.PlayMode == Celeste.PlayModes.Debug || Celeste.PlayMode == Celeste.PlayModes.Event)
            {
                MainMenuSmallButton mainMenuSmallButton = new MainMenuSmallButton("menu_pico8", "menu/pico8", (Oui)this, targetPosition, targetPosition + vector2, new Action(this.OnPico8));
                this.buttons.Add((MenuButton)mainMenuSmallButton);
                targetPosition += Vector2.UnitY * mainMenuSmallButton.ButtonHeight;
            }
            MainMenuSmallButton mainMenuSmallButton1 = new MainMenuSmallButton("menu_options", "menu/options", (Oui)this, targetPosition, targetPosition + vector2, new Action(this.OnOptions));
            if (this.startOnOptions)
                mainMenuSmallButton1.StartSelected();
            this.buttons.Add((MenuButton)mainMenuSmallButton1);
            targetPosition += Vector2.UnitY * mainMenuSmallButton1.ButtonHeight;
            MainMenuSmallButton mainMenuSmallButton2 = new MainMenuSmallButton("menu_credits", "menu/credits", (Oui)this, targetPosition, targetPosition + vector2, new Action(this.OnCredits));
            this.buttons.Add((MenuButton)mainMenuSmallButton2);
            targetPosition += Vector2.UnitY * mainMenuSmallButton2.ButtonHeight;
            MainMenuSmallButton mainMenuSmallButton3 = new MainMenuSmallButton("menu_exit", "menu/exit", (Oui)this, targetPosition, targetPosition + vector2, new Action(this.OnExit));
            this.buttons.Add((MenuButton)mainMenuSmallButton3);
            targetPosition += Vector2.UnitY * mainMenuSmallButton3.ButtonHeight;
            for (int index = 0; index < this.buttons.Count; ++index)
            {
                if (index > 0)
                    this.buttons[index].UpButton = this.buttons[index - 1];
                if (index < this.buttons.Count - 1)
                    this.buttons[index].DownButton = this.buttons[index + 1];
                this.Scene.Add((Entity)this.buttons[index]);
            }
            if (!this.Visible || !this.Focused)
                return;
            foreach (MenuButton button in this.buttons)
                button.Position = button.TargetPosition;
        }

        public override void Removed(Scene scene)
        {
            foreach (MenuButton button in this.buttons)
                scene.Remove((Entity)button);
            base.Removed(scene);
        }

        public override bool IsStart(Overworld overworld, Overworld.StartMode start)
        {
            if (start == Overworld.StartMode.ReturnFromOptions)
            {
                this.startOnOptions = true;
                this.Add((Component)new Coroutine(this.Enter((Oui)null)));
                return true;
            }
            if (start == Overworld.StartMode.MainMenu)
            {
                this.mountainStartFront = true;
                this.Add((Component)new Coroutine(this.Enter((Oui)null)));
                return true;
            }
            return start == Overworld.StartMode.ReturnFromOptions || start == Overworld.StartMode.ReturnFromPico8;
        }

        public override IEnumerator Enter(Oui from)
        {
            OuiMainMenu ouiMainMenu = this;
            if (from is OuiTitleScreen || from is OuiFileSelect)
            {
                Audio.Play("event:/ui/main/whoosh_list_in");
                yield return (object)0.1f;
            }
            if (from is OuiTitleScreen)
            {
                MenuButton.ClearSelection(ouiMainMenu.Scene);
                ouiMainMenu.climbButton.StartSelected();
            }
            ouiMainMenu.Visible = true;
            if (ouiMainMenu.mountainStartFront)
                ouiMainMenu.Overworld.Mountain.SnapCamera(-1, new MountainCamera(new Vector3(0.0f, 6f, 12f), MountainRenderer.RotateLookAt));
            ouiMainMenu.Overworld.Mountain.GotoRotationMode();
            ouiMainMenu.Overworld.Maddy.Hide();
            foreach (MenuButton button in ouiMainMenu.buttons)
                button.TweenIn(0.2f);
            yield return (object)0.2f;
            ouiMainMenu.Focused = true;
            ouiMainMenu.mountainStartFront = false;
            yield return (object)null;
        }

        public override IEnumerator Leave(Oui next)
        {
            /*
            OuiMainMenu ouiMainMenu = this;
            ouiMainMenu.Focused = false;
            Tween tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.CubeInOut, 0.2f, true);
            // ISSUE: reference to a compiler-generated method
            tween.OnUpdate = new Action<Tween>(ouiMainMenu.\u003CLeave\u003Eb__18_0);
            ouiMainMenu.Add((Component)tween);
            bool keepClimb = ouiMainMenu.climbButton.Selected && !(next is OuiTitleScreen);
            foreach (MenuButton button in ouiMainMenu.buttons)
            {
                if (!(button == ouiMainMenu.climbButton & keepClimb))
                    button.TweenOut(0.2f);
            }
            yield return (object)0.2f;
            if (keepClimb)
                ouiMainMenu.Add((Component)new Coroutine(ouiMainMenu.SlideClimbOutLate()));
            else
                ouiMainMenu.Visible = false;
            */
            this.Focused = false;
            Tween tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.CubeInOut, 0.2f, true);
            tween.OnUpdate = delegate (Tween t)
            {
                this.ease = 1f - t.Eased;
                this.Position = Vector2.Lerp(OuiMainMenu.TargetPosition, OuiMainMenu.TweenFrom, t.Eased);
            };
            base.Add(tween);
            bool keepClimb = this.climbButton.Selected && !(next is OuiTitleScreen);
            foreach (MenuButton menuButton in this.buttons)
            {
                if (menuButton != this.climbButton || !keepClimb)
                {
                    menuButton.TweenOut(0.2f);
                }
            }
            yield return 0.2f;
            if (keepClimb)
            {
                base.Add(new Coroutine(this.SlideClimbOutLate(), true));
            }
            else
            {
                this.Visible = false;
            }
            yield break;
        }

        private IEnumerator SlideClimbOutLate()
        {
            OuiMainMenu ouiMainMenu = this;
            yield return (object)0.2f;
            ouiMainMenu.climbButton.TweenOut(0.2f);
            yield return (object)0.2f;
            ouiMainMenu.Visible = false;
        }

        public Color SelectionColor
        {
            get
            {
                return !Settings.Instance.DisableFlashes && !this.Scene.BetweenInterval(0.1f) ? OuiMainMenu.SelectedColorB : OuiMainMenu.SelectedColorA;
            }
        }

        public override void Update()
        {
            if (this.Selected && this.Focused && Input.MenuCancel.Pressed)
            {
                this.Focused = false;
                Audio.Play("event:/ui/main/whoosh_list_out");
                Audio.Play("event:/ui/main/button_back");
                this.Overworld.Goto<OuiTitleScreen>();
            }
            base.Update();
        }

        public override void Render()
        {
            foreach (MenuButton button in this.buttons)
            {
                if (button.Scene == this.Scene)
                    button.Render();
            }
        }

        private void OnDebug()
        {
            Audio.Play("event:/ui/main/whoosh_list_out");
            Audio.Play("event:/ui/main/button_select");
            SaveData.InitializeDebugMode();
            this.Overworld.Goto<OuiChapterSelect>();
        }

        private void OnBegin()
        {
            Audio.Play("event:/ui/main/whoosh_list_out");
            Audio.Play("event:/ui/main/button_climb");
            if (Celeste.PlayMode == Celeste.PlayModes.Event)
            {
                SaveData.InitializeDebugMode(false);
                this.Overworld.Goto<OuiChapterSelect>();
            }
            else
                this.Overworld.Goto<OuiFileSelect>();
        }

        private void OnPico8()
        {
            Audio.Play("event:/ui/main/button_select");
            this.Focused = false;
            FadeWipe fadeWipe = new FadeWipe(this.Scene, false, (Action)(() =>
            {
                this.Focused = true;
                this.Overworld.EnteringPico8 = true;
                SaveData.Instance = (SaveData)null;
                SaveData.NoFileAssistChecks();
                Engine.Scene = (Scene)new Emulator((Scene)this.Overworld);
            }));
        }

        private void OnOptions()
        {
            Audio.Play("event:/ui/main/button_select");
            Audio.Play("event:/ui/main/whoosh_large_in");
            this.Overworld.Goto<OuiOptions>();
        }

        private void OnCredits()
        {
            Audio.Play("event:/ui/main/button_select");
            Audio.Play("event:/ui/main/whoosh_large_in");
            this.Overworld.Goto<OuiCredits>();
        }

        private void OnExit()
        {
            Audio.Play("event:/ui/main/button_select");
            this.Focused = false;
            FadeWipe fadeWipe = new FadeWipe(this.Scene, false, (Action)(() =>
            {
                Engine.Scene = new Scene();
                Engine.Instance.Exit();
            }));
        }
    }
}