using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralAlgorithms.PriorityQueue
{
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        public PriorityQueue(Comparison<T> comparison = null)
        {
            if (comparison == null)
            {
                if (typeof(T).GetInterfaces().Any(i => i == typeof(IComparable)))
                {
                    m_Comparison = (a, b) => ((IComparable)a).CompareTo(b);
                }
                else
                {
                    throw new ApplicationException("Add comparer");
                }
            }
            else
            {
                m_Comparison = comparison;
            }
        }

        public int Count { get; private set; }

        public void Enqueue(T item)
        {
            if (m_Indexes.ContainsKey(item))
            {
                Update(item);
                return;
            }

            m_List.Add(item);
            m_Indexes.Add(item, Count + 1);
            Count++;
            Up(Count);
        }

        public T Peek()
        {
            return m_List[0];
        }

        public T Dequeue()
        {
            if (Count > 0)
            {
                var result = m_List[0];

                Swap(0, m_List.Count - 1);
                m_Indexes.Remove(m_List[m_List.Count - 1]);
                m_List.RemoveAt(Count - 1);
                Count--;
                Down(1);

                return result;
            }
            throw new ApplicationException("Couldn't get element from empty queue");
        }

        public void Update(T item)
        {
            int index = m_Indexes[item];
            Up(index);
        }

        private readonly List<T> m_List = new List<T>();

        private readonly Dictionary<T, int> m_Indexes = new Dictionary<T, int>();

        private readonly Comparison<T> m_Comparison;        

        private void Up(int index)
        {
            while (index > 1 && m_Comparison.Invoke(m_List[index - 1], m_List[index / 2 - 1]) > 0)
            {
                Swap(index - 1, index / 2 - 1);

                index = index / 2;
            }
        }

        private void Down(int index)
        {
            while (2 * index <= Count && m_Comparison.Invoke(m_List[index - 1], m_List[2 * index - 1]) < 0 ||
                   2 * index + 1 <= Count && m_Comparison.Invoke(m_List[index - 1], m_List[2 * index]) < 0)
            {
                if (2 * index + 1 > Count || m_Comparison.Invoke(m_List[2 * index - 1], m_List[2 * index]) > 0)
                {
                    Swap(index - 1, 2 * index - 1);
                    index = 2 * index;
                }
                else
                {
                    Swap(index - 1, 2 * index);
                    index = 2 * index + 1;
                }
            }
        }

        private void Swap(int i, int j)
        {            
            var tmp = m_List[i];
            m_List[i] = m_List[j];
            m_List[j] = tmp;

            m_Indexes[m_List[i]] = i + 1;
            m_Indexes[m_List[j]] = j + 1;
        }
    }
}
