using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    class ElfController : IUpdateable
    {
        private Elf Elf;

        UpdatePriority IUpdateable.Priority => UpdatePriority.Behavior;

        IRemoveable IUpdateable.Root => Elf;

        public ElfController(Elf elf)
        {
            Elf = elf;
            Elf.Scene.AddObject(this);
        }

        void IUpdateable.Update(TimeSpan elapsedInFrame)
        {
            Elf.Face(Elf.Scene.Player);
        }
    }
}
