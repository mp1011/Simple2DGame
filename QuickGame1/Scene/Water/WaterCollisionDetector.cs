using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class WaterCollisionDetector : ICollidable
    {
        private IWithPosition[] waterBlocks;

        bool IRemoveable.IsRemoved => false;

        public WaterCollisionDetector(Layer layer, IEnumerable<WaterBlock> water)
        {
            layer.CollidableObjects.Add(this);
            waterBlocks = water.ToArray();
        }

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            foreach (var block in waterBlocks)
            {
                if (collidingObject.CollidesWith(block.Position,ignoreEdges))
                    return true;
            }

            return false;
        }

        void IRemoveable.Remove()
        {
        }
    }
}
