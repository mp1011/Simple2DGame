using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    interface IBounces : IMovingWorldObject
    {
    }

    static class IBouncesExtensions
    {
        public static AxisMotion GetBounceMotion(this IBounces actor)
        {
            var motion = actor.Motion.GetMotionByName<AxisMotion>("bounce motion") ?? new AxisMotion("bounce motion", actor).Set(deactivateAfterStart: true);
            motion.Active = false;
            return motion;
        }
    }
}
