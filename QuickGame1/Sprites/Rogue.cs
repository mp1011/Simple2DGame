using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Rogue : MovingActor, IPlatformerObject, IDamageable, IDamager, IEditorPlaceable
    {
        public bool RecoilsWhenHit { get; set; }
        public ManualCondition IsUnderWater { get; set; } = new ManualCondition();
        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public IMovingBlock RidingBlock { get; set; }
        public DamageHandler DamageHandler { get; private set; }
        public ManualCondition GravityOn { get; } = new ManualCondition(true);
        DamageType IDamageable.TakesDamageFrom => DamageType.PlayerAttack | DamageType.Trap;

        DamageType IDamager.DamageType => DamageType.Enemy;

        CellType IEditorPlaceable.EditorType => CellType.Rogue;

        public Rogue() : base(QuickGameScene.Current, Textures.RogueTexture)
        {
            Position.SetWidth(8, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(24, GameEngine.AnchorOrigin.Top);

            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0, 1, 2).Texture = Textures.RogueTexture;
            Animations.Add(AnimationKeys.Walk, this, TextureFlipBehavior.FlipWhenFacingLeft, 0,1,2,3,4,5).Texture = Textures.RogueRunTexture;
            Animations.AddRange(AnimationKeys.Attack, this, TextureFlipBehavior.FlipWhenFacingLeft, from: 0, to: 9).Texture = Textures.RogueAttackTexture;

       
            var behavior = new EnemyBehavior<Rogue>(this, EnemyBehaviorFlags.Moves | EnemyBehaviorFlags.HasGravity | EnemyBehaviorFlags.FollowsPlayer);

            behavior.Reactions.WalkingOffLedge = ObstacleReaction.TurnAround;
            behavior.Reactions.WalkingIntoWall = ObstacleReaction.TurnAround;

            behavior.AttackFlags = AttackFlags.ThrowFireball | AttackFlags.AttackPeriodically | AttackFlags.StopWhileAttacking;

            DamageHandler = new EnemyDamageHandler<Rogue>(5, this);

            behavior.Initialize();

            new AnimationController<Rogue>(this, behavior.IsAttacking, Condition.False);

            this.Direction = Direction.Left;
        }
    }
}
