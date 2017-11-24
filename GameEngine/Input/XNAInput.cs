using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class XNAKeyboardInputMap : InputMap<Keys>
    {
    }

    public class XNAKeyboardInputDevice : InputDevice<Keys>, IGameInputWithDPad
    {
        private KeyboardState StateA;
        private KeyboardState StateB;
        bool state;

        private KeyboardState LastState
        {
            get
            {
                return state ? StateB : StateA;
            }
        }

        private KeyboardState CurrentState
        {
            get
            {
                return state ? StateA : StateB;
            }
            set
            {
                if (state)
                    StateA = value;
                else
                    StateB = value;
            }
        }

        private DPad dPad;
        DPad IGameInputWithDPad.Pad { get { return dPad; } }

        public XNAKeyboardInputDevice(InputMap<Keys> inputMap, InputKey left, InputKey right, InputKey up, InputKey down, Scene scene) : base(inputMap, scene)
        {
            dPad = new DPad(this, left, right, up, down);
        }

        protected override object GetPressedButton()
        {
            var ret = CurrentState.GetPressedKeys().FirstOrDefault();
            if (ret == Keys.None)
                return null;
            else
                return ret;
        }

        protected override bool GetButtonDown(Keys key)
        {
            return CurrentState.IsKeyDown(key);
        }

        protected override bool GetButtonPressed(Keys key)
        {
            return GetButtonDown(key) && !LastState.IsKeyDown(key);
        }

        protected override void Update(TimeSpan frameTime)
        {
            state = !state;
            CurrentState = Keyboard.GetState();
        }
    }

    public enum MouseButton
    {
        None,
        Left,
        Right
    }

    public class XNAMouseInputDevice : InputDevice<MouseButton>, IMouseInput
    {
        public Vector2 MousePosition { get; private set; } = Vector2.Zero;

        public XNAMouseInputDevice(InputMap<MouseButton> inputMap, Scene scene) : base(inputMap, scene)
        {
        }

        private MouseState currentState;
        private MouseState lastState;

        protected override void Update(TimeSpan frameTime)
        {
            lastState = currentState;
            currentState = Mouse.GetState();

            var screenPosition = currentState.Position.ToVector2();

            var window = Engine.Instance.WindowPosition;
            var scene = Engine.Instance.Renderer.ScreenBounds.Position;
            var scale = new Vector2((float)scene.Width / (float)window.Width, (float)scene.Height / (float)window.Height);
            MousePosition = screenPosition.Scale(scale);
        }

        protected override object GetPressedButton()
        {
            return MouseButton.None; //todo
        }

        protected override bool GetButtonDown(MouseButton key)
        {
            switch(key)
            {
                case MouseButton.Left: return currentState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right: return currentState.RightButton == ButtonState.Pressed;
                default: return false;
            }
        }

        protected override bool GetButtonPressed(MouseButton key)
        {
            switch (key)
            {
                case MouseButton.Left: return currentState.LeftButton == ButtonState.Pressed && lastState.LeftButton == ButtonState.Released;
                case MouseButton.Right: return currentState.RightButton == ButtonState.Pressed && lastState.RightButton == ButtonState.Released;
                default: return false;
            }
        }
    }
}
