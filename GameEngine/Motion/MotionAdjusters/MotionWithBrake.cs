using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MotionWithBrake : MotionAdjuster
    {
        public MotionAdjuster Motion { get; private set; }
        public MotionAdjuster Brake { get; private set; }

        public MotionWithBrake(MotionAdjuster motion, MotionAdjuster brake) 
            : base(GameEngine.Condition.True)
        {
            Motion = motion;
            Brake = brake; 
        }

        protected override void AdjustMotion(IMoveable movingObject, InterpolatedVector motion)
        {
            if (!Motion.Active)
                Brake.Update(movingObject, motion);
            else 
                Motion.Update(movingObject, motion);
        }


    }
}
