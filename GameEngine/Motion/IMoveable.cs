using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IMoveable : IWithPositionAndDirection
    {
        MotionManager Motion { get; }
    }

    public static class IMoveableExtensions
    {
        /// <summary>
        /// Constant motion in the given direction
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="motionPerSecond"></param>
        public static void MoveInDirection(this IMoveable objectToMove, Direction dir, float motionPerSecond)
        {
            var xy = dir.ToXY() * motionPerSecond;

            objectToMove.Motion.AdjustImmediately(m =>
            {
                m.X.Current = xy.X;
                m.Y.Current = xy.Y;
                m.X.Target = xy.X;
                m.Y.Target = xy.Y;
            });

            objectToMove.Direction = dir;
        }

        /// <summary>
        /// Constant motion in the given direction
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="motionPerSecond"></param>
        public static void MoveInDirection(this IMoveable objectToMove, double degrees, float motionPerSecond)
        {
            var xy = degrees.DegreesToVector(motionPerSecond);
            objectToMove.Motion.AdjustImmediately(m =>
            {
                m.X.Current = xy.X;
                m.Y.Current = xy.Y;
                m.X.Target = xy.X;
                m.Y.Target = xy.Y;
            });
        }

        /// <summary>
        /// One-time push in the given direction 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="motionPerSecond"></param>
        public static void PushInDirection(this IMoveable objectToMove, Direction dir, float motionPerSecond, bool additive=false)
        {
            var xy = dir.ToXY() * motionPerSecond;
            objectToMove.Motion.AdjustImmediately(m =>
            {
                if (additive)
                {
                    m.X.Current += xy.X;
                    m.Y.Current += xy.Y;
                }
                else
                {
                    m.X.Current = xy.X;
                    m.Y.Current = xy.Y;
                }
            });
        }

        public static void Jump(this IMoveable m, float jumpStrength)
        {
            m.PushInDirection(Direction.Up, jumpStrength);
        }
    }
  
}
