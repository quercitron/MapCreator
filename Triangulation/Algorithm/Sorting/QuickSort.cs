using System;
using System.Collections.Generic;

namespace Triangulation.Algorithm.Sorting
{
    public static class Sort
    {
        public static void QuickSort<T>(this IList<T> list, Comparison<T> comparison)
        {
            if (list.Count > 1)
            {
                QuickSort(list, 0, list.Count - 1, comparison);
            }
        }

        private static void QuickSort<T>(IList<T> list, int l, int r, Comparison<T> comparison)
        {
            int i = l;
            int j = r;
            T x = list[(l + r)/2];

            do
            {
                while (comparison.Invoke(list[i], x) < 0)
                {
                    i++;
                }
                while (comparison.Invoke(list[j], x) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    T tmp = list[i];
                    list[i] = list[j];
                    list[j] = tmp;

                    i++;
                    j--;
                }
            } while (i <= j); // TODO: check why <= is better then <

            if (l < j)
            {
                QuickSort(list, l, j, comparison);
            }
            if (i < r)
            {
                QuickSort(list, i, r, comparison);
            }
        }
    }
}
