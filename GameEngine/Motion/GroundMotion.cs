using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class GroundMotion<T> : AxisMotion
        where T : IMoveable, IPlatformerObject
    {
        public GroundMotion(T movingObject, AxisMotionConfig Config)
            : base(Config, movingObject.IsOnGround)
        {
        }
    }
}
