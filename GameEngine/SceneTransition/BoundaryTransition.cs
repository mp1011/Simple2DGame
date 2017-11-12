using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class BoundaryTransition<TPlayer> : SceneTransition
        where TPlayer:class, IMovingWorldObject
    {
        private TPlayer Player;
        protected Scene Scene { get; private set; }

        protected SceneID CurrentMap => Scene.ID;

        private SceneID nextMap;
        public override SceneID NextMap
        {
            get
            {
                if(nextMap == null)
                {
                    var exitSide = Scene.Boundary.GetExitingSide(Player.Position);
                    nextMap = GetNextMap(exitSide, Player);
                }

                return nextMap;
            }
        }

        protected abstract SceneID GetNextMap(BorderSide exitSide, TPlayer player);

        public BoundaryTransition(TPlayer player, Scene scene) : base(new CollisionCondition<TPlayer, Boundary>(player))
        {
            Player = player;
            Scene = scene;
        }
    }
}
