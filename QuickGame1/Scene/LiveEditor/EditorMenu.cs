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
        public EditorMenu(QuickGameScene Scene, LiveEditor editor) : base(Scene.InterfaceLayer, Fonts.SmallFont, GameTiles.Border(), Input.GetInput(Scene), GameKeys.Attack)
        {
            AddOption(new ItemsOption() { Editor = editor });
            AddOption(new SaveLevelOption() { Editor = editor });
            AddOption(new CancelOption() { Editor = editor });
        }
    }

    abstract class EditorItem : IMenuItem
    {
        public LiveEditor Editor;

        protected abstract void OnItemChosen(LiveEditor editor);

        void IMenuItem.OnItemChosen()
        {
            OnItemChosen(Editor);
        }

        void IMenuItem.OnSelection()
        {
        }

        public abstract IEditorPlaceable CreateItem();

        public static EditorItem FromObject(IEditorPlaceable obj)
        {
            return Activator.CreateInstance(typeof(EditorItem<>).MakeGenericType(obj.GetType())) as EditorItem;
        }
    }

    class EditorItem<T> : EditorItem
        where T : IEditorPlaceable
    {
        public override string ToString()
        {
            return typeof(T).Name.ToUpper();
        }

        protected override void OnItemChosen(LiveEditor editor)
        {
            editor.ClipboardItem = this;
            editor.ItemSelector.Visible = false;
        }

        public override IEditorPlaceable CreateItem()
        {
            return Activator.CreateInstance<T>();
        }
    }

    class ItemSelector : Menu<EditorItem>
    {
        public ItemSelector(QuickGameScene Scene, LiveEditor editor) : base(Scene.InterfaceLayer, Fonts.SmallFont, GameTiles.Border(), Input.GetInput(Scene), GameKeys.Attack)
        {
            AddOption(new EditorItem<Coin>() { Editor = editor } );
            AddOption(new EditorItem<Snake>() { Editor = editor });

        }
    }
}
