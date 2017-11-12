using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Once activated, remains on for a set time
    /// </summary>
    public abstract class TimedCondition : Condition
    {
        private ICondition BaseCondition;
        private Timer Timer;

        public TimedCondition(Timer timer, ICondition baseCondition)
        {
            BaseCondition = baseCondition;
            Timer = timer;
        }

        public override bool IsActive => CheckActive(Timer, BaseCondition);

        protected abstract bool CheckActive(Timer timer, ICondition baseCondition);
    }

    /// <summary>
    /// Once the base condition is activated, this condition remains true for a length of time. 
    /// </summary>
    public class Sustain : TimedCondition
    {
        public Sustain(Timer duration, ICondition baseCondition) : base(duration, baseCondition) { }

        protected override bool CheckActive(Timer timer, ICondition baseCondition)
        {
            if (!timer.IsRunning)
            {
                if (baseCondition.WasJustActivated())
                {
                    timer.Reset();
                }
            }

            return timer.IsRunning;
        }
    }

    /// <summary>
    /// Once the base condition is activated, waits the specified amount of time, then returns true.
    /// </summary>
    public class WaitUntil : TimedCondition
    {
        public WaitUntil(Timer duration, ICondition baseCondition) : base(duration, baseCondition) { }

        private bool TimerWasStarted = false;

        protected override bool CheckActive(Timer timer, ICondition baseCondition)
        {
            if (!timer.IsRunning)
            {
                if (baseCondition.IsActive)
                {
                    timer.Reset();
                    TimerWasStarted = true;
                }
            }

            if(TimerWasStarted && !timer.IsRunning)
            {
                TimerWasStarted = false;
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Returns true on a periodic interval while the base condition is true
    /// </summary>
    public class OnceEvery : TimedCondition
    {
        public OnceEvery(Timer duration) : this(duration, Condition.True) { }

        public OnceEvery(Timer duration, ICondition baseCondition) : base(duration, baseCondition) { }

        protected override bool CheckActive(Timer timer, ICondition baseCondition)
        {
            if (!baseCondition.IsActive)
                return false;

            if (baseCondition.WasJustActivated())
                timer.Reset();
            else if (!timer.IsRunning && baseCondition.IsActive)
            {
                timer.Reset();
                return true;
            }

            return false;
        }
    }
}