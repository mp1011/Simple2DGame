using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameEngine
{
    public class PlatformerPathFollowerBehavior :IUpdateable
    {
        private IMovingWorldObject MovingObject;
        private PlatformerPath Path;
        private NearbyTileChecker TileChecker;
        
        private TileMap Map;
        private PathPoint[] PathPoints;

        private AxisMotion WalkMotion;

        public PlatformerPathFollowerBehavior(IMovingWorldObject obj, PathPoint[] pathPoints, NearbyTileChecker tileChecker, TileMap map)
        {
            MovingObject = obj;
            MovingObject.Layer.Scene.AddObject(this);
            TileChecker = tileChecker;
            Map = map;
            PathPoints = pathPoints;

            WalkMotion = new AxisMotion(target:Config.ReadValue<AxisMotionConfig>("npc walk"));
            obj.Motion.AddAdjuster(WalkMotion);
            
            var JumpTrigger = new XYMotion(Config.ReadValue<XYMotionConfig>("npc jump"));
            JumpTrigger.Condition = TileChecker.IsOnDangerousLedge.Or(TileChecker.IsAtWall).DebugWatch();
            obj.Motion.AddAdjuster(JumpTrigger);
        }

        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;

        IRemoveable IUpdateable.Root => MovingObject;

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            if(Path == null)
            {
                Path = new PlatformerPath(MovingObject, PathPoints, Map);
            }

            CheckIfTargetReached();

            MovingObject.Direction = TargetDirection;
        }

        private Vector2 StartPoint;

        private void CheckIfTargetReached()
        {
            bool reached = false;
            
            if (StartPoint == Vector2.Zero)
                reached = true;

            if (TargetDirection == Direction.Right && MovingObject.Position.Center.X > Path.CurrentPoint.Position.Center.X)
                reached = true;
            if (TargetDirection == Direction.Left && MovingObject.Position.Center.X < Path.CurrentPoint.Position.Center.X)
                reached = true;

            if (reached)
            {
                if (StartPoint != Vector2.Zero && Path.IsAtEnd)
                    return;

                Path.NextPoint();
                StartPoint = MovingObject.Position.Center;
                TargetDirection = MovingObject.CardinalDirectionTowards(Path.CurrentPoint, Axis.X);
            }
        }

        private Direction TargetDirection;


    }

    public class PlatformerPath
    {
        private List<PathPoint> PointsToFollow = new List<PathPoint>();
        private BoundedInteger CurrentPathIndex;
       
        public PathPoint CurrentPoint => PointsToFollow[CurrentPathIndex];

        public bool IsAtEnd => CurrentPathIndex.IsMax;

        public void NextPoint()
        {
            CurrentPathIndex++;
        }

        public PlatformerPath(IMovingWorldObject start, PathPoint[] path, TileMap map)
        {
            List<PathPoint> points = path.ToList();

            var pos = start.Position.Center;
            while (points.Any())
            {
                var nextPoint = CalcNextPointInPath(pos, points,map);
                if (nextPoint == null)
                    points.Clear();
                else
                {
                    points.Remove(nextPoint);
                    PointsToFollow.Add(nextPoint);
                    pos = nextPoint.Position.Center;
                }
            }

            CurrentPathIndex = new BoundedInteger(PointsToFollow.Count);
            CurrentPathIndex = CurrentPathIndex.SetToMin();
        }

        private PathPoint CalcNextPointInPath(Vector2 origin, List<PathPoint> points, TileMap map)
        {
            if (points.Count == 0)
                return null;
            else if (points.Count == 1)
                return points[0];

            var tilesInClearSight = points.Where(p => map.GetTilesInLine(origin, p.Position.Center).All(q => !q.IsSolid)).ToArray();
            if (tilesInClearSight.Length == 1)
                return tilesInClearSight[0];
            else if (tilesInClearSight.Length > 1)
                return tilesInClearSight.OrderBy(p => p.Position.Center.GetAbsoluteDistanceTo(origin)).First();

            return points.OrderBy(p => p.Position.Center.GetAbsoluteDistanceTo(origin)).First();

        }
    }

}
