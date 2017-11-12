using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    abstract class ShopOption : IMenuItem
    {
        public abstract string Name { get; }

        public abstract int Cost { get; }

        public override string ToString()
        {
            if (Cost > 0)
                return Name + "=" + Cost;
            else
                return Name;
        }

        public ICanGetPrizes Player;
        public LayoutPanel MenuPanel;

        protected abstract void OnItemSelected(ICanGetPrizes player);

        void IMenuItem.OnSelection()
        {
            AudioEngine.Instance.PlaySound(Sounds.GetCoin);
        }

        void IMenuItem.OnItemChosen()
        {
            if (Player.Coins < Cost)
            {
                AudioEngine.Instance.PlaySound(Sounds.PlayerHit);
            }
            else
            {
                Player.Coins -= Cost;
                OnItemSelected(Player);
                MenuPanel.Visible = false;
            }
        }
    }

    class ExtraHeartShopItem : ShopOption
    {
        public override string Name => "EXTRA HEART";

        public override int Cost => 2;

        protected override void OnItemSelected(ICanGetPrizes player)
        {
            var hp = player.DamageHandler.Hitpoints;
            player.DamageHandler.Hitpoints = new BoundedInteger(hp.GetMax() + 2);
        }
    }

    class BetterAttackShopItem : ShopOption
    {
        public override string Name => "BETTER ATTACK";

        public override int Cost => 5;

        protected override void OnItemSelected(ICanGetPrizes player)
        {
            player.HasBetterAttack = true;
        }
    }

    class ExitShopOption : ShopOption
    {
        public override string Name => "EXIT";

        public override int Cost => 0;

        protected override void OnItemSelected(ICanGetPrizes player)
        {
        }
    }
}
