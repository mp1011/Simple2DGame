using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class AutogenTiles
    {
        public TextureInfo Texture { get; private set; }

        public List<AutogenTile> AutoGenTiles = new List<AutogenTile>();

        public AutogenTiles(TextureInfo texture)
        {
            Texture = texture;
        }

        public void Apply(TileMap map)
        {
            var rng = new System.Random((int)(map.Position.Width * map.Position.Height));
            bool anyChanged = true;
            int maxPasses = 50;
            int pass = 0;

            while(anyChanged && pass < maxPasses)
            {
                anyChanged = false;

                var potentialMatches = AutoGenTiles.Where(p => p.MaxMatches > 0 && pass <= p.MaxPass)
                    .Select(p => new PotentialTileMatch(p, map)).Where(p=>p.PossibleTilesToReplace.Any()).ToArray();

                foreach(var match in potentialMatches)
                {
                    while (match.Tile.MaxMatches > 0 && match.PossibleTilesToReplace.Any())
                    {
                        var chosenTile = match.PossibleTilesToReplace.RandomElement(rng);
                        var current = map.Tiles.Cells.GetFromPoint(chosenTile);
                        match.Tile.Apply(map, chosenTile);
                        var changed = map.Tiles.Cells.GetFromPoint(chosenTile);
                        if (current != changed)
                            anyChanged = true;

                        match.PossibleTilesToReplace.Remove(chosenTile);
                        match.Tile.MaxMatches--;
                    }
                }
                
                pass++;
            }

        }
    }

    public class PotentialTileMatch
    {
        public AutogenTile Tile;
        public List<Vector2> PossibleTilesToReplace = new List<Vector2>();

        public PotentialTileMatch(AutogenTile tile, TileMap map)
        {
            Tile = tile;
            PossibleTilesToReplace = tile.GetEligibleTilesToReplace(map).ToList();
        }
    }

    public class AutogenTile
    {
        private static AutogenTile _empty;
        public static AutogenTile Empty
        {
            get
            {
                return _empty ?? (_empty = new AutogenTile(null));
            }
        }

        public AutogenTiles TileSet { get; private set; }

        public int Consecutive = 1;
        public int MaxMatches = int.MaxValue;
        public int MaxPass = 10;


        private List<int> CellIndexChoices = new List<int>();
        public Dictionary<BorderSide, AutogenTile> Conditions = new Dictionary<BorderSide, AutogenTile>();
      
        public AutogenTile(AutogenTiles tileset)
        {
            TileSet = tileset;
        }

        public AutogenTile(AutogenTiles tileset, int tileX, int tileY) : this(tileset)
        {
            AddTileChoice(tileX, tileY);
        }

        public AutogenTile(AutogenTiles tileset, IEnumerable<int> tiles) : this(tileset)
        {
            CellIndexChoices.AddRange(tiles);
        }

        public void AddTileChoice(int tileX, int tileY)
        {
            CellIndexChoices.Add(TileSet.Texture.PointToIndex(tileX, tileY));
        }
      
        public void Apply(TileMap map, Vector2 tile)
        {
            map.Tiles.Cells.Set(tile, CellIndexChoices.RandomElement());
        }

        public AutogenTile Append(int tileX, int tileY)
        {
            var copy = this.Copy();
            copy.AddTileChoice(tileX, tileY);
            return copy;
        }

        public AutogenTile Copy()
        {
            var copy = new AutogenTile(this.TileSet);
            copy.CellIndexChoices.AddRange(this.CellIndexChoices);
            return copy;
        }

        public IEnumerable<Vector2> GetEligibleTilesToReplace(TileMap map)
        {
            var ret = map.Tiles.Cells.Points.Where(pt => CheckCondition(map, pt)).ToArray();
            return ret;
        }

        private bool CheckCondition(TileMap map, Vector2 cell)
        {
            foreach(var side in Conditions.Keys)
            {
                int i = 0;
                var cellChoices = Conditions[side].CellIndexChoices;

                while (i++ < Consecutive)
                {                        
                    var adjacentCell = map.Tiles.Cells.GetFromPoint(cell.GetAdjacent(side));
                    if (cellChoices.Any())
                    {
                        if (!cellChoices.Contains(adjacentCell))
                            return false;
                    }
                    else
                    {
                        if (adjacentCell != map.EmptyCell)
                            return false;
                    }
                }
            }

            return true;
        }

    }

   
}
