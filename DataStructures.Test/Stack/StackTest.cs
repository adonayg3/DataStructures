using System;
using DataStructures.Stack;
using Xunit;

namespace DataStructures.Test.Stack
{
    public class StackTest
    {
        private readonly Stack<int?> _stack;

        public StackTest()
        {
            _stack = new Stack<int?>();
        }
        
        [Fact]
        public void TestEmptyStack() 
        {
            Assert.True(_stack.IsEmpty());
            Assert.Equal(0, _stack.Size());
        }

        [Fact]
        public void TestPopOnEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => _stack.Pop());
        }

        [Fact]
        public void TestPeekOnEmpty() 
        {
            Assert.Throws<InvalidOperationException>(() => _stack.Peek());
        }

        [Fact]
        public void TestPush() 
        {
            _stack.Push(2);
            Assert.Equal(1, _stack.Size());
        }

        [Fact]
        public void TestPeek() 
        {
            _stack.Push(2);
            Assert.True(_stack.Peek() == 2);
            Assert.Equal(1, _stack.Size());
        }

        [Fact]
        public void TestPop() 
        {
            _stack.Push(2);
            Assert.True(_stack.Pop() == 2);
            Assert.Equal(0, _stack.Size());
        }

        [Fact]
        public void TestExhaustively() 
        {
            Assert.True(_stack.IsEmpty());
            _stack.Push(1);
            Assert.True(!_stack.IsEmpty());
            _stack.Push(2);
            Assert.Equal(2, _stack.Size());
            Assert.True(_stack.Peek() == 2);
            Assert.Equal(2, _stack.Size());
            Assert.True(_stack.Pop() == 2);
            Assert.Equal(1, _stack.Size());
            Assert.True(_stack.Peek() == 1);
            Assert.Equal(1, _stack.Size());
            Assert.True(_stack.Pop() == 1);
            Assert.Equal(0, _stack.Size());
            Assert.True(_stack.IsEmpty());
        }
    }
}