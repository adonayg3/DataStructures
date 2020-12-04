using System;
using DataStructures.Queue;
using Xunit;

namespace DataStructures.Test.Queue
{
    public class QueueTest
    {
        private readonly Queue<int?> _queue;

        public QueueTest()
        {
            _queue = new Queue<int?>();
        }

        [Fact]
        public void TestEmptyQueue() 
        {
            Assert.True(_queue.IsEmpty());
            Assert.Equal(0, _queue.Size());
        }
        
        [Fact]
        public void testPollOnEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => _queue.Poll());
        }

        [Fact]
        public void testPeekOnEmpty() 
        {
            Assert.Throws<InvalidOperationException>(() => _queue.Peek());
        }

        [Fact]
        public void TestOffer() 
        {
            _queue.Offer(2);
            Assert.Equal(1, _queue.Size());
        }

        [Fact]
        public void TestPeek() 
        {
            _queue.Offer(2);
            Assert.True(_queue.Peek() == 2);
            Assert.Equal(1, _queue.Size());
        }

        [Fact]
        public void TestPoll() 
        {
            _queue.Offer(2);
            Assert.True(_queue.Poll() == 2);
            Assert.Equal(0, _queue.Size());
        }

        [Fact]
        public void TestExhaustively() 
        {
            Assert.True(_queue.IsEmpty());
            _queue.Offer(1);
            Assert.True(!_queue.IsEmpty());
            _queue.Offer(2);
            Assert.Equal(2, _queue.Size());
            Assert.True(_queue.Peek() == 1);
            Assert.Equal(2, _queue.Size());
            Assert.True(_queue.Poll() == 1);
            Assert.Equal(1, _queue.Size());
            Assert.True(_queue.Peek() == 2);
            Assert.Equal(1, _queue.Size());
            Assert.True(_queue.Poll() == 2);
            Assert.Equal(0, _queue.Size());
            Assert.True(_queue.IsEmpty());
        }
    }
}