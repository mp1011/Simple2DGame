using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    abstract class EditorOption : IMenuItem
    {
        protected abstract void OnOptionSelected(LiveEditor editor);

        public LiveEditor Editor;

        public void OnItemChosen()
        {
            OnOptionSelected(Editor.Require());
        }

        public void OnSelection()
        {
        }
    }

    class SaveLevelOption : EditorOption
    {
        public override string ToString()
        {
            return "SAVE LEVEL";
        }

        protected override void OnOptionSelected(LiveEditor editor)
        {
            editor.SaveLevel();
            editor.Menu.Visible = false;
        }
    }

    class ReloadOption : EditorOption
    {
        public override string ToString()
        {
            return "RELOAD";
        }

        private QuickGameScene Scene;

        public ReloadOption(QuickGameScene scene)
        {
            Scene = scene;
        }

        protected override void OnOptionSelected(LiveEditor editor)
        {
            var playerPos = Scene.Player.Position.Center;

            foreach (var objectInThisScene in Scene.SolidLayer.CollidableObjects.OfType<IEditorPlaceable>())
            {
                objectInThisScene.Remove();
            }

            Scene.SolidLayer.Cleanup();

            var reloadedScene = Engine.Instance.SceneLoader.LoadScene(Scene.ID, forceReload: true) as QuickGameScene;
            var newObjects = reloadedScene.SolidLayer.CollidableObjects.OfType<IEditorPlaceable>().ToArray().Select(p => FrozenObject.Create(p, Scene)).ToArray();

            Engine.Instance.Scene = Scene;
            QuickGameScene.Current = Scene;

            editor.Menu.Visible = false;
        }
    }

    class FreezeItemsOption : EditorOption
    {
        private QuickGameScene Scene;

        public FreezeItemsOption(QuickGameScene scene)
        {
            Scene = scene;
        }
        public override string ToString()
        {
            if (Editor.IsFrozen(Scene))
                return "UNFREEZE";
            else
                return "FREEZE";
        }

        protected override void OnOptionSelected(LiveEditor editor)
        {
            if (Editor.IsFrozen(Scene))
                editor.UnfreezeScene(Scene);
            else
                editor.FreezeScene(Scene);
          
            editor.Menu.Visible = false;
        }
    }

    class ItemsOption : EditorOption
    {
        public override string ToString()
        {
            return "ADD ITEM";
        }

        protected override void OnOptionSelected(LiveEditor editor)
        {
            editor.Menu.Visible = false;
            editor.ItemSelector.Visible = true;
        }
    }

    class CancelOption : EditorOption
    {
        public override string ToString()
        {
            return "CANCEL";
        }

        protected override void OnOptionSelected(LiveEditor editor)
        {
            editor.Menu.Visible = false;
        }
    }

    class EditorMenu : Menu<EditorOption>
    {
        public EditorMenu(QuickGameScene Scene, LiveEditor editor) : base(Scene.InterfaceLayer, Fonts.SmallFont, GameTiles.Border(), Input.GetInput(Scene), 
            new MenuKeys { Select = GameKeys.MenuOK, Cancel = GameKeys.EditorMenu })
        {
            AddOption(new ItemsOption() { Editor = editor });
            AddOption(new FreezeItemsOption(Scene) { Editor = editor });
            AddOption(new ReloadOption(Scene) { Editor = editor });
            AddOption(new SaveLevelOption() { Editor = editor });
            AddOption(new CancelOption() { Editor = editor });
        }
    }

    class EditorItem : IMenuItem
    {
        public override string ToString()
        {
            return ItemType.ToString().ToUpper();
        }

        public LiveEditor Editor;
        public CellType ItemType;
        
        void IMenuItem.OnItemChosen()
        {
            Editor.ClipboardItem = this;
            Editor.ItemSelector.Visible = false;
        }

        void IMenuItem.OnSelection()
        {
        }      

        public IEditorPlaceable CreateItem()
        {
            return new MapSaver().CreateObject(ItemType, new ObjectStartInfo());
        }
    }
    

    class ItemSelector : Menu<EditorItem>
    {
        public ItemSelector(QuickGameScene Scene, LiveEditor editor) : base(Scene.InterfaceLayer, Fonts.SmallFont, GameTiles.Border(), Input.GetInput(Scene),
            new MenuKeys { Select = GameKeys.MenuOK, Cancel = GameKeys.EditorMenu })
        {
            foreach(var cellType in EnumHelper.GetValues<CellType>().Where(p=>p!=CellType.Empty))
                AddOption(new EditorItem() { ItemType = cellType, Editor = editor} );

        }
    }
}
