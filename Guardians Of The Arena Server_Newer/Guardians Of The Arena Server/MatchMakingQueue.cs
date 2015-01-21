using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server
{
    public class MatchMakingQueue<T>
    {
        private LinkedList<T> list;

        #region  Property Regions
        public int Count
        {
            get { return list.Count; }
        }
        #endregion

        public MatchMakingQueue()
        {
            list = new LinkedList<T>();
        }

        public void enqueue(T t)
        {
            list.AddLast(t);
        }

        public T dequeue()
        {
            T t = list.First.Value;
            list.RemoveFirst();
            return t;
        }

        public T peak()
        {
            return list.First.Value;
        }

        public void removeFromQueue(T t)
        {
            list.Remove(t);
        }
    }
}
