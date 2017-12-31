using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class MrSkeltal : MovingActor, IPlatformerObject, IDamageable, IDamager, IEditorPlaceable
    {
        public bool RecoilsWhenHit { get; set; }
        public ManualCondition IsUnderWater { get; set; } = new ManualCondition();
        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public IMovingBlock RidingBlock { get; set; }
        public DamageHandler DamageHandler { get; private set; }
        public ManualCondition GravityOn { get; } = new ManualCondition(true);
        DamageType IDamageable.TakesDamageFrom => DamageType.PlayerAttack | DamageType.Trap;

        DamageType IDamager.DamageType => DamageType.Enemy;

        CellType IEditorPlaceable.EditorType => CellType.MrSkeltal;

        public MrSkeltal() : base(QuickGameScene.Current, Textures.SkeletonIdleTexture)
        {
            Position.SetWidth(8, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(24, GameEngine.AnchorOrigin.Top);

            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Texture = Textures.SkeletonIdleTexture;
            Animations.Add(AnimationKeys.Walk, this, TextureFlipBehavior.FlipWhenFacingLeft, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Texture = Textures.SkeletonWalkTexture;
            Animations.AddRange(AnimationKeys.Attack, this, TextureFlipBehavior.FlipWhenFacingLeft, from: 0, to: 18, holdFrame:4, holdFor:8).Texture = Textures.SkeletonAttackTexture;

       
            var behavior = new EnemyBehavior<MrSkeltal>(this, EnemyBehaviorFlags.HasGravity | EnemyBehaviorFlags.FollowsPlayer);

            //behavior.SetReaction(ObstacleType.ShortWall, ObstacleReaction.ShortJump);
            //behavior.SetReaction(ObstacleType.TallWall, ObstacleReaction.TurnAround);

            behavior.AttackFlags = AttackFlags.AttackWhenClose | AttackFlags.StopWhileAttacking;

            DamageHandler = new EnemyDamageHandler<MrSkeltal>(5, this);

            behavior.Initialize();

            new AnimationController<MrSkeltal>(this, behavior.IsAttacking, Condition.False);

            var hitbox = new EnemyHitbox<MrSkeltal>(this, 
                new AnimationControlledHitbox(this, Animations, AnimationKeys.Attack, 7, 7));
            Scene.SolidLayer.CollidableObjects.Add(hitbox);


         //   new DebugRectangle(hitbox, Scene.SolidLayer);
        }
    }
}
