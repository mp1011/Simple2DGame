using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameEngine
{
    public static class Extensions
    {
        public static T[] RemoveWhere<T>(this List<T> list, Func<T,bool> condition)
        {
            var ret = list.Where(condition).ToArray();
            list.RemoveAll(x=>condition(x));
            return ret;
        }

        public static string StringJoin(this IEnumerable<object> o)
        {
            return String.Join(", ", o.Select(p => p.ToString().ToUpper()));
        }

        public static T Require<T>(this T obj) where T:class
        {
            if (obj == null)
                throw new Exception($"Missing required object of type {typeof(T).Name}");

            return obj;
        }

        public static T[] EmptyIfNull<T>(this T[] list)
        {
            if (list == null)
                return new T[] { };
            else
                return list;
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> list)
        {
            if (list == null)
                return new T[] { };
            else
                return list;
        }

        public static float ParseFloat(this string str)
        {
            return float.Parse(str);
        }

        public static bool ParseBool(this string str)
        {
            return bool.Parse(str);
        }

        public static object ParseAny(this string str)
        {
            str = str.Trim();

            if (str.EndsWith("s"))
                return TimeSpan.FromSeconds(double.Parse(str.TrimEnd('s')));
            if (str.EndsWith("m"))            
                return TimeSpan.FromMilliseconds(double.Parse(str.TrimEnd('m')));            
            else if(Regex.IsMatch(str, @"-?\d+.?\d*"))
            {
                if (str.Contains("."))
                    return double.Parse(str);
                else
                    return int.Parse(str);
            }
            else 
                return str;
        }

        public static IEnumerable<Type> FindDerivedClassesWithAttribute<TAttr>(this Type baseType) where TAttr :Attribute
        {
            return baseType.Assembly.GetTypes().Where(p => baseType.IsAssignableFrom(p) && p.GetCustomAttribute<TAttr>() != null);
        }

        public static IEnumerable<Type> FindClassesWithAttribute<TAttr>(this Assembly assembly) where TAttr:Attribute
        {
            return assembly.GetTypes().Where(p => p.GetCustomAttribute<TAttr>() != null);
        }

        public static V TryGet<K,V>(this Dictionary<K,V> dict, K key, V defaultValue=default(V))
        {
            V ret;
            if (dict.TryGetValue(key, out ret))
                return ret;
            else
                return defaultValue;
        }
    }

    public static class EnumHelper
    {
        public static IEnumerable<T> GetValues<T>()
        {
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                yield return (T)value;
            }
        }
    }
}
