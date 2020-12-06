using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriorityQueue
{
    public class PriorityQueue <T> where T : IComparable<T>
    {
        // The number of elements currently inside the heap
        private int _heapSize;
        
        // The internal capacity of the heap
        private int _heapCapacity;
        
        // A dynamic list to track the elements inside the heap
        private readonly List<T> _heap;
        
        // This map keeps track of the possible indices a particular
        // node value is found in the heap. Having this mapping lets
        // us have O(log(n)) removals and O(1) element containment check
        // at the cost of some additional space and minor overhead
        private readonly Dictionary<T, SortedSet<int>> _map = new Dictionary<T, SortedSet<int>>();

        public PriorityQueue() : this(0) { }

        public PriorityQueue(int sz)
        {
            _heap = new List<T>(sz);
        }
        
        // Construct a priority queue using heapify in O(n) time, a great explanation can be found at:
        // http://www.cs.umd.edu/~meesh/351/mount/lectures/lect14-heapsort-analysis-part.pdf
        public PriorityQueue(IList<T> elems)
        {
            _heapSize = _heapCapacity = elems.Count;
            _heap = new List<T>(_heapCapacity);
            
            // Place all elements in heap
            for (var i = 0; i < _heapSize; i++)
            {
                MapAdd(elems[i], i);
                _heap.Add(elems[i]);
            }
            
            // Heapify process O(n)
            for (var i = Math.Max(0, (_heapSize/2)); i >= 0; i--)
            {
                Sink(i);
            }
        }

        // Priority queue construction, O(nlog(n))
        public PriorityQueue(ICollection<T> elems) : this(elems.Count)
        {
            foreach (var elem in elems)
                Add(elem);
        }
        
        // Returns true/false depending on if the priority queue is empty
        public bool IsEmpty() 
        {
            return _heapSize == 0;
        }
        
        // Clears everything inside the heap, O(n)
        public void Clear() 
        {
            for (var i = 0; i < _heapCapacity; i++) 
                _heap.Insert(i, default);
            _heapSize = 0;
            _map.Clear();
        }

        // Return the size of the heap
        public int Size() 
        {
            return _heapSize;
        }
        
        // Returns the value of the element with the lowest
        // priority in this priority queue. If the priority
        // queue is empty null is returned.
        public T Peek() 
        {
            if (IsEmpty()) 
                return default;
            return _heap[0];
        }

        // Removes the root of the heap, O(log(n))
        public T Poll() 
        {
            return RemoveAt(0);
        }

        // Test if an element is in heap, O(1)
        public bool Contains(T elem)
        {
            // Map lookup to check containment, O(1)
            return elem != null && _map.ContainsKey(elem);
        }

        // Adds an element to the priority queue, the
        // element must not be null, O(log(n))
        private void Add(T elem)
        {
            if (elem == null) 
                throw new ArgumentException();

            if (_heapSize < _heapCapacity) 
            {
                _heap.Insert(_heapSize, elem);
            } 
            else 
            {
                _heap.Add(elem);
                _heapCapacity++;
            }

            MapAdd(elem, _heapSize);

            Swim(_heapSize);
            _heapSize++;
        }
        
        // Tests if the value of node i <= node j
        // This method assumes i & j are valid indices, O(1)
        private bool Less(int i, int j) 
        {
            var node1 = _heap[i];
            var node2 = _heap[j];
            return node1.CompareTo(node2) <= 0;
        }
        // Perform bottom up node swim, O(log(n))
        private void Swim(int k)
        {
            // Grab the index of the next parent node WRT to k
            var parent = (k - 1) / 2;

            // Keep swimming while we have not reached the
            // root and while we're less than our parent.
            while (k > 0 && Less(k, parent))
            {

                // Exchange k with the parent
                Swap(parent, k);
                k = parent;

                // Grab the index of the next parent node WRT to k
                parent = (k - 1) / 2;
            }
        }

        // Top down node sink, O(log(n))
        private void Sink(int k)
        {
            while (true) {

                var left = 2 * k + 1; // Left  node
                var right = 2 * k + 2; // Right node
                var smallest = left; // Assume left is the smallest node of the two children

                // Find which is smaller left or right
                // If right is smaller set smallest to be right
                if (right < _heapSize && Less(right, left)) 
                    smallest = right;

                // Stop if we're outside the bounds of the tree
                // or stop early if we cannot sink k anymore
                if (left >= _heapSize || Less(k, smallest)) 
                    break;

                // Move down the tree following the smallest node
                Swap(smallest, k);
                k = smallest;
            }
        }
        
        // Swap two nodes. Assumes i & j are valid, O(1)
        private void Swap(int i, int j)
        {
            var iElem = _heap[i];
            var jElem = _heap[j];

            _heap.Insert(i, jElem);
            _heap.Insert(j, iElem);

            MapSwap(iElem, jElem, i, j);
        }
    
        // Removes a particular element in the heap, O(log(n))
        public bool Remove(T element) 
        {
            if (element == null) return false;

            // Logarithmic removal with map, O(log(n))
            var index = MapGet(element);
            if (index != null) 
                RemoveAt((int) index);
            return index != null;
        }

        private T RemoveAt(int i)
        {
            if (IsEmpty()) 
                return default;

            _heapSize--;

            var removedData = _heap[i];
            Swap(i, _heapSize);

            // Obliterate the value
            _heap.Insert(_heapSize, default);
            MapRemove(removedData, _heapSize);

            // Removed last element
            if (i == _heapSize) 
                return removedData;

            var elem = _heap[i];

            // Try sinking element
            Sink(i);

            // If sinking did not work try swimming
            if (_heap[i].Equals(elem)) 
                Swim(i);

            return removedData;
        }
        
        // Recursively checks if this heap is a min heap
        // This method is just for testing purposes to make
        // sure the heap invariant is still being maintained
        // Called this method with k=0 to start at the root
        public bool IsMinHeap(int k) 
        {

            // If we are outside the bounds of the heap return true
            if (k >= _heapSize) return true;

            var left = 2 * k + 1;
            var right = 2 * k + 2;

            // Make sure that the current node k is less than
            // both of its children left, and right if they exist
            // return false otherwise to indicate an invalid heap
            if (left < _heapSize && !Less(k, left)) 
                return false;
            if (right < _heapSize && !Less(k, right)) 
                return false;

            // Recurse on both children to make sure they're also valid heaps
            return IsMinHeap(left) && IsMinHeap(right);
        }

        // Add a node value and its index to the map
        private void MapAdd(T value, int index)
        {
            _map.TryGetValue(value,out var set);
            
            // New value being inserted in map
            if (set == null) 
            {
                set = new SortedSet<int> {index};
                _map.Add(value, set);
            } 
            // Value already exists in map
            else 
                set.Add(index);
        }
        
        // Removes the index at a given value, O(log(n))
        private void MapRemove(T value, int index)
        {
            var set = _map[value];
            
            // TreeSets take O(log(n)) removal time
            set.Remove(index); 
            
            if (set.Count == 0) 
                _map.Remove(value);
        }
        
        // Extract an index position for the given value
        // NOTE: If a value exists multiple times in the heap the highest
        // index is returned (this has arbitrarily been chosen)
        private int? MapGet(T value) {
            var set = _map[value];
            return set?.Last();
        }
        
        // Exchange the index of two nodes internally within the map
        private void MapSwap(T val1, T val2, int val1Index, int val2Index)
        {
            var set1 = _map[val1];
            var set2 = _map[val2];

            set1.Remove(val1Index);
            set2.Remove(val2Index);

            set1.Add(val2Index);
            set2.Add(val1Index);
        }

        public override string ToString()
        {
            return _heap.ToString();
        }
    }
}