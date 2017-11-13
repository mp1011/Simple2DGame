using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IPlatformerObject : IMovingWorldObject
    {
        IMovingBlock RidingBlock { get; set; }
        ManualCondition IsUnderWater { get; }
        ManualCondition IsOnGround { get; }
    }

    public static class PlatformerObjectExtensions
    {

        public static void PutOnGround(this IPlatformerObject obj, TileMap tiles)
        {
            var tile = tiles.GetTilesHit(obj.Position).First();
            while (!tile.IsSolid)
            {
                var nextTile = tile.GetAdjacent(Direction.Down);
                if (nextTile.Position.Top > tiles.Position.Height)
                    return;
                tile = nextTile;
            }

            obj.Position.SetBottom(tile.Position.Top);
        }
    }
}
