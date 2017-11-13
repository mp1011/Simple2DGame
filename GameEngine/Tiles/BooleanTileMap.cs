using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public enum TileMaskValue
    {
        Empty,
        Filled,
        Obscured
    }

    public class TileMaskMap 
    {
        public ArrayGrid<TileMaskValue> TileFlags { get; private set; }

        public BorderTileSet TileSet { get; private set; }

        public TileMaskMap(ArrayGrid<bool> tiles, BorderTileSet tileSet) 
        {
            TileFlags = new ArrayGrid<TileMaskValue>(tiles.Columns, tiles.Select(p => p ? TileMaskValue.Filled : TileMaskValue.Empty));
            TileFlags.ReplaceOutOfBoundsTilesWithAdjacent = true;
            TileSet = tileSet;
        }

        public TileMaskMap(int columns, int rows, BorderTileSet tileSet) 
            : this(new ArrayGrid<bool>(new Vector2(columns,rows)), tileSet)
        {
        }

        public void Apply(TileMap map, bool applyEmptyCells=true)
        { 
            foreach(var tile in TileFlags.PointItems)
            {
                if (tile.Value == TileMaskValue.Filled)
                {
                    var computedCell = TileSet.GetCell(GetNeighbors(tile));
                    map.Tiles.Cells.Set(tile.Position, computedCell);
                }
                else if (applyEmptyCells)
                {
                    map.Tiles.Cells.Set(tile.Position, TileSet.GetCell(BorderSide.EmptySpace));
                }

            }

        }

        private BorderSide GetNeighbors(ArrayGridPoint<TileMaskValue> tile)
        {
            bool hasLeft, hasAbove, hasBelow, hasRight;
            bool hasUpperLeftCorner, hasUpperRightCorner, hasLowerLeftCorner, hasLowerRightCorner;
            
            hasLeft = tile.GetAdjacent(Direction.Left).Value  != TileMaskValue.Empty;
            hasRight = tile.GetAdjacent(Direction.Right).Value != TileMaskValue.Empty;
            hasAbove = tile.GetAdjacent(Direction.Up).Value != TileMaskValue.Empty;
            hasBelow = tile.GetAdjacent(Direction.Down).Value != TileMaskValue.Empty;

            hasUpperLeftCorner = tile.GetAdjacent(BorderSide.TopLeftCorner).Value != TileMaskValue.Empty;
            hasUpperRightCorner = tile.GetAdjacent(BorderSide.TopRightCorner).Value != TileMaskValue.Empty;
            hasLowerLeftCorner = tile.GetAdjacent(BorderSide.BottomLeftCorner).Value != TileMaskValue.Empty;
            hasLowerRightCorner = tile.GetAdjacent(BorderSide.BottomRightCorner).Value != TileMaskValue.Empty;
            
            var ret = BorderSide.None;
            if (hasLeft)
                ret = ret | BorderSide.Left;
            if (hasAbove)
                ret = ret | BorderSide.Top;
            if (hasRight)
                ret = ret | BorderSide.Right;
            if (hasBelow)
                ret = ret | BorderSide.Bottom;
          
            if (hasUpperLeftCorner)
                ret = ret | BorderSide.TopLeftCorner;
            if (hasUpperRightCorner)
                ret = ret | BorderSide.TopRightCorner;
            if (hasLowerLeftCorner)
                ret = ret | BorderSide.BottomLeftCorner;
            if (hasLowerRightCorner)
                ret = ret | BorderSide.BottomRightCorner;

            return ret;

        }
    }
}
