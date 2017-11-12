using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class BorderedRectangle : IDynamicDisplayable 
    {     
        private TileMap tileMap;
        private BorderTileSet Tileset;
        public IRemoveable Root => Layer;

        public TextureDrawInfo DrawInfo { get; private set; } = new TextureDrawInfo();

        public Rectangle Position { get; private set; }

        public Layer Layer { get; private set; }

        public BorderedRectangle(BorderTileSet tileset, Rectangle position, Layer layer)
        {
            Layer = layer;
            layer.AddObject(this);
            Position = position;
            Tileset = tileset;
            RecalulateTiles();
        }

        public void SnapToGrid()
        {
            Position.SetWidth(Position.Width.SnapTo(Tileset.Texture.CellSize.X), AnchorOrigin.Left);
            Position.SetHeight(Position.Height.SnapTo(Tileset.Texture.CellSize.Y), AnchorOrigin.Top);
        }

        //todo: can we detect changes and fire this automatically?
        public void RecalulateTiles()
        {
            var tilesX = Position.Width / Tileset.Texture.CellSize.X;
            var tilesY = Position.Height / Tileset.Texture.CellSize.Y;
            tileMap = new TileMap(Layer, Tileset.Texture, new Vector2((float)tilesX, (float)tilesY));

            var cellArea = new Rectangle(tileMap.Tiles.Cells.Size);

            foreach (var point in tileMap.Tiles.Cells.Points)
            {
                var borderSide = point.GetBorderSide(cellArea);
                var tile = Tileset.GetCell(borderSide);
                tileMap.Tiles.Cells.Set(point, tile);
            }
        }

        public void Draw(IRenderer painter)
        {
            painter.DrawSpriteGrid(this, tileMap.Tiles);
        }
    }
}
