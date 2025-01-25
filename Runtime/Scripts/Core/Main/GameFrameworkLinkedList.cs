using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace OSK
{
    public sealed class GameFrameworkLinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection
    {
        private readonly LinkedList<T> m_LinkedList = new();
        private readonly Queue<LinkedListNode<T>> m_CachedNodes = new();

        public int Count => this.m_LinkedList.Count;
        public int CachedNodeCount => this.m_CachedNodes.Count;
        public LinkedListNode<T> First => this.m_LinkedList.First;
        public LinkedListNode<T> Last => this.m_LinkedList.Last;

        public bool IsReadOnly => ((ICollection<T>)this.m_LinkedList).IsReadOnly;
        public object SyncRoot => ((ICollection)this.m_LinkedList).SyncRoot;
        public bool IsSynchronized => ((ICollection)this.m_LinkedList).IsSynchronized;

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = this.AcquireNode(value);
            this.m_LinkedList.AddAfter(node, newNode);
            return newNode;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            this.m_LinkedList.AddAfter(node, newNode);
        }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = this.AcquireNode(value);
            this.m_LinkedList.AddBefore(node, newNode);
            return newNode;
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            this.m_LinkedList.AddBefore(node, newNode);
        }

        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> node = this.AcquireNode(value);
            this.m_LinkedList.AddFirst(node);
            return node;
        }

        public void AddFirst(LinkedListNode<T> node) => this.m_LinkedList.AddFirst(node);

        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> node = this.AcquireNode(value);
            this.m_LinkedList.AddLast(node);
            return node;
        }

        public void AddLast(LinkedListNode<T> node) => this.m_LinkedList.AddLast(node);

        public void Clear()
        {
            for (LinkedListNode<T> node = this.m_LinkedList.First; node != null; node = node.Next)
                this.ReleaseNode(node);
            this.m_LinkedList.Clear();
        }

        public void ClearCachedNodes() => this.m_CachedNodes.Clear();
        public bool Contains(T value) => this.m_LinkedList.Contains(value);
        public void CopyTo(T[] array, int index) => this.m_LinkedList.CopyTo(array, index);

        public void CopyTo(Array array, int index)
        {
            ((ICollection)this.m_LinkedList).CopyTo(array, index);
        }

        public LinkedListNode<T> Find(T value) => this.m_LinkedList.Find(value);
        public LinkedListNode<T> FindLast(T value) => this.m_LinkedList.FindLast(value);

        public bool Remove(T value)
        {
            LinkedListNode<T> node = this.m_LinkedList.Find(value);
            if (node == null)
                return false;
            this.m_LinkedList.Remove(node);
            this.ReleaseNode(node);
            return true;
        }

        public void Remove(LinkedListNode<T> node)
        {
            this.m_LinkedList.Remove(node);
            this.ReleaseNode(node);
        }

        public void RemoveFirst()
        {
            LinkedListNode<T> first = this.m_LinkedList.First;
            if (first == null)
                throw new Exception("First is invalid.");
            this.m_LinkedList.RemoveFirst();
            this.ReleaseNode(first);
        }

        public void RemoveLast()
        {
            LinkedListNode<T> last = this.m_LinkedList.Last;
            if (last == null)
                throw new Exception("Last is invalid.");
            this.m_LinkedList.RemoveLast();
            this.ReleaseNode(last);
        }

        public GameFrameworkLinkedList<T>.Enumerator GetEnumerator()
        {
            return new GameFrameworkLinkedList<T>.Enumerator(this.m_LinkedList);
        }

        private LinkedListNode<T> AcquireNode(T value)
        {
            LinkedListNode<T> linkedListNode;
            if (this.m_CachedNodes.Count > 0)
            {
                linkedListNode = this.m_CachedNodes.Dequeue();
                linkedListNode.Value = value;
            }
            else
                linkedListNode = new LinkedListNode<T>(value);

            return linkedListNode;
        }

        private void ReleaseNode(LinkedListNode<T> node)
        {
            node.Value = default(T);
            this.m_CachedNodes.Enqueue(node);
        }

        void ICollection<T>.Add(T value) => this.AddLast(value);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>)this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)this.GetEnumerator();

        [StructLayout(LayoutKind.Auto)]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private LinkedList<T>.Enumerator m_Enumerator;

            internal Enumerator(LinkedList<T> linkedList)
            {
                this.m_Enumerator = linkedList != null ? linkedList.GetEnumerator()  : throw new Exception("Linked list is invalid.");
            }

            public T Current => this.m_Enumerator.Current;

            object IEnumerator.Current => (object)this.m_Enumerator.Current;

            public void Dispose() => this.m_Enumerator.Dispose();

            public bool MoveNext() => this.m_Enumerator.MoveNext();

            void IEnumerator.Reset() => ((IEnumerator)this.m_Enumerator).Reset();
        }
    }
}