using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class MapFromImage
    {
        public static QuickGameTileMap Create(QuickGameScene scene)
        {
            var masterMap = scene.MasterTemplate;

            var template = masterMap.Extract(scene.ID.MapNumber);
            
            CalculateObscuredTiles(template.Cells, template.GrassMap);
            CalculateObscuredTiles(template.Cells, template.WaterMap);
            ExtendTileMask(template.GrassMap);
            ExtendTileMask(template.WaterMap);

            var solidTiles = new QuickGameTileMap(scene.SolidLayer, template.BrownRockMap.TileSet.Texture, template.Cells.Size);
            solidTiles.EmptyCell = template.BrownRockMap.TileSet.GetCell(BorderSide.EmptySpace);

            template.BrownRockMap.Apply(solidTiles);
            template.LadderMap.Apply(solidTiles, applyEmptyCells: false);
         
            var grassTiles = new QuickGameTileMap(scene.SolidLayer, template.GrassMap.TileSet.Texture, template.Cells.Size);
            template.GrassMap.Apply(grassTiles);

            var waterTiles = new QuickGameTileMap(scene.WaterLayer, template.BrownRockMap.TileSet.Texture, template.Cells.Size);
            waterTiles.EmptyCell = solidTiles.EmptyCell;            
            template.WaterMap.Apply(waterTiles, true);

            FixWaterSurfaceTiles(waterTiles, template.WaterMap.TileSet);

            scene.WaterLayer.FixedDisplayable = new IDisplayable[] { waterTiles };
            scene.SolidLayer.FixedDisplayable = new IDisplayable[] { grassTiles, solidTiles  };

            List<MovingBlockPiece> movingBlockPieces = new List<MovingBlockPiece>();
            List<PathPoint> pathPoints = new List<PathPoint>();

            foreach (var point in template.Cells.Points)
            {
                if (template.Cells.GetFromPoint(point) == ImageCellType.PlayerStart)
                {
                    scene.PlayerStart = new Vector2(point.X * 16, point.Y * 16);
                }              
                else if (template.Cells.GetFromPoint(point) == ImageCellType.Spike)
                {
                    solidTiles.Tiles.Cells.Set(point, solidTiles.Tiles.Texture.PointToIndex(3, 7));
                }
                else if (template.Cells.GetFromPoint(point) == ImageCellType.Spring)
                {
                    solidTiles.Tiles.Cells.Set(point, solidTiles.Tiles.Texture.PointToIndex(4, 2));
                }
                else if (template.Cells.GetFromPoint(point) == ImageCellType.BreakableBlock)
                {
                    solidTiles.Tiles.Cells.Set(point, solidTiles.Tiles.Texture.PointToIndex(1, 7));
                }
                else if (template.Cells.GetFromPoint(point) == ImageCellType.Box)
                {
                    new Box().SetPosition(point.X * 16, point.Y * 16);
                }
                else if (template.Cells.GetFromPoint(point) == ImageCellType.MovingBlock)
                {
                    movingBlockPieces.Add(new MovingBlockPiece(point, solidTiles.Tiles.Texture.CellSize));
                }
                else if (template.Cells.GetFromPoint(point) == ImageCellType.Path)
                {
                    pathPoints.Add(new PathPoint((int)point.X * 16, (int)point.Y * 16));
                }
            }

            MovingBlockFactory.CreateBlocks(scene, movingBlockPieces, pathPoints);
            
            var agt = GameTiles.AutoGenTiles();
            agt.Apply(solidTiles);

            return solidTiles;
        }

        private static void FixWaterSurfaceTiles(QuickGameTileMap tileMap, BorderTileSet tileSet)
        {
            int surfaceTile = tileSet.GetCell(BorderSide.Left | BorderSide.Right | BorderSide.Bottom);
            List<ArrayGridPoint<int>> tilesToChange = new List<ArrayGridPoint<int>>();

            foreach(var tile in tileMap.Tiles.Cells.PointItems)
            {
                if(tile.Value == surfaceTile)
                {
                    var left = tile.GetAdjacent(Direction.Left);
                    if (left.Value != surfaceTile)
                        tilesToChange.Add(left);

                    var right = tile.GetAdjacent(Direction.Right);
                    if (right.Value != surfaceTile)
                        tilesToChange.Add(right);
                }
            }

            foreach(var tile in tilesToChange)
            {
                tile.Set(surfaceTile);
            }
        }

        /// <summary>
        /// Fills in background tiles for any non-empty cell that is adjacent to a background tile. Returns true if any changes were made.
        /// </summary>
        /// <param name="mapGrid"></param>
        /// <param name="grassMap"></param>
        /// <returns></returns>
        public static void CalculateObscuredTiles(ArrayGrid<ImageCellType> mapGrid, TileMaskMap bgMap)
        {
            List<Vector2> tilesToAdd = new List<Vector2>();
            List<Vector2> tilesToRecheck = new List<Vector2>();
             
            foreach(var tile in mapGrid.PointItems)
            {
                if (bgMap.TileFlags.GetFromPoint(tile.Position) == TileMaskValue.Filled)
                    continue;

                if (tile.Value == ImageCellType.Empty)
                    continue;

                if (tile.Value.IsSolid())
                    bgMap.TileFlags.Set(tile.Position, TileMaskValue.Obscured);
                else if (tile.Position.GetAdjacentPoints(true).Select(bgMap.TileFlags.GetFromPoint).Where(p => p == TileMaskValue.Filled).Any())
                    bgMap.TileFlags.Set(tile.Position, TileMaskValue.Filled);               
            }
        }

        /// <summary>
        /// Extends tiles one unit into the obscured tiles, for smoother boundaries
        /// </summary>
        /// <param name="waterTiles"></param>
        public static void ExtendTileMask(TileMaskMap tiles)
        {
            List<ArrayGridPoint<TileMaskValue>> itemsToSet = new List<ArrayGridPoint<TileMaskValue>>();

            foreach(var tile in tiles.TileFlags.PointItems.Where(p=>p.Value == TileMaskValue.Filled))
            {
                itemsToSet.AddRange(tile.GetAdjacent().Where(p => p.Value == TileMaskValue.Obscured));
            }

            foreach (var item in itemsToSet)
                item.Set(TileMaskValue.Filled);
        }
        

        

    }
}
