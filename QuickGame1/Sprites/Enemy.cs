using GameEngine;
using System;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace QuickGame1
{
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
        CanJump=16, //gravity off
        StaysCloseToStart=32, //gravity off
        FallsOfLedges=32, //gravity on
        Shoots = 64
    }

    class EnemyBehavior<TEnemy> : IUpdateable
        where TEnemy:MovingActor,IPlatformerObject,IDamageable
    {
        private bool initialized = false;

        private EnemyBehaviorFlags Flags;
        private TEnemy Actor;
        private Vector2 OriginalLocation;
        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;
        IRemoveable IUpdateable.Root => Actor;

        private ICondition beginAttackCondition = Condition.False;
        private ICondition throwFireballCondition = Condition.False;
        private ICondition attackDurationCondition = Condition.False;

        private NearbyTileChecker TileChecker;
        private AxisMotion JumpForwardMotion;

        public EnemyBehavior(TEnemy actor, EnemyBehaviorFlags flags)
        {
            Actor = actor;
            actor.Scene.AddObject(this);

            Flags = flags;

            TileChecker = new NearbyTileChecker(actor, actor.Scene.TileMap);
        }

        void AddBehaviorsBasedOnFlags()
        {
            var player = Actor.Scene.Player;

            Actor.RecoilsWhenHit = Flags.HasFlag(EnemyBehaviorFlags.HasGravity);

            if (Flags.HasFlag(EnemyBehaviorFlags.Shoots))
            {
                beginAttackCondition = new OnceEvery(new Timer(TimeSpan.FromSeconds(5), Actor), Condition.True);
                attackDurationCondition = beginAttackCondition.ContinueWhileAnimationPlaying(Actor.Animations, AnimationKeys.Attack);
            }

            new AnimationController<TEnemy>(Actor, attackDurationCondition, null);

            throwFireballCondition = beginAttackCondition.DebugWatch().WaitUntil(new Timer(TimeSpan.FromMilliseconds(400), Actor));

            if (Flags.HasFlag(EnemyBehaviorFlags.FollowsPlayer))
            {
                Actor.Face(player);
                Actor.AlwaysFace(player, new OnceEvery(new Timer(TimeSpan.FromSeconds(2), Actor)));
            }
            else
            {
                Actor.Direction = Direction.Left;
            }
            
            if(Flags.HasFlag(EnemyBehaviorFlags.MovesFast))
            {
                Actor.Motion.Forces.Add(new GroundMotion<TEnemy>("fast motion", Actor));
            }
            else if(Flags.HasFlag(EnemyBehaviorFlags.MovesMore))
            {
                Actor.Motion.Forces.Add(new GroundMotion<TEnemy>("medium motion", Actor));
            }
            else if(Flags.HasFlag(EnemyBehaviorFlags.Moves))
            {
                Actor.Motion.Forces.Add(new GroundMotion<TEnemy>("slow motion", Actor));
            }

            if (Flags.HasFlag(EnemyBehaviorFlags.HasGravity))
            {
                Actor.AddGravity();

                if(!Flags.HasFlag(EnemyBehaviorFlags.FallsOfLedges))
                {
                    new LambdaAction<Actor>(Actor, UpdatePriority.Behavior, Actor.Scene, TileChecker.IsAtWall.Or(TileChecker.IsOnLedge),
                        (a, t) =>
                        {
                            if (a.Position.GetEdgeDistanceTo(TileChecker.NextWallTile.Position, Axis.X) <= 1)
                                a.Direction = a.DirectionAwayFrom(TileChecker.NextWallTile, Axis.X);                            
                        });                    
                }

                if(Flags.HasFlag(EnemyBehaviorFlags.CanJump))
                {
                    var jumpMotion = new AxisMotion("enemy jump", Actor).Set(deactivateAfterStart: true);
                    JumpForwardMotion = new AxisMotion("enemy jump X", Actor).Set(flipWhen: Direction.Left, deactivateWhenTargetReached:false);

                    Actor.Motion.Forces.Add(jumpMotion);

                    new LambdaAction<Actor>(Actor, UpdatePriority.Behavior, Actor.Scene, TileChecker.IsAtWall.Or(TileChecker.IsOnLedge),
                       (a, t) =>
                       {
                           if (TileChecker.IsAtWall.IsActive ||
                                Flags.HasFlag(EnemyBehaviorFlags.FallsOfLedges) && TileChecker.IsAtLedgeToSpikePit.IsActive)
                           {
                               jumpMotion.Active = true;
                               JumpForwardMotion.Active = true;
                           }
                       });
                }
            }
            else
            {


            }

            foreach (var f in Actor.Motion.Forces.OfType<AxisMotion>())
            {
                if(f.Axis == Axis.X)
                    f.Active = true;
            }
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if(!initialized)
            {
                initialized = true;
                OriginalLocation = Actor.Position.Center;
                AddBehaviorsBasedOnFlags();
            }

            if(Actor.IsOnGround.Active)
            {
                if(JumpForwardMotion != null)
                    JumpForwardMotion.Active = false;
            }

            if(attackDurationCondition.IsActive)
            {
                Actor.Motion.Stop(Axis.X);
            }

            if(throwFireballCondition.IsActiveAndNotNull())
            {
                var bullet = new EnemyBullet();
                bullet.Direction = Actor.Direction;
                bullet.PositionRelativeTo(Actor, 0, 4);
                bullet.MoveInDirection(bullet.Direction, new ConfigValue<int>("enemy bullet speed").Value);
            }
        }
    }
}
