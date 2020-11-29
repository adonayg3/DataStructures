using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.LinkedList
{
    public class DoublyLinkedList<T>: IEnumerable
    {
        private int _size;
        private Node<T> _head;
        private Node<T> _tail;

        // Internal node class to represent data
        private class Node<T>
        {
            private T _data;
            private Node<T> _prev, _next;
            
            public Node(T data, Node<T> prev, Node<T> next)
            {
                _data = data;
                _prev = prev;
                _next = next;
            }
            public T Data
            {
                get => _data;
                set => _data = value;
            }
            
            public Node<T> Prev
            {
                get => _prev;
                set => _prev = value;
            }
            
            public Node<T> Next
            {
                get => _next;
                set => _next = value;
            }

            public override string ToString()
            {
                return _data.ToString();
            }
        }
        
        // Empty this linked list, O(n)
        public void Clear()
        {
            var trav = _head;
            while (trav != null)
            {
                Node<T> next = trav.Next;
                trav.Prev = trav.Next = null;
                trav.Data = default(T);
                trav = next;
            }
            _head = _tail = null;
            _size = 0;
        }
        
        // Return the size of this linked list
        public int Size() 
        {
            return _size;
        }
        
        // Is this linked list empty?
        public bool IsEmpty() 
        {
            return Size() == 0;
        }
        
        // Add an element to the tail of the linked list, O(1)
        public void Add(T elem) 
        {
            AddLast(elem);
        }
        
        // Add a node to the tail of the linked list, O(1)
        public void AddLast(T elem) 
        {
            if (IsEmpty()) {
                _head = _tail = new Node<T>(elem, null, null);
            } else {
                _tail.Next = new Node<T>(elem, _tail, null);
                _tail = _tail.Next;
            }
            _size++;
        }
        
        // Add an element to the beginning of this linked list, O(1)
        public void AddFirst(T elem) 
        {
            if (IsEmpty()) 
            {
                _head = _tail = new Node<T>(elem, null, null);
            } 
            else 
            {
                _head.Prev = new Node<T>(elem, null, _head);
                _head = _head.Prev;
            }
            _size++;
        }
        // Check the value of the first node if it exists, O(1)
        public T PeekFirst() 
        {
            if (IsEmpty()) 
                throw new Exception("Empty list");
            return _head.Data;
        }
        
        // Check the value of the last node if it exists, O(1)
        public T PeekLast() 
        {
            if (IsEmpty()) 
                throw new Exception("Empty list");
            return _tail.Data;
        }
        
        // Remove the first value at the head of the linked list, O(1)
        public T RemoveFirst() 
        {
            // Can't remove data from an empty list
            if (IsEmpty()) 
                throw new Exception("Empty list");

            // Extract the data at the head and move
            // the head pointer forwards one node
            T data = _head.Data;
            _head = _head.Next;
            --_size;

            // If the list is empty set the tail to null
            if (IsEmpty()) 
                _tail = null;
            // Do a memory cleanup of the previous node
            else 
                _head.Prev = null;

            // Return the data that was at the first node we just removed
            return data;
        }
        
        // Remove the last value at the tail of the linked list, O(1)
        public T RemoveLast() 
        {
            // Can't remove data from an empty list
            if (IsEmpty()) 
                throw new Exception("Empty list");

            // Extract the data at the tail and move
            // the tail pointer backwards one node
            T data = _tail.Data;
            _tail = _tail.Prev;
            --_size;

            // If the list is now empty set the head to null
            if (IsEmpty()) 
                _head = null;
            // Do a memory clean of the node that was just removed
            else 
                _tail.Next = null;

            // Return the data that was in the last node we just removed
            return data;
        }
        
        // Remove an arbitrary node from the linked list, O(1)
        private T Remove(Node<T> node) 
        {
            // If the node to remove is somewhere either at the
            // head or the tail handle those independently
            if (node.Prev == null) 
                return RemoveFirst();
            if (node.Next == null) 
                return RemoveLast();

            // Make the pointers of adjacent nodes skip over 'node'
            node.Next.Prev = node.Prev;
            node.Prev.Next = node.Next;

            // Temporarily store the data we want to return
            T data = node.Data;

            // Memory cleanup
            node.Data = default;
            node.Prev = node.Next = null;

            --_size;

            // Return the data in the node we just removed
            return data;
        }
        
        // Remove a node at a particular index, O(n)
        public T RemoveAt(int index) 
        {
            // Make sure the index provided is valid
            if (index < 0 || index >= _size) {
                throw new ArgumentException();
            }

            int i;
            Node<T> trav;

            // Search from the front of the list
            if (index < _size / 2) {
                for (i = 0, trav = _head; i != index; i++) {
                    trav = trav.Next;
                }
            }
            // Search from the back of the list
            else
            {
                for (i = _size - 1, trav = _tail; i != index; i--) {
                    trav = trav.Prev;
                }
            }
            
            return Remove(trav);
        }
        
        
        // Remove a particular value in the linked list, O(n)
        public bool Remove(T obj) 
        {
            Node<T> trav;
            
            // Support searching for null
            if (obj == null) {
                for (trav = _head; trav != null; trav = trav.Next) {
                    if (trav.Data == null) {
                        Remove(trav);
                        return true;
                    }
                }
               
            } 
            // Search for non null object
            else 
            {
                for (trav = _head; trav != null; trav = trav.Next) {
                    if (obj.Equals(trav.Data)) {
                        Remove(trav);
                        return true;
                    }
                }
            }
            return false;
        }
        
        // Find the index of a particular value in the linked list, O(n)
        public int IndexOf(T obj) 
        {
            int index = 0;
            Node<T> trav = _head;

            // Support searching for null
            if (obj == null) {
                for (; trav != null; trav = trav.Next, index++) {
                    if (trav.Data == null) {
                        return index;
                    }
                }
            }
            // Search for non null object
            else
            {
                for (; trav != null; trav = trav.Next, index++) {
                    if (obj.Equals(trav.Data)) {
                        return index;
                    }
                }
            }

            return -1;
        }
        
        // Check is a value is contained within the linked list
        public bool Contains(T obj) {
            return IndexOf(obj) != -1;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> trav = _head;
            while (trav != null)
            {
                yield return trav.Data;
                trav = trav.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            Node<T> trav = _head;
            while (trav != null) {
                sb.Append(trav.Data + ", ");
                trav = trav.Next;
            }
            sb.Append(" ]");
            return sb.ToString();
        }
    }
}