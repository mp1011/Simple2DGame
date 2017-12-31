using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public static class VectorExtensions
    {
        public static Vector2 SnapTo(this Vector2 v, float snap)
        {
            return new Vector2((float)v.X.SnapTo(snap), (float)v.Y.SnapTo(snap));
        }
        public static double GetDegrees(this Vector2 v)
        {
            var rad = v.GetRadians();
            var ret = rad * (180.0 / Math.PI);

            while (ret < 0)
                ret += 360;

            while (ret >= 360)
                ret -= 360;

            return ret;
        }

        public static Vector2 DegreesToVector(this float degrees, float length)
        {
            var ret = RadToXY(degrees * (Math.PI / 180.0), length);
            return ret;
        }

        public static Vector2 DegreesToVector(this double degrees, float length)
        {
            var ret = RadToXY(degrees * (Math.PI / 180.0), length);
            return ret;
        }

        public static Vector2 SetDegrees(this Vector2 v, double degrees)
        {          
            var distance = v.Length();
            var ret = RadToXY(degrees * (Math.PI / 180.0), distance);
            return ret;
        }

        public static double GetAbsoluteDistanceTo(this Vector2 v1, Vector2 v2)
        {
            var diff = v2.Subtract(v1);
            var dist = diff.Length();
            return Math.Abs(dist);
        }

        /// <summary>
        /// Converts an angle in radians to a point
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector2 RadToXY(double angle, double length)
        {
            double a, b;

            double t = Math.Tan(angle);

            double c2 = length * length;
            double t2 = t * t;

            a = Math.Sqrt(c2 / (t2 + 1));
            b = Math.Sqrt(c2 - a * a);

            if (angle < (Math.PI / 2))
            {
                b *= -1;
            }
            if (angle >= (Math.PI / 2) && angle < Math.PI)
            {
                a *= -1;
                b *= -1;
            }
            else if (angle >= Math.PI && angle < ((Math.PI / 2) + Math.PI))
            {
                a *= -1;
            }
            else if (angle >= ((Math.PI / 2) + Math.PI))
            {
                ;
            }

            var f = 1f;
            if (length < 0)
                f = -1f;


            return new Vector2((float)Math.Round(a, 4) * f, (float)Math.Round(b, 4) * f);
        }

        public static double GetRadians(this Vector2 v)
        {
            double a, b;
            double angle;

            a = v.X;
            b = v.Y;

            if (a == 0 && b == 0)
                return 0;

            angle = Math.Atan(Math.Abs(b) / Math.Abs(a));

            if (a >= 0 && b >= 0)
            {
                angle *= -1;
            }
            else if (a < 0 && b >= 0)
            {
                angle += Math.PI;
            }
            else if (a >= 0 && b < 0)
            {

            }
            else if (a < 0 && b < 0)
            {
                angle = Math.PI - angle;
            }

            while (angle < 0)
                angle += Math.PI * 2;

            while (angle >= Math.PI * 2)
                angle -= Math.PI * 2;

            return angle;
        }

        public static Vector2 SetLength(this Vector2 v, float length)
        {
            var angle = v.GetRadians();
            return CreateVector(angle, length);
        }

        public static Vector2 SetLength(this Vector2 v, double length)
        {
            return v.SetLength((float)length);
        }

        public static Vector2 CreateVector(double rad, float length)
        {
            double a, b;

            double t = Math.Tan(rad);

            double c2 = length * length;
            double t2 = t * t;

            a = Math.Sqrt(c2 / (t2 + 1));
            b = Math.Sqrt(Math.Max(0, c2 - a * a));

            if (rad < (Math.PI / 2))
            {
                b *= -1;
            }
            if (rad >= (Math.PI / 2) && rad < Math.PI)
            {
                a *= -1;
                b *= -1;
            }
            else if (rad >= Math.PI && rad < ((Math.PI / 2) + Math.PI))
            {
                a *= -1;
            }
            else if (rad >= ((Math.PI / 2) + Math.PI))
            {
                ;
            }

            var f = 1f;
            if (length < 0)
                f = -1f;

            return new Vector2((float)Math.Round(a, 4) * f, (float)Math.Round(b, 4) * f);
        }

        public static Vector2 Scale(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vector2 Scale(this Vector2 v1, Point v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vector2 Scale(this Vector2 v1, float scale)
        {
            return new Vector2(v1.X * scale, v1.Y * scale);
        }

        public static Vector2 Scale(this Vector2 v1, double scale)
        {
            return new Vector2(v1.X * (float)scale, v1.Y * (float)scale);
        }
        
        public static Vector2 Subtract(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 Translate(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 Translate(this Vector2 v1, float x, float y)
        {
            return new Vector2(v1.X + x, v1.Y + y);
        }

        public static Vector2 Translate(this Vector2 v1, Point v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static double GetDegreesTo(this Vector2 v1, Vector2 v2)
        {
            var diff = v2.Subtract(v1);
            var ret = diff.GetDegrees();
            return ret;
        }

        public static Rectangle ToGridCell(this Vector2 v, Vector2 cellSize)
        {
            return new Rectangle(v.X * cellSize.X, v.Y * cellSize.Y, cellSize.X, cellSize.Y);
        }

        public static float GetAxis(this Vector2 v, Axis a)
        {
            if (a == Axis.X)
                return v.X;
            else
                return v.Y;
        }

        public static Vector2 SetAxis(this Vector2 v, Axis a, float value)
        {
            if (a == Axis.X)
            {
                return new Vector2(value, v.Y);
            }
            else
            {
                return new Vector2(v.X, value);
            }        
        }

        public static Direction ToDirection(this Vector2 v, Direction emptyDir, Axis axis=Axis.Any)
        {
            if (v.X < 0 && axis != Axis.Y)
                return Direction.Left;
            else if (v.X > 0 && axis != Axis.Y)
                return Direction.Right;
            else if (v.Y < 0 && axis != Axis.X)
                return Direction.Up;
            else if (v.Y > 0 && axis != Axis.X)
                return Direction.Down;

            return emptyDir;
        }

        public static Vector2 FlipIfLeft(this Vector2 v, Direction d)
        {
            if (d == Direction.Left)
                return new Vector2(-v.X, v.Y);
            else
                return v;
        }
        public static Vector2 Flip(this Vector2 v, bool flipX, bool flipY)
        {
            return new Vector2(flipX ? -v.X : v.X, flipY ? -v.Y: v.Y);
        }

        public static Vector2 ToXY(this int number, int columns)
        {
            if (columns <= 0)
                return new Vector2(number, 0);

            int x = number;
            int y = 0;
            while (x >= columns)
            {
                x -= columns;
                y++;
            }
            return new Vector2(x, y);
        }

        public static Vector2 GetAdjacent(this Vector2 vector, Direction dir)
        {
            return vector.Translate(dir.ToXY());
        }

        public static Vector2 GetAdjacent(this Vector2 vector, BorderSide side)
        {
            switch(side)
            {
                case BorderSide.Top: return vector.Translate(0, -1);
                case BorderSide.Left: return vector.Translate(-1, 0);
                case BorderSide.Right: return vector.Translate(1, 0);
                case BorderSide.Bottom: return vector.Translate(0, 1);
              
                case BorderSide.TopLeftCorner: return vector.Translate(-1, -1);
                case BorderSide.TopRightCorner: return vector.Translate(1, -1);
                case BorderSide.BottomLeftCorner: return vector.Translate(-1, 1);
                case BorderSide.BottomRightCorner: return vector.Translate(1, 1);

                case BorderSide.None: return vector.Translate(0, 0);

                default: throw new NotImplementedException();
            }
        }

        public static Vector2[] GetAdjacentPoints(this Vector2 vector, bool includeDiagonal)
        {
            var sides = new List<BorderSide> { BorderSide.Left, BorderSide.Top, BorderSide.Right, BorderSide.Bottom };
            if (includeDiagonal)
            {
                sides.Add(BorderSide.TopLeftCorner);
                sides.Add(BorderSide.TopRightCorner);
                sides.Add(BorderSide.BottomLeftCorner);
                sides.Add(BorderSide.BottomRightCorner);
            }

            return sides.Select(side => vector.GetAdjacent(side)).ToArray();
        }

        public static BorderSide GetBorderSide(this Vector2 vector, Rectangle area)
        {
            if (vector.Y == area.Top)
            {
                if (vector.X == area.Left)
                    return BorderSide.Right | BorderSide.Bottom;
                else if (vector.X == area.Right - 1)
                    return BorderSide.Left | BorderSide.Bottom;
                else
                    return BorderSide.Left | BorderSide.Right | BorderSide.Bottom;
            }
            else if (vector.Y == area.Bottom - 1)
            {
                if (vector.X == area.Left)
                    return BorderSide.Right | BorderSide.Top;
                else if (vector.X == area.Right - 1)
                    return BorderSide.Left | BorderSide.Top;
                else
                    return BorderSide.Left | BorderSide.Right | BorderSide.Top;
            }
            else if (vector.X == area.Left)
                return BorderSide.Right | BorderSide.Top | BorderSide.Bottom;
            else if(vector.X == area.Right-1)
                return BorderSide.Left | BorderSide.Top | BorderSide.Bottom;
            else if (area.Contains(vector))
                return BorderSide.AllSides;
            else
                return BorderSide.None;

          

        }
     
        /// <summary>
        /// Returns how far the vector would need to translate in the given direction before it would pass the other
        /// </summary>
        /// <param name="position"></param>
        /// <param name="other"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float GetDistanceInDirection(this Vector2 position, Vector2 other, Direction d)
        {
            switch(d)
            {
                case Direction.Right: return other.X - position.X;
                case Direction.Left: return position.X - other.X;
                case Direction.None: return 0f;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
