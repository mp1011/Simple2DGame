using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IWithPosition
    {
        Rectangle Position { get; }
    }

    public interface IWithPositionAndDirection : IWithPosition
    {
        Direction Direction { get; set; }
    }

    public static class PositionExtensions
    {
        public static void SetPosition(this IWithPosition pos, int x, int y)
        {
            pos.Position.UpperLeft = new Vector2(x, y);
        }

        public static void SetPosition(this IWithPosition pos, float x, float y)
        {
            pos.Position.UpperLeft = new Vector2(x, y);
        }

        public static void Face(this IWithPositionAndDirection obj, IWithPosition other)
        {
            if (obj.Position.Center.X < other.Position.Center.X)
                obj.Direction = Direction.Right;
            else if (obj.Position.Center.X > other.Position.Center.X)
                obj.Direction = Direction.Left;
        }

        public static void AlwaysFace<T>(this T obj, IWithPosition other, ICondition condition)
            where T:IWorldObject
        {
            new LambdaAction<T>(obj, UpdatePriority.Behavior, obj.Layer.Scene, condition, (o, t) =>
            {
                obj.Face(other);
            });
        }

        public static void PositionRelativeTo(this IWithPositionAndDirection obj, IWithPositionAndDirection parent, int offsetX, int offsetY)
        {
            var offset = new Vector2(offsetX, offsetY);
            obj.Position.Center = parent.Position.Center.Translate(offset.FlipIfLeft(parent.Direction));
            obj.Direction = parent.Direction;
        }
    }

   
}
