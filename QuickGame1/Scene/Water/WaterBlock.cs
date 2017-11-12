using GameEngine;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace QuickGame1
{
    class WaterBlock : IWithPosition, IDynamicDisplayable
    {
        Rectangle pos;
        Rectangle IWithPosition.Position => pos;
        TileMap overlayTiles;

        IRemoveable IDynamicDisplayable.Root => Layer;

        private TextureDrawInfo drawInfo;
        TextureDrawInfo IDisplayable.DrawInfo => drawInfo;

   

        private Layer Layer;

        public WaterBlock(Layer layer, Rectangle rec)
        {
            Layer = layer;
            pos = rec;
            drawInfo = new TextureDrawInfo() { Opacity = 0.5f };

            var map = layer.FixedDisplayable.OfType<TileMap>().Last();
            var cs = map.Tiles.Texture.CellSize;

            var mapSize = new Vector2((float)rec.Width / cs.X, (float)rec.Height / cs.Y);
            overlayTiles = new TileMap(layer, map.Tiles.Texture, mapSize);
            overlayTiles.Position.UpperLeft = rec.UpperLeft;

            Vector2 tileOffset = new Vector2((float)rec.Left / cs.X, (float)rec.Top / cs.Y);

            foreach (var pt in overlayTiles.Tiles.Cells.Points)
            {
                var baseTile = map.GetTileFromGridPoint(pt.Translate(tileOffset));
                overlayTiles.Tiles.Cells.Set(pt, baseTile.TileID);
            }

        }

        private Microsoft.Xna.Framework.Color waterColor = new Microsoft.Xna.Framework.Color(0, 50, 190);
        void IDisplayable.Draw(IRenderer painter)
        {
            painter.DrawRectangle(this, waterColor);
            painter.DrawSpriteGrid(this, overlayTiles.Tiles);
        }

      
    }
}
