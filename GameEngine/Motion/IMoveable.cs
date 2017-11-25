using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IMoveable : IWithPositionAndDirection
    {
        MotionManager Motion { get; }
    }

    public static class IMoveableExtensions
    {
        public static Vector2 FrameMotion(this IMoveable m)
        {
            return m.Position.Center.Subtract(m.Motion.FrameStartPosition.Center);
        }

        public static void MoveInDirection(this IMoveable m, Direction d, int speed)
        {
            m.Direction = d;

            var xyMotion = m.Motion.Forces.OfType<XYMotion>().FirstOrDefault();
            if(xyMotion == null)
            {
                xyMotion = new XYMotion(m.ToString() + " XY", m);
                m.Motion.Forces.Add(xyMotion);
            }

            xyMotion.MotionPerSecond = d.ToXY().Scale(speed);
        }

        public static void MoveInDirection(this IMoveable m, Vector2 unitVector, int speed)
        {
            var xyMotion = m.Motion.Forces.OfType<XYMotion>().FirstOrDefault();
            if (xyMotion == null)            
                xyMotion = new XYMotion(m.ToString() + " XY", m);
            
            xyMotion.MotionPerSecond = unitVector.Scale(speed);
        }

        public static void PushInDirection(this IMoveable m, Direction d, ConfigValue<AxisMotionConfig> motionConfig)
        {
            var motion = m.Motion.Forces.OfType<AxisMotion>().FirstOrDefault(p => p.Name == motionConfig.Name);
            if (motion == null)
            {
                motion = new AxisMotion(motionConfig.Name, m, motionConfig.Value).Set(deactivateWhenTargetReached:true,flipWhen: Direction.Left);
            }

            m.Direction = d;
            motion.Active = true;
        }

        public static void Jump(this IMoveable m, ConfigValue<AxisMotionConfig> config)
        {
            var jump = m.Motion.Forces.OfType<AxisMotion>().FirstOrDefault(p => p.Name == config.Name);
            if (jump == null)
            {
                jump = new AxisMotion(config.Name, m, config.Value).Set(deactivateAfterStart: true);              
            }

            jump.Active = true;
            
        }
    }
}
