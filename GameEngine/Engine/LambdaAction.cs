using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class LambdaAction<T> : IUpdateable
        where T:IRemoveable
    {
        private UpdatePriority priority;
        UpdatePriority IUpdateable.Priority => priority;

        private T Object;
        private Action<T, TimeSpan> Action;

        IRemoveable IUpdateable.Root => Object;

        private ICondition Condition;

        public LambdaAction(T obj, UpdatePriority updatePriority, Scene scene, ICondition condition, Action<T, TimeSpan> action)
        {
            priority = updatePriority;
            Object = obj;
            Action = action;
            Condition = condition;
            scene.AddObject(this);
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if(Condition.IsActive)
                Action(Object, elapsedInFrame);
        }
    }
}
