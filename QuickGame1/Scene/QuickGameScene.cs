using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class QuickGameScene : Scene
    {
        public MapTemplate MasterTemplate { get; private set; }

        public static QuickGameScene Current { get; set; }

        public Layer ParallaxLayer {  get { return Layers[0]; } }   
        public Layer Background1 { get { return Layers[1]; } }
        public Layer WaterLayer { get { return Layers[2]; } }
        public Layer SolidLayer { get { return Layers[3]; } }
        public Layer InterfaceLayer {  get { return Layers[4]; } }

       
        private QuickGameTileMap _tileMap;
        public QuickGameTileMap TileMap
        {
            get
            {
                return _tileMap ?? (_tileMap = SolidLayer.FixedDisplayable.OfType<QuickGameTileMap>().Last());
            }
        }

        public Vector2 PlayerStart { get; set; }
        public King Player { get; private set; }
        
        public QuickGameScene(SceneID id, MapTemplate masterTemplate) :base(id)
        {
            MasterTemplate = masterTemplate;
            Current = this;
            Load();
        }

        protected override IEnumerable<Layer> LoadLayers()
        {
             return new Layer[]
             {
                new Layer(this).SetParallaxScrolling(0.5f,0),
                new Layer(this).SetNormalScrolling(),
                new Layer(this).SetNormalScrolling(),
                new Layer(this).SetNormalScrolling(),
                new Layer(this).SetNoScrolling()
             };
        }

        protected override void LoadSceneContent()     
        {
            ParallaxLayer.FixedDisplayable = new IDisplayable[] 
            {
                new SimpleGraphic(new Vector2(150, 0), new Sprite(Textures.TreeTexture), ParallaxLayer),
                new SimpleGraphic(new Vector2(150, 64), new Sprite(Textures.TreeTexture), ParallaxLayer)
            };

            MapFromImage.Create(this);

            CollisionManager = new QuickGameCollisionManager(this);


            SolidLayer.CollidableObjects.Add(TileMap);

            Player = new King();
            new Fairy(Player);
       
            CameraCenter = Player;

            new Interface(this);
            NeededSounds.AddRange(Sounds.GetAllSounds());

            new Spike(this);
            new Spring(this);

            new WaterMover(WaterLayer);

            WaterHelper.AddWaterOverlays(WaterLayer, SolidLayer);
            new LadderCollisionDetector(TileMap);

            var editor = new LiveEditor(this);

            new PauseHandler();

           // var sh = new ShopHandler(Input.GetInput(this),this);
            //new Sign().SetShopHandler(sh).MoveTo(200, 50);
            //new Chest().SetShopHandler(sh).MoveTo(216, 50);
            //new Elf().SetShopHandler(sh).MoveTo(180, 50).Direction = Direction.Left;

            new MapSaver().LoadFromDisk(this);

            //DebugText.DebugWatch(this, Fonts.SmallFont, InterfaceLayer, s => "O=" + s.SolidLayer.CollidableObjects.Count().ToString());
            //DebugText.DebugWatch(this, Fonts.SmallFont, InterfaceLayer, s => "F=" + s.SolidLayer.CollidableObjects.OfType<FrozenObject>().Count().ToString());
            //DebugText.DebugWatch(this, Fonts.SmallFont, InterfaceLayer, s => "C=" + s.SolidLayer.CollidableObjects.OfType<Coin>().Count().ToString());
        }

        protected override IEnumerable<SceneTransition> LoadTransitions()
        {
            yield return new QuickGameBoundaryTransition(Player, this);
        }
    }
}
