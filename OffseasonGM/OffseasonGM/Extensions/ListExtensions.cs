using OffseasonGM.Global;
using System.Collections.Generic;

namespace OffseasonGM.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                Swap(list, i, GlobalObjects.Random.Next(i, list.Count));
            }
        }

        public static void Swap<T>(this List<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
