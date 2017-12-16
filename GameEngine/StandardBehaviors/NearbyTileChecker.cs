using System;

namespace GameEngine
{
    public abstract class NearbyTileChecker : IUpdateable
    {
        private ManualCondition OnGround, OnLedge, AtWall, OnDangerousLedge;

        public ICondition IsOnGround => OnGround;
        public ICondition IsOnLedge => OnLedge;
        public ICondition IsAtWall => AtWall;
        public ICondition IsOnDangerousLedge => OnDangerousLedge;

        public IMovingWorldObject MovingObject;
        private TileMap Map;

        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;
        IRemoveable IUpdateable.Root => MovingObject;

        public Tile GroundTile { get; private set; }
        public Tile NextWallTile { get; private set; }
        public Tile NextGroundTile { get; private set; }

        private Rectangle Hitbox;
        private float LookAhead;

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

            GroundTile = Map.GetSolidHitTile(Hitbox);
            if (GroundTile == null)
            {
                OnGround.Active = false;
                OnLedge.Active = false;
                AtWall.Active = false;
                OnDangerousLedge.Active = false;
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

            if (OnLedge.IsActive)
            {
                var pitTile = NextGroundTile;
                while (!pitTile.IsSolid && !pitTile.IsBoundary)
                    pitTile = pitTile.GetAdjacent(BorderSide.Bottom);

                pitTile = pitTile.GetAdjacent(BorderSide.Top);
                OnDangerousLedge.Active = IsTileDangerous(pitTile);
            }

        }

        protected abstract bool IsTileDangerous(Tile tile);
    }
}
