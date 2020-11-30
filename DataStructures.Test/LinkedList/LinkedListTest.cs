using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.LinkedList;
using Xunit;

namespace DataStructures.Test.LinkedList
{ 
  public class LinkedListTest
    { 
      private const int Loops = 10000;
      private const int TestSz = 40;
      private const int NumNulls = TestSz / 5;
      private const int MaxRandNum = 250;

      private readonly DoublyLinkedList<int?> _list;
        
      // Random Number Generator Mimic Math.random() in java.
      private static readonly Random Rng = new Random();
      
      public LinkedListTest()
      {
          _list = new DoublyLinkedList<int?>();
      }
      
      [Fact]
      public void TestEmptyList() 
      {
          Assert.True(_list.IsEmpty());
          Assert.Equal(0, _list.Size());
      }

      [Fact]
      public void TestRemoveFirstOfEmpty() 
      {
          Assert.Throws<Exception>(
              () => _list.RemoveFirst()
              );
      }

      [Fact] 
      public void TestRemoveLastOfEmpty() 
      {
          Assert.Throws<Exception>(
              () => _list.RemoveLast()
          );
      }

      [Fact]
      public void TestPeekFirstOfEmpty()
      {
          Assert.Throws<Exception>(
              () => _list.PeekFirst()
          );
      }        
        
      [Fact]
      public void TestPeekLastOfEmpty()
      {
          Assert.Throws<Exception>(
              () => _list.PeekLast()
          );
      }
        
      [Fact]
      public void TestPeeking() 
      {
        // 5
        _list.AddFirst(5);
        Assert.True(_list.PeekFirst() == 5);
        Assert.True(_list.PeekLast() == 5);

        // 6 - 5
        _list.AddFirst(6);
        Assert.True(_list.PeekFirst() == 6);
        Assert.True(_list.PeekLast() == 5);

        // 7 - 6 - 5
        _list.AddFirst(7);
        Assert.True(_list.PeekFirst() == 7);
        Assert.True(_list.PeekLast() == 5);

        // 7 - 6 - 5 - 8
        _list.AddLast(8);
        Assert.True(_list.PeekFirst() == 7);
        Assert.True(_list.PeekLast() == 8);

        // 7 - 6 - 5
        _list.RemoveLast();
        Assert.True(_list.PeekFirst() == 7);
        Assert.True(_list.PeekLast() == 5);

        // 7 - 6
        _list.RemoveLast();
        Assert.True(_list.PeekFirst() == 7);
        Assert.True(_list.PeekLast() == 6);

        // 6
        _list.RemoveFirst();
        Assert.True(_list.PeekFirst() == 6);
        Assert.True(_list.PeekLast() == 6);
      }

      [Fact]
      public void TestRemoving() 
      {
        var strings = new DoublyLinkedList<string> {"a", "b", "c", "d", "e", "f"};
        
        strings.Remove("b");
        strings.Remove("a");
        strings.Remove("d");
        strings.Remove("e");
        strings.Remove("c");
        strings.Remove("f");
        
        Assert.Equal(0, strings.Size());
      }

      [Fact]
      public void TestRemoveAt()
      {
        _list.Add(1);
        _list.Add(2);
        _list.Add(3);
        _list.Add(4);
        _list.RemoveAt(0);
        _list.RemoveAt(2);
        Assert.True(_list.PeekFirst() == 2);
        Assert.True(_list.PeekLast() == 3);
        _list.RemoveAt(1);
        _list.RemoveAt(0);
        Assert.Equal(0, _list.Size());
      }

      [Fact]
      public void TestClear() 
      {
        _list.Add(22);
        _list.Add(33);
        _list.Add(44);
        Assert.Equal(3, _list.Size());
        _list.Clear();
        Assert.Equal(0, _list.Size());
        _list.Add(22);
        _list.Add(33);
        _list.Add(44);
        Assert.Equal(3, _list.Size());
        _list.Clear();
        Assert.Equal(0, _list.Size());
      }

      [Fact]
      public void TestRandomizedRemoving() 
      {
        var inBuiltLinkedList = new LinkedList<int?>();
        for (var loops = 0; loops < Loops; loops++) {
          _list.Clear();
          inBuiltLinkedList.Clear();
      
          var randNums = GenRandList(TestSz);
          foreach (var value in randNums)
          {
            inBuiltLinkedList.AddLast(value);
            _list.Add(value);
          }
          
          Shuffle(randNums);
      
          for (var i = 0; i < randNums.Count; i++) {
            var rmVal = randNums.ElementAt(i);
            Assert.Equal(inBuiltLinkedList.Remove(rmVal), _list.Remove(rmVal));
            Assert.Equal(inBuiltLinkedList.Count, _list.Size());

            using var iter1 = inBuiltLinkedList.GetEnumerator();
            using var iter2 = _list.GetEnumerator();
            while (iter1.MoveNext() && iter2.MoveNext()) 
              Assert.Equal(iter1.Current, iter2.Current);
          }
      
          _list.Clear();
          inBuiltLinkedList.Clear();
      
          foreach (var value in randNums)
          {
            inBuiltLinkedList.AddLast(value);
            _list.Add(value);
          }
      
          // Try removing elements whether or not they exist
          for (var i = 0; i < randNums.Count; i++) {
      
            var rmVal = (int) (MaxRandNum * Rng.NextDouble());
            Assert.Equal(inBuiltLinkedList.Remove(rmVal), _list.Remove(rmVal));
            Assert.Equal(inBuiltLinkedList.Count, _list.Size());
      
            using var iter1 = inBuiltLinkedList.GetEnumerator();
            using var iter2 = _list.GetEnumerator();
            while (iter1.MoveNext() && iter2.MoveNext()) 
              Assert.Equal(iter1.Current, iter2.Current);
          }
        }
      }
        
      [Fact]
      public void TestRandomizedRemoveAt() 
      {
        var inBuiltLinkedList = new LinkedList<int?>();
      
        for (var loops = 0; loops < Loops; loops++) {
          inBuiltLinkedList.Clear();
          _list.Clear();
          var randNums = GenUniqueRandList(TestSz);
      
          foreach (var value in randNums)
          {
            inBuiltLinkedList.AddLast(value);
            _list.Add(value);
          }
      
          for (var i = 0; i < randNums.Count; i++) {
            var rmIndex = (int) (_list.Size() * Rng.NextDouble());
            // Linked list doesn't have RemoveAt implementation,
            // but has been added using Extension Method in linkedList folder
            var num1 = inBuiltLinkedList.RemoveAt(rmIndex);
            var num2 = _list.RemoveAt(rmIndex);
            Assert.Equal(num1.Value, num2);
            Assert.Equal(inBuiltLinkedList.Count, _list.Size());
      
            using var iter1 = inBuiltLinkedList.GetEnumerator();
            using var iter2 = _list.GetEnumerator();
            while (iter1.MoveNext() && iter2.MoveNext()) 
              Assert.Equal(iter1.Current, iter2.Current);
          }
        }
      }
      
      [Fact]
      public void TestRandomizedIndexOf() 
      {
        var inBuiltLinkedList = new LinkedList<int?>();
      
        for (var loops = 0; loops < Loops; loops++) {
          inBuiltLinkedList.Clear();
          _list.Clear();
          var randNums = GenUniqueRandList(TestSz);

          foreach (var value in randNums)
          {
            inBuiltLinkedList.AddLast(value);
            _list.Add(value);
          }
          
          Shuffle(randNums);
      
          for (var i = 0; i < randNums.Count; i++) {
      
            var elem = randNums.ElementAt(i);
            
            // In built Linked list doesn't have IndexOf implementation
            int? index1 = inBuiltLinkedList.Select((n, x) => n == elem ? (int?)x : null).
              FirstOrDefault(n => n != null) ?? -1;
            int? index2 = _list.IndexOf(elem);
      
            Assert.Equal(index1, index2);
            Assert.Equal(inBuiltLinkedList.Count, _list.Size());

            using var iter1 = inBuiltLinkedList.GetEnumerator();
            using var iter2 = _list.GetEnumerator();
            while (iter1.MoveNext() && iter2.MoveNext()) 
              Assert.Equal(iter1.Current, iter2.Current);
          }
        }
      }
      
      // Generate a list of random numbers
      private static List<int?> GenRandList(int sz) 
      {
        var lst = new List<int?>(sz);
        for (var i = 0; i < sz; i++) 
          lst.Add((int) (Rng.NextDouble() * MaxRandNum));
        for (var i = 0; i < NumNulls; i++) 
          lst.Add(null);
        Shuffle(lst);
        return lst;
      }
      
      // Generate a list of unique random numbers
      private static List<int?> GenUniqueRandList(int sz) 
      {
        var lst = new List<int?>(sz);
        for (var i = 0; i < sz; i++) 
          lst.Add(i);
        for (var i = 0; i < NumNulls; i++) 
          lst.Add(null);
        Shuffle(lst);
        return lst;
      }
      
      // Mimicking Collections.Shuffle in java.
      private static void Shuffle<T>(IList<T> list)  
      {  
        var n = list.Count;  
        while (n > 1) {  
          n--;  
          var k = Rng.Next(n + 1);  
          var value = list[k];  
          list[k] = list[n];  
          list[n] = value;  
        }  
      }
    }
}