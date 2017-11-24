using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Rectangle
    {
        public override string ToString()
        {
            return $"{Center.ToString()} ({Width}x{Height})";
        }

        public static Rectangle Empty()
        {
            return new Rectangle();
        }

        public override bool Equals(object obj)
        {
            var otherRec = obj as Rectangle;
            if (otherRec != null)
                return Left == otherRec.Left && Right == otherRec.Right
                    && Top == otherRec.Top && Bottom == otherRec.Bottom;

            return false;
        }

        public override int GetHashCode()
        {
            return (int)(Left * Top * Right * Bottom);
        }

        public double Left { get; private set; }
        public double Top { get; private set; }
        public double Right { get; private set; }
        public double Bottom { get; private set; }

        public double Width { get { return Right - Left; } }
        public double Height { get { return Bottom - Top; } }

        public Rectangle() { }

        public Rectangle(double x, double y, double w, double h)
        {
            Left = x;
            Top = y;
            Right = x + w;
            Bottom = y + h;
        }

        public Rectangle(Vector2 size)
        {
            Right = size.X;
            Bottom = size.Y;
        }

        public void Set(Microsoft.Xna.Framework.Rectangle r)
        {
            Left = r.Left;
            Top = r.Top;
            Right = r.Right;
            Bottom = r.Bottom;
        }

        public void Set(double x, double y, double w, double h)
        {
            Left = x;
            Top = y;
            Right = x + w;
            Bottom = y + h;
        }

        public void Set(Rectangle other)
        {
            Left = other.Left;
            Top = other.Top;
            Right = other.Right;
            Bottom = other.Bottom;
        }

        public Rectangle CeilingInflate()
        {
            var newLeft = Math.Floor(Left);
            var newTop = Math.Floor(Top);
            var newRight = Math.Ceiling(Right);
            var newBottom = Math.Ceiling(Bottom);
            return new Rectangle(newLeft, newTop, newRight - newLeft, newBottom - newTop);
        }

        public void SetWidth(double width, AnchorOrigin anchor)
        {
            if (anchor == AnchorOrigin.Left)
            {
                Right = Left + width;
            }
            else if (anchor == AnchorOrigin.Center)
            {
                var center = Center;
                Right = Left + width;
                Center = center;
            }
            else throw new NotImplementedException();
        }

        public void SetHeight(double height, AnchorOrigin anchor)
        {
            if (anchor == AnchorOrigin.Top)
            {
                Bottom = Top + height;
            }
            else if (anchor == AnchorOrigin.Bottom)
            {
                var bottom = BottomCenter;
                Bottom = Top + height;
                BottomCenter = bottom;
            }
            else if (anchor == AnchorOrigin.Center)
            {
                var center = Center;
                Bottom = Top + height;
                Center = center;
            }
            else
                throw new NotImplementedException();
        }

        public Rectangle Translate(double x, double y)
        {
            this.Left += x;
            this.Right += x;
            this.Top += y;
            this.Bottom += y;
            return this;
        }

        public Rectangle FlipFromCenter(bool flipX, bool flipY)
        {
            if (flipX)
            {
                var centerToLeft = this.Center.X - this.Left;

                var newCenterToLeft = this.Width - centerToLeft;
                SetLeft(this.Center.X - newCenterToLeft);
            }

            if (flipY)
                throw new NotImplementedException();


            return this;
        }


        public Rectangle FlipFromTopLeft(bool flipX, bool flipY)
        {
            if (flipX)
            {
                SetLeft(this.Left - this.Width);
            }

            if (flipY)
                throw new NotImplementedException();


            return this;
        }

        public Rectangle Translate(Vector2 vector)
        {
            return Translate(vector.X, vector.Y);
        }

        public Rectangle SetCorner(float left, float top)
        {
            SetLeft(left);
            SetTop(top);
            return this;
        }

        public Rectangle SetCorner(Vector2 value)
        {
            SetLeft(value.X);
            SetTop(value.Y);
            return this;
        }

        public void SetLeft(int value, bool maintainWidth = true)
        {
            var w = this.Width;
            Left = value;
            if (maintainWidth)
                Right = Left + w;
        }

        public void SetRight(int value, bool maintainWidth = true)
        {
            if (maintainWidth)
                SetLeft(value - Width);
            else
                Right = value;
        }

        public void SetRight(double value, bool maintainWidth = true)
        {
            if (maintainWidth)
                SetLeft(value - Width);
            else
                Right = value;
        }

        public void SetBottom(int value, bool maintainHeight = true)
        {
            if (maintainHeight)
                SetTop(value - Height);
            else
                Bottom = value;
        }

        public void SetBottom(double value, bool maintainHeight = true)
        {
            if (maintainHeight)
                SetTop(value - Height);
            else
                Bottom = value;
        }

        public void SetTop(int value, bool maintainHeight = true)
        {
            var h = this.Height;
            Top = value;
            if (maintainHeight)
                Bottom = Top + h;
        }

        public void SetLeft(double value, bool maintainWidth = true)
        {
            var w = this.Width;
            Left = value;
            if (maintainWidth)
                Right = Left + w;
        }

        public void SetTop(double value, bool maintainHeight = true)
        {
            var h = this.Height;
            Top = value;
            if (maintainHeight)
                Bottom = Top + h;
        }

        #region Anchor Points

        public Vector2 Center
        {
            get
            {
                return new Vector2((float)(Left + (Width / 2.0)), (float)(Top + (Height / 2.0)));
            }
            set
            {
                this.SetLeft(value.X - (Width / 2));
                this.SetTop(value.Y - (Height / 2));
            }
        }

        public Vector2 UpperLeft
        {
            get
            {
                return new Vector2((float)(this.Left), (float)(this.Top));
            }
            set
            {
                SetLeft(value.X);
                SetTop(value.Y);
            }
        }

        public Vector2 CenterTop
        {
            get
            {
                return new Vector2((float)(Left + (Width / 2.0)), (float)Top);
            }
            set
            {
                this.SetLeft(value.X - (Width / 2));
                this.SetTop(value.Y);
            }
        }

        public Vector2 UpperRight
        {
            get
            {
                return new Vector2((float)Right, (float)Top);
            }
            set
            {
                SetRight(value.X);
                SetTop(value.Y);
            }
        }

        public Vector2 RightCenter
        {
            get
            {
                return new Vector2((float)Right, (float)(Top + (Height / 2.0)));
            }
            set
            {
                SetRight(value.X);
                SetTop(value.Y - (Height / 2));
            }
        }

        public Vector2 BottomRight
        {
            get
            {
                return new Vector2((float)(this.Right), (float)(this.Bottom));
            }
            set
            {
                var w = this.Width;
                var h = this.Height;
                this.SetLeft(value.X - w);
                this.SetTop(value.Y - h);
            }
        }

        public Vector2 BottomCenter
        {
            get
            {
                return new Vector2((float)(this.Left + (this.Width / 2.0)), (float)(this.Bottom));
            }
            set
            {
                this.SetLeft(value.X - (this.Width / 2));
                this.SetTop(value.Y - Height);
            }
        }

        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2((float)Left, (float)Bottom);
            }
            set
            {
                SetLeft(value.X);
                SetBottom(value.Y);
            }
        }

        public Vector2 LeftCenter
        {
            get
            {
                return new Vector2((float)Left, (float)(Top + (Height / 2.0)));
            }
            set
            {
                SetLeft(value.X);
                SetTop(value.Y - (Height / 2));
            }
        }

        #endregion

        public bool CollidesWith(Rectangle other, bool ignoreEdges)
        {
            if(ignoreEdges)
                return (Bottom > other.Top && Top < other.Bottom && Right > other.Left && Left < other.Right);
            else
                return (Bottom >= other.Top && Top <= other.Bottom && Right >= other.Left && Left <= other.Right);
        }


        public bool Contains(Vector2 point)
        {
            return point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;
        }

        public float GetDistanceTo(Rectangle other, Axis axis)
        {
            if (axis == Axis.X)
            {
                return Math.Abs(Center.X - other.Center.X);
            }
            else
            {
                return Math.Abs(Center.Y - other.Center.Y);
            }
        }

        public float GetEdgeDistanceTo(Rectangle other, Axis axis)
        {
            if (axis == Axis.X)
            {
                if (Center.X < other.Center.X)
                    return (float)(other.Left - Right);
                else
                    return (float)(Left - other.Right);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Vector2> GetPoints(Vector2 offset)
        {
            for (float y = (float)Top; y <= Bottom; y += offset.Y)
            {
                for (float x = (float)Left; x <= Right; x += offset.X)
                {
                    yield return new Vector2(x, y);
                }
            }
        }

        public Rectangle KeepWithin(Rectangle other, int distanceFromEdge=0)
        {
            if (Left < other.Left+distanceFromEdge)
                SetLeft(other.Left+distanceFromEdge);
            if (Top < other.Top+distanceFromEdge)
                SetTop(other.Top + distanceFromEdge);
            if (Right > other.Right-distanceFromEdge)
                SetLeft(other.Right - Width - distanceFromEdge);
            if (Bottom > other.Bottom - distanceFromEdge)
                SetTop(other.Bottom - Height - distanceFromEdge);

            return this;
        }

        public Rectangle ExpandToContain(Rectangle other)
        {
            Left = Math.Min(Left, other.Left);
            Top = Math.Min(Top, other.Top);
            Right = Math.Max(Right, other.Right);
            Bottom = Math.Max(Bottom, other.Bottom);

            return this;
        }

        public Rectangle Copy()
        {
            return new Rectangle(this.Left, this.Top, this.Width, this.Height);
        }
    }

}