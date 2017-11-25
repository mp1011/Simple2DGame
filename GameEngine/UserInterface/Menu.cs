using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IMenuItem
    {
        void OnSelection();
        void OnItemChosen();
    }
    
    public struct MenuKeys
    {
        public InputKey Select;
        public InputKey Cancel;
    }

    public class Menu<TItem> :IUpdateable
        where TItem : IMenuItem
    {
        public LayoutPanel MenuPanel { get; private set; }
        public Layer Layer { get; private set; }
        public SpriteFont Font { get; private set; }
        public string SelectedCharacter { get; private set; } = ">";
        private List<MenuOption<TItem>> Options = new List<MenuOption<TItem>>();
        UpdatePriority IUpdateable.Priority => UpdatePriority.ModalMenu;
        IRemoveable IUpdateable.Root => Layer;
        private IGameInputWithDPad Input;
        private MenuKeys MenuKeys;

        private bool setVisible = false;
        private bool hideNextFrame = false;

        public bool Visible
        {
            get
            {
                return MenuPanel.Visible;
            }
            set
            {
                if (!value)
                    MenuPanel.Visible = false;
                else
                {
                    setVisible = true;
                    foreach (var item in Options)
                        item.RefreshText();
                }
            }
        }

        protected IGameInputWithDPad GetInput() { return Input; }

        public Menu(Layer interfaceLayer, SpriteFont font, BorderTileSet tileSet, IGameInputWithDPad input, MenuKeys menuKeys)
        {
            MenuPanel = new LayoutPanel(tileSet, interfaceLayer);
            MenuPanel.Position.Center = Engine.GetScreenSize().Center;
            MenuPanel.Visible = false;

            Layer = interfaceLayer;
            Font = font;         
            Input = input;

            MenuKeys = menuKeys;

            foreach (var group in interfaceLayer.Scene.UpdateGroups)
            {
                if (group.Priority != UpdatePriority.ModalMenu && group.Priority != UpdatePriority.Input)
                    group.AddPauseCondition(MenuPanel.IsVisible());
            }

            interfaceLayer.Scene.AddObject(this);
        }

        public void AddOption(TItem option)
        {
            bool wasVisible = this.Visible;
            var newOption = new MenuOption<TItem>(option, this);
            Options.Add(newOption);
            SelectedOption = new CyclingInteger(Options.Count);
            
            MenuPanel.AddItem(newOption);

            this.Visible = wasVisible;
        }

        private CyclingInteger _selectedOption;

        public CyclingInteger SelectedOption
        {
            get
            {
                return _selectedOption;
            }
            set
            {
                _selectedOption = value;
                foreach (var option in Options)
                    option.Selected = false;

                Options[_selectedOption].Selected = true;
            }
        }

        public TItem SelectedItem => Options[SelectedOption].Item;
            
        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if(Input.GetButtonDown(MenuKeys.Cancel))
            {
                hideNextFrame = true;
                return;
            }

            if(hideNextFrame)
            {
                hideNextFrame = false;
                Visible = false;
                return;
            }

            if (!MenuPanel.Visible)
            {
                if(setVisible)
                {
                    MenuPanel.Visible = true;
                    setVisible = false;
                }

                return;
            }

            switch (Input.Pad.GetPressedVector().ToDirection(Direction.Right))
            {
                case Direction.Up:                    
                    SelectedOption--;
                    SelectedItem.OnSelection();
                    break;
                case Direction.Down:
                    SelectedOption++;
                    SelectedItem.OnSelection();
                    break;
            }

            if (Input.GetButtonPressed(MenuKeys.Select))
            {
                SelectedItem.OnItemChosen();
                Options[SelectedOption].Selected = true;
            }
        }

    }

    public class MenuOption<T> : IDynamicDisplayable
        where T:IMenuItem
    {
        private Menu<T> Menu;
        private GameText Text;

        public MenuOption(T item, Menu<T> menu)
        {
            Menu = menu;
            Text = new GameText(menu.Font, " " + item.ToString(), menu.Layer);
            Item = item;
        }

        public T Item { get; private set; }

        private bool _selected = false;
        public bool Selected
        {
            get
            {
                return _selected;
            } 
            set
            {
                _selected = value;
                RefreshText();
            }
        }

        public void RefreshText()
        {
            Text.Message = (Selected ? Menu.SelectedCharacter : " ") + Item.ToString();
        }

        public IRemoveable Root => Menu.Layer;

        public TextureDrawInfo DrawInfo => Text.DrawInfo;

        public Rectangle Position => Text.Position;
        
        TextureDrawInfo IDisplayable.DrawInfo => Text.DrawInfo;

        Rectangle IWithPosition.Position => Text.Position;

        void IDisplayable.Draw(IRenderer painter)
        {
            ((IDynamicDisplayable)Text).Draw(painter);
        }
    }
}
