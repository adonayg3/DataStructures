using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Stack
{
    public class Stack<T> : IEnumerable
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();

        // Create an empty stack
        public Stack() { }

        // Create a Stack with an initial element
        public Stack(T firstElem)
        {
            Push(firstElem);
        }
        
        // Return the number of elements in the stack
        public int Size()
        {
            return _list.Count;
        }
        
        // Check if the stack is empty
        public bool IsEmpty() 
        {
            return Size() == 0;
        }
        
        // Push an element on the stack
        public void Push(T elem)
        {
            _list.AddLast(elem);
        }
        
        // Pop an element off the stack
        // Throws an error is the stack is empty
        public T Pop() {
            if (IsEmpty()) 
                throw new InvalidOperationException("Stack is empty");
            if (_list.Last != null)
            {
                var lastValue = _list.Last.Value;
                _list.RemoveLast();
                return lastValue;
            }
            
            // If last node is null
            throw new InvalidOperationException();
        }

        // Peek the top of the stack without removing an element
        // Throws an exception if the stack is empty
        public T Peek()
        {
            if (IsEmpty()) 
                throw new InvalidOperationException("Stack is empty");
            if (_list.Last != null) 
                return _list.Last.Value;
            
            // If last node is null
            throw new InvalidOperationException();
        }

        // Allow users to iterate through the stack using an iterator
        public IEnumerator GetEnumerator()
        {
            return _list.AsEnumerable().GetEnumerator();
        }
    }
}