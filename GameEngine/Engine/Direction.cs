using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    //numbers match with BorderSide
    public enum Direction
    {
        None = 0,
        Right=4,
        Left=1,
        Up=2,
        Down=8,
    }

    public static class DirectionExtensions
    {
        public static BorderSide ToSide(this Direction d)
        {
            switch(d)
            {
                case Direction.Left: return BorderSide.Left;
                case Direction.Right: return BorderSide.Right;
                case Direction.Up: return BorderSide.Top;
                case Direction.Down: return BorderSide.Bottom;
                default: return BorderSide.None;
            }
        }

        public static int ToFlipMod(this Direction d)
        {
            if (d == Direction.Left)
                return -1;
            else
                return 1;
        }

        public static Direction Opposite(this Direction d)
        {
            switch (d)
            {
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                case Direction.Up: return Direction.Down;
                default:
                    return d;
            }
        }

        public static Direction CardinalDirectionTowards(this IWithPosition thisObject, IWithPosition other, Axis axis= GameEngine.Axis.Any)
        {
            var thisPos = thisObject.Position.Center;
            var otherPos = other.Position.Center;

            bool right = thisPos.X < otherPos.X;
            bool left = thisPos.X > otherPos.X;
            bool down = thisPos.Y < otherPos.Y;
            bool up = thisPos.Y > otherPos.Y;

            if (axis == GameEngine.Axis.X)
            {
                if (left)
                    return Direction.Left;
                else
                    return Direction.Right;
            }
            else if(axis == GameEngine.Axis.Y)
            {
                if (up)
                    return Direction.Up;
                else
                    return Direction.Down;
            }
            else
            {
                if (left && !up && !down)
                    return Direction.Left;
                else if (right && !up && !down)
                    return Direction.Right;
                else if (down && !left && !right)
                    return Direction.Down;
                else if (up && !left && !right)
                    return Direction.Up;
                else
                    return Direction.None;                     
            }
            
        }


        public static Direction DirectionAwayFrom(this IWithPosition thisObject, IWithPosition other, Axis axis)
        {
            return thisObject.CardinalDirectionTowards(other, axis).Opposite();
        }

        public static Axis Axis(this Direction d)
        {
            switch (d)
            {
                case Direction.Down: return GameEngine.Axis.Y;
                case Direction.Up: return GameEngine.Axis.Y;
                default:
                    return GameEngine.Axis.X;
            }
        }

        public static Vector2 ToXY(this Direction d)
        {
            switch (d)
            {
                case Direction.Down: return new Vector2(0, 1);
                case Direction.Left: return new Vector2(-1, 0);
                case Direction.Right: return new Vector2(1, 0);
                case Direction.Up: return new Vector2(0, -1);
                default:
                    return Vector2.Zero;
            }
        }
    }

}
