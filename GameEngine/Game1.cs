using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace GameEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public double TimeScale = 1.0;

        XNAGameEngine engine;

          
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SceneID InitialScene;

        public Game1(ISceneLoader sceneLoader, SceneID initialScene)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 256*4;
            graphics.PreferredBackBufferHeight = 192*4;
            
            Content.RootDirectory = "Content";

            engine = new XNAGameEngine();
            engine.SceneLoader = sceneLoader;

            InitialScene = initialScene;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            engine.Renderer = new SpriteBatchRenderer(this.spriteBatch, engine, 256,192);
            engine.GraphicsDevice = this.GraphicsDevice;
            new XNAAudioEngine(Content);            
            new TextureInfoReader(engine, Content);


            engine.Scene = engine.SceneLoader.LoadScene(InitialScene);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    TimeScale = 0.5;
                else if (Keyboard.GetState().IsKeyDown(Keys.F))
                    TimeScale = 2.0;
                else if (Keyboard.GetState().IsKeyDown(Keys.N))
                    TimeScale = 1;

            }

            var egt = gameTime.ElapsedGameTime;

            if (TimeScale != 1.0)
                egt = TimeSpan.FromMilliseconds(egt.TotalMilliseconds * TimeScale);

            engine.Update(egt);
           
            base.Update(gameTime);
        }

        private RenderTarget2D RenderTarget;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (RenderTarget == null)
            {
                var screen = engine.Renderer.ScreenBounds.Position;
                RenderTarget = new RenderTarget2D(this.GraphicsDevice, (int)screen.Width, (int)screen.Height);
            }

            GraphicsDevice.SetRenderTarget(RenderTarget);
          //  engine.GraphicsDevice = GraphicsDevice;
            GraphicsDevice.Clear(engine.Scene.BackgroundColor);
            spriteBatch.Begin();
            engine.DrawFrame();
            spriteBatch.End();



            Matrix m = Matrix.CreateScale(4.0f);
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Red);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, m);
            spriteBatch.Draw(RenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
