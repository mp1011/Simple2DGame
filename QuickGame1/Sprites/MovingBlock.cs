using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace QuickGame1
{

    class MovingBlockPiece : IWithPosition
    {
        public Rectangle Position { get; } 

        public MovingBlockPiece(Vector2 tile, Vector2 cellSize)
        {
            Position = tile.ToGridCell(cellSize);
        }
    }

    class MovingBlockFactory
    {
        /// <summary>
        /// Combines adjacent pieces into MovingBlock objects and adds them to the scene
        /// </summary>
        /// <param name="allPieces"></param>
        public static void CreateBlocks(QuickGameScene scene, IEnumerable<MovingBlockPiece> allPieces, IEnumerable<PathPoint> pathPoints)
        {
            var groups = AdjacentTileGroup<MovingBlockPiece>.ExtractGroups(allPieces);
            var paths = AdjacentTileGroup<PathPoint>.ExtractGroups(pathPoints);

            foreach (var group in groups)
            {
                var groupBox = group.GetBoundingBox();
                var path = paths.FirstOrDefault(p => p.Tiles.Any(q => groupBox.CollidesWith(q.Position,false)));
                if(path != null)
                    new MovingBlock(scene, group, path);
            }
        }
    }

    class MovingBlock : IMovingCollidable, IMovingWorldObject<QuickGameScene>, IDynamicDisplayable, IMovingBlock
    {
        private QuickGameScene Scene;
        private SpriteGrid SpriteGrid;

        public MovingBlock(QuickGameScene scene, AdjacentTileGroup<MovingBlockPiece> tiles, AdjacentTileGroup<PathPoint> path) 
        {
            Scene = scene;
            motionManager = new MotionManager(this);

            SpriteGrid = new SpriteGrid
            {
                Texture = Textures.MovingBlockTexture
            };

            Position = tiles.GetBoundingBox();
            
            float tilesX = (float)Position.Width / 16f; //todo
            float tilesY = (float)Position.Height / 16f;

            SpriteGrid.Cells = new ArrayGrid<int>(new Vector2(tilesX, tilesY));
            foreach(var pt in SpriteGrid.Cells.Points)
            {
                //todo, nonstandard shapes
                SpriteGrid.Cells.Set(pt, 0);
            }
                           
            new PathMoverBehavior<MovingBlock>(this, path);

            Scene.SolidLayer.AddObject(this);
        }

        #region Interface Impl
        private bool isRemoved = false;
        bool IRemoveable.IsRemoved => isRemoved;

        QuickGameScene IWorldObject<QuickGameScene>.Scene => Scene;

        Layer IWorldObject.Layer => Scene.SolidLayer;

        private MotionManager motionManager;
        MotionManager IMoveable.Motion => motionManager;

        Direction IWithPositionAndDirection.Direction { get; set; } = Direction.None;
        
        public Rectangle Position { get; } 

        IRemoveable IDynamicDisplayable.Root => this;

        TextureDrawInfo IDisplayable.DrawInfo => TextureDrawInfo.Fixed();        

        bool ICollidable.DetectCollision(Rectangle collidingObject, bool ignoreEdges)
        {
            return Position.CollidesWith(collidingObject, ignoreEdges);
        }

        bool IMovingCollidable.DetectFrameStartCollision(Rectangle collidingObject)
        {
            return motionManager.FrameStartPosition.CollidesWith(collidingObject, false);
        }

        void IRemoveable.Remove()
        {
            isRemoved = true;
        }

        void IDisplayable.Draw(IRenderer painter)
        {
            painter.DrawSpriteGrid(this, SpriteGrid);
        }
        #endregion

    }
}
