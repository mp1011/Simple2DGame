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
        private AxisMotion walk;
        private AxisMotion climb;
        private AxisMotion brake, climbBrake;

        private ConfigValue<AxisMotionConfig> JumpConfig = new ConfigValue<AxisMotionConfig>("jump");

        private ICondition hasLandedOnGround;
        private ICondition isAttacking;

        public PlayerControl(King player, IGameInputWithDPad input)
        {
            Input = input;
            Player = player;
            Player.Scene.AddObject(this);
            walk = new AxisMotion("walk", Player).Set(flipWhen: Direction.Left);
            climb = new AxisMotion("climb", Player).Set(flipWhen: Direction.Up);
            climbBrake = new AxisMotion("climb brake", Player);
            brake = new AxisMotion("brake", Player);
            walk.Active = false;
            brake.Active = false;
            climb.Active = false;
            climbBrake.Active = false;

            hasLandedOnGround = new CollisionCondition<IPlatformerObject, TileMap>(player)
                .When(player.IsOnGround)
                .SetActiveTime(new ConfigValue<TimeSpan>("landing time"), player);
       
            isAttacking = new InputCondition(GameKeys.Attack, Input)
                .ContinueWhileAnimationPlaying(player.Animations, AnimationKeys.Attack);

            new AnimationController<King>(Player, isAttacking, hasLandedOnGround);
        }


        public void Update(TimeSpan elapsedInFrame)
        {
            if (Player.Motion.MotionPerSecond.Y == 0 && Input.GetButtonPressed(GameKeys.Jump))
                Player.Jump(JumpConfig);


            Player.Direction = Input.Pad.GetInputDirection(Player.Direction, Axis.X);

            var isWalking = Input.Pad.GetInputAmount(Axis.X) != 0;

            if (Player.Motion.MotionPerSecond.Y == 0 && isAttacking.IsActive)
                isWalking = false;

            if(Player.IsOnLadder.Active)
            {                 
                brake.Active = true;
              
                if (Player.Direction.Axis() == Axis.X)
                    Player.Direction = Direction.Up;

                Player.Direction = Input.Pad.GetInputDirection(Direction.Up, Axis.Y);

                climb.Active = Input.Pad.GetInputAmount(Axis.Y) != 0;
                climbBrake.Active = !climb.Active;                 
            }
            else if (isWalking)
            {
                climb.Active = false;
                climbBrake.Active = false;
                brake.Active = false;
                walk.Active = true;
            }
            else
            {                
                climb.Active = false;
                climbBrake.Active = false;
                brake.Active = true;
                walk.Active = false;
            }
            
            if(hasLandedOnGround.WasJustActivated())
            {
                AudioEngine.Instance.PlaySound(Sounds.HitGround);
            }

            if(isAttacking.WasJustActivated())
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
