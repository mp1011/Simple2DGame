using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Represents a number by showing several icons which may be full, half, or empty versions.
    /// </summary>
    public class IconCounter : IDynamicDisplayable
    {
        private SpriteGrid icons;
        private int fullIndex;
        private int halfIndex;
        private int emptyIndex;

        private BoundedInteger _value;

        public BoundedInteger Value
        {
            get
            {
                return _value;
            }
            set
            {
                if(value.GetMax() != _value.GetMax())
                {
                    icons.Cells = new ArrayGrid<int>(new Vector2(value.GetMax() / 2, 1));
                }

                if (value != _value.Value)
                {
                    _value = value;
                    CalculateIcons();
                }
            }
        }
        
        TextureDrawInfo IDisplayable.DrawInfo => TextureDrawInfo.Fixed();

        public Rectangle Position { get; set; }

        public Layer Layer { get; private set; }

        IRemoveable IDynamicDisplayable.Root => Layer;
    
        public IconCounter(int max, TextureInfo texture, int fullCell, int halfCell, int emptyCell, Layer layer)
        {
            fullIndex = fullCell;
            halfIndex = halfCell;
            emptyIndex = emptyCell;

            icons = new SpriteGrid { Texture = texture, Cells = new ArrayGrid<int>(new Vector2(max / 2, 1)) };

            CalculateIcons();

            Layer = layer;
        
            Position = new Rectangle(0, 0, (max / 2) * texture.CellSize.X, texture.CellSize.Y);
            Layer.AddObject(this);
        }


        private void CalculateIcons()
        {
            int fullCells = Value / 2;
            int halfCells = Value % 2;
         
            foreach(var ix in Enumerable.Range(0,(int)icons.Cells.Size.X))
            {
                if (ix < fullCells)
                    icons.Cells.Set(ix, fullIndex);
                else if (ix == (fullCells + halfCells)-1)
                    icons.Cells.Set(ix, halfIndex);
                else
                    icons.Cells.Set(ix, emptyIndex);
            }
        }

        void IDisplayable.Draw(IRenderer painter)
        {
            painter.DrawSpriteGrid(this, icons);
        }
    }
}
