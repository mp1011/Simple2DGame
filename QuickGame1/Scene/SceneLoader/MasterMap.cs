using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = GameEngine.Rectangle;

namespace QuickGame1
{
    enum ImageCellType
    {
        Empty,
        BrownRock,
        BlueRock,
        Water,
        GrassBackground,
        Prize,
        Spike,
        Box,
        Enemy,
        Ladder,
        MovingBlock,
        Path,
        PlayerStart,
        MapBoundary
    }

    static class ImageCellTypeExtensions
    {
        public static bool IsSolid(this ImageCellType ct)
        {
            return ct == ImageCellType.BrownRock || ct == ImageCellType.BlueRock;
        }
    }

    class MapTemplate
    {
        public List<Rectangle> MapRegions = new List<Rectangle>();
        public ArrayGrid<ImageCellType> Cells;
        public BooleanTileMap BrownRockMap, GrassMap, WaterMap, LadderMap;
        private Vector2 CellSize;

        public MapTemplate(string mapfile, Vector2 cellSize)
        {
            CellSize = cellSize;
            var colors = TextureInfoReader.Instance.TextureToColors(mapfile);
            Cells = colors.Map(ColorToType);
            Cells.OutOfBoundsFixedValue = ImageCellType.MapBoundary;

            InitMaps();
            MapRegions.AddRange(FindRegions());
            VerifyMinimumRegionSize();

            foreach(var cell in Cells.PointItems)
            {
                if(cell.Value == ImageCellType.MapBoundary)
                {
                    bool aboveIsBoundary = cell.GetAdjacent(Direction.Up).Value == ImageCellType.MapBoundary;
                    bool belowIsBoundary = cell.GetAdjacent(Direction.Down).Value == ImageCellType.MapBoundary;
                    bool leftIsBoundary = cell.GetAdjacent(Direction.Left).Value == ImageCellType.MapBoundary;
                    bool rightisBoundary = cell.GetAdjacent(Direction.Right).Value == ImageCellType.MapBoundary;

                    List<Direction> possibleCopyDirections = new List<Direction>();

                    if ((aboveIsBoundary || belowIsBoundary) && !leftIsBoundary && !rightisBoundary)
                    {
                        possibleCopyDirections.Add(Direction.Left);
                        possibleCopyDirections.Add(Direction.Right);
                    }
                    else if ((leftIsBoundary || rightisBoundary) && !aboveIsBoundary && !belowIsBoundary)
                    {
                        possibleCopyDirections.Add(Direction.Up);
                        possibleCopyDirections.Add(Direction.Down);
                    }
                    else
                    {
                        possibleCopyDirections.AddRange(Direction.None.GetEnumValues());
                    }

                    var copyDirection = possibleCopyDirections.Where(p => cell.GetAdjacent(p).Value != ImageCellType.MapBoundary)
                        .OrderBy(p => cell.GetAdjacent(p).Value.IsSolid() ? 0 : 1)
                        .FirstOrDefault();
                    
                  
                    var adjacent = cell.GetAdjacent(copyDirection);
                    cell.Set(adjacent.Value);
                }
            }
        }

        public MapTemplate(ArrayGrid<ImageCellType> cells)
        {
            Cells = cells;
            Cells.ReplaceOutOfBoundsTilesWithAdjacent = true;
            InitMaps();    
        }

        private void InitMaps()
        {
            BrownRockMap = new BooleanTileMap(Cells.Map(p => p == ImageCellType.BrownRock || p == ImageCellType.MapBoundary), GameTiles.BrownRock());
            GrassMap = new BooleanTileMap(Cells.Map(p => p == ImageCellType.GrassBackground), GameTiles.Grass());
            WaterMap = new BooleanTileMap(Cells.Map(p => p == ImageCellType.Water), GameTiles.Water());
            LadderMap = new BooleanTileMap(Cells.Map(p => p == ImageCellType.Ladder), GameTiles.Ladder());
        }

        private static ImageCellType ColorToType(Color c)
        {
            if (c == new Color(255, 106, 0))
                return ImageCellType.BrownRock;
            else if (c == new Color(0, 38, 255))
                return ImageCellType.Water;
            else if (c == Color.White)
                return ImageCellType.Empty;
            else if (c == new Color(255, 247, 38))
                return ImageCellType.Prize;
            else if (c == new Color(0, 255, 0))
                return ImageCellType.GrassBackground;
            else if (c == Color.Black)
                return ImageCellType.MapBoundary;
            else if (c == Color.Yellow)
                return ImageCellType.Prize;
            else if (c == Color.Gray)
                return ImageCellType.Spike;
            else if (c == Color.Red)
                return ImageCellType.Enemy;
            else if (c == new Color(178, 0, 255))
                return ImageCellType.Box;
            else if (c == new Color(127, 51, 0))
                return ImageCellType.Ladder;
            else if (c == new Color(127, 0, 0))
                return ImageCellType.MovingBlock;
            else if (c == new Color(38, 127, 0))
                return ImageCellType.PlayerStart;
            else
                throw new NotSupportedException();
        }

        private IEnumerable<Rectangle> FindRegions()
        {
            Vector2 pointInMap = Vector2.Zero;

            List<Rectangle> ret = new List<Rectangle>();
            Stack<Rectangle> regionsToProcess = new Stack<Rectangle>();

            var firstRegion = GetRegionAtLocation(Vector2.Zero);
            ret.Add(firstRegion);
            regionsToProcess.Push(firstRegion);

            while(regionsToProcess.Any())
            {
                var regionToProcess = regionsToProcess.Pop();

                var regionToTheRight = GetRegionAtLocation(regionToProcess.UpperRight.Translate(2, 1));
                var regionBelow = GetRegionAtLocation(regionToProcess.BottomRight.Translate(-1, 2));
                var regionToTheLeft = GetRegionAtLocation(regionToProcess.BottomLeft.Translate(-2, -1));
                var regionAbove = GetRegionAtLocation(regionToProcess.UpperLeft.Translate(2, -1));

                var adjacentRegions = new Rectangle[] { regionToTheRight, regionBelow, regionToTheLeft, regionAbove }
                    .Where(p => p.Width > 0 && !ret.Any(q => q.Equals(p))).ToArray();

                ret.AddRange(adjacentRegions);

                foreach (var adjacentRegion in adjacentRegions)
                    regionsToProcess.Push(adjacentRegion);
            }
            
            return ret;       
        }

        private void VerifyMinimumRegionSize()
        {
            var screen = Engine.Instance.Renderer.ScreenBounds.Position;

            var minSize = new Vector2((float)screen.Width / CellSize.X, (float)screen.Height / CellSize.Y);

            foreach(var region in MapRegions)
            {
                if (region.Width < minSize.X || region.Height < minSize.Y)
                    throw new Exception($"The map region at ({region.Left},{region.Top}) is smaller than the minimum size of ({minSize.X},{minSize.Y}");
            }
        }

        private Rectangle GetRegionAtLocation(Vector2 pointInMap)
        {
            var top = (int)Cells.GetPointsInLine(pointInMap, Direction.Up).First(p => p.Value == ImageCellType.MapBoundary).Position.Y;
            var left = (int)Cells.GetPointsInLine(pointInMap, Direction.Left).First(p => p.Value == ImageCellType.MapBoundary).Position.X;
            var bottom = (int)Cells.GetPointsInLine(pointInMap, Direction.Down).First(p => p.Value == ImageCellType.MapBoundary).Position.Y;
            var right = (int)Cells.GetPointsInLine(pointInMap, Direction.Right).First(p => p.Value == ImageCellType.MapBoundary).Position.X;
            return new Rectangle(Math.Max(0,left), Math.Max(0, top), right - left, bottom - top);
        }

        public MapTemplate Extract(int regionIndex)
        {
            var extractedRange = Cells.ExtractBlock(MapRegions[regionIndex]);
            return new MapTemplate(extractedRange);
        }

        /// <summary>
        /// Converts a pixel position within a certain map to the equivalent tile position within the entire template
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="positionInMap"></param>
        /// <returns></returns>
        public Vector2 PositionInMapToPointInTemplate(SceneID scene, Vector2 positionInMap)
        {
            var mapRegion = MapRegions[scene.MapNumber];
            var tilePos = new Vector2((int)(positionInMap.X / 16.0), (int)(positionInMap.Y / 16.0));
            var ret=  mapRegion.UpperLeft.Translate(tilePos);
            return ret;
        }

        /// <summary>
        /// Converts a pixel position within a certain map to the equivalent tile position within the entire template
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="positionInMap"></param>
        /// <returns></returns>
        public Vector2 PointInTemplateToPositionInMap(SceneID scene, Vector2 pointInTemplate)
        {
            var mapRegion = MapRegions[scene.MapNumber];
            var pointInMap = pointInTemplate.Subtract(mapRegion.UpperLeft);
            var ret = pointInMap.Scale(16);
            return ret;
        }

    }
}
