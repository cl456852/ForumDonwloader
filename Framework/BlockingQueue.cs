using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Framework
{

    public class BlockingQueue<T> : ConcurrentQueue<T>
    {
        private Semaphore semaphore;

        public BlockingQueue()
        {
            semaphore = new Semaphore(0, Int32.MaxValue);//消息不知道有多少，设个足够大的吧，初始=0
        }

        public T Dequeue()
        {
            T t = default(T);//创建一个类型T的对象
            semaphore.WaitOne();//请求信号量，信号量=0，就会阻塞了
            base.TryDequeue(out t);//然后出队，继承于同步队列，因此不用加锁了
            return t;
        }

        public new void Enqueue(T t)
        {
            base.Enqueue(t);//出队
            semaphore.Release();//释放信号量
        }

        public T Peek()
        {
            T t = default(T);//创建一个类型T的对象
            semaphore.WaitOne();//请求信号量，信号量=0，就会阻塞了
            base.TryPeek(out t);//然后出队，继承于同步队列，因此不用加锁了
            semaphore.Release();
            return t;
        }
    }
}
