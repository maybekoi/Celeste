// Decompiled with JetBrains decompiler
// Type: Monocle.MInput
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Monocle
{

    public static class MInput
    {
      internal static List<VirtualInput> VirtualInputs;
      public static bool Active = true;
      public static bool Disabled = false;
      public static bool ControllerHasFocus = false;
      public static bool IsControllerFocused = false;

      public static MInput.KeyboardData Keyboard { get; private set; }

      public static MInput.MouseData Mouse { get; private set; }

      public static MInput.GamePadData[] GamePads { get; private set; }

      internal static void Initialize()
      {
        MInput.Keyboard = new MInput.KeyboardData();
        MInput.Mouse = new MInput.MouseData();
        MInput.GamePads = new MInput.GamePadData[4];
        for (int playerIndex = 0; playerIndex < 4; ++playerIndex)
          MInput.GamePads[playerIndex] = new MInput.GamePadData(playerIndex);
        MInput.VirtualInputs = new List<VirtualInput>();
      }

      internal static void Shutdown()
      {
        foreach (MInput.GamePadData gamePad in MInput.GamePads)
          gamePad.StopRumble();
      }

      internal static void Update()
      {
        if (Engine.Instance.IsActive && MInput.Active)
        {
          if (Engine.Commands.Open)
          {
            MInput.Keyboard.UpdateNull();
            MInput.Mouse.UpdateNull();
          }
          else
          {
            MInput.Keyboard.Update();
            MInput.Mouse.Update();
          }
          bool flag1 = false;
          bool flag2 = false;
          for (int index = 0; index < 4; ++index)
          {
            MInput.GamePads[index].Update();
            if (MInput.GamePads[index].HasAnyInput())
            {
              MInput.ControllerHasFocus = true;
              flag1 = true;
            }
            if (MInput.GamePads[index].Attached)
              flag2 = true;
          }
          if (!flag2 || !flag1 && MInput.Keyboard.HasAnyInput())
            MInput.ControllerHasFocus = false;
        }
        else
        {
          MInput.Keyboard.UpdateNull();
          MInput.Mouse.UpdateNull();
          for (int index = 0; index < 4; ++index)
            MInput.GamePads[index].UpdateNull();
        }
        MInput.UpdateVirtualInputs();
      }

      public static void UpdateNull()
      {
        MInput.Keyboard.UpdateNull();
        MInput.Mouse.UpdateNull();
        for (int index = 0; index < 4; ++index)
          MInput.GamePads[index].UpdateNull();
        MInput.UpdateVirtualInputs();
      }

      private static void UpdateVirtualInputs()
      {
        foreach (VirtualInput virtualInput in MInput.VirtualInputs)
          virtualInput.Update();
      }

      public static void RumbleFirst(float strength, float time)
      {
        MInput.GamePads[0].Rumble(strength, time);
      }

      public static int Axis(bool negative, bool positive, int bothValue)
      {
        return negative ? (positive ? bothValue : -1) : (positive ? 1 : 0);
      }

      public static int Axis(float axisValue, float deadzone)
      {
        return (double) Math.Abs(axisValue) >= (double) deadzone ? Math.Sign(axisValue) : 0;
      }

      public static int Axis(
        bool negative,
        bool positive,
        int bothValue,
        float axisValue,
        float deadzone)
      {
        int num = MInput.Axis(axisValue, deadzone);
        if (num == 0)
          num = MInput.Axis(negative, positive, bothValue);
        return num;
      }

      public class KeyboardData
      {
        public KeyboardState PreviousState;
        public KeyboardState CurrentState;

        internal KeyboardData()
        {
        }

        internal void Update()
        {
          this.PreviousState = this.CurrentState;
          this.CurrentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        internal void UpdateNull()
        {
          this.PreviousState = this.CurrentState;
          this.CurrentState = new KeyboardState();
          this.CurrentState.GetPressedKeys();
        }

        public bool HasAnyInput() => this.CurrentState.GetPressedKeys().Length != 0;

        public bool Check(Keys key)
        {
          return !MInput.Disabled && key != Keys.None && this.CurrentState.IsKeyDown(key);
        }

        public bool Pressed(Keys key)
        {
          return !MInput.Disabled && key != Keys.None && this.CurrentState.IsKeyDown(key) && !this.PreviousState.IsKeyDown(key);
        }

        public bool Released(Keys key)
        {
          return !MInput.Disabled && key != Keys.None && !this.CurrentState.IsKeyDown(key) && this.PreviousState.IsKeyDown(key);
        }

        public bool Check(Keys keyA, Keys keyB) => this.Check(keyA) || this.Check(keyB);

        public bool Pressed(Keys keyA, Keys keyB) => this.Pressed(keyA) || this.Pressed(keyB);

        public bool Released(Keys keyA, Keys keyB) => this.Released(keyA) || this.Released(keyB);

        public bool Check(Keys keyA, Keys keyB, Keys keyC)
        {
          return this.Check(keyA) || this.Check(keyB) || this.Check(keyC);
        }

        public bool Pressed(Keys keyA, Keys keyB, Keys keyC)
        {
          return this.Pressed(keyA) || this.Pressed(keyB) || this.Pressed(keyC);
        }

        public bool Released(Keys keyA, Keys keyB, Keys keyC)
        {
          return this.Released(keyA) || this.Released(keyB) || this.Released(keyC);
        }

        public int AxisCheck(Keys negative, Keys positive)
        {
          return this.Check(negative) ? (this.Check(positive) ? 0 : -1) : (this.Check(positive) ? 1 : 0);
        }

        public int AxisCheck(Keys negative, Keys positive, int both)
        {
          return this.Check(negative) ? (this.Check(positive) ? both : -1) : (this.Check(positive) ? 1 : 0);
        }
      }

      public class MouseData
      {
        public MouseState PreviousState;
        public MouseState CurrentState;

        internal MouseData()
        {
          this.PreviousState = new MouseState();
          this.CurrentState = new MouseState();
        }

        internal void Update()
        {
          this.PreviousState = this.CurrentState;
          this.CurrentState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        internal void UpdateNull()
        {
          this.PreviousState = this.CurrentState;
          this.CurrentState = new MouseState();
        }

        public bool CheckLeftButton => this.CurrentState.LeftButton == ButtonState.Pressed;

        public bool CheckRightButton => this.CurrentState.RightButton == ButtonState.Pressed;

        public bool CheckMiddleButton => this.CurrentState.MiddleButton == ButtonState.Pressed;

        public bool PressedLeftButton
        {
          get
          {
            return this.CurrentState.LeftButton == ButtonState.Pressed && this.PreviousState.LeftButton == ButtonState.Released;
          }
        }

        public bool PressedRightButton
        {
          get
          {
            return this.CurrentState.RightButton == ButtonState.Pressed && this.PreviousState.RightButton == ButtonState.Released;
          }
        }

        public bool PressedMiddleButton
        {
          get
          {
            return this.CurrentState.MiddleButton == ButtonState.Pressed && this.PreviousState.MiddleButton == ButtonState.Released;
          }
        }

        public bool ReleasedLeftButton
        {
          get
          {
            return this.CurrentState.LeftButton == ButtonState.Released && this.PreviousState.LeftButton == ButtonState.Pressed;
          }
        }

        public bool ReleasedRightButton
        {
          get
          {
            return this.CurrentState.RightButton == ButtonState.Released && this.PreviousState.RightButton == ButtonState.Pressed;
          }
        }

        public bool ReleasedMiddleButton
        {
          get
          {
            return this.CurrentState.MiddleButton == ButtonState.Released && this.PreviousState.MiddleButton == ButtonState.Pressed;
          }
        }

        public int Wheel => this.CurrentState.ScrollWheelValue;

        public int WheelDelta
        {
          get => this.CurrentState.ScrollWheelValue - this.PreviousState.ScrollWheelValue;
        }

        public bool WasMoved
        {
          get
          {
            return this.CurrentState.X != this.PreviousState.X || this.CurrentState.Y != this.PreviousState.Y;
          }
        }

        public float X
        {
          get => this.Position.X;
          set => this.Position = new Vector2(value, this.Position.Y);
        }

        public float Y
        {
          get => this.Position.Y;
          set => this.Position = new Vector2(this.Position.X, value);
        }

        public Vector2 Position
        {
          get
          {
            return Vector2.Transform(new Vector2((float) this.CurrentState.X, (float) this.CurrentState.Y), Matrix.Invert(Engine.ScreenMatrix));
          }
          set
          {
            Vector2 vector2 = Vector2.Transform(value, Engine.ScreenMatrix);
            Microsoft.Xna.Framework.Input.Mouse.SetPosition((int) Math.Round((double) vector2.X), (int) Math.Round((double) vector2.Y));
          }
        }
      }

      public class GamePadData
      {
        public readonly PlayerIndex PlayerIndex;
        public GamePadState PreviousState;
        public GamePadState CurrentState;
        public bool Attached;
        public bool HadInputThisFrame;
        private float rumbleStrength;
        private float rumbleTime;

        internal GamePadData(int playerIndex)
        {
          this.PlayerIndex = (PlayerIndex) Calc.Clamp(playerIndex, 0, 3);
        }

        public bool HasAnyInput()
        {
          if (!this.PreviousState.IsConnected && this.CurrentState.IsConnected || this.PreviousState.Buttons != this.CurrentState.Buttons || this.PreviousState.DPad != this.CurrentState.DPad || (double) this.CurrentState.Triggers.Left > 0.0099999997764825821 || (double) this.CurrentState.Triggers.Right > 0.0099999997764825821)
            return true;
          Vector2 vector2 = this.CurrentState.ThumbSticks.Left;
          if ((double) vector2.Length() <= 0.0099999997764825821)
          {
            vector2 = this.CurrentState.ThumbSticks.Right;
            if ((double) vector2.Length() <= 0.0099999997764825821)
              return false;
          }
          return true;
        }

        public void Update()
        {
          this.PreviousState = this.CurrentState;
          this.CurrentState = GamePad.GetState(this.PlayerIndex);
          if (!this.Attached && this.CurrentState.IsConnected)
            MInput.IsControllerFocused = true;
          this.Attached = this.CurrentState.IsConnected;
          if ((double) this.rumbleTime <= 0.0)
            return;
          this.rumbleTime -= Engine.DeltaTime;
          if ((double) this.rumbleTime > 0.0)
            return;
          GamePad.SetVibration(this.PlayerIndex, 0.0f, 0.0f);
        }

        public void UpdateNull()
        {
          this.PreviousState = this.CurrentState;
          this.CurrentState = new GamePadState();
          this.Attached = GamePad.GetState(this.PlayerIndex).IsConnected;
          if ((double) this.rumbleTime > 0.0)
            this.rumbleTime -= Engine.DeltaTime;
          GamePad.SetVibration(this.PlayerIndex, 0.0f, 0.0f);
        }

        public void Rumble(float strength, float time)
        {
          if ((double) this.rumbleTime > 0.0 && (double) strength <= (double) this.rumbleStrength && ((double) strength != (double) this.rumbleStrength || (double) time <= (double) this.rumbleTime))
            return;
          GamePad.SetVibration(this.PlayerIndex, strength, strength);
          this.rumbleStrength = strength;
          this.rumbleTime = time;
        }

        public void StopRumble()
        {
          GamePad.SetVibration(this.PlayerIndex, 0.0f, 0.0f);
          this.rumbleTime = 0.0f;
        }

        public bool Check(Buttons button) => !MInput.Disabled && this.CurrentState.IsButtonDown(button);

        public bool Pressed(Buttons button)
        {
          return !MInput.Disabled && this.CurrentState.IsButtonDown(button) && this.PreviousState.IsButtonUp(button);
        }

        public bool Released(Buttons button)
        {
          return !MInput.Disabled && this.CurrentState.IsButtonUp(button) && this.PreviousState.IsButtonDown(button);
        }

        public bool Check(Buttons buttonA, Buttons buttonB)
        {
          return this.Check(buttonA) || this.Check(buttonB);
        }

        public bool Pressed(Buttons buttonA, Buttons buttonB)
        {
          return this.Pressed(buttonA) || this.Pressed(buttonB);
        }

        public bool Released(Buttons buttonA, Buttons buttonB)
        {
          return this.Released(buttonA) || this.Released(buttonB);
        }

        public bool Check(Buttons buttonA, Buttons buttonB, Buttons buttonC)
        {
          return this.Check(buttonA) || this.Check(buttonB) || this.Check(buttonC);
        }

        public bool Pressed(Buttons buttonA, Buttons buttonB, Buttons buttonC)
        {
          return this.Pressed(buttonA) || this.Pressed(buttonB) || this.Check(buttonC);
        }

        public bool Released(Buttons buttonA, Buttons buttonB, Buttons buttonC)
        {
          return this.Released(buttonA) || this.Released(buttonB) || this.Check(buttonC);
        }

        public Vector2 GetLeftStick()
        {
          Vector2 left = this.CurrentState.ThumbSticks.Left;
          left.Y = -left.Y;
          return left;
        }

        public Vector2 GetLeftStick(float deadzone)
        {
          Vector2 leftStick = this.CurrentState.ThumbSticks.Left;
          if ((double) leftStick.LengthSquared() < (double) deadzone * (double) deadzone)
            leftStick = Vector2.Zero;
          else
            leftStick.Y = -leftStick.Y;
          return leftStick;
        }

        public Vector2 GetRightStick()
        {
          Vector2 right = this.CurrentState.ThumbSticks.Right;
          right.Y = -right.Y;
          return right;
        }

        public Vector2 GetRightStick(float deadzone)
        {
          Vector2 rightStick = this.CurrentState.ThumbSticks.Right;
          if ((double) rightStick.LengthSquared() < (double) deadzone * (double) deadzone)
            rightStick = Vector2.Zero;
          else
            rightStick.Y = -rightStick.Y;
          return rightStick;
        }

        public bool LeftStickLeftCheck(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.X <= -(double) deadzone;
        }

        public bool LeftStickLeftPressed(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.X <= -(double) deadzone && (double) this.PreviousState.ThumbSticks.Left.X > -(double) deadzone;
        }

        public bool LeftStickLeftReleased(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.X > -(double) deadzone && (double) this.PreviousState.ThumbSticks.Left.X <= -(double) deadzone;
        }

        public bool LeftStickRightCheck(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.X >= (double) deadzone;
        }

        public bool LeftStickRightPressed(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.X >= (double) deadzone && (double) this.PreviousState.ThumbSticks.Left.X < (double) deadzone;
        }

        public bool LeftStickRightReleased(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.X < (double) deadzone && (double) this.PreviousState.ThumbSticks.Left.X >= (double) deadzone;
        }

        public bool LeftStickDownCheck(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.Y <= -(double) deadzone;
        }

        public bool LeftStickDownPressed(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.Y <= -(double) deadzone && (double) this.PreviousState.ThumbSticks.Left.Y > -(double) deadzone;
        }

        public bool LeftStickDownReleased(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.Y > -(double) deadzone && (double) this.PreviousState.ThumbSticks.Left.Y <= -(double) deadzone;
        }

        public bool LeftStickUpCheck(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.Y >= (double) deadzone;
        }

        public bool LeftStickUpPressed(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.Y >= (double) deadzone && (double) this.PreviousState.ThumbSticks.Left.Y < (double) deadzone;
        }

        public bool LeftStickUpReleased(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Left.Y < (double) deadzone && (double) this.PreviousState.ThumbSticks.Left.Y >= (double) deadzone;
        }

        public float LeftStickHorizontal(float deadzone)
        {
          float x = this.CurrentState.ThumbSticks.Left.X;
          return (double) Math.Abs(x) < (double) deadzone ? 0.0f : x;
        }

        public float LeftStickVertical(float deadzone)
        {
          float y = this.CurrentState.ThumbSticks.Left.Y;
          return (double) Math.Abs(y) < (double) deadzone ? 0.0f : -y;
        }

        public bool RightStickLeftCheck(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.X <= -(double) deadzone;
        }

        public bool RightStickLeftPressed(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.X <= -(double) deadzone && (double) this.PreviousState.ThumbSticks.Right.X > -(double) deadzone;
        }

        public bool RightStickLeftReleased(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.X > -(double) deadzone && (double) this.PreviousState.ThumbSticks.Right.X <= -(double) deadzone;
        }

        public bool RightStickRightCheck(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.X >= (double) deadzone;
        }

        public bool RightStickRightPressed(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.X >= (double) deadzone && (double) this.PreviousState.ThumbSticks.Right.X < (double) deadzone;
        }

        public bool RightStickRightReleased(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.X < (double) deadzone && (double) this.PreviousState.ThumbSticks.Right.X >= (double) deadzone;
        }

        public bool RightStickDownCheck(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.Y <= -(double) deadzone;
        }

        public bool RightStickDownPressed(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.Y <= -(double) deadzone && (double) this.PreviousState.ThumbSticks.Right.Y > -(double) deadzone;
        }

        public bool RightStickDownReleased(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.Y > -(double) deadzone && (double) this.PreviousState.ThumbSticks.Right.Y <= -(double) deadzone;
        }

        public bool RightStickUpCheck(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.Y >= (double) deadzone;
        }

        public bool RightStickUpPressed(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.Y >= (double) deadzone && (double) this.PreviousState.ThumbSticks.Right.Y < (double) deadzone;
        }

        public bool RightStickUpReleased(float deadzone)
        {
          return (double) this.CurrentState.ThumbSticks.Right.Y < (double) deadzone && (double) this.PreviousState.ThumbSticks.Right.Y >= (double) deadzone;
        }

        public float RightStickHorizontal(float deadzone)
        {
          float x = this.CurrentState.ThumbSticks.Right.X;
          return (double) Math.Abs(x) < (double) deadzone ? 0.0f : x;
        }

        public float RightStickVertical(float deadzone)
        {
          float y = this.CurrentState.ThumbSticks.Right.Y;
          return (double) Math.Abs(y) < (double) deadzone ? 0.0f : -y;
        }

        public int DPadHorizontal
        {
          get
          {
            if (this.CurrentState.DPad.Right == ButtonState.Pressed)
              return 1;
            return this.CurrentState.DPad.Left != ButtonState.Pressed ? 0 : -1;
          }
        }

        public int DPadVertical
        {
          get
          {
            if (this.CurrentState.DPad.Down == ButtonState.Pressed)
              return 1;
            return this.CurrentState.DPad.Up != ButtonState.Pressed ? 0 : -1;
          }
        }

        public Vector2 DPad => new Vector2((float) this.DPadHorizontal, (float) this.DPadVertical);

        public bool DPadLeftCheck => this.CurrentState.DPad.Left == ButtonState.Pressed;

        public bool DPadLeftPressed
        {
          get
          {
            return this.CurrentState.DPad.Left == ButtonState.Pressed && this.PreviousState.DPad.Left == ButtonState.Released;
          }
        }

        public bool DPadLeftReleased
        {
          get
          {
            return this.CurrentState.DPad.Left == ButtonState.Released && this.PreviousState.DPad.Left == ButtonState.Pressed;
          }
        }

        public bool DPadRightCheck => this.CurrentState.DPad.Right == ButtonState.Pressed;

        public bool DPadRightPressed
        {
          get
          {
            return this.CurrentState.DPad.Right == ButtonState.Pressed && this.PreviousState.DPad.Right == ButtonState.Released;
          }
        }

        public bool DPadRightReleased
        {
          get
          {
            return this.CurrentState.DPad.Right == ButtonState.Released && this.PreviousState.DPad.Right == ButtonState.Pressed;
          }
        }

        public bool DPadUpCheck => this.CurrentState.DPad.Up == ButtonState.Pressed;

        public bool DPadUpPressed
        {
          get
          {
            return this.CurrentState.DPad.Up == ButtonState.Pressed && this.PreviousState.DPad.Up == ButtonState.Released;
          }
        }

        public bool DPadUpReleased
        {
          get
          {
            return this.CurrentState.DPad.Up == ButtonState.Released && this.PreviousState.DPad.Up == ButtonState.Pressed;
          }
        }

        public bool DPadDownCheck => this.CurrentState.DPad.Down == ButtonState.Pressed;

        public bool DPadDownPressed
        {
          get
          {
            return this.CurrentState.DPad.Down == ButtonState.Pressed && this.PreviousState.DPad.Down == ButtonState.Released;
          }
        }

        public bool DPadDownReleased
        {
          get
          {
            return this.CurrentState.DPad.Down == ButtonState.Released && this.PreviousState.DPad.Down == ButtonState.Pressed;
          }
        }

        public bool LeftTriggerCheck(float threshold)
        {
          return !MInput.Disabled && (double) this.CurrentState.Triggers.Left >= (double) threshold;
        }

        public bool LeftTriggerPressed(float threshold)
        {
          return !MInput.Disabled && (double) this.CurrentState.Triggers.Left >= (double) threshold && (double) this.PreviousState.Triggers.Left < (double) threshold;
        }

        public bool LeftTriggerReleased(float threshold)
        {
          return !MInput.Disabled && (double) this.CurrentState.Triggers.Left < (double) threshold && (double) this.PreviousState.Triggers.Left >= (double) threshold;
        }

        public bool RightTriggerCheck(float threshold)
        {
          return !MInput.Disabled && (double) this.CurrentState.Triggers.Right >= (double) threshold;
        }

        public bool RightTriggerPressed(float threshold)
        {
          return !MInput.Disabled && (double) this.CurrentState.Triggers.Right >= (double) threshold && (double) this.PreviousState.Triggers.Right < (double) threshold;
        }

        public bool RightTriggerReleased(float threshold)
        {
          return !MInput.Disabled && (double) this.CurrentState.Triggers.Right < (double) threshold && (double) this.PreviousState.Triggers.Right >= (double) threshold;
        }

        public float Axis(Buttons button, float threshold)
        {
          if (MInput.Disabled)
            return 0.0f;
          switch (button)
          {
            case Buttons.DPadUp:
            case Buttons.DPadDown:
            case Buttons.DPadLeft:
            case Buttons.DPadRight:
            case Buttons.Start:
            case Buttons.Back:
            case Buttons.LeftStick:
            case Buttons.RightStick:
            case Buttons.LeftShoulder:
            case Buttons.RightShoulder:
            case Buttons.A:
            case Buttons.B:
            case Buttons.X:
            case Buttons.Y:
              if (this.Check(button))
                return 1f;
              break;
            case Buttons.LeftThumbstickLeft:
              if ((double) this.CurrentState.ThumbSticks.Left.X <= -(double) threshold)
                return -this.CurrentState.ThumbSticks.Left.X;
              break;
            case Buttons.RightTrigger:
              if ((double) this.CurrentState.Triggers.Right >= (double) threshold)
                return this.CurrentState.Triggers.Right;
              break;
            case Buttons.LeftTrigger:
              if ((double) this.CurrentState.Triggers.Left >= (double) threshold)
                return this.CurrentState.Triggers.Left;
              break;
            case Buttons.RightThumbstickUp:
              if ((double) this.CurrentState.ThumbSticks.Right.Y >= (double) threshold)
                return this.CurrentState.ThumbSticks.Right.Y;
              break;
            case Buttons.RightThumbstickDown:
              if ((double) this.CurrentState.ThumbSticks.Right.Y <= -(double) threshold)
                return -this.CurrentState.ThumbSticks.Right.Y;
              break;
            case Buttons.RightThumbstickRight:
              if ((double) this.CurrentState.ThumbSticks.Right.X >= (double) threshold)
                return this.CurrentState.ThumbSticks.Right.X;
              break;
            case Buttons.RightThumbstickLeft:
              if ((double) this.CurrentState.ThumbSticks.Right.X <= -(double) threshold)
                return -this.CurrentState.ThumbSticks.Right.X;
              break;
            case Buttons.LeftThumbstickUp:
              if ((double) this.CurrentState.ThumbSticks.Left.Y >= (double) threshold)
                return this.CurrentState.ThumbSticks.Left.Y;
              break;
            case Buttons.LeftThumbstickDown:
              if ((double) this.CurrentState.ThumbSticks.Left.Y <= -(double) threshold)
                return -this.CurrentState.ThumbSticks.Left.Y;
              break;
            case Buttons.LeftThumbstickRight:
              if ((double) this.CurrentState.ThumbSticks.Left.X >= (double) threshold)
                return this.CurrentState.ThumbSticks.Left.X;
              break;
          }
          return 0.0f;
        }

        public bool Check(Buttons button, float threshold)
        {
          if (MInput.Disabled)
            return false;
          switch (button)
          {
            case Buttons.DPadUp:
            case Buttons.DPadDown:
            case Buttons.DPadLeft:
            case Buttons.DPadRight:
            case Buttons.Start:
            case Buttons.Back:
            case Buttons.LeftStick:
            case Buttons.RightStick:
            case Buttons.LeftShoulder:
            case Buttons.RightShoulder:
            case Buttons.A:
            case Buttons.B:
            case Buttons.X:
            case Buttons.Y:
              if (this.Check(button))
                return true;
              break;
            case Buttons.LeftThumbstickLeft:
              if (this.LeftStickLeftCheck(threshold))
                return true;
              break;
            case Buttons.RightTrigger:
              if (this.RightTriggerCheck(threshold))
                return true;
              break;
            case Buttons.LeftTrigger:
              if (this.LeftTriggerCheck(threshold))
                return true;
              break;
            case Buttons.RightThumbstickUp:
              if (this.RightStickUpCheck(threshold))
                return true;
              break;
            case Buttons.RightThumbstickDown:
              if (this.RightStickDownCheck(threshold))
                return true;
              break;
            case Buttons.RightThumbstickRight:
              if (this.RightStickRightCheck(threshold))
                return true;
              break;
            case Buttons.RightThumbstickLeft:
              if (this.RightStickLeftCheck(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickUp:
              if (this.LeftStickUpCheck(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickDown:
              if (this.LeftStickDownCheck(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickRight:
              if (this.LeftStickRightCheck(threshold))
                return true;
              break;
          }
          return false;
        }

        public bool Pressed(Buttons button, float threshold)
        {
          if (MInput.Disabled)
            return false;
          switch (button)
          {
            case Buttons.DPadUp:
            case Buttons.DPadDown:
            case Buttons.DPadLeft:
            case Buttons.DPadRight:
            case Buttons.Start:
            case Buttons.Back:
            case Buttons.LeftStick:
            case Buttons.RightStick:
            case Buttons.LeftShoulder:
            case Buttons.RightShoulder:
            case Buttons.A:
            case Buttons.B:
            case Buttons.X:
            case Buttons.Y:
              if (this.Pressed(button))
                return true;
              break;
            case Buttons.LeftThumbstickLeft:
              if (this.LeftStickLeftPressed(threshold))
                return true;
              break;
            case Buttons.RightTrigger:
              if (this.RightTriggerPressed(threshold))
                return true;
              break;
            case Buttons.LeftTrigger:
              if (this.LeftTriggerPressed(threshold))
                return true;
              break;
            case Buttons.RightThumbstickUp:
              if (this.RightStickUpPressed(threshold))
                return true;
              break;
            case Buttons.RightThumbstickDown:
              if (this.RightStickDownPressed(threshold))
                return true;
              break;
            case Buttons.RightThumbstickRight:
              if (this.RightStickRightPressed(threshold))
                return true;
              break;
            case Buttons.RightThumbstickLeft:
              if (this.RightStickLeftPressed(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickUp:
              if (this.LeftStickUpPressed(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickDown:
              if (this.LeftStickDownPressed(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickRight:
              if (this.LeftStickRightPressed(threshold))
                return true;
              break;
          }
          return false;
        }

        public bool Released(Buttons button, float threshold)
        {
          if (MInput.Disabled)
            return false;
          switch (button)
          {
            case Buttons.DPadUp:
            case Buttons.DPadDown:
            case Buttons.DPadLeft:
            case Buttons.DPadRight:
            case Buttons.Start:
            case Buttons.Back:
            case Buttons.LeftStick:
            case Buttons.RightStick:
            case Buttons.LeftShoulder:
            case Buttons.RightShoulder:
            case Buttons.A:
            case Buttons.B:
            case Buttons.X:
            case Buttons.Y:
              if (this.Released(button))
                return true;
              break;
            case Buttons.LeftThumbstickLeft:
              if (this.LeftStickLeftReleased(threshold))
                return true;
              break;
            case Buttons.RightTrigger:
              if (this.RightTriggerReleased(threshold))
                return true;
              break;
            case Buttons.LeftTrigger:
              if (this.LeftTriggerReleased(threshold))
                return true;
              break;
            case Buttons.RightThumbstickUp:
              if (this.RightStickUpReleased(threshold))
                return true;
              break;
            case Buttons.RightThumbstickDown:
              if (this.RightStickDownReleased(threshold))
                return true;
              break;
            case Buttons.RightThumbstickRight:
              if (this.RightStickRightReleased(threshold))
                return true;
              break;
            case Buttons.RightThumbstickLeft:
              if (this.RightStickLeftReleased(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickUp:
              if (this.LeftStickUpReleased(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickDown:
              if (this.LeftStickDownReleased(threshold))
                return true;
              break;
            case Buttons.LeftThumbstickRight:
              if (this.LeftStickRightReleased(threshold))
                return true;
              break;
          }
          return false;
        }
      }
    }
}
