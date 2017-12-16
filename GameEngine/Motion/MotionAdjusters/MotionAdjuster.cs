using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class MotionAdjuster
    {
        public ICondition Condition { get; set; }

        public MotionAdjuster() : this(GameEngine.Condition.True)
        { }

        public MotionAdjuster(ICondition condition)
        {
            Condition = condition;
        }

        public void Update(IMoveable movingObject, InterpolatedVector motion)
        {
            if (Condition.IsActiveAndNotNull())
                AdjustMotion(movingObject, motion);
        }

        protected abstract void AdjustMotion(IMoveable movingObject, InterpolatedVector motion);

        public void ActivateWhen<T>(T arg, Predicate<T> condition)
        {
            Condition = new LambdaCondition<T>(arg, condition);
        }

        public bool Active
        {
            get
            {
                return Condition.IsActiveAndNotNull();
            }
            set
            {
                if (Condition.IsActiveAndNotNull() != value)
                {
                    var cc = Condition as ConstantCondition;
                    if (cc != null)
                        Condition = new ManualCondition();

                    var mc = Condition as ManualCondition;
                    if (mc == null)
                        throw new Exception("Condition cannot be changed");

                    mc.Active = value;
                }
            }
        }
    }
       
}
