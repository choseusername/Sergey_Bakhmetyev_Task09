using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Task2
{
    public class DynamicArray<T>: IEnumerable<T>
    {
        static readonly T[] _emptyArray = new T[0];

        private const int _defaultCapacity = 8;
        private T[] _items;
        private int _size;

        public int Length => _size;
        public int Capacity
        {
            get => _items.Length;
            set
            {
                if (value < _size)
                    throw new ArgumentOutOfRangeException("Small capacity.");

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                            Array.Copy(_items, 0, newItems, 0, _size);
                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        public T this[int index]
        {
            get
            {
                if (index >= _size)
                    throw new ArgumentOutOfRangeException();
                return _items[index];
            }

            set
            {
                if (index >= _size)
                    throw new ArgumentOutOfRangeException();
                _items[index] = value;
            }
        }

        public DynamicArray()
        {
            _items = new T[_defaultCapacity];
        }

        public DynamicArray(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("Need non neg numbers capacity.");

            if (capacity == 0)
                _items = _emptyArray;
            else
                _items = new T[capacity];
        }

        public DynamicArray(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            _items = collection.ToArray();
            _size = collection.Count();
        }

        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                const int MaxArrayLength = 0X7FEFFFFF;

                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if (newCapacity > MaxArrayLength) newCapacity = MaxArrayLength;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }

        public void Add(T item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size++] = item;
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            if (index > _size)
                throw new ArgumentOutOfRangeException();

            ICollection<T> c = collection as ICollection<T>;
            if (c != null)
            {
                int count = c.Count;
                if (count > 0)
                {
                    EnsureCapacity(_size + count);
                    if (index < _size)
                        Array.Copy(_items, index, _items, index + count, _size - index);

                    if (this == c)
                    {
                        // Copy first part of _items to insert location
                        Array.Copy(_items, 0, _items, index, index);
                        // Copy last part of _items back to inserted location
                        Array.Copy(_items, index + count, _items, index * 2, _size - index);
                    }
                    else
                    {
                        T[] itemsToInsert = new T[count];
                        c.CopyTo(itemsToInsert, 0);
                        itemsToInsert.CopyTo(_items, index);
                    }
                    _size += count;
                }
            }
            else
            {
                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                        Insert(index++, en.Current);
                }
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            InsertRange(_size, collection);
        }


        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, _size);
        }

        public void RemoveAt(int index)
        {
            if (index >= _size)
                throw new ArgumentOutOfRangeException();
            _size--;
            if (index < _size)
                Array.Copy(_items, index + 1, _items, index, _size - index);
            _items[_size] = default(T);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void Insert(int index, T item)
        {
            if (index > _size)
                throw new ArgumentOutOfRangeException();

            if (_size == _items.Length) EnsureCapacity(_size + 1);

            if (index < _size)
                Array.Copy(_items, index, _items, index + 1, _size - index);

            _items[index] = item;
            _size++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
        {
            private DynamicArray<T> list;
            private int index;
            private T current;

            internal Enumerator(DynamicArray<T> list)
            {
                this.list = list;
                index = 0;
                current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {

                DynamicArray<T> localList = list;

                if (index < localList._size)
                {
                    current = localList._items[index];
                    index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                index = list._size + 1;
                current = default(T);
                return false;
            }

            public T Current
            {
                get
                {
                    return current;
                }
            }

            Object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (index == 0 || index == list._size + 1)
                        throw new InvalidOperationException("EnumOpCantHappen");
                    return Current;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                index = 0;
                current = default(T);
            }

        }
    }
}
