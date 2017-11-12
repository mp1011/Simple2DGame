using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    static class Motion
    {
        public static Actor AddGravity(this MovingActor actor, string configKey="gravity")
        {
            var gravity = new AxisMotion(configKey, actor, new ConfigValue<AxisMotionConfig>(configKey).Value);
            gravity.Active = true;
            return actor;
        }
    }
}
