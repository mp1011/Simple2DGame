using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class CollisionCondition<TFirst,TSecond> : Condition, IUpdateable, ICondition<TFirst>
         where TFirst : class, IMovingWorldObject
         where TSecond : ICollidable
    {
        private TFirst Object;

        private bool isActive;
        public override bool IsActive => isActive;

        private bool DeactiveNextFrame = false;
        public UpdatePriority Priority => UpdatePriority.EndUpdate;
        public IRemoveable Root => Object;

        TFirst ICondition<TFirst>.Item => Object;

        public CollisionCondition(TFirst objectToTest)
        {
            Object = objectToTest;

            objectToTest.Layer.Scene.AddObject(this);
            objectToTest.Layer.Scene.CollisionManager.OnCollisionBetween<TFirst, TSecond>()
                .Then((f, s) =>
                {
                    if (f == Object)
                    {
                        isActive = true;
                        DeactiveNextFrame = false;
                    }
                });
        }
        
        public void Update(TimeSpan elapsedInFrame)
        {
            if (!DeactiveNextFrame)
                DeactiveNextFrame = true;
            else
                isActive = false;
        }
    }
}
