using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    enum DamageType
    {
        Player = 1,
        Enemy = 2,
        PlayerAttack = 4,
        EnemyAttack = 8,
        Trap = 16
    }

    interface IDamageable : IMovingWorldObject
    {  
        bool RecoilsWhenHit { get; set; }
        DamageType TakesDamageFrom { get; }
        DamageHandler DamageHandler { get; }
    }

    interface IDamager : ICollidable, IWithPositionAndDirection
    {
        DamageType DamageType { get; }
    }

    abstract class DamageHandler
    {
        public BoundedInteger Hitpoints;
        public abstract bool ObjectIsInvincible { get; }
        public abstract void TakeDamageFrom(IDamager damager);
    }

    abstract class DamageHandler<T> : DamageHandler, IUpdateable
        where T:IDamageable,IWithSprite
    {
        private Timer InvincibilityTimer;
        protected T Object;
        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;
        IRemoveable IUpdateable.Root => Object;

        public override bool ObjectIsInvincible => InvincibilityTimer.IsRunning;

        public DamageHandler(int hp, T obj)
        {
            InvincibilityTimer = new Timer(new ConfigValue<TimeSpan>(InvincibilityTimeSettingName), obj);
            InvincibilityTimer.Stop();
            Hitpoints = new BoundedInteger(hp);
            Object = obj;
            obj.Layer.Scene.AddObject(this);
        }

        protected abstract string InvincibilityTimeSettingName { get; }

        public override void TakeDamageFrom(IDamager damager)
        {
            if (!InvincibilityTimer.IsRunning)
            {
                InvincibilityTimer.Reset();
                Hitpoints -= 1;

                if (Hitpoints == 0)
                    OnDeath(damager);
                else
                    OnHit(damager);
            }
        }

        protected abstract void OnHit(IDamager damager);
        protected abstract void OnDeath(IDamager damager);

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if (InvincibilityTimer.IsRunning)
                Object.Sprite.DrawInfo.Visible = !Object.Sprite.DrawInfo.Visible;
            else
                Object.Sprite.DrawInfo.Visible = true;

        }
    }

    static class DamagerExtensions
    {
        public static bool TakesDamageFrom(this IDamageable victim, IDamager damager)
        {
            return !victim.DamageHandler.ObjectIsInvincible && (damager.DamageType & victim.TakesDamageFrom) > 0;
        }
    }

    class PlayerDamageHandler : DamageHandler<King>
    {
        public PlayerDamageHandler(int hp, King obj) : base(hp, obj)
        {
        }

        protected override string InvincibilityTimeSettingName => "player invincibility time";

        protected override void OnDeath(IDamager damager)
        {
            new Poof().PositionRelativeTo(Object, 0, 0);
            Object.Remove();
            
            new GameOverText(Object.Scene);
        }

        protected override void OnHit(IDamager damager)
        {

        }
    }

    class EnemyDamageHandler<T> : DamageHandler<T>
        where T : IDamageable, IWithSprite, IWorldObject<QuickGameScene>
    {
        private static RandomChance PrizeDropChance = new RandomChance(2, 6);
        public EnemyDamageHandler(int hp, T obj) : base(hp, obj)
        {
        }

        protected override string InvincibilityTimeSettingName => "enemy invincibility time";

        protected override void OnDeath(IDamager damager)
        {
            new Poof().PositionRelativeTo(Object, 0, 0);
            Object.Remove();
            Object.Scene.Player.Score += 50;

            if(PrizeDropChance.GetNext())
                new SmallHeart().PositionRelativeTo(Object, 0, -8);
        }

        protected override void OnHit(IDamager damager)
        {

        }
    }

}
