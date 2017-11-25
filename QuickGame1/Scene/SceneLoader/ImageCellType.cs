using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    enum ImageCellType
    {
        Empty,
        BrownRock,
        BlueRock,
        Water,
        GrassBackground,
        Spring,
        Spike,
        Box,
        BreakableBlock,
        Ladder,
        MovingBlock,
        Path,
        PlayerStart,
        MapBoundary
    }

    static class ImageCellTypeExtensions
    {
        public static bool IsSolid(this ImageCellType ct)
        {
            return ct == ImageCellType.BrownRock || ct == ImageCellType.BlueRock;
        }
    }
}
