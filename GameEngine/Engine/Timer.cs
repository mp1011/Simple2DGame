using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public class Timer : Condition, IUpdateable
    {
        #region Timer Logic

        UpdatePriority IUpdateable.Priority => UpdatePriority.BeginUpdate;

        private IRemoveable Owner;
        IRemoveable IUpdateable.Root => Owner;

        private ConfigValue<TimeSpan> TimePerState;
        private double msRemaining;

        public Timer(TimeSpan timePerState, IWorldObject owner) : this ( new ConstValue<TimeSpan>(timePerState), owner)
        {}

        public Timer(ConfigValue<TimeSpan> timePerState, IWorldObject owner) : this(owner)
        {
            TimePerState = timePerState;
            msRemaining = TimePerState.Value.TotalMilliseconds;
        }

        public Timer(IWorldObject owner)
        {
            Owner = owner;
            owner.Layer.Scene.AddObject(this);
        }


        public void Reset()
        {
            msRemaining = TimePerState.Value.TotalMilliseconds;
        }

        public void Stop()
        {
            msRemaining = 0;
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            msRemaining -= elapsedInFrame.TotalMilliseconds;
        }

        public override bool IsActive
        {
            get
            {
                return msRemaining > 0;
            }           
        }

        public bool IsRunning { get { return IsActive; } }

        #endregion

        #region Timer Conditions

        public Condition OnceEvery(TimeSpan span)
        {
            TimePerState = new ConstValue<TimeSpan>(span);
            return new OnceEvery(this);
        }

        #endregion
    }

    public class CyclingTimer<T> : CyclingList<T>
    {
        public TimeSpan TimePerState;     
        private double msRemaining;

        public CyclingTimer(TimeSpan timePerState, params T[] states) 
        {
            this.AddRange(states);
            TimePerState = timePerState;
            msRemaining = TimePerState.TotalMilliseconds;
        }

        public void Increment(TimeSpan elapsedInFrame)
        {
            msRemaining -= elapsedInFrame.TotalMilliseconds;
            if(msRemaining < 0)
            {
                Next();
                msRemaining = TimePerState.TotalMilliseconds;
            }
        }

        public T IncrementAndReturn(TimeSpan elapsedInFrame)
        {
            Increment(elapsedInFrame);
            return Current;
        }
    }

    public class ToggleCycle : CyclingTimer<bool>
    {
        public ToggleCycle(TimeSpan timePerState) : base(timePerState, true, false) { }
    }
}
