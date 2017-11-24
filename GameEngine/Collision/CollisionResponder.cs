using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface ICollidable : IRemoveable
    {
        bool DetectCollision(Rectangle collidingObject, bool ignoreEdges);      
    }

    public interface IMovingCollidable : ICollidable
    {
        bool DetectFrameStartCollision(Rectangle collidingObject);
    }
    
    public interface ICollisionResponder<TFirst, TSecond>
        where TFirst : IMoveable
        where TSecond : ICollidable         
    {
        void RespondToCollision(TFirst obj, TSecond collidable, CollisionInfo collisionInfo);
    }

    public class LambdaCollisionResponder<TFirst, TSecond> : ICollisionResponder<TFirst, TSecond>
        where TFirst : IMoveable
        where TSecond : ICollidable
    {
        private Action<TFirst, TSecond, CollisionInfo> Action;

        public LambdaCollisionResponder(Action<TFirst, TSecond, CollisionInfo> action)
        {
            Action = action;
        }

        public LambdaCollisionResponder(Action<TFirst, TSecond> action)
        {
            Action = (f, s, r) => action(f, s);
        }

        void ICollisionResponder<TFirst, TSecond>.RespondToCollision(TFirst obj, TSecond collidable, CollisionInfo collisionInfo)
        {
            Action(obj, collidable, collisionInfo);
        }
    }


}
