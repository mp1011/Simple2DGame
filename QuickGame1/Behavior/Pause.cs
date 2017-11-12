using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class PauseHandler : IUpdateable
    {
        public ManualCondition IsPaused { get; private set; } = new ManualCondition();

        UpdatePriority IUpdateable.Priority => UpdatePriority.Input;

        IRemoveable IUpdateable.Root => GameRoot.Current;

        private InputCondition pauseInputCondition;
        private LayoutPanel pauseMenu;

        public PauseHandler()
        {
            var scene = QuickGameScene.Current;
            scene.AddObject(this);

            pauseInputCondition = new InputCondition(GameKeys.Start, Input.GetInput(scene));
           
            foreach (var group in scene.UpdateGroups)
            {
                if (group.Priority != UpdatePriority.ModalMenu && group.Priority != UpdatePriority.Input)
                    group.AddPauseCondition(IsPaused);
            }

            pauseMenu = new LayoutPanel(GameTiles.Border(), scene.InterfaceLayer);
            pauseMenu.AddItem(new GameText(Fonts.BigFont, "PAUSED", scene.InterfaceLayer));
            pauseMenu.AddItem(new DynamicText<King>(scene.Player, p => "COINS: " + p.Coins, Fonts.BigFont, scene.InterfaceLayer));

            pauseMenu.Position.Center = Engine.GetScreenSize().Center;
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            pauseMenu.Visible = IsPaused.IsActive;

            if (pauseInputCondition.WasJustActivated())
            {
                AudioEngine.Instance.PlaySound(Sounds.GetCoin);
                IsPaused.Active = !IsPaused.Active;
            }
        }
    }


 
}
