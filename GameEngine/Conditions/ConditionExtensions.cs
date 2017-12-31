using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public static class IConditionExtensions
    {

        public static Condition Or(this ICondition condition, Condition other)
        {
            if (condition == null)
                return other;
            else
                return new OrCondition(condition, other);
        }

        public static Condition And(this ICondition condition, Condition other)
        {
            if (condition == null)
                return other;
            else
                return new AndCondition(condition, other);
        }

        public static Condition Negate(this ICondition condition)
        {
            return new NegativeCondition(condition);
        }

        public static bool IsActiveAndNotNull(this ICondition condition)
        {
            return condition != null && condition.IsActive;
        }

        public static bool JustStarted(this ICondition condition)
        {
            return condition.IsActive && condition.LastChangedFrame == Engine.Instance.FrameNumber;
        }

        public static bool JustEnded(this ICondition condition)
        {
            return !condition.IsActive && condition.LastChangedFrame == Engine.Instance.FrameNumber;
        }

        public static Condition SetActiveTime(this ICondition condition, ConfigValue<TimeSpan> time, IWorldObject owner)
        {
            return new Sustain(new Timer(time, owner), condition);
        }

        public static Condition ContinueWhile(this ICondition condition, ICondition whileCondition)
        {
            return new ContinueWhileCondition(condition, whileCondition);
        }

        public static Condition When(this ICondition condition, ICondition secondCondition)
        {
            return new AndCondition(condition, secondCondition);
        }

        public static Condition When<T>(this ICondition<T> condition, T item, Predicate<T> predicate)
        {
            return new AndCondition(condition, new LambdaCondition<T>(item, predicate));
        }

        public static Condition IsVisible(this IDisplayable d)
        {
            return new LambdaCondition<IDisplayable>(d, x => x.DrawInfo.Visible);
        }

        public static Condition WaitUntil(this ICondition c, Timer t)
        {
            return new WaitUntil(t, c);
        }       
    }

}
