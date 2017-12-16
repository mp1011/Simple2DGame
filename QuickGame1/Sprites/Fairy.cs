using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class Fairy : MovingActor
    {
        public Fairy(MovingActor player) : base(QuickGameScene.Current, Textures.FairyTexture)
        {
            Position.SetWidth(16, GameEngine.AnchorOrigin.Left);
            Position.SetHeight(16, GameEngine.AnchorOrigin.Top);            
            Animations.Add(AnimationKeys.Stand, this, TextureFlipBehavior.None, 1,2);

            new FairyBehavior(this, player);

            this.PositionRelativeTo(player, 0, 50);
        }
    }

    class FairyBehavior : IUpdateable
    {
        private MovingActor Fairy;
        private MovingActor Player;
        private ICondition followPlayer;
        private ConfigValue<float> FairySpeed = new ConfigValue<float>("fairy speed");

        private DirectedMotion FairyMotion;

        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;

        IRemoveable IUpdateable.Root => Fairy;

        public FairyBehavior(MovingActor fairy, MovingActor player)
        {
            Fairy = fairy;
            Player = player;
            fairy.Scene.AddObject(this);

            followPlayer = new OnceEvery(new Timer(TimeSpan.FromSeconds(1), Fairy));

            FairyMotion = new DirectedMotion();
            Fairy.Motion.AddAdjuster(FairyMotion);
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            var targetAngle = Fairy.Position.Center.GetDegreesTo(Player.Position.Center);
            
            FairyMotion.AngleInDegrees = FairyMotion.AngleInDegrees.RotateAngleInDegreesTowards(targetAngle, 1);
          
            var distance = Fairy.Position.Center.GetAbsoluteDistanceTo(Player.Position.Center);
            if (distance > 50)
            {
                FairyMotion.DistancePerSecond = FairySpeed.Value * 3;
            }
            else
            {
                FairyMotion.DistancePerSecond = FairySpeed.Value;
            }
        }
    }

}
