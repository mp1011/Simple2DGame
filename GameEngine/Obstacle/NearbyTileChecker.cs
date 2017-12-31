using System;
using System.Linq;

namespace GameEngine
{
    public abstract class NearbyTileChecker : IUpdateable
    {
        private ManualCondition OnGround, OnLedge, AtWall, OnDangerousLedge;

        public Condition IsOnGround => OnGround;
        public Condition IsOnLedge => OnLedge;
        public Condition IsAtWall => AtWall;
        public Condition IsOnDangerousLedge => OnDangerousLedge;

        public IMovingWorldObject MovingObject;
        private TileMap Map;

        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;
        IRemoveable IUpdateable.Root => MovingObject;

        public Tile GroundTileHit { get; private set; }
        public Ledge CurrentLedge { get; private set; }
        public Wall CurrentWall { get; private set; }

        private Rectangle Hitbox;

        public NearbyTileChecker(IMovingWorldObject movingObject, TileMap map)
        {
            OnGround = new ManualCondition();
            OnLedge = new ManualCondition();
            OnDangerousLedge = new ManualCondition();
            AtWall = new ManualCondition();

            MovingObject = movingObject;
            Map = map.Require();
            movingObject.Layer.Scene.AddObject(this);

            Hitbox = movingObject.Position.Copy();
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            Hitbox.Center = MovingObject.Position.Center;

            GroundTileHit = Map.GetSolidHitTile(MovingObject.Position);
           
            if (GroundTileHit == null)
            {
                OnGround.Active = false;
                OnLedge.Active = false;
                AtWall.Active = false;
                OnDangerousLedge.Active = false;
                
                return;
            }

            CurrentLedge = new Ledge(GroundTileHit);
            CurrentWall = new Wall(GroundTileHit, MovingObject.Direction);
            if (CurrentWall.Position.Width == 0)
                CurrentWall = null;
            

            AtWall.Active = (CurrentWall != null) && CurrentWall.ActorIsAboutToWalkInto(MovingObject);

            OnLedge.Active = CurrentLedge.ActorIsAboutToWalkOff(MovingObject);
            
            OnGround.Active = true;            
            

        }

        protected abstract bool IsTileDangerous(Tile tile);

        private Obstacle GetWall(Tile tileHit)
        {
            throw new NotImplementedException();
            //var tile = tileHit.GetAdjacent(BorderSide.Top);

            //var wallTile = tile.GetAdjacentWhile(MovingObject.Direction.ToSide(), t => !t.IsSolid).Take(MaxWallLookAhead).Last();
            //wallTile = wallTile.GetAdjacent(MovingObject.Direction.ToSide());
            //if (!wallTile.IsSolid)
            //    return null;

            //var wall = new Obstacle(ObstacleFlags.Solid | ObstacleFlags.Vertical, TallWallSize, tileHit, t => t.IsSolid && t.Position.Top < tileHit.Position.Top);
            //return wall;
        }

        private Obstacle GetPit(Tile tileHit)
        {
            throw new NotImplementedException();
            //if (Ground == null)
            //    return null;

            //var ledge = tileHit.GetFirstAdjacentNotMatching(MovingObject.Direction.ToSide(), t => t.IsSolid);
            //if(ledge == null)
            //    return null;

            //var pitTile = ledge.GetFirstAdjacentNotMatching(BorderSide.Bottom, t => !t.IsSolid);
            //var obstacleType = ObstacleFlags.Solid;
            //if (IsTileDangerous(pitTile))
            //    obstacleType = obstacleType | ObstacleFlags.Dangerous;                

            //var pit = new Obstacle(obstacleType, LongPitSize, pitTile, t => t.IsSolid && !t.GetAdjacent(BorderSide.Top).IsSolid);
            //return pit;
        }
        
    }
}
