using GameEngine;
using Microsoft.Xna.Framework;
using System;

namespace QuickGame1
{
    class Snake : MovingActor, IPlatformerObject, IDamageable, IDamager, IEditorPlaceable
    {
        CellType IEditorPlaceable.EditorType => CellType.Snake;

        public bool RecoilsWhenHit { get; set; } = true;

        private ManualCondition isUnderwater = new ManualCondition();
        public ManualCondition IsUnderWater => isUnderwater;

        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public DamageHandler DamageHandler { get; private set; }
        public IMovingBlock RidingBlock { get; set; }
        DamageType IDamageable.TakesDamageFrom => DamageType.PlayerAttack | DamageType.Trap;

        DamageType IDamager.DamageType => DamageType.Enemy;

        public Snake() : base(QuickGameScene.Current, Textures.SnakeTexture)
        {
            DamageHandler = new EnemyDamageHandler<Snake>(2, this);

            Position.SetWidth(8, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(16, GameEngine.AnchorOrigin.Top);

            Animations.Add(AnimationKeys.Walk, this, TextureFlipBehavior.FlipWhenFacingLeft, 0, 1, 2, 3);
            this.AddGravity();

            var motion = new GroundMotion<Snake>("snake", this).Set(flipWhen: Direction.Left);
            motion.Active = true;
          //  new StayOnLedge(this, Scene.TileMap);

            WaterHelper.AddWaterPhysics(this);

        }

    }
}
