using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace Assets.Scripts
{
    
    public class SafeQueue<T>
    {

        Queue<T> queue;

        public SafeQueue()
        {
            queue = new Queue<T>();
            
        }

        public bool Enqueue(T item)
        {
            bool result = false;

            lock (queue)
            {

                queue.Enqueue(item);

            }

            return result;
        }

        public T Dequeue()
        {

            T result = default(T);

            lock (queue)
            {

                if(queue.Count > 0)
                {
                    result = queue.Dequeue();
                }
                

            }

            return result;
        }

        public bool isEmpty()
        {

            bool result = true;

            lock (queue)
            {

                result = (queue.Count == 0);

            }

            return result;

        }
    }
}
