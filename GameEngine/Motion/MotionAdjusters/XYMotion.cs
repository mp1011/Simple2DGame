using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class XYMotion : MotionAdjuster
    {
        private XYMotionConfig Config;

        public XYMotion(XYMotionConfig config = null) : base()
        {
            Config = config;
        }

        protected override void AdjustMotion(IMoveable movingObject, InterpolatedVector motion)
        {           
            motion.X.Current = Config.MotionPerSecond.X;
            motion.Y.Current = Config.MotionPerSecond.Y;            
        }
    }
}
