using GameEngine;
using System;

namespace QuickGame1
{
    class NearbyTileChecker : IUpdateable
    {
        private ManualCondition OnGround, OnLedge, AtWall, OnLedgeToSpikePit;

        public ICondition IsOnGround => OnGround;
        public ICondition IsOnLedge => OnLedge;
        public ICondition IsAtWall => AtWall;
        public ICondition IsAtLedgeToSpikePit => OnLedgeToSpikePit;

        public IMovingWorldObject MovingObject;
        private QuickGameTileMap Map;

        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;
        IRemoveable IUpdateable.Root => MovingObject;

        public Tile GroundTile { get; private set; }
        public Tile NextWallTile { get; private set; }
        public Tile NextGroundTile { get; private set; }

        private Rectangle Hitbox;
        private float LookAhead;

        public NearbyTileChecker(IMovingWorldObject movingObject, QuickGameTileMap map)
        {
            OnGround = new ManualCondition();
            OnLedge = new ManualCondition();
            OnLedgeToSpikePit = new ManualCondition();
            AtWall = new ManualCondition();

            MovingObject = movingObject;
            Map = map.Require();
            movingObject.Layer.Scene.AddObject(this);

            Hitbox = movingObject.Position.Copy();
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            Hitbox.Center = MovingObject.Position.Center;

            GroundTile = Map.GetSolidHitTile(Hitbox);
            if (GroundTile == null)
            {
                OnGround.Active = false;
                OnLedge.Active = false;
                AtWall.Active = false;
                OnLedgeToSpikePit.Active = false;
                return;
            }

            OnGround.Active = true;

            NextGroundTile = GroundTile.GetAdjacent(MovingObject.Direction);
            
            NextWallTile = NextGroundTile.GetAdjacent(BorderSide.Top);
            int maxLookAhead = 2;
            while (--maxLookAhead > 0 && !NextWallTile.IsSolid)
                NextWallTile = NextWallTile.GetAdjacent(MovingObject.Direction);

            OnLedge.Active = !NextGroundTile.IsSolid && !NextWallTile.IsSolid;
            AtWall.Active = NextWallTile.IsSolid;

            if(OnLedge.IsActive)
            {
                var pitTile = NextGroundTile;
                while (!pitTile.IsSolid && !pitTile.IsBoundary)
                    pitTile = pitTile.GetAdjacent(BorderSide.Bottom);

                pitTile = pitTile.GetAdjacent(BorderSide.Top);
                OnLedgeToSpikePit.Active = (pitTile as QuickGameTile).IsSpike;
            }

        }
    }
}
