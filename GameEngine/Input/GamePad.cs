using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IGameInputWithDPad : IGameInput
    {
        DPad Pad { get; }
    }
    
    public class DPad
    {
        private InputKey Left;
        private InputKey Right;
        private InputKey Up;
        private InputKey Down;
        private IGameInput InputDevice;

        public DPad(IGameInput inputDevice, InputKey left, InputKey right, InputKey up, InputKey down)
        {
            this.InputDevice = inputDevice;
            this.Left = left;
            this.Right = right;
            this.Up = up;
            this.Down = down;
        }

        public Vector2 GetInputVector()
        {
            float x = 0f, y = 0f;

            if (InputDevice.GetButtonDown(Left))
                x = -1;
            else if (InputDevice.GetButtonDown(Right))
                x = 1;

            if (InputDevice.GetButtonDown(Up))
                y = -1;
            else if (InputDevice.GetButtonDown(Down))
                y = 1;

            return new Vector2(x, y);
        }

        public Vector2 GetPressedVector()
        {
            float x = 0f, y = 0f;

            if (InputDevice.GetButtonPressed(Left))
                x = -1;
            else if (InputDevice.GetButtonPressed(Right))
                x = 1;

            if (InputDevice.GetButtonPressed(Up))
                y = -1;
            else if (InputDevice.GetButtonPressed(Down))
                y = 1;

            return new Vector2(x, y);
        }

        public Direction GetInputDirection(Direction emptyReturn, Axis axis = Axis.Any)
        {
            return GetInputVector().ToDirection(emptyReturn, axis);
        }

        public float GetInputAmount(Axis axis)
        {
            return GetInputVector().GetAxis(axis);
        }
    }
}
