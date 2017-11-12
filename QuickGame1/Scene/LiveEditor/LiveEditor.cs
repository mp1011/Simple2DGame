using GameEngine;
using System;
using System.Linq;
using XColor = Microsoft.Xna.Framework.Color;

namespace QuickGame1
{
    class EditorCursor : IDynamicDisplayable
    {
        private Layer Layer;
        IRemoveable IDynamicDisplayable.Root => Layer;

        private TextureDrawInfo drawInfo = new TextureDrawInfo { Opacity = 0.5f };
        TextureDrawInfo IDisplayable.DrawInfo => drawInfo;

        public Rectangle Position { get; } = new Rectangle(0, 0, 16, 16);

        public EditorCursor(QuickGameScene scene)
        {
            scene.InterfaceLayer.AddObject(this);
            Layer = scene.InterfaceLayer;
        }

        void IDisplayable.Draw(IRenderer painter)
        {
            painter.DrawRectangle(this, XColor.White);
        }
    }

    class LiveEditor : IUpdateable
    {
        public EditorItem ClipboardItem;

        private QuickGameScene Scene;
        private IMouseInput MouseInput;
        private IGameInput KeyboardInput;

        private EditorCursor Cursor;
        public EditorMenu Menu { get; private set; }
        public ItemSelector ItemSelector { get; private set; }

        UpdatePriority IUpdateable.Priority => UpdatePriority.BeginUpdate;

        IRemoveable IUpdateable.Root => Scene.InterfaceLayer;

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            Cursor.Position.Center = MouseInput.MousePosition.SnapTo(16f);
            Cursor.Position.KeepWithin(Engine.GetScreenSize());

            if (MouseInput.GetButtonPressed(GameKeys.PlaceObject))
            {
                var obj = GetObjectUnderCursor();
                if (obj != null)
                {
                    obj.Remove();
                }
                else
                {
                    if(ClipboardItem != null)
                    {
                        var newItem = ClipboardItem.CreateItem();
                        newItem.Position.Center = Cursor.Position.Center;
                    }
                }
            }
            else if (MouseInput.GetButtonPressed(GameKeys.CopyObject))
            {
                var obj = GetObjectUnderCursor();
                if (obj != null)
                {
                    ClipboardItem = EditorItem.FromObject(obj);
                }
            }

            if(!Menu.Visible && !ItemSelector.Visible && KeyboardInput.GetButtonPressed(GameKeys.EditorMenu))
            {
                Menu.Visible = true;
            }
        }

        private IEditorPlaceable GetObjectUnderCursor()
        {
            return Scene.SolidLayer.CollidableObjects.OfType<IEditorPlaceable>().FirstOrDefault(p => p.Position.CollidesWith(Cursor.Position,true));
        }

        public void SaveLevel()
        {
            new MapSaver().SaveToDisk(Scene);
        }

        public LiveEditor(QuickGameScene scene)
        {
            Scene = scene;
            KeyboardInput = Input.GetInput(scene);
            MouseInput = Input.GetMouseInput(scene);
            scene.AddObject(this);
            Cursor = new EditorCursor(scene);

            Menu = new EditorMenu(scene,this);
            ItemSelector = new ItemSelector(scene, this);
        }

      
    }

 
}
