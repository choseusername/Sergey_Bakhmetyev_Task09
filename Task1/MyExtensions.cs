using System;
using System.Linq;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        // Общий случай, n воинов  стоящих по кругу, убивают каждого k. O(n).
        static int JosephusProblem(int n, int k)
        {
            if (n == 1) return 1;
            return 1 + (JosephusProblem(n - 1, k) + k - 1) % n;
        }

        // Замкнутая формула для случая k = 2. O(1).
        static int JosephusProblemForK2(int n)
        {
            return 2 * (n - (int)Math.Pow(2, (int)Math.Log(n, 2))) + 1;
        }

        public static void RemoveEachSecondItem<T>(this ICollection<T> list)
        {
            var item = list.ElementAt(JosephusProblemForK2(list.Count) - 1);
            list.Clear();
            list.Add(item);
        }

    }
}
