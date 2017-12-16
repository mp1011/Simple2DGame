using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class InterpolatedNumber
    {
        public float TargetScale = 1f;

        public float Current { get; set; }

        private float _target;
        public float Target
        {
            get { return _target * TargetScale; }
            set { _target = value; }
        }

        public float DeltaScale = 1f;
        private float _cps;
        public float ChangePerSecond { get { return _cps * DeltaScale; } set { _cps = value; } }

        public InterpolatedNumber(float start, float target, float changePerSecond)
        {
            Current = start;
            Target = target;
            ChangePerSecond = changePerSecond;
        }

        public void Adjust(TimeSpan elapsedFrameTime)
        {
            float delta = (float)(ChangePerSecond * elapsedFrameTime.TotalSeconds);
            Current = Current.Approach(Target, delta);
        }

        public void SetTarget(float target, float changePerSecond)
        {
            Target = target;
            ChangePerSecond = changePerSecond;
        }
    }
}
