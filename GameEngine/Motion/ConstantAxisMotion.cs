using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class ConstantAxisMotion : IMotionAdjuster
    {
        public float DistancePerSecond { get; set; }

        public Direction Direction { get; set; }
        private Vector2 _mps = Vector2.Zero;
        
        public ConstantAxisMotion(string name, IMoveable objectToMove)
        {
            _name = name;
            objectToMove.Motion.Forces.Add(this);     
        }

        private string _name;
        string IMotionAdjuster.Name => _name;

        bool IMotionAdjuster.Active { get; set; } = true;
        
        Vector2 IMotionAdjuster.AdjustMotionPerSecond(TimeSpan elapsedInFrame, Vector2 currentMotionPerSecond)
        {
            var axis = Direction.Axis();
            var axisMotion = currentMotionPerSecond.GetAxis(axis).Abs();

            if (axisMotion >= DistancePerSecond)
                return currentMotionPerSecond;

            var dps = DistancePerSecond;
            if (Direction == Direction.Left || Direction == Direction.Up)
                dps *= -1;

            return currentMotionPerSecond.SetAxis(axis, dps);
        }
    }
}
