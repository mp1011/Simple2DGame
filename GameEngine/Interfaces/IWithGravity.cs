using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IWithGravity : IMoveable
    {
        ManualCondition GravityOn { get; }
    }

    public static class GravityExtensions
    {
        public static void AddGravity(this IWithGravity actor, string configKey = "gravity")
        {
            var gravity = Config.ReadValue<AxisMotionConfig>(configKey);
            actor.Motion.AddAdjuster(new AxisMotion(gravity, actor.GravityOn));
        }
    }
}
