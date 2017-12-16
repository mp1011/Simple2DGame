using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameEngine
{

    public class XYMotionConfig
    {
        public Vector2 MotionPerSecond;
        public Direction FlipXWhen;
        public Direction FlipYWhen;

        public Vector2 GetMotionPerSecond(IMoveable objectToMove)
        {
            int xFlipAdjust = (objectToMove.Direction == FlipXWhen) ? -1 : 1;
            int yFlipAdjust = (objectToMove.Direction == FlipYWhen) ? -1 : 1;

            return new Vector2(MotionPerSecond.X * xFlipAdjust, MotionPerSecond.Y * yFlipAdjust);
        }
    }

    public class AxisMotionConfig
    {
        public Axis Axis { get; set; }
        public float StartSpeed { get; set; }
        public float TargetSpeed { get; set; }
        public ScaledFloat Change { get; set; }
        public bool DeactivateAfterStart { get; set; }
        public bool DeactivateWhenTargetReached { get; set; }
        public Direction FlipWhen { get; set; }
        public float Vary { get; set; }

        public override string ToString()
        {
            return $"{Axis} {StartSpeed}->{TargetSpeed}";
        }

        public float GetStartSpeed(IMoveable objectToMove)
        {
            int flipAdjust = (FlipWhen != Direction.None && objectToMove.Direction == FlipWhen) ? -1 : 1;
            return StartSpeed * flipAdjust;
        }

        public float GetTargetSpeed(IMoveable objectToMove)
        {
            int flipAdjust = (FlipWhen != Direction.None && objectToMove.Direction == FlipWhen) ? -1 : 1;
            return TargetSpeed * flipAdjust;
        }
    }
}
