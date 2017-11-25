using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class GameTiles
    {

        public static BorderTileSet Water()
        {
            var ts = new BorderTileSet(Textures.RockTiles);
            ts.Set(BorderSide.EmptySpace, 0, 7);

            ts.Set(BorderSide.None, 1, 6);
            ts.Set(BorderSide.Bottom | BorderSide.Left | BorderSide.Right, 1, 5);
            ts.Set(BorderSide.Bottom | BorderSide.Left, 1, 5);
            ts.Set(BorderSide.Bottom | BorderSide.Right, 1, 5);

            return ts;
        }

        public static BorderTileSet Ladder()
        {
            var ts = new BorderTileSet(Textures.RockTiles);
            ts.Set(BorderSide.EmptySpace, 0, 7);

            ts.Set(BorderSide.None, 0, 4);
            ts.Set(BorderSide.Bottom, 0, 5);
            ts.Set(BorderSide.Top | BorderSide.Bottom, 0, 5);
            ts.Set(BorderSide.Top, 0, 6);

            return ts;
        }

        public static BorderTileSet BreakableBlock()
        {
            var ts = new BorderTileSet(Textures.RockTiles);
            ts.Set(BorderSide.EmptySpace, 0, 7);
            ts.Set(BorderSide.None, 1, 7);

            return ts;
        }


        public static BorderTileSet Grass()
        {
            var ts = new BorderTileSet(Textures.GrassTiles);

            ts.Set(BorderSide.EmptySpace, 0, 3);

            ts.Set(BorderSide.None, 3, 1);

            ts.Set(BorderSide.Right | BorderSide.Bottom, 2, 0);
            ts.Set(BorderSide.Left | BorderSide.Right | BorderSide.Bottom, 3, 0);
            ts.Set(BorderSide.Left | BorderSide.Bottom, 4, 0);

            ts.Set(BorderSide.Right | BorderSide.Top | BorderSide.Bottom, 2, 1);
            ts.Set(BorderSide.AllSides, 3, 1);

            ts.Set(BorderSide.Left | BorderSide.Top | BorderSide.Bottom, 4, 1);

            ts.Set(BorderSide.Top | BorderSide.Right, 2, 2);
            ts.Set(BorderSide.Left | BorderSide.Top | BorderSide.Right, 3, 2);
            ts.Set(BorderSide.Left | BorderSide.Top, 4, 2);

            ts.Set(BorderSide.AllSides | BorderSide.NotTopLeftCorner, 1, 1);
            ts.Set(BorderSide.AllSides | BorderSide.NotTopRightCorner, 0, 1);
            ts.Set(BorderSide.AllSides | BorderSide.NotBottomLeftCorner, 1, 0);
            ts.Set(BorderSide.AllSides | BorderSide.NotBottomRightCorner, 0, 0);

            return ts;
        }

        public static BorderTileSet BrownRock()
        {
            var ts = new BorderTileSet(Textures.RockTiles);

            ts.Set(BorderSide.EmptySpace, 0, 7);

            ts.Set(BorderSide.None, 0, 1);

            ts.Set(BorderSide.Right | BorderSide.Bottom, 3, 0);
            ts.Set(BorderSide.Left | BorderSide.Right | BorderSide.Bottom, 4, 0);
            ts.Set(BorderSide.Left | BorderSide.Bottom, 5, 0);
            ts.Set(BorderSide.Bottom, 6, 0);

            ts.Set(BorderSide.Top | BorderSide.Bottom | BorderSide.Right, 3, 1);

            ts.Set(BorderSide.Left | BorderSide.Top, 8, 7);
            ts.Set(BorderSide.Left | BorderSide.Right | BorderSide.Top, 7, 7);
            ts.Set(BorderSide.Right | BorderSide.Top, 6, 7);


            ts.Set(BorderSide.AllSides, 4, 1);
           
            ts.Set(BorderSide.Top | BorderSide.Bottom | BorderSide.Left, 5, 1);
            ts.Set(BorderSide.Top | BorderSide.Bottom, 6, 1);

            ts.Set(BorderSide.AllSides | BorderSide.NotTopLeftCorner, 4, 7);
            ts.Set(BorderSide.AllSides | BorderSide.NotTopRightCorner, 5, 7);


            return ts;
        }

        public static BorderTileSet BlueRock()
        {
            var ts = new BorderTileSet(Textures.RockTiles);

            ts.Set(BorderSide.EmptySpace, 0, 7);
            ts.Set(BorderSide.None, 0, 1);

            ts.Set(BorderSide.Right | BorderSide.Bottom, 3, 2);
            ts.Set(BorderSide.Left | BorderSide.Right | BorderSide.Bottom, 4, 2);
            ts.Set(BorderSide.Left | BorderSide.Bottom, 5, 2);
            ts.Set(BorderSide.Bottom, 6, 2);

            ts.Set(BorderSide.Top | BorderSide.Bottom | BorderSide.Right, 3, 3);

            ts.Set(BorderSide.AllSides, 4, 1);

            ts.Set(BorderSide.Top | BorderSide.Bottom | BorderSide.Left, 5, 3);
            ts.Set(BorderSide.Top | BorderSide.Bottom, 6, 3);


            return ts;
        }

        public static AutogenTiles AutoGenTiles()
        {
            var texture = Textures.RockTiles;

            var brownRockSet = BrownRock();
            var blueRockSet = BlueRock();

            var tileSet = new AutogenTiles(texture);

            var grassGroundTiles = new AutogenTile(tileSet);
            grassGroundTiles.AddTileChoice(0, 0);
            grassGroundTiles.AddTileChoice(1, 0);
            grassGroundTiles.AddTileChoice(2, 0);
            grassGroundTiles.AddTileChoice(1, 1);
            grassGroundTiles.AddTileChoice(2, 1);
            grassGroundTiles.AddTileChoice(0, 2);
            grassGroundTiles.AddTileChoice(1, 2);
            grassGroundTiles.AddTileChoice(2, 2);
            grassGroundTiles.AddTileChoice(1, 3);
            grassGroundTiles.AddTileChoice(2, 3);

            var brownRockTiles = new AutogenTile(tileSet, brownRockSet.TileIndices);
            brownRockTiles.AddTileChoice(0, 0);
            brownRockTiles.AddTileChoice(1, 0);
            brownRockTiles.AddTileChoice(2, 0);
            brownRockTiles.AddTileChoice(0, 1);
            brownRockTiles.AddTileChoice(1, 1);
            brownRockTiles.AddTileChoice(2, 1);



            var brownRockGrassTop = new AutogenTile(tileSet, 1, 0);
            brownRockGrassTop.Conditions.Add(BorderSide.None, new AutogenTile(tileSet, 4, 0));
            brownRockGrassTop.MaxMatchesPerScreen = 6;
            brownRockGrassTop.MaxPass = 0;

            var brownRockGrassTopLeft = new AutogenTile(tileSet, 1, 1);
            brownRockGrassTopLeft.Conditions.Add(BorderSide.None, new AutogenTile(tileSet, 3, 0));
            brownRockGrassTopLeft.Conditions.Add(BorderSide.Right, new AutogenTile(tileSet, 1, 0));
      
            var brownRockGrassTopRight = new AutogenTile(tileSet, 2, 1);
            brownRockGrassTopRight.Conditions.Add(BorderSide.None, new AutogenTile(tileSet, 5, 0));
            brownRockGrassTopRight.Conditions.Add(BorderSide.Left, new AutogenTile(tileSet, 1, 0));
          
            var brownRockGrassLeft = new AutogenTile(tileSet, 0, 0);
            brownRockGrassLeft.Conditions.Add(BorderSide.None, new AutogenTile(tileSet, 4, 0));
            brownRockGrassLeft.Conditions.Add(BorderSide.Right, new AutogenTile(tileSet, 1, 0));
         
            var brownRockGrassRight = new AutogenTile(tileSet, 2, 0);
            brownRockGrassRight.Conditions.Add(BorderSide.None, new AutogenTile(tileSet, 4, 0));
            brownRockGrassRight.Conditions.Add(BorderSide.Left, new AutogenTile(tileSet, 1, 0));
          

            var grass = new AutogenTile(tileSet, 4, 3);
            grass.Conditions.Add(BorderSide.Bottom, grassGroundTiles);
            grass.Conditions.Add(BorderSide.None, AutogenTile.Empty);

            var flowers = new AutogenTile(tileSet);
            flowers.MaxMatchesPerScreen = 4;
            flowers.MaxPass = 2;
            flowers.AddTileChoice(7, 5);
            flowers.AddTileChoice(8, 5);
            flowers.AddTileChoice(7, 6);
            flowers.AddTileChoice(8, 6);
            flowers.Conditions.Add(BorderSide.None, grass);

            var stones = new AutogenTile(tileSet);
            stones.AddTileChoice(9, 5);
            stones.AddTileChoice(9, 6);

            stones.MaxMatchesPerScreen = 2;
            stones.Conditions.Add(BorderSide.Bottom, brownRockTiles);
            stones.Conditions.Add(BorderSide.None, AutogenTile.Empty);

            tileSet.AutoGenTiles.Add(flowers);
            tileSet.AutoGenTiles.Add(grass);
            tileSet.AutoGenTiles.Add(brownRockGrassTopLeft);
            tileSet.AutoGenTiles.Add(brownRockGrassTopRight);
            tileSet.AutoGenTiles.Add(brownRockGrassLeft);
            tileSet.AutoGenTiles.Add(brownRockGrassRight);
            tileSet.AutoGenTiles.Add(brownRockGrassTop);
            tileSet.AutoGenTiles.Add(stones);

            return tileSet;
        }

        public static BorderTileSet Border()
        {
            var ts = new BorderTileSet(Textures.Border);
            ts.SetSquare(0, 0);
            return ts;
        }

    }
}
