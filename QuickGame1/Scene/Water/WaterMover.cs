using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{ 
    class WaterMover : IUpdateable
    {
        public UpdatePriority Priority => UpdatePriority.BeginUpdate;
        private TileMap WaterTiles;
        private Layer Layer;
        private int WaterMoveSpeed;

        private CyclingDouble WaterPos;

        public IRemoveable Root => Layer;

        public WaterMover(Layer layer)
        {
            Layer = layer;
            WaterTiles = layer.FixedDisplayable.OfType<TileMap>().Single();
            WaterMoveSpeed = Config.Provider.GetValue<int>("water move speed");

            WaterPos = new CyclingDouble(0, WaterTiles.Tiles.Texture.CellSize.X);

            layer.Scene.AddObject(this);
        }

        public void Update(TimeSpan elapsedInFrame)
        {
            var delta = WaterMoveSpeed * elapsedInFrame.TotalSeconds;
            WaterPos += delta;
            WaterTiles.Position.SetLeft(WaterPos.Value);

        }
    }

}
