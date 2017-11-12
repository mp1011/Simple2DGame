using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class CollissionPlaceholder<T> : ICollidable where T:ICollidable
    {         
        public T Actual { get; set; }
    
        public bool IsRemoved { get; private set; }

        void IRemoveable.Remove()
        {
            IsRemoved = true;
        }

        public CollissionPlaceholder()
        {
        }

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            if (Actual == null)
                return false;
            else
                return Actual.DetectCollision(collidingObject,ignoreEdges);
        }
    }
}
