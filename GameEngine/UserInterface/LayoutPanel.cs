using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class LayoutPanel : IWithPosition, IDisplayable
    {
        private BorderedRectangle Border;
        private List<IDisplayable> Items = new List<IDisplayable>();

        private int Padding = 8;
        
        public bool Visible
        {
            get
            {
                return Border.DrawInfo.Visible; ;
            }
            set
            {
                foreach (var item in Items)
                    item.DrawInfo.Visible = value;

                if(value && !Border.DrawInfo.Visible)
                    PerformLayout();

                Border.DrawInfo.Visible = value;
            }
        }

        public Rectangle Position => Border.Position;

        public TextureDrawInfo DrawInfo => ((IDisplayable)Border).DrawInfo;

        public LayoutPanel(BorderTileSet tileSet, Layer layer)
        {
            Border = new BorderedRectangle(tileSet, Rectangle.Empty(), layer);
        }

        public void AddItem(IDisplayable item)
        {
            Items.Add(item);
            PerformLayout();
        }

        private void PerformLayout()
        {
            var center = Border.Position.Center;

            Border.Position.SetWidth(Items.Max(p => p.Position.Width) + (Padding * 2), AnchorOrigin.Left);
            Border.Position.SetHeight(Items.Sum(p => p.Position.Height + Padding)  +Padding, AnchorOrigin.Top);

            Border.SnapToGrid();

            Border.Position.Center = center;

            var lastItem = Items.First();
            lastItem.DockInside(Border, BorderSide.Top).Nudge(0, Padding);

            foreach (var item in Items.Skip(1))
            {
                item.PutNextTo(lastItem, BorderSide.Bottom).Nudge(0,Padding);
                lastItem = item;
            }

            Border.RecalulateTiles();
        }

        public void Draw(IRenderer painter)
        {
            ((IDisplayable)Border).Draw(painter);
        }
    }
}
