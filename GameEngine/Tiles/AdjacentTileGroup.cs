using System.Collections.Generic;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameEngine
{
    public class AdjacentTileGroup<T>
        where T:class, IWithPosition
    {
        public T[] Tiles { get; set; }

        public AdjacentTileGroup(IEnumerable<T> tiles)
        {
            Tiles = tiles.ToArray();
        }

        public Rectangle GetBoundingBox()
        {
            var left = Tiles.Min(p => p.Position.Left);
            var top = Tiles.Min(p => p.Position.Top);
            var right = Tiles.Max(p => p.Position.Right);
            var bottom = Tiles.Max(p => p.Position.Bottom);

            return new Rectangle(left, top, right - left, bottom - top);
        }

        public static AdjacentTileGroup<T>[] ExtractGroups(IEnumerable<T> tiles)
        {
            List<AdjacentTileGroup<T>> ret = new List<AdjacentTileGroup<T>>();
            List<T> tileList = tiles.ToList();

            while(tileList.Any())
            {
                var tile = tileList.First();
                tileList.Remove(tile);

                var group = ExtractAdjacentGroup(tile, tileList);
                if (group != null)
                    ret.Add(group);
                else
                    tileList.Clear(); //just in case
            }

            return ret.ToArray();
        }

        private static AdjacentTileGroup<T> ExtractAdjacentGroup(T rootTile, List<T> tileList)
        {
            List<T> tilesInGroup = new List<T>();
            tilesInGroup.Add(rootTile);

            bool addedAny = true;
            while(addedAny)
            {
                var adjacentToAny = tileList.RemoveWhere(t => tilesInGroup.Any(x => x.Position.CollidesWith(t.Position,false)));
                addedAny = adjacentToAny.Length > 0;
                tilesInGroup.AddRange(adjacentToAny);
            }

            return new AdjacentTileGroup<T>(tilesInGroup);                        
        }

        public IEnumerable<T> GetContinuousPath(Vector2 start) 
        {
            var closest = Tiles.OrderBy(p => p.Position.Center.GetAbsoluteDistanceTo(start)).Take(2).ToArray();
            var direction = closest[0].CardinalDirectionTowards(closest[1]);

            var current = closest[0];
            var next = closest[1];

            yield return current;
            yield return next;

           
            while (next != null)
            {
                current = next;
                next = Tiles.Where(p => p != current
                        && p.Position.CollidesWith(current.Position,false)
                        && current.CardinalDirectionTowards(p) == direction).FirstOrDefault();             

                if (next != null)                
                    yield return next;                
            }
        }
    }
}
