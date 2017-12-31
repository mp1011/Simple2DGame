using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class King : MovingActor, ICanClimb, IDamageable, ICanGetPrizes, IMovesBetweenScenes
    {
        public bool RecoilsWhenHit { get; set; } = true;

        private ManualCondition isUnderwater = new ManualCondition();
        public ManualCondition IsUnderWater => isUnderwater;
        public ManualCondition IsOnGround { get; set; } = new ManualCondition();
        public IMovingBlock RidingBlock { get; set; }
        public ManualCondition IsOnLadder { get; set; } = new ManualCondition();
        public ManualCondition GravityOn { get; } = new ManualCondition(true);
        public int Score { get; set; }
        public int Coins { get; set; }

     
        public DamageHandler DamageHandler { get; private set; } 

        public bool HasBetterAttack { get; set; }
        DamageType IDamageable.TakesDamageFrom => DamageType.Enemy | DamageType.EnemyAttack | DamageType.Trap;

        public King() : base(QuickGameScene.Current, Textures.KingTexture)
        {
            DamageHandler = new PlayerDamageHandler(6, this);

            Position.Center = Scene.PlayerStart;
            this.AddGravity();
            
            Position.SetWidth(8, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(24, GameEngine.AnchorOrigin.Top);
          
            var input = Input.GetInput(this.Scene);

            new PlayerControl(this, input);

            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.FlipWhenFacingLeft, 0);
            Animations.Add(AnimationKeys.Walk, this, TextureFlipBehavior.FlipWhenFacingLeft, 1,2,3,4);
            Animations.Add(AnimationKeys.Jump, this, TextureFlipBehavior.FlipWhenFacingLeft, 5,6);
            Animations.Add(AnimationKeys.Fall, this, TextureFlipBehavior.FlipWhenFacingLeft, 8);
            Animations.Add(AnimationKeys.Land, this, TextureFlipBehavior.FlipWhenFacingLeft, 7);
            Animations.Add(AnimationKeys.Attack, this, TextureFlipBehavior.FlipWhenFacingLeft, 10,11,12);

            Animations.Add(AnimationKeys.Climb, this, TextureFlipBehavior.FlipWhenFacingLeft, 18, 19, 20, 21);
            Animations.Add(AnimationKeys.ClimbStop, this, TextureFlipBehavior.FlipWhenFacingLeft, 18);
           
            new MovingPlatformPositionAdjuster<King>(this);
            WaterHelper.AddWaterPhysics(this);

            Scene.InterSceneActors.Add(this);

          //  DebugText.DebugWatch(this, Fonts.SmallFont, Scene.InterfaceLayer, t => t.Position.Center.X.ToString());
         //   DebugText.DebugWatch(this, Fonts.SmallFont, Scene.InterfaceLayer, t => t.Position.Center.Y.ToString());

        }

        SceneID IMovesBetweenScenes.NextScene { get; set; }

        void IMovesBetweenScenes.HandleTransition(Scene current, SceneID next)
        {
            Engine.Instance.Scene =  Engine.Instance.SceneLoader.LoadScene(next);
        }
    }

}
