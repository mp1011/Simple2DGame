using GameEngine;
using System;

namespace QuickGame1
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Config.Provider = new DevelopmentConfigurationProvider();
            
            using (var game = new GameEngine.Game1(new QuickGameSceneLoader(), Scenes.Intro))
            {
                game.Run();
            }
            
        }
    }
}
