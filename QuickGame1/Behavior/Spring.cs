using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Spring : ICollidable
    {
        private Rectangle position = new Rectangle();
        private QuickGameTileMap TileMap;

        bool IRemoveable.IsRemoved => false;        

        public Spring(QuickGameScene scene)
        {
            TileMap = scene.TileMap;
            scene.SolidLayer.CollidableObjects.Add(this);
        }

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            foreach (var tileHit in TileMap.GetTilesHit(collidingObject))
            {
                if (tileHit.IsSpring)
                    return true;
            }

            return false;
        }

        void IRemoveable.Remove()
        {
            throw new NotSupportedException();
        }
    }
}
