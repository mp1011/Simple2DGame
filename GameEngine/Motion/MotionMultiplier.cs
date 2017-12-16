using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MotionMultiplier
    {
        private ICondition Condition;
        private ConfigValue<float> DeltaMod, TargetMod;

        public MotionMultiplier(ICondition condition, string name)
        {
            Condition = condition;
            DeltaMod = new ConfigValue<float>(name + " delta mod");
            TargetMod = new ConfigValue<float>(name + " target mod");
        }

        public InterpolatedVector Apply(InterpolatedVector v)
        {
            if (Condition.IsActive)
            {
                var ret = new InterpolatedVector();
                ret.X.Current = v.X.Current;
                ret.X.SetTarget(v.X.Target * TargetMod.Value, v.X.ChangePerSecond * DeltaMod.Value);

                ret.Y.Current = v.Y.Current;
                ret.Y.SetTarget(v.Y.Target * TargetMod.Value, v.Y.ChangePerSecond * DeltaMod.Value);
                
                return ret;
            }
            else
                return v;
        }

        public float GetDeltaMod()
        {
            if (Condition.IsActive)
                return DeltaMod.Value;
            else
                return 1f;
        }

        public float GetTargetMod()
        {
            if (Condition.IsActive)
                return TargetMod.Value;
            else
                return 1f;
        }
    }
}
