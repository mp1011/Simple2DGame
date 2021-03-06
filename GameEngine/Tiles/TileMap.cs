﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Tile : IWithPosition
    {
        public int TileID { get; set; }
        public Rectangle Position { get; set; }
        public Vector2 TileIndex { get; set; }
        public bool IsSolid { get; set; }
        public bool IsCollidable { get; set; }
        public bool IsBoundary { get; set; }
        public TileMap TileMap { get; set; }

        public bool IsEmpty => TileID == TileMap.EmptyCell;

        public Tile GetAdjacent(BorderSide side)
        {
            return TileMap.GetTileFromGridPoint(TileIndex.GetAdjacent(side));
        }

        public Tile GetAdjacent(Direction side)
        {
            return TileMap.GetTileFromGridPoint(TileIndex.GetAdjacent(side));
        }

        public Tile GetFirstAdjacent(Direction side, Predicate<Tile> condition)
        {
            var possible = GetAdjacentWhile(side, t => !condition(t)).Last().GetAdjacent(side);
            if (condition(possible))
                return possible;
            else
                return null;
        }

        public IEnumerable<Tile> GetAdjacentWhile(Direction side, Predicate<Tile> condition)
        {
            return GetAdjacentWhile((BorderSide)side, condition);
        }

        public IEnumerable<Tile> GetAdjacentWhile(BorderSide side, Predicate<Tile> condition)
        {
            var tile = this;
            yield return this;

            while(side != BorderSide.None)
            {
                var adjacent = tile.GetAdjacent(side);
                if (!adjacent.Position.CollidesWith(tile.TileMap.Position,false))
                    break;

                if (condition(adjacent))
                    yield return adjacent;
                else
                    break;

                tile = adjacent;
            }
        }
    }

    public class TileMap<TSpecialTileInfo> : TileMap where TSpecialTileInfo : Tile, new()
    {
        public TileMap(Layer layer, TextureInfo texture, Vector2 mapSize) : base(layer,texture,mapSize)
        {
        }

        protected virtual TSpecialTileInfo GetSpecialTile(int tileID)
        {
            return new TSpecialTileInfo();
        }

        public override Tile GetTileFromGridPoint(Vector2 gridPoint)
        {
            int tileCel = Tiles.Cells.GetFromPoint(gridPoint);
           
            Tile tileHit = base.GetTileFromGridPoint(gridPoint);

            var ret = GetSpecialTile(tileCel);            
            ret.Position = tileHit.Position;
            ret.TileMap = tileHit.TileMap;
            ret.TileID = tileCel;
            ret.TileIndex = gridPoint;
            ret.IsBoundary = tileHit.IsBoundary;
            return ret;
        }

        public new IEnumerable<TSpecialTileInfo> GetTilesHit(Rectangle d)
        {
            return base.GetTilesHit(d).OfType<TSpecialTileInfo>();
        }
    }

    public class TileMap : IDisplayable, ICollidable, IBlock
    {
        public IRemoveable Root => Layer;
        public Scene Scene => Layer.Scene;
        public TextureDrawInfo DrawInfo { get { return TextureDrawInfo.Fixed(); } }

        public SpriteGrid Tiles { get; private set; }

        private Rectangle _position = null;
        public Rectangle Position
        {
            get
            {
                if (_position == null)
                {
                    var totalSize = Tiles.Texture.CellSize.Scale(Tiles.Cells.Size);
                    _position = new Rectangle(0, 0, totalSize.X, totalSize.Y);
                }
                return _position;
            }
        }

        public int NumberOfScreens
        {
            get
            {
                var screensX = (int)Math.Ceiling(Position.Width / Engine.GetScreenSize().Width);
                var screensY = (int)Math.Ceiling(Position.Height / Engine.GetScreenSize().Height);
                return screensX * screensY;
            }
        }

     
        public Layer Layer { get; private set; }

        bool IRemoveable.IsRemoved => false;

        private int _emptyCell;
        public int EmptyCell
        {
            get
            {
                return _emptyCell;
            }
            set
            {
                _emptyCell = value;
                this.Tiles.Cells.OutOfBoundsFixedValue = value;
            }
        }

        public TileMap(Layer layer, TextureInfo texture, Vector2 mapSize)
        {
            Layer = layer;
            Tiles = new SpriteGrid { Texture = texture, Cells = new ArrayGrid<int>(mapSize) };
        }

        void IDisplayable.Draw(IRenderer painter)
        {
            painter.DrawSpriteGrid(this, Tiles);
        }
        
        public Tile GetSolidHitTile(Rectangle collidingObjectPosition)
        {
            return GetTilesHit(collidingObjectPosition).FirstOrDefault(p => p.IsSolid);
        }

        public IEnumerable<Tile> GetTilesInLine(Vector2 pt1, Vector2 pt2)
        {
            var scale = new Vector2(1.0f / this.Tiles.Texture.CellSize.X, 1.0f / this.Tiles.Texture.CellSize.Y);
         
            var pt = pt1;

            while (pt.X != pt2.X && pt.Y != pt2.Y)
            {
                var tile = GetTileFromGridPoint(pt.Scale(scale));
                yield return tile;

                pt = new Vector2(pt.X.Approach(pt2.X, Tiles.Texture.CellSize.X),
                    pt.Y.Approach(pt2.Y, Tiles.Texture.CellSize.Y));
            }

        }

        public IEnumerable<Tile> GetTilesHit(Rectangle collidingObjectPosition)
        {
            var scale = new Vector2(1.0f / this.Tiles.Texture.CellSize.X, 1.0f / this.Tiles.Texture.CellSize.Y);
            Point upperLeftTile = collidingObjectPosition.UpperLeft.Scale(scale).ToPoint();
            Point bottomRightTile = collidingObjectPosition.BottomRight.Scale(scale).ToPoint();

            for (int x = upperLeftTile.X; x <= bottomRightTile.X; x++)
            {
                for (int y = upperLeftTile.Y; y <= bottomRightTile.Y; y++)
                {
                    var tile = GetTileFromGridPoint(new Vector2(x, y));
                    if (tile != null)
                        yield return tile;                    
                }
            }
        }

        public virtual Tile GetTileFromGridPoint(Vector2 gridPoint)
        {
            var tileCel = Tiles.Cells.GetFromPoint(gridPoint);
            return new Tile
            {
                TileID = tileCel,
                TileMap = this,
                TileIndex = gridPoint,
                Position = gridPoint.ToGridCell(Tiles.Texture.CellSize),
                IsSolid = tileCel != EmptyCell,
                IsBoundary = gridPoint.X <= 0 || gridPoint.Y <= 0 || 
                gridPoint.X >= Tiles.Cells.Size.X -1 || gridPoint.Y == Tiles.Cells.Size.Y -1
            };
        }

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            foreach(var tileHit in GetTilesHit(collidingObject))
            {
                if(tileHit.IsSolid)
                {
                    //if object is exactly on the ground don't count this as a collision
                    if (!collidingObject.Bottom.IsCloseTo(tileHit.Position.Top))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds all non-empty cells of the other map to this one
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public TileMap CombineWith(TileMap other)
        {
            int index = 0;
            foreach(var cell in other.Tiles.Cells)
            {
                if (cell != other.EmptyCell)
                    Tiles.Cells.Set(index, cell);

                index++;
            }

            return this;
        }

        void IRemoveable.Remove()
        {
            throw new NotImplementedException();
        }
    }
}
