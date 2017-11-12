using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{

    class QuickGameCollisionManager : CollisionManager
    {
        public QuickGameCollisionManager(QuickGameScene scene) : base(scene.SolidLayer)
        {
            AddBackgroundCollisions();
            AddObjectCollisions();          
        }

        private void AddBackgroundCollisions()
        {
            OnCollisionBetween<IBounces, TileMap>()
                .Stop()
                .Then((bouncingThing, map, collisionInfo) =>
                {
                    var landingSpeed = collisionInfo.OriginalVelocity.Y;
                    if (landingSpeed > 20)
                    {
                        var bounceSpeed = -0.7f * landingSpeed;

                        var bouncingMotion = bouncingThing.GetBounceMotion();
                        bouncingMotion.SetStartSpeed(bounceSpeed);
                    }
                });


            OnCollisionBetween<IPlatformerObject, IMovingBlock>()
              .ThenDoOnce((platformer, block, collisionInfo) =>
              {
                  platformer.RidingBlock = block;
              })
              .Else((platformer, b)=>
                {
                    platformer.RidingBlock = null;
                });
            

            OnCollisionBetween<IPlatformerObject, IBlock>()
                .Stop()
                .Then((platformer, map, collisionInfo) =>
                {
                    if (collisionInfo.OriginalVelocity.Y > 1 && platformer.Position.Bottom <= collisionInfo.OriginalPosition.Bottom)
                        platformer.IsOnGround.Active = true;
                })
                .Else((platformer, tm) =>
                {
                    platformer.IsOnGround.Active = false;
                });
            
            OnCollisionBetween<IPlatformerObject, WaterCollisionDetector>()
                .Then((actor, water) =>
                {                           
                    if (!actor.IsUnderWater.Active)
                    {
                        actor.IsUnderWater.Active = true;
                        AudioEngine.Instance.PlaySound(Sounds.Splash);
                        new Splash().PositionRelativeTo(actor, 0, 16);
                    }
                })
                .Else((actor, w) =>
                {
                    if (actor.IsUnderWater.Active)
                    {
                        actor.IsUnderWater.Active = false;
                        AudioEngine.Instance.PlaySound(Sounds.Splash);
                        new Splash().PositionRelativeTo(actor, 0, 16);
                    }
                });

            OnCollisionBetween<ICanClimb, LadderCollisionDetector>()
               .Then((actor, ladder) =>
               {
                   if(!actor.IsOnLadder.IsActiveAndNotNull())
                       LadderHandler.CheckClimbStart(actor, Input.GetInput(actor.Layer.Scene));
                   else
                       LadderHandler.CheckClimbStop(actor, Input.GetInput(actor.Layer.Scene));
               })
               .Else((actor, ladder) =>
               {
                   LadderHandler.EndClimb(actor);
               });

        }

        private void AddObjectCollisions()
        {

            OnCollisionBetween<King, IShop>()                
                .Then((king, shop) =>
                {
                    shop.ShopMenu.CheckEnterShop();
                });

            OnCollisionBetween<ICanGetPrizes, IPrize>()
              .PlaySound(Sounds.GetCoin)
              .Then((player, prize) =>
              {
                  new Poof().PositionRelativeTo(prize, 0, 0);
                  prize.Remove();
                  prize.OnCollected(player);
              });

            OnCollisionBetween<IDamageable, IDamager>()               
                .Then((victim, damager) =>
                {
                    if (victim.TakesDamageFrom(damager))
                    {
                        AudioEngine.Instance.PlaySound(Sounds.PlayerHit);
                        victim.DamageHandler.TakeDamageFrom(damager);

                        if(victim.RecoilsWhenHit)
                            victim.RecoilFrom(damager);
                    }
                });

            OnCollisionBetween<IPlatformerObject, IBlock>()
                .Stop();

            OnCollisionBetween<Swoosh, Box>()
                .Then((s, box) =>
                {
                    box.Direction = box.DirectionAwayFrom(s, Axis.X);
                    box.PushInDirection(box.Direction, new ConfigValue<AxisMotionConfig>("box push"));
                    box.Jump(new ConfigValue<AxisMotionConfig>("box jump"));
                });
             
        }
    }

    
}
