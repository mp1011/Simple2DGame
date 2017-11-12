using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Spike : ICollidable, IDamager
    {
        private Rectangle position = new Rectangle();
        private QuickGameTileMap TileMap;

        DamageType IDamager.DamageType => DamageType.Trap;
        bool IRemoveable.IsRemoved => false;        
        Rectangle IWithPosition.Position => position;

        Direction IWithPositionAndDirection.Direction { get; set; }

        public Spike(QuickGameScene scene)
        {
            TileMap = scene.TileMap;
            scene.SolidLayer.CollidableObjects.Add(this);
        }

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            foreach (var tileHit in TileMap.GetTilesHit(collidingObject))
            {
                var isSpike = tileHit.IsSpike && collidingObject.Bottom > tileHit.Position.Center.Y;
                if (isSpike)
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
