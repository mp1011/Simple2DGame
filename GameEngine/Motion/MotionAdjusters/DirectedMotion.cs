using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{ 
    public class DirectedMotion : MotionAdjuster
    {
        public float DistancePerSecond { get; set; }
        public double AngleInDegrees { get; set; }
        
        protected override void AdjustMotion(IMoveable movingObject, InterpolatedVector motion)
        {
            var speed = motion.Current;
            speed = speed.SetDegrees(AngleInDegrees).SetLength(DistancePerSecond);

            motion.X.Current = speed.X;
            motion.Y.Current = speed.Y;
        }
    }
}
