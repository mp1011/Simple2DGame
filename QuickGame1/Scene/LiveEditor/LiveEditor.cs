using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;


namespace QuickGame1
{
    
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
            Cursor.UpdateCursor(MouseInput.MousePosition, elapsedInFrame);

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
                        FreezeScene(Scene);

                        var newItem = ClipboardItem.CreateItem();
                        newItem.Position.Center = Cursor.Position.Center;
                        FrozenObject.Create(newItem, Scene);
                    }
                }
            }
            else if (MouseInput.GetButtonPressed(GameKeys.CopyObject))
            {
                var obj = GetObjectUnderCursor();
                if (obj != null)
                {
                    ClipboardItem = new EditorItem { ItemType = obj.EditorType, Editor = this };
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

        public bool IsFrozen(QuickGameScene scene)
        {
            return scene.SolidLayer.CollidableObjects.OfType<FrozenObject>().Any();
        }

        public void FreezeScene(QuickGameScene scene)
        {
            var mapObjects = scene.SolidLayer.CollidableObjects.OfType<IEditorPlaceable>().ToArray();
            foreach (var mo in mapObjects)
                FrozenObject.Create(mo, scene);
            scene.SolidLayer.Cleanup();
        }

        public void UnfreezeScene(QuickGameScene scene)
        {
            var mapObjects = scene.SolidLayer.CollidableObjects.OfType<FrozenObject>().ToArray();
            foreach (var frozenObject in mapObjects)                
                frozenObject.Unfreeze();

            scene.SolidLayer.Cleanup();
        }
        
    }

 
}
