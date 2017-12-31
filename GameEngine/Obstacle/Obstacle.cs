using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace GameEngine
{
    public abstract class Obstacle : IWithPosition 
    {
        protected abstract bool IsVertical { get; }
        public abstract Direction GetDirectionAwayFrom(IWithPositionAndDirection obj);

        public Rectangle Position { get; private set; }

        public Obstacle(Tile tile, Predicate<Tile> condition)
        {
            if (tile == null)
                Position = Rectangle.Empty();
            else
            {
                var start = tile.GetAdjacentWhile((IsVertical ? BorderSide.Top : BorderSide.Left), condition).Last();
                var end = tile.GetAdjacentWhile((IsVertical ? BorderSide.Bottom : BorderSide.Right), condition).Last();
                var area = start.GetEnclosingArea(end);
                Position = area;
            }
        }
    }
}
