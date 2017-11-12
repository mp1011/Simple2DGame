using System;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameEngine
{
    public static class DebugText
    {
        public static Vector2 NewTextPosition = new Vector2(50, 50);

        public static void DebugWatch<T>(this T obj, SpriteFont font, Layer displayLayer, Func<T, string> fn)
        {
            new DebugText<T>(font, obj, displayLayer, fn);
        }
    }

    public class DebugText<T> : IUpdateable, IDynamicDisplayable
    {
        public UpdatePriority Priority { get { return UpdatePriority.BeginUpdate; } }

        private Layer Layer;
        IRemoveable IUpdateable.Root => Layer;
        IRemoveable IDynamicDisplayable.Root => Layer;

        private T Object;
        private Func<T, string> GetText;
        private GameText Text;
        
        public TextureDrawInfo DrawInfo => ((IDisplayable)Text).DrawInfo;

        public Rectangle Position => ((IDisplayable)Text).Position;
              
        internal DebugText(SpriteFont font, T obj, Layer layer, Func<T,string> fn)
        {
            Layer = layer;
            Object = obj;
            GetText = fn;
            Text = new GameText(font, GetText(Object), layer);

            Text.Position.SetLeft(DebugText.NewTextPosition.X);
            Text.Position.SetTop(DebugText.NewTextPosition.Y);

            DebugText.NewTextPosition.Y = (float)(Text.Position.Bottom + 16);

            layer.Scene.AddObject(this);
        }

        public void Update(TimeSpan elapsedInFrame)
        {
            Text.Message = GetText(Object);
        }

        public void Draw(IRenderer painter)
        {
            ((IDisplayable)Text).Draw(painter);
        }
    }
}
