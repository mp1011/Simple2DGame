using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace QuickGame1
{
    [Flags]
    enum AttackFlags
    {
        NoAttack,
        AttackWhenClose=1,
        StopWhileAttacking=2,
        AttackPeriodically=4,
        ThrowFireball=8
    }

    [Flags]
    public enum EnemyBehaviorFlags
    {
        None=0,
        Moves=1,
        MovesMore=2,
        MovesFast=Moves + MovesMore,
        HasGravity=4,
        FollowsPlayer=8,
        GoesThroughWalls=16, //gravity off
        CanJump=16 //gravity off
    }

    class EnemyBehavior<TEnemy> : IUpdateable
        where TEnemy:MovingActor,IPlatformerObject,IDamageable
    {
        private bool initialized = false;
       
        public AttackFlags AttackFlags { get; set; }
        public ObstacleReactions Reactions { get; }  = new ObstacleReactions();

        public Condition IsAttacking { get; private set; }

        private EnemyBehaviorFlags Flags;
        private TEnemy Actor;
        private Vector2 OriginalLocation;
        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;
        IRemoveable IUpdateable.Root => Actor;

        private Condition isStartingAttack = Condition.False;
                
        private QuickGameNearbyTileChecker TileChecker;
       
        public EnemyBehavior(TEnemy actor, EnemyBehaviorFlags flags)
        {
            Actor = actor;
            actor.Scene.AddObject(this);

            Flags = flags;

            TileChecker = new QuickGameNearbyTileChecker(actor, actor.Scene.TileMap);           
        }

        public void Initialize()
        {
            initialized = true;
            OriginalLocation = Actor.Position.Center;
            AddBehaviorsBasedOnFlags();
        }

        void AddBehaviorsBasedOnFlags()
        {
            var player = Actor.Scene.Player;

            Actor.RecoilsWhenHit = Flags.HasFlag(EnemyBehaviorFlags.HasGravity);

            //if(AttackFlags.HasFlag(AttackFlags.AttackWhenClose))
            //{
            //    beginAttackCondition = beginAttackCondition.Or(Actor.IsCloseTo(player, 64)); //todo - config
            //}

            if (AttackFlags.HasFlag(AttackFlags.AttackPeriodically))
            {
                isStartingAttack = new Timer(Actor).OnceEvery(QuickGameConfig.NormalAttackFrequency).And(Actor.IsOnGround);
            }

            IsAttacking = isStartingAttack.ContinueWhile(Actor.Animations.IsAnimationPlaying(AnimationKeys.Attack));

            ////todo - config
            //if (AttackFlags.HasFlag(AttackFlags.ThrowFireball))
            //    throwFireballCondition = beginAttackCondition.And(                 
            //        WaitUntil(new PlayingAnimationCondition(Actor.Animations, AnimationKeys.Attack).Negate());


            //if (Flags.HasFlag(EnemyBehaviorFlags.Shoots))
            //{
            //    beginAttackCondition = new OnceEvery(new Timer(TimeSpan.FromSeconds(5), Actor), Condition.True);
            //    attackDurationCondition = beginAttackCondition.ContinueWhileAnimationPlaying(Actor.Animations, AnimationKeys.Attack);
            //}

            //    new AnimationController<TEnemy>(Actor, attackDurationCondition, null);


            //if (Flags.HasFlag(EnemyBehaviorFlags.FollowsPlayer))
            //{
            //    Actor.Face(player);
            //    Actor.AlwaysFace(player, new OnceEvery(new Timer(TimeSpan.FromSeconds(2), Actor)));
            //}
            //else
            //{
            //    Actor.Direction = Direction.Left;
            //}

            if (Flags.HasFlag(EnemyBehaviorFlags.MovesFast))
            {
                Actor.Motion.AddAdjuster(new GroundMotion<TEnemy>(Actor, Config.ReadValue<AxisMotionConfig>("fast motion")));
            }
            else if (Flags.HasFlag(EnemyBehaviorFlags.MovesMore))
            {
                Actor.Motion.AddAdjuster(new GroundMotion<TEnemy>(Actor, Config.ReadValue<AxisMotionConfig>("medium motion")));
            }
            else if (Flags.HasFlag(EnemyBehaviorFlags.Moves))
            {
                Actor.Motion.AddAdjuster(new GroundMotion<TEnemy>(Actor, Config.ReadValue<AxisMotionConfig>("slow motion")));
            }

            if (Flags.HasFlag(EnemyBehaviorFlags.HasGravity))
            {
                Actor.AddGravity();
            }

            //    //todo - make work better with FacePlayer

            //    //if(!Flags.HasFlag(EnemyBehaviorFlags.FallsOfLedges))
            //    //{
            //    //    new LambdaAction<Actor>(Actor, UpdatePriority.Behavior, Actor.Scene, TileChecker.IsAtWall.Or(TileChecker.IsOnLedge),
            //    //        (a, t) =>
            //    //        {
            //    //            //if (a.Position.GetEdgeDistanceTo(TileChecker.NextWallTile.Position, Axis.X) <= 1)
            //    //                a.Direction = a.DirectionAwayFrom(TileChecker.NextWallTile, Axis.X);                            
            //    //        });                    
            //    //}

            //    if(Flags.HasFlag(EnemyBehaviorFlags.CanJump))
            //    {
            //        throw new NotImplementedException();
            //        //var jumpMotion = new AxisMotion("enemy jump", Actor).Set(deactivateAfterStart: true);
            //        //JumpForwardMotion = new AxisMotion("enemy jump X", Actor).Set(flipWhen: Direction.Left, deactivateWhenTargetReached:false);

            //        //Actor.Motion.Forces.Add(jumpMotion);

            //        //new LambdaAction<Actor>(Actor, UpdatePriority.Behavior, Actor.Scene, TileChecker.IsAtWall.Or(TileChecker.IsOnLedge),
            //        //   (a, t) =>
            //        //   {
            //        //       if (TileChecker.IsAtWall.IsActive ||
            //        //            Flags.HasFlag(EnemyBehaviorFlags.FallsOfLedges) && TileChecker.IsOnDangerousLedge.IsActive)
            //        //       {
            //        //           jumpMotion.Active = true;
            //        //           JumpForwardMotion.Active = true;
            //        //       }
            //        //   });
            //    }
            //}

        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if(!initialized)
            {
                Initialize();
            }
            
            //if(Actor.IsOnGround.Active)
            //{
            //    if(JumpForward != null)
            //        JumpForwardMotion.Active = false;
            //}

            if(IsAttacking.JustStarted())
            {
                Actor.Face(Actor.Scene.Player);
            }

            if(IsAttacking && AttackFlags.HasFlag(AttackFlags.StopWhileAttacking))
            {
                Actor.Motion.Stop(Axis.X);
            }

            if(AttackFlags.HasFlag(AttackFlags.ThrowFireball))
            {
                if(IsAttacking.JustEnded())
                {
                    var bullet = new EnemyBullet();
                    bullet.Direction = Actor.Direction;
                    bullet.PositionRelativeTo(Actor, 0, 0);
                    bullet.MoveInDirection(bullet.Direction, new ConfigValue<int>("enemy bullet speed").Value);
                }
            }

            if(TileChecker.IsOnLedge)
                ReactToObstacle(TileChecker.CurrentLedge, Reactions.WalkingOffLedge);

            if (TileChecker.IsAtWall)
            {
                if (TileChecker.CurrentWall.ActorCouldJumpOver(Actor))
                    ReactToObstacle(TileChecker.CurrentWall, Reactions.WalkingIntoShortWall);
                else
                    ReactToObstacle(TileChecker.CurrentWall, Reactions.WalkingIntoWall);
            }


        }

   
        private bool ReactToObstacle(Obstacle obstacle, ObstacleReaction reaction)
        {
            if (obstacle == null)
                return false;

            switch(reaction)
            {
                case ObstacleReaction.TurnAround:
                    Actor.Direction = obstacle.GetDirectionAwayFrom(Actor);
                    break;

                case ObstacleReaction.ShortJump:
                    Actor.Jump(200); //todo, config
                    Actor.PushInDirection(Actor.Direction, 100, additive:true); //todo, config
                    break;

                case ObstacleReaction.LongJump:
                    Actor.Jump(200); //todo, config
                    Actor.PushInDirection(Actor.Direction, 100, additive: true); //todo, config
                    break;

                case ObstacleReaction.None:
                    return false;
            }

            return true;
        }



    }
}
