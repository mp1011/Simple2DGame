using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class DebugRectangle : IDynamicDisplayable
    {
        public TextureDrawInfo DrawInfo { get { return TextureDrawInfo.Fixed(); } }

        public IWithPosition Object { get; private set; }

        Rectangle IWithPosition.Position { get { return Object.Position; } }

        private Layer Layer;
        IRemoveable IDynamicDisplayable.Root => Layer;

        private TextureDrawInfo drawInfo = new TextureDrawInfo { Opacity = 0.5f };
        TextureDrawInfo IDisplayable.DrawInfo => drawInfo;

        void IDisplayable.Draw(IRenderer painter)
        {
            painter.DrawRectangle(this, Color.Red);
        }

        public DebugRectangle(IWorldObject obj) : this(obj, obj.Layer) { }

        public DebugRectangle(IWithPosition obj, Layer layer)
        {
            Layer = layer;
            Object = obj;
            Layer.AddObject(this);
        }
    }
}
