using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameEngine
{
    public class PathPoint : IWithPosition
    {   
        public Vector2 Point { get { return pos.Center; } set { pos.Center = value; } }
        private Rectangle pos = new Rectangle();
        public Rectangle Position => pos;

        public PathPoint(int x, int y)
        {
            pos = new Rectangle(x, y, 16, 16); 
        }

        public PathPoint(Vector2 v)
        {
            pos = new Rectangle(0, 0, 16, 16);
            Point = v;
        }        
    }

    public class PathMoverBehavior<TMoveable> : IUpdateable
        where TMoveable : IMoveable, IWorldObject
    {
        private TMoveable MovingObject;
        private CyclingList<PathPoint> Path;

        public PathMoverBehavior(TMoveable movingObject, AdjacentTileGroup<PathPoint> path)
        {
            MovingObject = movingObject;          
            movingObject.Layer.Scene.AddObject(this);

            CalculatePath(path);
        }

        public UpdatePriority Priority => UpdatePriority.Motion;

        public IRemoveable Root => MovingObject;

        private void CalculatePath(AdjacentTileGroup<PathPoint> path)
        {
            Path = new CyclingList<PathPoint>();

            Vector2 start = MovingObject.Position.Center;
            while (path.Tiles.Count() > 1)
            {
                var continousPath = path.GetContinuousPath(start).ToArray();                
                Path.AddItem(continousPath.First());
                
                path.Tiles = path.Tiles.Where(p => p == continousPath.Last() || !continousPath.Contains(p)).ToArray();
                start = continousPath.Last().Position.Center;
            }

            if (path.Tiles.Count() == 1)
                Path.AddItem(path.Tiles[0]);

            var reverse = Path.Items.Skip(1).ToList();
            reverse.Reverse();
            Path.AddRange(reverse.Skip(1));            
        }

        private Direction DirectionToTarget = Direction.None;

        public void Update(TimeSpan elapsedInFrame)
        {
            var dirToNextTarget = MovingObject.CardinalDirectionTowards(Path.Current);

            if(dirToNextTarget == Direction.None || dirToNextTarget != DirectionToTarget)
            {
                MovingObject.Position.Center = Path.Current.Point;
                Path.Next();
                DirectionToTarget = MovingObject.CardinalDirectionTowards(Path.Current);
            }

            MovingObject.MoveInDirection(DirectionToTarget, 40); // todo - speed
            
        }
    }
}
