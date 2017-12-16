using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Engine
    {
        public static Rectangle GetScreenSize()
        {
            var screenPos = Instance.Renderer.ScreenBounds.Position;
            return new Rectangle(0, 0, screenPos.Width, screenPos.Height);
        }

        public static Boundary GetScreenBoundary()
        {
            return Instance.Renderer.ScreenBounds;
        }

        public Rectangle WindowPosition = new Rectangle();
        public ulong FrameNumber { get; private set; }
        public static Engine Instance { get; private set;}      

        public Scene Scene;
        public IRenderer Renderer;
        public ISceneLoader SceneLoader;
        private const int CleanupSeconds = 5;
        private double SecondsUntilNextCleanup;
        
        protected Engine()
        {
            Engine.Instance = this;
        }
    
        public void Update(TimeSpan elapsedInFrame)
        {
            FrameNumber++;

            if(Scene.NeededSounds != null)           
            {
                foreach (var sound in Scene.NeededSounds)
                {
                    AudioEngine.Instance.LoadSound(sound);
                }

                Scene.NeededSounds = null;
            }

            bool needsCleanup = false;
            foreach (var updateGroup in Scene.UpdateGroups)
            {
                updateGroup.Update(elapsedInFrame);
                if (updateGroup.HasRemovedObjects)
                    needsCleanup = true;
            }

            Scene.AdjustCamera();

            if (needsCleanup)
            {
                SecondsUntilNextCleanup -= elapsedInFrame.TotalSeconds;
                if (SecondsUntilNextCleanup <= 0)
                {
                    SecondsUntilNextCleanup = CleanupSeconds;

                    foreach (var updateGroup in Scene.UpdateGroups)
                        updateGroup.Cleanup();

                    foreach (var layer in Scene.Layers)
                        layer.Cleanup();
                }
            }
            else
            {
                foreach(var layer in Scene.Layers)
                {
                    if (layer.NeedsCleanup)
                        layer.Cleanup();
                }
            }

            Scene.CheckTransitions();
        }

        public void DrawFrame()
        {
            foreach (var layer in Scene.Layers)
            {
                Renderer.CurrentLayer = layer;
                layer.Draw(Renderer);
            }
        }
       
    }
    
    class XNAGameEngine : Engine
    {
        public GraphicsDevice GraphicsDevice;

        public GraphicsDevice GetGraphicsDevice()
        {
            return GraphicsDevice;
        }
    }
}
