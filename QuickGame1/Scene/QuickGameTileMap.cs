using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class QuickGameTileMap : TileMap<QuickGameTile>
    {
        public QuickGameTileMap(Layer layer, TextureInfo texture, Vector2 tileDimensions) : base(layer,texture, tileDimensions)
        {
           
        }

        protected override QuickGameTile GetSpecialTile(int tileID)
        {

            var tileIDPt = Tiles.Texture.IndexToPoint(tileID);
            if(tileIDPt.X == 0 && tileIDPt.Y >= 4 && tileIDPt.Y <= 6)
            {
                return new QuickGameTile
                {
                    IsSolid = false,
                    IsCollidable = true,
                    IsSpike = false,
                    IsLadder = true,
                    Position = new GameEngine.Rectangle(Tiles.Texture.CellSize)
                };
            }

            if (tileID == Tiles.Texture.PointToIndex(3, 7))
            {
                return new QuickGameTile
                {
                    IsSolid = false,
                    IsCollidable = true,
                    IsSpike = true,
                    Position = new GameEngine.Rectangle(Tiles.Texture.CellSize)
                };
            }

            if(tileID == Tiles.Texture.PointToIndex(1,6) || tileID == Tiles.Texture.PointToIndex(1,5))
            {
                return new QuickGameTile
                {
                    IsWater = true,
                    IsSolid = false,
                    IsCollidable = false,
                    Position = new GameEngine.Rectangle(Tiles.Texture.CellSize)
                };
            }

            foreach (var emptyCell in new Vector2[] {
                new Vector2(7, 5), new Vector2(8, 5), new Vector2(7, 6), new Vector2(8, 6), new Vector2(4,3), new Vector2(9,5), new Vector2(9,6) })
            {
                if (tileID == Tiles.Texture.PointToIndex(emptyCell))
                {
                    return new QuickGameTile
                    {
                        IsSolid = false,
                        Position = new GameEngine.Rectangle(Tiles.Texture.CellSize)
                    };
                }
            }

            return new QuickGameTile()
            {
                IsSolid = tileID != EmptyCell
            };
        }
    }

    class QuickGameTile : Tile
    {
        public bool IsSpike { get; set; }
        public bool IsWater { get; set; }
        public bool IsLadder { get; set; }
    }
}
