using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class EnemyHitbox<TEnemy> : IDamager
        where TEnemy : MovingActor, IRemoveable
    {
        private TEnemy Enemy;
        private Hitbox Hitbox;

        DamageType IDamager.DamageType => DamageType.EnemyAttack;

        bool IRemoveable.IsRemoved => Enemy.IsRemoved;

        Direction IWithPositionAndDirection.Direction { get => Enemy.Direction; set => Enemy.Direction = value; }

        Rectangle IWithPosition.Position => Hitbox.Position;

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            if (Hitbox.Condition.IsActiveAndNotNull())
                return Hitbox.Position.CollidesWith(collidingObject, ignoreEdges);
            else
                return false;
        }

        void IRemoveable.Remove()
        {            
        }

        public EnemyHitbox(TEnemy enemy, Hitbox hitbox)
        {
            Enemy = enemy;
            Hitbox = hitbox;
        }
    }
}
