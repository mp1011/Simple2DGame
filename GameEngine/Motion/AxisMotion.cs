using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class AxisMotionConfig
    {
        public Axis Axis { get; set; }
        public float StartSpeed { get; set; }
        public float TargetSpeed { get; set; }
        public ScaledFloat Change { get; set; }
        public bool DeactivateAfterStart { get; set; }
        public bool DeactivateWhenTargetReached { get; set; }
        public Direction FlipWhen { get; set; } 
        public float Vary { get; set; }
    }

    public class AxisMotion : IMotionAdjuster
    {
        public string Name { get; private set; }

        private bool justActivated = false;
        private bool active;
        public virtual bool Active
        {
            get
            {
                return active;
            }
            set
            {
                if (value && !active)
                {
                    justActivated = true;
                }

                active = value;
            }
        }

        private AxisMotionConfig Config;
        public Axis Axis => Config.Axis;

        public IMoveable ObjectToMove { get; private set; }
        
        public MotionMultiplier Multiplier;
        
        public AxisMotion(string name, IMoveable objectToMove, AxisMotionConfig config)
        {
            Name = name;
            Config = config;
            ObjectToMove = objectToMove;
            objectToMove.Motion.Forces.Add(this);
            Config.Change = Config.Change ?? new ScaledFloat(config.TargetSpeed,1f);
        }

        public AxisMotion(string name, IMoveable objectToMove) : this(name, objectToMove, new ConfigValue<AxisMotionConfig>(name).Value)
        {
        }

        public AxisMotion Set(bool? deactivateAfterStart=null, bool? deactivateWhenTargetReached=null, Direction? flipWhen=null)
        {
            if (deactivateAfterStart.HasValue)
                Config.DeactivateAfterStart = deactivateAfterStart.Value;

            if (deactivateWhenTargetReached.HasValue)
                Config.DeactivateWhenTargetReached = deactivateWhenTargetReached.Value;

            if (flipWhen.HasValue)
                Config.FlipWhen = flipWhen.Value;

            return this;
        }

        Vector2 IMotionAdjuster.AdjustMotionPerSecond(TimeSpan elapsedInFrame, Vector2 current)
        {
            float targetMultiplier = 1f;
            
            if (Multiplier != null)
            {
                Config.Change.Scale = Multiplier.GetDeltaMod();
                targetMultiplier = Multiplier.GetTargetMod();
            }

            var currentAxisSpeed = current.GetAxis(Axis);
            float newAxisSpeed = currentAxisSpeed;

            if (justActivated)
            {
                justActivated = false;

                if (Config.StartSpeed != 0)
                {
                    float start = Config.StartSpeed;
                    if (Config.Vary != 0)
                        start += Random.Get(Config.Vary);

                    newAxisSpeed = start * FlipAdjust() * targetMultiplier;

                }
            }
            else
            {
                var target = Config.TargetSpeed * FlipAdjust() * targetMultiplier;
                newAxisSpeed = currentAxisSpeed.Approach(target, Config.Change.Value * elapsedInFrame.TotalSeconds);
            }

            if (Config.DeactivateAfterStart)
                Active = false;

            if (Config.DeactivateWhenTargetReached && newAxisSpeed.IsCloseTo(Config.TargetSpeed * targetMultiplier))
            {
                newAxisSpeed = Config.TargetSpeed * FlipAdjust() * targetMultiplier; 
                Active = false;
            }

            var ret = current.SetAxis(Axis, newAxisSpeed);
            return ret;
        }

        public void SetStartSpeed(float newValue)
        {
            Config.StartSpeed = newValue;
            Active = true;
        }

        protected float FlipAdjust()
        {
            if (Config.FlipWhen != Direction.None && ObjectToMove.Direction == Config.FlipWhen)
                return -1;
            else
                return 1;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class InstantAxisMotion : IMotionAdjuster
    {
        public string Name { get { return new Guid().ToString(); } }

        bool IMotionAdjuster.Active
        {
            get { return true; }
            set { }
        }

        public Axis Axis;
        public float Speed;

        public InstantAxisMotion(Axis axis, float speed)
        {
            Axis = axis;
            Speed = speed;
        }

        Vector2 IMotionAdjuster.AdjustMotionPerSecond(TimeSpan elapsedInFrame, Vector2 current)
        {
            return Vector2.Zero.SetAxis(Axis, Speed);
        }
    }

}
