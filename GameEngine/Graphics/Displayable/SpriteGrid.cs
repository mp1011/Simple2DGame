using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public sealed class SpriteGrid
    {
        public TextureInfo Texture { get; set; }
        public ArrayGrid<int> Cells { get; set; }
    }

}
