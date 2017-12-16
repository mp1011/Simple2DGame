using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class AxisMotion : MotionAdjuster
    {
        private AxisMotionConfig StartConfig;
        private AxisMotionConfig TargetConfig;
        
        public AxisMotion(AxisMotionConfig start, AxisMotionConfig target, ICondition condition) : base(condition)
        {
            StartConfig = start;
            TargetConfig = target;
        }

        public AxisMotion(AxisMotionConfig start = null, AxisMotionConfig target = null) 
        {
            StartConfig = start;
            TargetConfig = target;
        }


        protected override void AdjustMotion(IMoveable movingObject, InterpolatedVector motion)
        {
            if(StartConfig != null)
                motion.GetAxis(StartConfig.Axis).Current = StartConfig.GetStartSpeed(movingObject);

            if (TargetConfig != null)
                motion.GetAxis(TargetConfig.Axis).SetTarget(TargetConfig.GetTargetSpeed(movingObject), TargetConfig.Change.Value);
        }

        public override string ToString()
        {
            return (StartConfig ?? TargetConfig).ToString();
        }
    }
}
