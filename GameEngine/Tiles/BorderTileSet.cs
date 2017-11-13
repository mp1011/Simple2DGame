using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{


    public class BorderTileSet
    {
        private Dictionary<BorderSide, int> Cells = new Dictionary<BorderSide, int>();
      
        public TextureInfo Texture { get; private set; }
        public bool TreatObscuredAsEmpty = false;

        public BorderTileSet(TextureInfo texture)
        {
            Texture = texture;
        }

        public int[] TileIndices
        {
            get
            {
                var blank = Cells.TryGet(BorderSide.EmptySpace, -1);
                return Cells.Values.Where(p => p != blank).ToArray();
            }
        }

        public int GetCell(BorderSide neighbors)
        {
            return Cells.TryGet(neighbors, Cells.TryGet(neighbors.WithoutCorners(), Cells[BorderSide.None]));
        }
       
        public void Set(BorderSide side, int x, int y)
        {
            Cells.Add(side, (y* Texture.Columns)+x);
        }     
        
        /// <summary>
        /// Sets a 3x3 square
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetSquare(int x, int y)
        {
            Set(BorderSide.None, 1, 1);
          
            Set(BorderSide.Bottom | BorderSide.Right, 0, 0);
            Set(BorderSide.Bottom | BorderSide.Right | BorderSide.Left, 1, 0);
            Set(BorderSide.Bottom | BorderSide.Left, 2, 0);

            Set(BorderSide.Bottom | BorderSide.Top | BorderSide.Right, 0, 1);
            Set(BorderSide.Bottom | BorderSide.Top | BorderSide.Right | BorderSide.Left, 1, 1);
            Set(BorderSide.Bottom | BorderSide.Top | BorderSide.Left, 2, 1);

            Set(BorderSide.Top | BorderSide.Right, 0, 2);
            Set(BorderSide.Top | BorderSide.Right | BorderSide.Left, 1, 2);
            Set(BorderSide.Top | BorderSide.Left, 2, 2);

        }
    }
}
