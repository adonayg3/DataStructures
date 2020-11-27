using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace DataStructures.DynamicArray
{
    public class DynamicArray<T> : IEnumerable
    {
        private T[] _arr;
        private int _len; // length user thinks array is
        private int _capacity; // Actual array size

        public DynamicArray()
        {
            _arr = new T[16];
        }

        public DynamicArray(int capacity)
        {
            if (capacity < 0) 
                throw new ArgumentException("Illegal Capacity: " + capacity);
            _capacity = capacity;
            _arr = new T[capacity];
        }

        public int Size()
        {
            return _len;
        }

        public bool IsEmpty()
        {
            return Size() == 0;
        }

        public T Get(int index)
        {
            return _arr[index];
        }

        public void Set(int index, T elem)
        {
            _arr[index] = elem;
        }

        public void Clear()
        {
            for (int i = 0; i < _len; i++)
                _arr[i] = default(T);

            _len = 0;
        }

        public void Add(T elem)
        {
            // Time to resize!
            if (_len + 1 >= _capacity)
            {
                if (_capacity == 0)
                    _capacity = 1;
                else
                    _capacity *= 2; // double the size

                T[] newArr = new T[_capacity];

                for (int i = 0; i < _len; i++)
                    newArr[i] = _arr[i];

                _arr = newArr; // arr has extra nulls(default) padded
            }

            _arr[_len++] = elem;
        }

        // Removes an element at the specified index in this array.
        public T RemoveAt(int rmIndex)
        {
            if(rmIndex >= _len || rmIndex < 0)
                throw new IndexOutOfRangeException();
            T data = _arr[rmIndex];
            T[] newArr = new T[_len - 1];
            
            for (int i = 0,j = 0; i < _len; i++, j++)
            {
                if (i == rmIndex)
                    j--; // Skip over rmIndex by fixing j temporarily
                else
                    newArr[j] = _arr[i];
            }

            _arr = newArr;
            _capacity = --_len;
            return data;
        }

        public bool Remove(T obj)
        {
            int index = IndexOf(obj);
            if (index == -1)
                return false;
            RemoveAt(index);
            return true;
        }

        public int IndexOf(T obj)
        {
            for (int i = 0; i < _len; i++)
            {
                if (obj == null)
                {
                    if(_arr[i] == null)
                        return i;
                }
                else
                {
                    if (obj.Equals(_arr[i]))
                        return i;
                }
            }
            return -1;
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator GetEnumerator()
        {
            return _arr.AsEnumerable().GetEnumerator();
        }

        public override string ToString()
        {
            if (_len == 0)
                return "[]";

            StringBuilder sb = new StringBuilder(_len).Append("[");
            for (int i = 0; i < _len - 1; i++)
            {
                sb.Append(_arr[i] + ", ");
            }
            return sb.Append(_arr[_len - 1] + "]").ToString();
        }
    }
}