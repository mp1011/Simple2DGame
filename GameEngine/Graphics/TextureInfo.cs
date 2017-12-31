using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public struct TextureID : IKey
    {
        public string Name { get; private set; }

        public TextureID(string name)
        {
            Name = name;
        }
    }

    public sealed class TextureInfo
    {
        public string ID { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 CellSize { get; set; }

        public int Columns
        {
            get
            {
                return ((int)(Size.X / CellSize.X)).MustBePositive();
            }
        }

        public int Rows => (int)(Size.Y / CellSize.Y);

        public int CellCount
        {
            get
            {
                return Columns * Rows;
            }
        }


        public Vector2 IndexToPoint(int index)
        {
            return index.ToXY(Columns);
        }

        public int PointToIndex(Vector2 point)
        {
            var ret = (int)(point.Y * Columns + point.X);
            return ret;
        }

        public int PointToIndex(int x, int y)
        {
            return y * Columns + x;
        }

      
        public AnchorOrigin AnchorOrigin { get; set; }
      

        public Vector2? CellAnchorOffset;
    }

}
