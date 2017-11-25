using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    public interface ITileBreaker : IMovingWorldObject
    {
        bool CanBreakTiles { get; set; }
    }


    static class TileBreakerHandler
    {
        public static void CheckForBrokenTiles(ITileBreaker breaker, QuickGameTileMap map)
        {
            if (breaker.CanBreakTiles)
            {
                var tilesHit = map.GetTilesHit(breaker.Position).Where(p => p.IsBreakable).ToArray();

                var tileToBreak = tilesHit.OrderBy(p => p.Position.Center.GetAbsoluteDistanceTo(breaker.Position.Center)).FirstOrDefault();
                if (tileToBreak != null)
                {
                    breaker.CanBreakTiles = false;
                    BreakTile(tileToBreak);
                }
            }
        }

        private static void BreakTile(QuickGameTile tile)
        {
            AudioEngine.Instance.PlaySound(Sounds.HitGround, 1.0f);
            tile.TileMap.Tiles.Cells.Set(tile.TileIndex, tile.TileMap.EmptyCell);
            Shrapnel.CreateBlockShrapnel(tile.Position.Center);
        }
    }
}
