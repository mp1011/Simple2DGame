using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class Scene :IWithPosition
    {
        public SceneID ID { get; }
        public List<SoundID> NeededSounds = new List<SoundID>();
        public IWithPosition CameraCenter { get; set; }
        public Layer[] Layers { get; private set; }
        private List<UpdateGroup> updateGroups = new List<UpdateGroup>();
        private SceneTransition[] transitions;
        public IEnumerable<UpdateGroup> UpdateGroups => updateGroups;

        public Rectangle Position { get; private set; }
        public Color BackgroundColor { get; set; }

        public bool IsFinished { get; private set; }

        public CollisionManager CollisionManager { get; protected set; }

        public Boundary Boundary { get; private set; }

        protected void Load()
        {
            Layers = LoadLayers().ToArray();
            BackgroundColor = new Color(20, 30, 20);
            Position = new Rectangle();
            Position.Set(Engine.GetScreenSize());
            LoadSceneContent();

            foreach (var displayable in Layers.SelectMany(p => p.FixedDisplayable))
            {
                Position.ExpandToContain(displayable.Position);
            }

            Boundary = new Boundary(this);
            if(CollisionManager != null)
                CollisionManager.Layer.CollidableObjects.Add(Boundary);

            if (CameraCenter == null)
                CameraCenter = this;

            transitions = LoadTransitions().ToArray();
        }
        
        protected Scene(SceneID id)
        {
            ID = id;
        }

        protected virtual void BeforeLoad() { }
        protected abstract IEnumerable<Layer> LoadLayers();
        protected abstract void LoadSceneContent();
        protected abstract IEnumerable<SceneTransition> LoadTransitions();
        
        public SceneTransition CheckTransitions()
        {
            foreach(var transition in transitions)
            {
                if(transition.ExitCondition.IsActiveAndNotNull())
                    return transition;
            }

            return null;
        }
        public void AddObject(IUpdateable updatable)
        {
            var group = UpdateGroups.FirstOrDefault(p => p.Priority == updatable.Priority);
            if(group == null)
            {
                group = new UpdateGroup(updatable.Priority);
                updateGroups.Add(group);
                updateGroups = updateGroups.OrderBy(p => p.Priority).ToList();
            }
            group.Add(updatable);

        }

    }
}
