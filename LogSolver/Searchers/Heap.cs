using System;
using System.Collections;
using System.Collections.Generic;

namespace LogSolver.Searchers
{
    public class Heap<TNode> : ICollection<TNode> where TNode : IComparable<TNode>
    {
        protected readonly List<TNode> buffer;

        public Heap(IEnumerable<TNode> items)
        {
            buffer = new List<TNode>(items);
            make_heap(0, buffer.Count);
        }

        protected void fix_heap(int start, int end, int i)
        {
            int parent = start + i;
            int left = start + 2 * i;
            int right = start + 2 * i + 1;

            int max = parent;

            if (right < end && buffer[right].CompareTo(buffer[max]) < 0)
                max = right;


            if (left < end && buffer[left].CompareTo(buffer[max]) < 0)
                max = left;

            if (max != parent)
            {
                var temp = buffer[max];
                buffer[max] = buffer[parent];
                buffer[parent] = temp;

                fix_heap(start, end, max - start);
            }
        }

        protected void make_heap(int start, int end)
        {
            int size = end - start;
            for (int i = size - 1; i >= 0; --i)
                fix_heap(start, end, i);
        }

        protected TNode extract_min(int start, int end)
        {
            var res = buffer[start];
            buffer[start] = buffer[end - 1];
            fix_heap(start, end - 1, 0);
            return res;
        }


        protected void heap_sort(int start, int end)
        {
            make_heap(start, end);

            int size = end - start;
            for (int i = size - 1; i >= 0; --i)
            {
                int last = start + i;
                buffer[last] = extract_min(start, last + 1);
            }
        }

        public void Add(TNode i)
        {
            buffer.Add(i);
            fix_heap(0, buffer.Count, buffer.Count);
        }

        public TNode ExtractMin()
        {
            TNode res = extract_min(0, buffer.Count);
            buffer.RemoveAt(buffer.Count - 1);
            return res;
        }

        public void Sort() => heap_sort(0, buffer.Count);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TNode> GetEnumerator() => buffer.GetEnumerator();

        public void Clear() => buffer.Clear();

        public bool Contains(TNode item) => buffer.Contains(item);

        public void CopyTo(TNode[] array, int arrayIndex)
        {
            buffer.CopyTo(array, arrayIndex);
        }

        public int Count => buffer.Count;
        public bool IsReadOnly => false;
        public bool Remove(TNode item)
        {
            throw new NotImplementedException();
        }
    }
}