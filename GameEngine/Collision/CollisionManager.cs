using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class CollisionManager :IUpdateable
    {
        public Layer Layer { get; private set; }
        private List<ICollisionCheck> Checks = new List<ICollisionCheck>();

        UpdatePriority IUpdateable.Priority => UpdatePriority.Collision;
        IRemoveable IUpdateable.Root => Layer;
        
        public CollisionManager(Layer layer)
        {
            Layer = layer;
            Layer.Scene.AddObject(this);
        }

        public CollisionCheck<TFirst,TSecond> OnCollisionBetween<TFirst,TSecond>()
            where TFirst : class, IMovingWorldObject
            where TSecond : ICollidable
        {
            var check = Checks.OfType<CollisionCheck<TFirst, TSecond>>().FirstOrDefault();
            if(check == null)
            {
                check = new CollisionCheck<TFirst, TSecond>();
                Checks.Add(check);
            }
            
            return check;
        }
        
        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            foreach(var check in Checks)
            {
                check.CheckForCollisions(this);
            }
        }
    }

}
