using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface ICollisionCheck
    {
        void CheckForCollisions(CollisionManager manager);
    }

    public class CollisionCheck<TFirst, TSecond> : ICollisionCheck
        where TFirst : class, IMovingWorldObject
        where TSecond : ICollidable
    {
        private List<ICollisionResponder<TFirst, TSecond>> Responders = new List<ICollisionResponder<TFirst, TSecond>>();
        private List<ICollisionResponder<TFirst, TSecond>> NoCollisionResponders = new List<ICollisionResponder<TFirst, TSecond>>();
        private List<ICollisionResponder<TFirst, TSecond>> OneTimeResponders = new List<ICollisionResponder<TFirst, TSecond>>();

        public void AddResponder(ICollisionResponder<TFirst, TSecond> responder)
        {
            Responders.Add(responder);
        }

        public void AddOneTimeResponder(ICollisionResponder<TFirst, TSecond> responder)
        {
            OneTimeResponders.Add(responder);
        }

        public void AddNoCollisionResponder(ICollisionResponder<TFirst, TSecond> responder)
        {
            NoCollisionResponders.Add(responder);
        }

        protected bool CheckForCollisions(TFirst Object, TSecond collidable, bool collidedAlready)
        {
            var originalInfo = new CollisionInfo { OriginalPosition = Object.Position.Copy(), OriginalVelocity = Object.Motion.CurrentMotionPerSecond };

            if (Object.IsRemoved || collidable.IsRemoved)
                return false;

            //get position after motion is adjusted but before collision is corrected
            var testPosition = Object.Motion.FrameStartPosition.Copy().Translate(Object.Motion.FrameVelocity);

            if (collidable.DetectCollision(testPosition, false))
            {
                foreach (var responder in Responders)
                    responder.RespondToCollision(Object, collidable, originalInfo);

                if(!collidedAlready)
                {
                    foreach (var responder in OneTimeResponders)
                        responder.RespondToCollision(Object, collidable, originalInfo);
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        void ICollisionCheck.CheckForCollisions(CollisionManager manager)
        {
            //this needs to be done better
            var listA = manager.Layer.CollidableObjects.OfType<TFirst>().ToArray();
            var listB = manager.Layer.CollidableObjects.OfType<TSecond>().ToArray();

            foreach(var a in listA)
            {
                bool collidedWithAny = false;

                foreach(var b in listB)
                {
                    if (!a.Equals(b))
                    {
                        if (CheckForCollisions(a, b, collidedWithAny))
                            collidedWithAny = true;
                    }
                }

                if (!collidedWithAny)
                {
                    foreach (var responder in NoCollisionResponders)
                        responder.RespondToCollision(a, default(TSecond), null);
                }
            }
        }

        #region Responses

        public CollisionCheck<TFirst,TSecond> PlaySound(SoundEffect sound)
        {
            return Then((f, s) => AudioEngine.Instance.PlaySound(sound));
        }

        public CollisionCheck<TFirst, TSecond> Stop()
        {
            if (Responders.OfType<BlockingCollisionResponder<TFirst, TSecond>>().Any())
                return this;

            AddResponder(new BlockingCollisionResponder<TFirst, TSecond>());
            return this;
        }
        
        public CollisionCheck<TFirst,TSecond> HandleWith(ICollisionResponder<TFirst,TSecond> responder)
        {
            AddResponder(responder);
            return this;
        }

        public CollisionCheck<TFirst, TSecond> Then(Action<TFirst, TSecond> response)
        {
            AddResponder(new LambdaCollisionResponder<TFirst, TSecond>(response));
            return this;
        }

        public CollisionCheck<TFirst, TSecond> Then(Action<TFirst, TSecond, CollisionInfo> response)
        {
            AddResponder(new LambdaCollisionResponder<TFirst, TSecond>(response));
            return this;
        }

        public CollisionCheck<TFirst, TSecond> ThenDoOnce(Action<TFirst, TSecond, CollisionInfo> response)
        {
            AddOneTimeResponder(new LambdaCollisionResponder<TFirst, TSecond>(response));
            return this;
        }

        public CollisionCheck<TFirst, TSecond> Else(Action<TFirst, TSecond> response)
        {
            AddNoCollisionResponder(new LambdaCollisionResponder<TFirst, TSecond>(response));
            return this;
        }

        #endregion
    }

    


}
