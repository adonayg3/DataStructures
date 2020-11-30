using System;
using System.Collections.Generic;

namespace DataStructures.LinkedList
{
    public static class ExtensionMethods
    {
        public static LinkedListNode<T> RemoveAt<T>(this LinkedList<T> list, int index)
        {
            var currentNode = list.First;
            for (var i = 0; i <= index && currentNode != null; i++)
            {
                if (i != index)
                {
                    currentNode = currentNode.Next;
                    continue;
                }

                list.Remove(currentNode);
                return currentNode;
            }
            throw new IndexOutOfRangeException();
        }
    }
}