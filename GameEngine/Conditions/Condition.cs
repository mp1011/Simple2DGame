using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface ICondition
    {
        ulong ActivatedFrame { get; }
        bool IsActive { get; }
    }

    public interface ICondition<T> : ICondition
    {
        T Item { get; }
    }

    public class ConstantCondition : ICondition
    {
        public ulong ActivatedFrame => 0;
        public bool IsActive { get; }

        public ConstantCondition(bool value)
        {
            IsActive = value;
        }
    }

    public abstract class Condition : ICondition
    {
        private ulong activatedFrame = 0;
        ulong ICondition.ActivatedFrame => activatedFrame;

        private bool wasActive = false;
        bool ICondition.IsActive
        {
            get
            {
                if (activatedFrame == Engine.Instance.FrameNumber)
                    return true;

                var nowActive = IsActive;

                if(nowActive && !wasActive)
                    activatedFrame = Engine.Instance.FrameNumber;

                wasActive = nowActive;
                return nowActive;
            }
        }

        public abstract bool IsActive { get; }

        private static ConstantCondition trueCondition = new ConstantCondition(true);
        private static ConstantCondition falseCondition = new ConstantCondition(false);
        public static ConstantCondition True => trueCondition;
        public static ConstantCondition False => falseCondition;
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
                    isActive = StartCondition.WasJustActivated() || ContinueCondition.IsActive;
                
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
