using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class AxisMotion : MotionAdjuster
    {
        private AxisMotionConfig Config;
        
        public AxisMotion(AxisMotionConfig config, ICondition condition) : base(condition)
        {
            Config = config;
        }

        public AxisMotion(AxisMotionConfig config = null) 
        {
            Config = config;
        }

        protected override void OnActivated()
        {
            setStart = false;
        }

        private bool setStart = false;
        protected override void AdjustMotion(IMoveable movingObject, InterpolatedVector motion)
        {
            if (!setStart)
            {
                setStart = true;
                motion.GetAxis(Config.Axis).Current = Config.GetStartSpeed(movingObject);
            }

            motion.GetAxis(Config.Axis).SetTarget(Config.GetTargetSpeed(movingObject), Config.Change.Value);
        }

        public override string ToString()
        {
            return Config.ToString();
        }
    }
}
