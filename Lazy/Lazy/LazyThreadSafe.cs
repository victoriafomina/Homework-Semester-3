using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lazy
{
    public class LazyThreadSafe<T> : ILazy<T>
    {
        public LazyThreadSafe(Func<T> supplier)
        {
            Volatile.Write(ref isCalculated, false);
            this.supplier = supplier;
        }

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

        private object locker;
        private bool isCalculated = false;
        private Func<T> supplier;
        private T value;
    }
}
