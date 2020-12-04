using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Queue
{
    public class Queue<T> : IEnumerable
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();

        // Create an empty queue
        public Queue() { }
        
        // Create a queue with an initial element
        public Queue(T firstElem)
        {
            Offer(firstElem);
        }
        
        // Return the size of the queue
        public int Size() 
        {
            return _list.Count;
        }

        // Returns whether or not the queue is empty
        public bool IsEmpty() 
        {
            return Size() == 0;
        }

        // Peek the element at the front of the queue
        // The method throws an error is the queue is empty
        public T Peek()
        {
            if (IsEmpty()) 
                throw new InvalidOperationException("Queue is empty");
            
            if (_list.First != null) 
                return _list.First.Value;
            
            // If first node is null
            throw new InvalidOperationException();
        }

        // Poll an element from the front of the queue
        // The method throws an error is the queue is empty
        public T Poll() 
        {
            if (IsEmpty()) 
                throw new InvalidOperationException("Queue is empty");
            
            if (_list.First != null)
            {
                var firstValue = _list.First.Value;
                _list.RemoveFirst();
                return firstValue;
            }
            
            // If first node is null
            throw new InvalidOperationException();
        }

        // Add an element to the back of the queue
        public void Offer(T elem) 
        {
            _list.AddLast(elem);
        }
        
        // Allow users to iterate through the stack using an iterator
        public IEnumerator GetEnumerator()
        {
            return _list.AsEnumerable().GetEnumerator();
        }
    }
}