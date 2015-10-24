using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;


namespace FTP.FileLib
{

    public class UniqueList<T> : IUniqueList<T>, IDisposable, ICloneable, IEnumerable, IEnumerable<T>
    {
        protected List<T> Values;
        protected int IndexPointer;

        #region Methods

        public List<T> ToList()
        {
            List<T> lst = new List<T>();
            lst.AddRange(Values);

            return lst;
        }

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <returns>The element at the specified index.</returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than 0.-or-<paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Generic.List`1.Count" />. </exception>
        public T this[int index]
        {
            get { return Peek(index); }
            set { Push(value); }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that is empty and has the default initial capacity.
        /// </summary>
        public UniqueList()
        {
            Values = new List<T>();
            IndexPointer = 0;
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.List`1" />.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.List`1" />.</returns>
        public int Count
        {
            get { return Values.Count; }
        }

        public bool IsEmpty
        {
            get { return Values.Count == 0; }
        }

        /// <summary>Inserts an object at the top of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
        /// <param name="item">The object to push onto the <see cref="T:System.Collections.Generic.List`1" />. The value can not be null from reference types.</param>
        public virtual void Push(T item)
        {
            if (ReferenceEquals(item, null)) return;

            if (!this.Contains(item) && item != null)
                Values.Add(item);
        }

        /// <summary>Inserts an object at the top of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
        /// <param name="item">The IEnumerable objects to push onto the <see cref="T:System.Collections.Generic.List`1" />. The value can not be null from reference types.</param>
        public virtual void PushRange(IEnumerable<T> items)
        {
            foreach (T t in items)
                this.Push(t);
        }

        /// <summary>Returns the object at the bottom of the <see cref="T:System.Collections.Generic.List`1" /> without removing it.</summary>
        /// <param name="index">The int index to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be null for reference types.</param>
        /// <returns>The object at the bottom of the <see cref="T:System.Collections.Generic.List`1" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.List`1" /> is empty.</exception>
        public virtual T Peek(int index)
        {
            if (Values.Count <= index)
            {
                throw new IndexOutOfRangeException();
            }

            return Values[index];
        }

        /// <summary>Returns the object at the bottom of the <see cref="T:System.Collections.Generic.List`1" /> without removing it.</summary>
        /// <returns>The object at the bottom of the <see cref="T:System.Collections.Generic.List`1" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.List`1" /> is empty.</exception>
        public virtual T Peek()
        {
            if (Values.Count <= IndexPointer)
            {
                Reset();
                throw new InvalidOperationException("The UniqueList is Empty");
            }

            return Values[IndexPointer];
        }

        /// <summary>Removes and returns the object at the index of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
        /// <param name="index">The int index to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be null for reference types.</param>
        /// <returns>The object removed from the index of the <see cref="T:System.Collections.Generic.List`1" />.</returns>
        /// <exception cref="T:System.IndexOutOfRangeException">The index is out of range.</exception>
        public virtual T Pop(int index)
        {
            if (Values.Count <= index)
            {
                throw new IndexOutOfRangeException();
            }

            T t = Values[index];

            Values.RemoveAt(index);

            return t;
        }

        /// <summary>Removes and returns the object at the bottom of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
        /// <returns>The object removed from the bottom of the <see cref="T:System.Collections.Generic.List`1" />.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.List`1" /> is empty.</exception>
        public virtual T Pop()
        {
            if (Values.Count <= IndexPointer)
            {
                Reset();
                throw new InvalidOperationException("The UniqueList is Empty");
            }

            T t = Values[IndexPointer];

            Values.RemoveAt(IndexPointer);

            return t;
        }

        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.List`1" />.
        /// </summary>
        /// <returns>true if <paramref name="item" /> is successfully removed; otherwise, false.  This method also returns false if <paramref name="item" /> was not found in the <see cref="T:System.Collections.Generic.List`1" />.</returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.List`1" />. The value can be null for reference types.</param>
        public virtual void Remove(T item)
        {
            Values.Remove(item);
        }

        /// <summary>Determines whether an element is in the <see cref="T:System.Collections.Generic.List`1" />.</summary>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.List`1" />; otherwise, false.</returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be null for reference types.</param>
        public virtual bool Contains(T item)
        {
            if (item == null) return false;

            bool found = false;

            Parallel.ForEach(Values, (T value, ParallelLoopState state) =>
                {
                    if (value.Equals(item))
                    { found = true; state.Break(); }
                });

            return found;
        }

        /// <summary>Removes all objects from the <see cref="T:System.Collections.Generic.List`1" />.</summary>
        /// <filterpriority>1</filterpriority>
        public virtual void Clear()
        {
            if (Values != null && Values.Count > 0)
                Values.Clear();

            Reset();
        }

        #endregion

        #region Implement ICloneable
        public object Clone()
        {
            UniqueList<T> Ulist = new UniqueList<T>();

            Ulist.Values.AddRange(this.Values.GetRange(0, Count));

            return Ulist;
        }
        #endregion

        #region Implement ICollection

        /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="arrayIndex" /> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }
            if (array.Rank != 1)
            {
                throw new ArgumentException("Argument Rank MultiDim Not Supported");
            }
            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("Argument Non Zero Lower Bound");
            }
            if (index < 0 || index > (int)array.Length)
            {
                throw new ArgumentOutOfRangeException("Index: " + index.ToString());
            }
            if ((int)array.Length - index < Count)
            {
                throw new ArgumentException("this list length - your index must be less than this list count");
            }
            try
            {
                Array.Copy(this.Values.ToArray(), 0, array, index, Count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Argument Invalid Array Type");
            }
        }

        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.List`1" />, this property always returns false.</returns>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>Copies the <see cref="T:System.Collections.Generic.List`1" /> to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.List`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="arrayIndex" /> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.List`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }
            if (arrayIndex < 0 || arrayIndex > (int)array.Length)
            {
                throw new ArgumentOutOfRangeException("Index: " + arrayIndex.ToString());
            }
            if ((int)array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("this list length - your index must be less than this list count");
            }
            Array.Copy(this.Values.ToArray(), 0, array, arrayIndex, Count);
        }

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.List`1" />, this property always returns false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region IEnumerator and IEnumerable require these methods.
        public IEnumerator<T> GetEnumerator()
        {
            return new UniqueListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new UniqueListEnumerator<T>(this);
        }

        /// <summary>
        /// Reset indexer point to index of the bottom of the <see cref="T:System.Collections.Generic.List`1" />.
        /// </summary>
        public void Reset()
        {
            IndexPointer = 0;
        }

        /// <summary>
        /// Move indexer to Next T object for Pop or Peek operates.
        /// </summary>
        public void Skip()
        { IndexPointer++; }

        public void Dispose()
        {
            this.Values.Clear();
            this.Reset();
        }


        // Declare an inner class that implements the IEnumerator interface. 
        private class UniqueListEnumerator<T> : IEnumerator, IEnumerator<T>
        {
            private int position = -1;
            private UniqueList<T> t;

            public UniqueListEnumerator(UniqueList<T> t)
            {
                this.t = t;
            }

            // The IEnumerator interface requires a MoveNext method. 
            public bool MoveNext()
            {
                if (position < t.Count - 1)
                {
                    position++;
                    return true;
                }

                return false;
            }

            // The IEnumerator interface requires a Reset method. 
            public void Reset()
            {
                position = -1;
            }

            // The IEnumerator interface requires a Current method. 
            public object Current
            {
                get
                {
                    return t.Values[position];
                }
            }

            T IEnumerator<T>.Current
            {
                get
                {
                    return t.Values[position];
                }
            }

            public void Dispose()
            {
                Reset();
            }
        }

        #endregion
    }

}