using System;
using System.Threading;

namespace Lazy
{
    public class LazyThreadSafe<T> : ILazy<T>
    {
        /// <summary>
        /// Implements ILazy interface. Lets make thread-safe late initialization.
        /// </summary>
        public LazyThreadSafe(Func<T> supplier)
        {
            Volatile.Write(ref isCalculated, false);
            this.supplier = supplier;
        }

        /// <returns>value of the Lazy object</returns>
        public T Get()
        {  
            if (!Volatile.Read(ref isCalculated))
            {
                lock (locker)
                {
                    value = supplier();
                    isCalculated = true;
                }
            }

            return value;
        }

        private readonly object locker;
        private bool isCalculated = false;
        private readonly Func<T> supplier;
        private T value;
    }
}
