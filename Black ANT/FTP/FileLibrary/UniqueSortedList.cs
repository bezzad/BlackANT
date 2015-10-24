using System;
using System.Collections.Generic;

namespace FTP.FileLib
{
    public class UniqueSortedList<T> : UniqueList<T>, IComparer<T>
    {
        Comparison<T> ComparisonMethod;
        
        public UniqueSortedList(Comparison<T> CompareFunction)
        {
            if(CompareFunction == null)
            {
                throw new ArgumentNullException("comparer function");
            }
            this.ComparisonMethod = CompareFunction;
        }

        public override bool Contains(T item)
        {
            if (Values.BinarySearch(item, this) < 0) return false;

            return true;
        }

        public override void Push(T item)
        {
            if (ReferenceEquals(item, null)) return;
            if (item == null) return;

            int InsertIndex = Values.BinarySearch(item, this);
            if (InsertIndex < 0) Values.Insert(~InsertIndex, item);
        }

        public override void PushRange(IEnumerable<T> items)
        {
            foreach (T t in items)
                this.Push(t);
        }

        public int Compare(T x, T y)
        {
            return ComparisonMethod(x, y);
        }
    }
}
