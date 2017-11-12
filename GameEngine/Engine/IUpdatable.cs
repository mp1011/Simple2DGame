using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public enum UpdatePriority
    {
        BeginUpdate,
        Input,
        Behavior,
        Motion,
        Collision,
        ModalMenu,
        EndUpdate
    }

    public interface IUpdateable
    {
        UpdatePriority Priority { get; }
        IRemoveable Root { get; }
        void Update(TimeSpan elapsedInFrame);
    }

    public class UpdateGroup
    {
        public ICondition Paused { get; private set; }

        public void AddPauseCondition(ICondition condition)
        {
            Paused = Paused.Or(condition);
        }

        public UpdatePriority Priority { get; private set; }

        public bool HasRemovedObjects { get; private set; }

        private List<IUpdateable> Objects = new List<IUpdateable>();

        private List<IUpdateable> ObjectsToAdd = new List<IUpdateable>();

        public string Name { get; private set; }

        public UpdateGroup(UpdatePriority priority)
        {
            Priority = priority;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Add(IUpdateable o)
        {
            if (o != null)
            {
                if (ObjectsToAdd.Contains(o))
                    throw new Exception("Already added");

                ObjectsToAdd.Add(o);
            }
        }

        public void Cleanup()
        {
            Objects.RemoveAll(p => p.Root.IsRemoved);
        }

        public void Update(TimeSpan frameTime)
        {
            if (Paused.IsActiveAndNotNull())
                return;

            if(ObjectsToAdd.Any())
            {
                Objects.AddRange(ObjectsToAdd);
                ObjectsToAdd.Clear();
            }

            HasRemovedObjects = false;

            foreach (var o in Objects)
            {
                if (!o.Root.IsRemoved)
                    o.Update(frameTime);
                else
                    HasRemovedObjects = true;
            }
        }
    }
}
