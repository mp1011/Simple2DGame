using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{    
    class PlayerControl : GameEngine.IUpdateable
    {

        public UpdatePriority Priority { get { return UpdatePriority.Behavior; } }
        private IGameInputWithDPad Input;
        private King Player;
        IRemoveable GameEngine.IUpdateable.Root => Player;

        private MotionWithBrake WalkMotion;
        private MotionWithBrake ClimbMotion;
        private AxisMotion JumpMotion;
        
        private ICondition hasLandedOnGround;
        private ICondition isAttacking;
        private float highestPointAboveGround = float.MaxValue;

        public PlayerControl(King player, IGameInputWithDPad input)
        {
            Input = input;
            Player = player;
            Player.Scene.AddObject(this);

            var walk = new AxisMotion(target: Config.ReadValue<AxisMotionConfig>("walk"));
            var brake = new AxisMotion(target: Config.ReadValue<AxisMotionConfig>("brake"));

            var climb = new AxisMotion(target: Config.ReadValue<AxisMotionConfig>("climb"));
            var climbBrake = new AxisMotion(target: Config.ReadValue<AxisMotionConfig>("climb brake"));

            WalkMotion = new MotionWithBrake(walk, brake);
            player.Motion.AddAdjuster(WalkMotion);
            ClimbMotion = new MotionWithBrake(climb, climbBrake);
            player.Motion.AddAdjuster(ClimbMotion);
                       
            JumpMotion = new AxisMotion(Config.ReadValue<AxisMotionConfig>("jump"));

            player.Motion.AddAdjuster(JumpMotion);

            hasLandedOnGround = new CollisionCondition<IPlatformerObject, TileMap>(player)
                .When(player.IsOnGround)
                .SetActiveTime(new ConfigValue<TimeSpan>("landing time"), player);
       
            isAttacking = new InputCondition(GameKeys.Attack, Input)
                .ContinueWhileAnimationPlaying(player.Animations, AnimationKeys.Attack);

            new AnimationController<King>(Player, isAttacking, hasLandedOnGround);

            Player.Direction = Direction.Right;
        }


        public void Update(TimeSpan elapsedInFrame)
        {
            JumpMotion.Active = Player.Motion.CurrentMotionPerSecond.Y == 0 && Input.GetButtonPressed(GameKeys.Jump);
                 
            Player.Direction = Input.Pad.GetInputDirection(Player.Direction, Axis.X);

            bool isWalking = Input.Pad.GetInputAmount(Axis.X) != 0;
            if (Player.Motion.CurrentMotionPerSecond.Y == 0 && isAttacking.IsActive)
                isWalking = false;

            WalkMotion.Motion.Active = isWalking;

            if (Player.IsOnLadder.Active)
            {
                if (Player.Direction.Axis() == Axis.X)
                    Player.Direction = Direction.Up;

                Player.Direction = Input.Pad.GetInputDirection(Direction.Up, Axis.Y);

                ClimbMotion.Active = true;
                ClimbMotion.Motion.Active = Input.Pad.GetInputAmount(Axis.Y) != 0;
            }
            else
                ClimbMotion.Active = false;

            if (!Player.IsOnGround.Active)
            {
                highestPointAboveGround = (float)Math.Min(highestPointAboveGround, Player.Position.Bottom);
            }

            if (hasLandedOnGround.WasJustActivated())
            {
                float maxSoundDistance = 64;
                var distanceFallen = ((float)Player.Position.Bottom - highestPointAboveGround).KeepInsideRange(0, maxSoundDistance);

                var distanceFallenPct = distanceFallen / maxSoundDistance;
                if (distanceFallen > 0.1)
                    AudioEngine.Instance.PlaySound(Sounds.HitGround, distanceFallenPct);

                highestPointAboveGround = Int32.MaxValue;
            }

            if (isAttacking.WasJustActivated())
            {
                AudioEngine.Instance.PlaySound(Sounds.Swish);
                var attack = new Swoosh();
                if (Player.HasBetterAttack)
                {
                    attack.PositionRelativeTo(Player, 8, -4);
                    attack.MoveInDirection(Player.Direction, Config.Provider.GetValue<int>("better attack speed"));
                }
                else
                    attack.StayRelativeTo(Player, 8, -4);


            }
        }

       
    }
}
