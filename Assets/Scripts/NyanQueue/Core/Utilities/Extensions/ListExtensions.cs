using System;
using System.Collections.Generic;

namespace NyanQueue.Core.Utilities.Extensions
{
    public static class ListExtensions
    {
        public static void MergeWithReplaceByType<T>(this List<T> target, List<T> other)
        {
            var map = new Dictionary<Type, int>();

            for (int i = 0; i < target.Count; i++) map[target[i].GetType()] = i;

            foreach (var item in other)
            {
                var type = item.GetType();
                if (map.TryGetValue(type, out int index))
                    target[index] = item;
                else
                {
                    map[type] = target.Count;
                    target.Add(item);
                }
            }
        }
        
        public static List<T> MergeWithReplaceByTypeSafe<T>(this List<T> target, List<T> other)
        {
            var result = new List<T>(target);
            var map = new Dictionary<Type, int>();

            for (int i = 0; i < result.Count; i++) map[result[i].GetType()] = i;

            foreach (var item in other)
            {
                var type = item.GetType();
                if (map.TryGetValue(type, out int index))
                    result[index] = item;
                else
                {
                    map[type] = result.Count;
                    result.Add(item);
                }
            }
            
            return result;
        }
    }
}