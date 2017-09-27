using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Structures;

namespace LogSolver.Helpers
{
    public class Heap<TNode> : ICollection<TNode> where TNode : IComparable<TNode>
    {
        protected readonly List<TNode> buffer;
        public int Count => buffer.Count;
        public bool IsReadOnly => false;

        public Heap()
        {
            buffer = new List<TNode>();
        }

        public Heap(IEnumerable<TNode> items)
        {
            buffer = new List<TNode>(items);
            MakeHeap();
        }

        protected void FixHeap(int i)
        {
            int parent = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int min = parent;

            if (right < buffer.Count && buffer[right].CompareTo(buffer[min]) < 0)
                min = right;

            if (left < buffer.Count && buffer[left].CompareTo(buffer[min]) < 0)
                min = left;

            if (min != parent)
            {
                var temp = buffer[min];
                buffer[min] = buffer[parent];
                buffer[parent] = temp;

                FixHeap(min);
            }
        }

        protected void MakeHeap()
        {
            for (int i = buffer.Count - 1; i >= 0; --i)
                FixHeap(i);
        }

        public void Add(TNode item)
        {
            buffer.Add(item);
            int currentIndex = buffer.Count - 1;
            int parentIndex = (currentIndex - 1) / 2;

            while (currentIndex != parentIndex && buffer[currentIndex].CompareTo(buffer[parentIndex]) < 0)
            {
                var temp = buffer[parentIndex];
                buffer[parentIndex] = buffer[currentIndex];
                buffer[currentIndex] = temp;

                currentIndex = parentIndex;
                parentIndex = (currentIndex - 1) / 2;
            }
        }

        public TNode ExtractMin()
        {
            var res = buffer[0];
            buffer[0] = buffer[buffer.Count - 1];
            buffer.RemoveAt(buffer.Count - 1);
            FixHeap(0);
            return res;
        }

        public TNode PeekMin()
        {
            return buffer.Any() ? buffer[0] : throw new InvalidOperationException("Heap is empty.");
        }

        public void UpdateKey(TNode item)
        {
            var index = buffer.IndexOf(item);
            if (index == -1)
                throw new InvalidOperationException("Key is not present in collection.");
            FixHeap(index);
        }

        public bool Remove(TNode item)
        {
            int index = buffer.IndexOf(item);
            if (index == -1)
                return false;

            buffer[index] = buffer[buffer.Count - 1];
            buffer.RemoveAt(buffer.Count);
            FixHeap(index);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TNode> GetEnumerator() => buffer.GetEnumerator();

        public void Clear() => buffer.Clear();

        public bool Contains(TNode item) => buffer.Contains(item);

        public void CopyTo(TNode[] array, int arrayIndex)
        {
            buffer.CopyTo(array, arrayIndex);
        }




    }
}