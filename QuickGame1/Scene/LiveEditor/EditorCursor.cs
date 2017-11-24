using GameEngine;
using XColor = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System;

namespace QuickGame1
{
    class EditorCursor : IDynamicDisplayable
    {
        private Layer Layer;
        IRemoveable IDynamicDisplayable.Root => Layer;

        private TextureDrawInfo drawInfo = new TextureDrawInfo { Opacity = 0.5f };
        TextureDrawInfo IDisplayable.DrawInfo => drawInfo;

        private Animation CursorSprite;

        public Rectangle Position { get; } = new Rectangle(0, 0, 16, 16);
        private AbstractPosition PointerPosition = new AbstractPosition();

        public EditorCursor(QuickGameScene scene)
        {
            scene.SolidLayer.AddObject(this);
            Layer = scene.InterfaceLayer;
            CursorSprite = new Animation(Layer, PointerPosition, new Sprite(Textures.CursorTexture), 0, 1, 2, 3, 4, 5, 6, 7);         
        }

        public void UpdateCursor(Vector2 mousePosition, TimeSpan elapsedInFrame)
        {
            mousePosition = Engine.GetScreenBoundary().Position.UpperLeft.Translate(mousePosition);

            CursorSprite.Update(elapsedInFrame);
            PointerPosition.Position.Center = mousePosition;
            Position.UpperLeft = mousePosition.SnapTo(16f);
            Position.KeepWithin(Engine.Instance.Renderer.ScreenBounds.Position);
        }

        void IDisplayable.Draw(IRenderer painter)
        {
            painter.DrawRectangle(this, XColor.White);
            CursorSprite.Draw(painter);
        }
    }

}
