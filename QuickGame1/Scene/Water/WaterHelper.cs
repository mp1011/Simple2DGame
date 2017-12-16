using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{

    class WaterHelper
    {

        public static void AddWaterPhysics(IPlatformerObject actor)
        {
            actor.Motion.AddMultiplier(new MotionMultiplier(actor.IsUnderWater, "underwater"));  
        }

        public static void AddWaterOverlays(Layer waterLayer, Layer solidLayer)
        {
            Rectangle[] waterSections = FindWaterSections(waterLayer).ToArray();
            
            List<WaterBlock> waterBlocks = new List<WaterBlock>();

            foreach (var rectangle in waterSections)
            {
                var waterBlock = new WaterBlock(solidLayer, rectangle);
                waterBlocks.Add(waterBlock);
                solidLayer.AddObject(waterBlock);
            }

            new WaterCollisionDetector(solidLayer, waterBlocks);
        }

        private static IEnumerable<Rectangle> FindWaterSections(Layer waterLayer)
        {
            var map = waterLayer.FixedDisplayable.OfType<TileMap>().Single();
            var waterTiles = map.Tiles.Cells.Points.Select(pt => map.GetTileFromGridPoint(pt))
                .Where(p => !p.IsEmpty).ToArray();

            while (waterTiles.Any())
            {
                var topLeftTile = waterTiles.OrderBy(p => p.Position.Left).ThenBy(p => p.Position.Top).First();

                Rectangle waterBlock = topLeftTile.Position.Copy();
                var waterTile = topLeftTile;
                while (!waterTile.IsEmpty && waterTiles.Any(p=>p.TileIndex == waterTile.TileIndex))
                {
                    waterBlock.SetRight((int)waterTile.Position.Right, false);
                    waterTile = waterTile.GetAdjacent(BorderSide.Right);
                }

                waterTile = waterTile.GetAdjacent(BorderSide.Left);
                while (!waterTile.IsEmpty && waterTiles.Any(p => p.TileIndex == waterTile.TileIndex))
                {
                    waterBlock.SetBottom((int)waterTile.Position.Bottom, false);
                    waterTile = waterTile.GetAdjacent(BorderSide.Bottom);
                }

                waterTiles = waterTiles.Where(p => !waterBlock.CollidesWith(p.Position,true)).ToArray();

                yield return waterBlock;
            }

        }
    }
}
