using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IConfigurationProvider
    {
        T GetValue<T>(string name, bool optional = false);
        IEnumerable<T> GetValues<T>(string name);
        int ChangeIndicator { get; }        
        IConfigurationProvider GetSection(string name);
        string[] GetKeys();
        int ItemCount { get; }
    }

    public static class Config
    {
        public static IConfigurationProvider Provider;

        public static T ReadValue<T>(string key)
        {
            var cv = new ConfigValue<T>(key);
            return cv.Value;
        }
    }

    public class StringConfigProvider : IConfigurationProvider
    {
        private Dictionary<string, string[]> values;

        public StringConfigProvider()
        {
        }

        protected void OnConfigLoaded(Dictionary<string,string[]> loadedValues)
        {
            values = loadedValues;
            ChangeIndicator++;
        }

        protected virtual void BeforeGetValue()
        {

        }

        public int ItemCount => values.Count;

        public int ChangeIndicator { get; private set; }

        public T GetIndexedValue<T>(string name, int index)
        {
            return StringToObject<T>(values[name][index]);
        }

        public int GetIndexedValueLength(string name)
        {
            return values[name].Length;
        }

        public IConfigurationProvider GetSection(string name)
        {
            BeforeGetValue();
            var keys = values.Keys.Where(p => p.StartsWith(name + " ")).ToArray();
            var ret = new StringConfigProvider() { values = keys.ToDictionary(k => k.Substring(name.Length+1), v => values[v]) };
            return ret;
        }

        public string[] GetKeys()
        {
            return values.Keys.Select(p => p.Split(' ').First()).Distinct().ToArray();
        }

        public T GetValue<T>(string name, bool optional)
        {
            var stringValue = GetString(name, optional);
            if (optional && stringValue == null)
                return default(T);
            else
                return StringToObject<T>(stringValue);
        }

        public IEnumerable<T> GetValues<T>(string name)
        {
            var stringValue = GetString(name, true);
            if (stringValue == null || stringValue.Length == 0)
                return new T[] { };
            else
                return stringValue.Select(p => StringToObject<T>(p)).ToArray();
        }

        private string[] GetString(string name, bool optional)
        {
            BeforeGetValue();
            if (!values.ContainsKey(name))
            {
                if (!optional)
                    throw new Exception("Missing config: " + name);
                else
                    return null;
            }

            return values[name];
        }

        private T StringToObject<T>(string stringValue, T defaultValue=default(T))
        {
            object ret = StringToObject(stringValue, typeof(T));
            if (ret == null)
                return defaultValue;
            else
                return (T)ret;
        }

        private object StringToObject(string stringValue, Type type)
        { 
            object ret;

            if (String.IsNullOrEmpty(stringValue))
                return null;

            if (type == typeof(string))
                ret = stringValue;
            else if (type == typeof(float))
                ret = float.Parse(stringValue);
            else if (type == typeof(float?))
            {
                if (stringValue == "")
                    return null;
                ret = float.Parse(stringValue);
            }
            else if (type == typeof(TimeSpan))
            {
                return stringValue.ParseAny();
            }
            else if (type == typeof(Int32))
            {
                ret = Int32.Parse(stringValue);
            }
            else if (type.IsEnum)
            {
                ret = Enum.Parse(type, stringValue);
            }
            else
                throw new Exception("Invalid type");

            return ret;
        }
        
        private T StringToObject<T>(string[] stringValue)
        {
            object ret;

            if (typeof(T) == typeof(Vector2))
            {
                var x = StringToObject<float>(stringValue[0]);
                var y = StringToObject<float>(stringValue[1]);
                ret = new Vector2(x, y);
            }
            else if (typeof(T) == typeof(Rectangle))
            {
                var x = StringToObject<float>(stringValue[0]);
                var y = StringToObject<float>(stringValue[1]);
                var w = StringToObject<float>(stringValue[2]);
                var h = StringToObject<float>(stringValue[3]);
                ret = new Rectangle(x, y, w, h);
            } 
            else if (typeof(T) == typeof(AxisMotionConfig))
            {
                var values = StringToDictionary(stringValue);
                var cfg = new AxisMotionConfig();

                cfg.StartSpeed = values.TryGet("start", "0").ParseFloat();
                cfg.TargetSpeed = values.TryGet("target", "0").ParseFloat();
                cfg.Change = new ScaledFloat(values.TryGet("delta", "0").ParseFloat(), 1f);
                cfg.Axis = StringToObject(values.TryGet("axis") ?? "", Axis.X);
                cfg.Vary = values.TryGet("vary", "0").ParseFloat();

                bool flipLeft = values.TryGet("flip left", "false").ParseBool();
                bool flipUp = values.TryGet("flip up", "false").ParseBool();

                if (flipLeft)
                    cfg.FlipWhen = Direction.Left;
                else if (flipUp)
                    cfg.FlipWhen = Direction.Up;

                ret = cfg;
            }
            else if (typeof(T) == typeof(XYMotionConfig))
            {
                var cfg = new XYMotionConfig();
                cfg.MotionPerSecond = new Vector2(stringValue[0].ParseFloat(), stringValue[1].ParseFloat());
                if (stringValue.Any(p => p == "flip left"))
                    cfg.FlipXWhen = Direction.Left;
                ret = cfg;
            }
            else if(typeof(T).IsArray)
            {
                object[] array = stringValue.Select(p => StringToObject(p, typeof(T).GetElementType())).ToArray();
                ret = array;
            }
            else if (stringValue.Length == 1)
                ret = StringToObject<T>(stringValue[0]);
            else
                throw new Exception("Invalid type");

            return (T)ret;
        }
        
        private Dictionary<string,string> StringToDictionary(string[] values)
        {
            return values.ToDictionary(k => k.Split(':')[0].Trim(), v => v.Split(':')[1].Trim());
        }
    }

    public class DevelopmentConfigurationProvider : StringConfigProvider
    {
        private string ConfigFile;
        private DateTime LastModified;
        private DateTime LastCheckTime = DateTime.MinValue;


        public DevelopmentConfigurationProvider(string file=@"Content\Config.txt")
        {
            ConfigFile = file;
        }

        private Dictionary<string, string[]> ReadConfig()
        {
            var ret = new Dictionary<string, string[]>();
            
            Stack<string> context = new Stack<string>();

            int lastIndent = -1;
            int thisIndent = 0;
            int currentListIndex = 0;

            foreach (var line in File.ReadAllLines(ConfigFile).Select(p=> p.Contains(@"//") ? p.Substring(0, p.IndexOf(@"//")) : p))
            {
                thisIndent = line.TakeWhile(p => Char.IsWhiteSpace(p)).Count();

                var i = lastIndent;
                while (i-- > thisIndent)
                {
                    currentListIndex = 0;
                    context.Pop();
                }

                var currentPrefix = String.Join(" ", context.Reverse());
                if (currentPrefix.Length > 0)
                    currentPrefix += " ";

                if(line.Trim().StartsWith("+"))
                {
                    ret.Add(currentPrefix + currentListIndex, line.Trim().Substring(1).Split(','));
                    currentListIndex++;
                }
                else if (line.Contains('='))
                {
                    var left = line.Split('=')[0].Trim();
                    var right = line.Split('=')[1].Trim();
                    ret.Add(currentPrefix + left, right.Split(','));
                }
                else 
                {
                    context.Push(line.Trim());
                    currentListIndex = 0;
                }
         
                lastIndent = thisIndent;
            }

            
            return ret;

        }

        protected override void BeforeGetValue()
        {
            if ((DateTime.Now - LastCheckTime).TotalSeconds > 5)
            {
                LastCheckTime = DateTime.Now;

                var lwt = File.GetLastWriteTime(ConfigFile);
                if (lwt > LastModified)
                {
                    LastModified = lwt;

                    var val = ReadConfig();
                    OnConfigLoaded(val);
                }

            }
        }
    }


}
