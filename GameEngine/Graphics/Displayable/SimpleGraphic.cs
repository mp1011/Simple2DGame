using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class SimpleGraphic : IDisplayable 
    {
        public Sprite Sprite { get; private set; }

        public IRemoveable Root => Layer;

        public TextureDrawInfo DrawInfo => TextureDrawInfo.Fixed();

        public Rectangle Position { get; private set; }

        public Layer Layer { get; private set; }

        public SimpleGraphic(Vector2 topLeft, Sprite sprite, Layer layer)
        {
            Position = new Rectangle(topLeft.X, topLeft.Y, sprite.Texture.CellSize.X, sprite.Texture.CellSize.Y);
            Sprite = sprite;
            Layer = layer;
        }

        public void Draw(IRenderer painter)
        {
            painter.DrawSprite(this, Sprite.Texture, Sprite.Cell);
        }
    }
}
