using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class GameText : IDynamicDisplayable, IWithPosition
    {
        public SpriteFont Font { get; private set; }

        private SpriteGrid CharactersGrid;

        private string msg;
        public string Message
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
                CharactersGrid = Font.GetStringSprite(msg);
                CalculateSize();
            }
        }

        public GameText(SpriteFont font, string text, Layer layer)
        {
            Font = font;
            Layer = layer;
            Message = text;

            layer.AddObject(this);
        }

        public TextureDrawInfo DrawInfo { get; private set; } = new TextureDrawInfo();

        private Rectangle pos = new Rectangle();
        public Rectangle Position => pos;

        IRemoveable IDynamicDisplayable.Root => Layer;

        private Layer Layer;

        private void CalculateSize()
        {
            var width = Font.Texture.CellSize.X * Message.Length;
            var height = Font.Texture.CellSize.Y;

            pos.SetWidth(width, AnchorOrigin.Left);
            pos.SetHeight(height, AnchorOrigin.Top);
        }

        void IDisplayable.Draw(IRenderer painter)
        {
            painter.DrawSpriteGrid(this, CharactersGrid);
        }
    }

}
