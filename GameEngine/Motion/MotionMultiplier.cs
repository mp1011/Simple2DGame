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
