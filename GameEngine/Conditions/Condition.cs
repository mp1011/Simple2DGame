using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface ICondition
    {
        ulong LastChangedFrame { get; }
        bool IsActive { get; }
    }
    

    public interface ICondition<T> : ICondition
    {
        T Item { get; }
    }

    public class ConstantCondition : Condition
    {
        private bool constValue;
        public ConstantCondition(bool value)
        {
            constValue = value;
        }

        public override bool IsActive => constValue;
    }

    public abstract class Condition : ICondition
    {
        private ulong lastChangedFrame = 0;
        private ulong lastCheckedFrame = 0;

        ulong ICondition.LastChangedFrame => lastChangedFrame;

        private bool wasActive = false;
        bool ICondition.IsActive
        {
            get
            {
                if (Engine.Instance.FrameNumber == lastCheckedFrame)
                    return wasActive;

                var nowActive = IsActive;

                if(nowActive != wasActive)
                    lastChangedFrame = Engine.Instance.FrameNumber;

                wasActive = nowActive;

                lastCheckedFrame = Engine.Instance.FrameNumber;
                return nowActive;
            }
        }

        public abstract bool IsActive { get; }

        private static ConstantCondition trueCondition = new ConstantCondition(true);
        private static ConstantCondition falseCondition = new ConstantCondition(false);
        public static ConstantCondition True => trueCondition;
        public static ConstantCondition False => falseCondition;

        public static implicit operator bool(Condition c)
        {
            return c.IsActiveAndNotNull();
        }
    }

    public class LambdaCondition<T> : Condition, ICondition<T>
    {
        public override bool IsActive => Condition(item);

        private T item;
        T ICondition<T>.Item => item;

        private Predicate<T> Condition;

        public LambdaCondition(T itemToCheck, Predicate<T> condition)
        {
            item = itemToCheck;
            Condition = condition;
        }
    }

    public class ManualCondition : Condition
    {
        public bool Active { get; set; }

        public override bool IsActive => Active;

        public ManualCondition(bool active=false)
        {
            Active = active;
        }
    }

    public class NegativeCondition : Condition
    {
        private ICondition BaseCondition;
        public override bool IsActive => !BaseCondition.IsActive;

        public NegativeCondition(ICondition baseCondition)
        {
            BaseCondition = baseCondition;
        }
    }

    public class ContinueWhileCondition : Condition
    {
        private ICondition StartCondition;
        private ICondition ContinueCondition;

        private bool isStarted;
        public override bool IsActive
        {
            get
            {
                bool isActive = false;

                if(!isStarted)                
                    isStarted = StartCondition.IsActive;
                 
                if(isStarted)                
                    isActive = StartCondition.IsActive || ContinueCondition.IsActive;
                
                if (!isActive)
                    isStarted = false;

                return isActive;

            }
        }
        
        public ContinueWhileCondition(ICondition start, ICondition continueCondition)
        {
            StartCondition = start;
            ContinueCondition = continueCondition;
        }
    }

    public class AndCondition : Condition
    {
        private ICondition First;
        private ICondition Second;

        public override bool IsActive
        {
            get
            {
                return First.IsActive && Second.IsActive;
            }
        }

        public AndCondition(ICondition first, ICondition second)
        {
            First = first;
            Second = second;
        }
    }

    public class OrCondition : Condition
    {
        private ICondition First;
        private ICondition Second;

        public override bool IsActive
        {
            get
            {
                return First.IsActive || Second.IsActive;
            }
        }

        public OrCondition(ICondition first, ICondition second)
        {
            First = first;
            Second = second;
        }
    }

}
