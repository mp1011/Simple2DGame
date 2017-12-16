using GameEngine;
using System;

namespace QuickGame1
{
    class QuickGameNearbyTileChecker : NearbyTileChecker
    {       
        public QuickGameNearbyTileChecker(IMovingWorldObject movingObject, QuickGameTileMap map) : base(movingObject, map)
        {
        }

        protected override bool IsTileDangerous(Tile tile)
        {
            return (tile as QuickGameTile).IsSpike;
        }
    }
}
