using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public static class GlobalDebugHelper
    {
        private static DateTime lastPlayerMoveTime;

        private static string _debugString;

        public static string DebugString
        {
            get
            {
                return _debugString;
            }
            set
            {
                if(value != _debugString)
                {
                    _debugString = value;
                    System.Diagnostics.Debug.WriteLine("DEBUG: " + _debugString);
                }
            }
        }

        private static bool _playerIsMoving;
        public static bool PlayerIsMoving
        {
            get
            {
                return _playerIsMoving;
            }
            set
            {
                _playerIsMoving = value;
                if (value)
                    lastPlayerMoveTime = DateTime.Now;
            }
        }

        public static bool PlayerIsOrWasMoving
        {
            get
            {
                return PlayerIsMoving || (DateTime.Now - lastPlayerMoveTime).TotalSeconds < 2;
            }
        }

        public static void NoOp() { }
    }
}
