using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class BooleanTileMap 
    {
        public ArrayGrid<bool> TileFlags { get; private set; }

        public BorderTileSet TileSet { get; private set; }
      
        public BooleanTileMap(ArrayGrid<bool> tiles, BorderTileSet tileSet) 
        {
            TileFlags = tiles;
            TileFlags.ReplaceOutOfBoundsTilesWithAdjacent = true;
            TileSet = tileSet;
        }

        public BooleanTileMap(int columns, int rows, BorderTileSet tileSet) 
            : this(new ArrayGrid<bool>(new Vector2(columns,rows)), tileSet)
        {
        }

        public void Apply(TileMap map, bool applyEmptyCells=true)
        { 
            TileFlags.Select((tile, index) =>
            {
                if (tile)
                {
                    var computedCell = TileSet.GetCell(GetNeighbors(index));
                    map.Tiles.Cells.Set(index, computedCell);
                }
                else if(applyEmptyCells)
                {
                    map.Tiles.Cells.Set(index, TileSet.GetCell(BorderSide.EmptySpace));
                }
                  
                return 0;
            }).ToArray();
        }

        private BorderSide GetNeighbors(int index)
        {
            var pt = TileFlags.IndexToPoint(index);

            bool hasLeft = TileFlags.GetFromPoint(pt.Translate(-1, 0));
            bool hasAbove = TileFlags.GetFromPoint(pt.Translate(0, -1));
            bool hasBelow = TileFlags.GetFromPoint(pt.Translate(0, 1));
            bool hasRight = TileFlags.GetFromPoint(pt.Translate(1, 0));

            var ret = BorderSide.None;
            if (hasLeft)
                ret = ret | BorderSide.Left;
            if (hasAbove)
                ret = ret | BorderSide.Top;
            if (hasRight)
                ret = ret | BorderSide.Right;
            if (hasBelow)
                ret = ret | BorderSide.Bottom;

            bool hasUpperLeftCorner = TileFlags.GetFromPoint(pt.Translate(-1, -1));
            bool hasUpperRightCorner = TileFlags.GetFromPoint(pt.Translate(1, -1));
            bool hasLowerLeftCorner = TileFlags.GetFromPoint(pt.Translate(-1, 1));
            bool hasLowerRightCorner = TileFlags.GetFromPoint(pt.Translate(1, 1));

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
