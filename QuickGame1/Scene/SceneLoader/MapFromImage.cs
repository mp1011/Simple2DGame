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
            
            FillMissingBackgroundTiles(template.Cells, template.GrassMap, BorderSide.Left, BorderSide.Right, BorderSide.Top, BorderSide.Bottom);
            FillMissingBackgroundTiles(template.Cells, template.WaterMap, BorderSide.Left, BorderSide.Right, BorderSide.Top, BorderSide.Bottom);

            var solidTiles = new QuickGameTileMap(scene.SolidLayer, template.BrownRockMap.TileSet.Texture, template.Cells.Size);
            solidTiles.EmptyCell = template.BrownRockMap.TileSet.GetCell(BorderSide.EmptySpace);

            template.BrownRockMap.Apply(solidTiles);
            template.LadderMap.Apply(solidTiles, applyEmptyCells: false);

            var grassTiles = new QuickGameTileMap(scene.SolidLayer, template.GrassMap.TileSet.Texture, template.Cells.Size);
            template.GrassMap.Apply(grassTiles);

            var waterTiles = new QuickGameTileMap(scene.WaterLayer, template.BrownRockMap.TileSet.Texture, template.Cells.Size);
            waterTiles.EmptyCell = solidTiles.EmptyCell;

            template.WaterMap.Apply(waterTiles, true);

            List<Vector2> ignore = new List<Vector2>();

            foreach (var pt in waterTiles.Tiles.Cells.Points)
            {
              
                var tile = waterTiles.GetTileFromGridPoint(pt);
                var leftAdjacent = tile.GetAdjacent(BorderSide.Left);
                var rightAdjacent = tile.GetAdjacent(BorderSide.Right);

                if (!ignore.Contains(pt) && !ignore.Contains(leftAdjacent.TileIndex) && !ignore.Contains(rightAdjacent.TileIndex))
                {
                    if (tile.IsEmpty && (!leftAdjacent.IsEmpty || !rightAdjacent.IsEmpty))
                    {
                        if (!leftAdjacent.IsEmpty)
                            waterTiles.Tiles.Cells.Set(pt, leftAdjacent.TileID);
                        else
                            waterTiles.Tiles.Cells.Set(pt, rightAdjacent.TileID);

                        ignore.Add(pt);
                        ignore.Add(leftAdjacent.TileIndex);
                        ignore.Add(rightAdjacent.TileIndex);
                    }
                }
            }

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
                else if (template.Cells.GetFromPoint(point) == ImageCellType.Prize)
                {
                    var coin = new Coin();
                    coin.SetPosition(point.X * 16, point.Y * 16);
                }
                else if (template.Cells.GetFromPoint(point) == ImageCellType.Spike)
                {
                    solidTiles.Tiles.Cells.Set(point, solidTiles.Tiles.Texture.PointToIndex(3, 7));
                }
                else if (template.Cells.GetFromPoint(point) == ImageCellType.Enemy)
                {
                    //new Grapeman().SetPosition(point.X * 16, (point.Y * 16)-16);
                    // new Snake().MoveTo(point.X * 16, point.Y * 16);
                    new Slime().SetPosition(point.X * 16, (point.Y * 16)-32);
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

        /// <summary>
        /// Fills in background tiles for any non-empty cell that is adjacent to a background tile. Returns true if any changes were made.
        /// </summary>
        /// <param name="mapGrid"></param>
        /// <param name="grassMap"></param>
        /// <returns></returns>
        public static void FillMissingBackgroundTiles(ArrayGrid<ImageCellType> mapGrid, BooleanTileMap bgMap, params BorderSide[] sidesToCheck)
        {
            List<Vector2> tilesToAdd = new List<Vector2>();

            foreach(var point in mapGrid.Points)
            {              
                var thisCell = mapGrid.GetFromPoint(point);

                if (bgMap.TileFlags.GetFromPoint(point) == false && thisCell != ImageCellType.Empty)
                {
                    if(point.GetAdjacentPoints(sidesToCheck).Select(bgMap.TileFlags.GetFromPoint).Where(p=>p).Any())
                        tilesToAdd.Add(point);
                }
            }

            foreach(var pt in tilesToAdd)
                bgMap.TileFlags.Set(pt, true);
        }
        

        

    }
}
