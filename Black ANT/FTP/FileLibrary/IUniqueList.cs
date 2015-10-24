using System;

namespace FTP.FileLib
{
    interface IUniqueList<T>
    {
        void Clear();
        object Clone();
        bool Contains(T item);
        void CopyTo(Array array, int index);
        void CopyTo(T[] array, int arrayIndex);
        int Count { get; }
        bool IsEmpty { get; }
        bool IsReadOnly { get; }
        bool IsSynchronized { get; }
        T Peek();
        T Peek(int index);
        T Pop();
        T Pop(int index);
        void Push(T item);
        void PushRange(System.Collections.Generic.IEnumerable<T> items);
        void Remove(T item);
        void Reset();
        void Skip();
        T this[int index] { get; set; }
        System.Collections.Generic.List<T> ToList();
    }
}
