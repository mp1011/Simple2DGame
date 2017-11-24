using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface ISceneLoader
    {
        Scene LoadScene(SceneID map, bool forceReload=false);
    }
}
