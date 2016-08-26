using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace NigelGott.Terra.NetworkState
{
    internal class SingleThreadedTaskRunner : IDisposable
    {
        private readonly ConcurrentQueue<Action> actions;
        private readonly object newActionLock;

        private volatile bool continueRunning;

        public SingleThreadedTaskRunner()
        {
            actions = new ConcurrentQueue<Action>();
            newActionLock = new object();
            continueRunning = true;
            Task.Factory.StartNew(() =>
            {
                while (continueRunning)
                {
                    lock (newActionLock)
                    {
                        Monitor.Wait(newActionLock);
                    }
                    Action nextAction;
                    while (actions.TryDequeue(out nextAction))
                    {
                        nextAction.Invoke();
                    }
                }
                
            });
        }

        public void Submit(Action action)
        {
            actions.Enqueue(action);
            lock (newActionLock)
            {
                Monitor.PulseAll(newActionLock);
            }
        }

        public void Dispose()
        {
            continueRunning = false;
        }
    }
}