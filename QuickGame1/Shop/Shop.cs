using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    interface IShop :ICollidable
    {
        ShopMenu ShopMenu { get; set; }
    }

    static class IShopExtensions
    {
        public static T SetShopHandler<T>(this T item, ShopMenu sh) where T:IShop
        {
            item.ShopMenu = sh;
            return item;
        }
    }
    

    class ShopMenu : Menu<ShopOption>
    {
    
        public ShopMenu(QuickGameScene scene) : base(scene.InterfaceLayer, Fonts.SmallFont, GameTiles.Border(), Input.GetInput(scene), GameKeys.Attack)
        {

            MenuPanel.AddItem(new GameText(Fonts.SmallFont, "WELCOME!", scene.InterfaceLayer));
            MenuPanel.AddItem(new DynamicText<King>(scene.Player, p => "COINS: " + p.Coins, Fonts.SmallFont, scene.InterfaceLayer));

            AddOption(new ExtraHeartShopItem() { Player = scene.Player, MenuPanel = MenuPanel });
            AddOption(new BetterAttackShopItem() { Player = scene.Player, MenuPanel = MenuPanel });
            AddOption(new ExitShopOption() { Player = scene.Player, MenuPanel = MenuPanel });

        }

        public void CheckEnterShop()
        {
            if (GetInput().GetButtonPressed(GameKeys.Up))
            {
                Visible = true;
            }
        }
    }
}
