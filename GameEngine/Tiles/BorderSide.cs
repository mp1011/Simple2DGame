using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    [Flags]
    public enum BorderSide
    {
        None = 0,
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8,
        AllSides = 15,
        TopLeftCorner=16,
        TopRightCorner=32,
        BottomLeftCorner=64,
        BottomRightCorner=128,
        AllSidesAndCorners=256,
        AllCorners=TopLeftCorner+TopRightCorner+BottomLeftCorner+BottomRightCorner,
        EmptySpace=512,
        NotTopLeftCorner=TopRightCorner + BottomLeftCorner + BottomRightCorner,
        NotTopRightCorner = TopLeftCorner + BottomLeftCorner + BottomRightCorner,
        NotBottomLeftCorner = TopRightCorner + BottomRightCorner + TopLeftCorner,
        NotBottomRightCorner = TopLeftCorner + TopRightCorner + BottomLeftCorner
    }

    public static class BorderSideUtil
    {
        public static BorderSide WithoutCorners(this BorderSide b)
        {
            return b & ~BorderSide.AllCorners;
        }

        public static BorderSide Opposite(this BorderSide b)
        {
            switch(b)
            {
                case BorderSide.Left: return BorderSide.Right;
                case BorderSide.Right: return BorderSide.Left;
                case BorderSide.Top: return BorderSide.Bottom;
                case BorderSide.Bottom: return BorderSide.Top;
                default: return b;
            }
        }
    }
}
