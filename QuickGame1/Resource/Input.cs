using GameEngine;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    public static class GameKeys
    {
        public static InputKey Up = new InputKey("Up");
        public static InputKey Down = new InputKey("Down");
        public static InputKey Left = new InputKey("Left");
        public static InputKey Right = new InputKey("Right");
        public static InputKey Jump = new InputKey("Jump");
        public static InputKey Attack = new InputKey("Attack");
        public static InputKey Start = new InputKey("Start");

        public static InputKey PlaceObject = new InputKey("PlaceObject");
        public static InputKey CopyObject = new InputKey("CopyObject");
        public static InputKey EditorMenu = new InputKey("EditorMenu");

    }

    class Input
    {
        private static XNAMouseInputDevice _mouseInput;
        private static XNAKeyboardInputDevice _input;
        private static Scene lastScene;

        public static IGameInputWithDPad GetInput(Scene scene)
        {
            if (_input != null && scene == lastScene)
                return _input;

            _mouseInput = null;

            var keyMap = new XNAKeyboardInputMap();
            keyMap.SetMapping(GameKeys.Jump, Keys.S);
            keyMap.SetMapping(GameKeys.Attack, Keys.D);

            keyMap.SetMapping(GameKeys.Left, Keys.Left);
            keyMap.SetMapping(GameKeys.Right, Keys.Right);
            keyMap.SetMapping(GameKeys.Up, Keys.Up);
            keyMap.SetMapping(GameKeys.Down, Keys.Down);
            keyMap.SetMapping(GameKeys.Start, Keys.Space);

            keyMap.SetMapping(GameKeys.EditorMenu, Keys.M);

            _input = new XNAKeyboardInputDevice(keyMap, GameKeys.Left, GameKeys.Right, GameKeys.Up, GameKeys.Down, scene);
            lastScene = scene;
            return _input;
        }

        public static IMouseInput GetMouseInput(Scene scene)
        {
            if (_mouseInput != null)
                return _mouseInput;

            var mouseButtonMap = new InputMap<MouseButton>();
            mouseButtonMap.SetMapping(GameKeys.PlaceObject, MouseButton.Left);
            mouseButtonMap.SetMapping(GameKeys.CopyObject, MouseButton.Right);

            _mouseInput = new XNAMouseInputDevice(mouseButtonMap, scene);

            return _mouseInput;
        }
    }
}
