using GameEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace QuickGame1
{
    enum CellType
    {
        Empty,
        Coin,
        Snake
    }
      
    interface IEditorPlaceable : IWithPosition, IRemoveable
    {
        CellType EditorType { get; }
    }

    class MapSaver
    {
        public MapSaveInfo GetMapObjects(QuickGameScene scene)
        {
            var ret = new MapSaveInfo();
            foreach(var obj in scene.SolidLayer.CollidableObjects.OfType<IEditorPlaceable>())
            {
                ret.GetList(obj.EditorType).Add(new ObjectStartInfo(obj));
            }
            return ret;
        }    

        public void LoadFromDisk(QuickGameScene scene)
        {
            var formatter = new BinaryFormatter();
            MapSaveInfo mapInfo;

            using (var stream = GetFileStream(scene.ID))
            {
                if (stream.Length == 0)
                    mapInfo = new MapSaveInfo();
                else 
                    mapInfo = formatter.Deserialize(stream) as MapSaveInfo;
            }
            
            foreach(var key in mapInfo.Items.Keys)
            {
                foreach(var obj in mapInfo.Items[key])
                {
                    CreateObject(key, obj);
                }
            }
        }
        
        private void CreateObject(CellType type, ObjectStartInfo startInfo)
        {
            if(type == CellType.Coin)
            {
                new Coin().Position.Center = new Vector2(startInfo.X, startInfo.Y);
            }
        }

        public void SaveToDisk(QuickGameScene scene)
        {
            var saveInfo = GetMapObjects(scene);
            var formatter = new BinaryFormatter();
            using (var stream = GetFileStream(scene.ID))
            {
                formatter.Serialize(stream, saveInfo);
                stream.Flush();
            }
        }

        private Stream GetFileStream(SceneID sceneID)
        {
            //todo, use resource 
            var path = @"C:\Users\Miko\Documents\GitHub\SomeGame\QuickGame1\Content\Maps\" + sceneID.ToString() + ".map";
            return File.Open(path, FileMode.OpenOrCreate);
        }
    }

    [Serializable]
    class MapSaveInfo
    {
        public Dictionary<CellType, List<ObjectStartInfo>> Items = new Dictionary<CellType, List<ObjectStartInfo>>();

        public List<ObjectStartInfo> GetList(CellType cellType)
        {
            var ret = Items.TryGet(cellType);
            if(ret == null)
            {
                ret = new List<ObjectStartInfo>();
                Items.Add(cellType, ret);
            }
            return ret;
        }
    }

    [Serializable]
    class ObjectStartInfo
    {
        public float X;
        public float Y;

        public ObjectStartInfo() { }

        public ObjectStartInfo(IEditorPlaceable obj)
        {
            X = obj.Position.Center.X;
            Y = obj.Position.Center.Y;
        }
    }

}
