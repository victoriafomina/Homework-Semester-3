using System;
using System.Threading;

namespace Lazy
{
    /// <summary>
    /// Implements thread-safe ILazy. Lets make late initialization.
    /// </summary>
    public class LazyThreadSafe<T> : ILazy<T>
    {
        /// <summary>
        /// Initializes an instance of the LazyThreadSafe object.
        /// </summary>
        public LazyThreadSafe(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }

            isCalculated = false;
            this.supplier = supplier;
        }

        /// <returns>value of the Lazy object</returns>
        public T Get()
        {  
            if (!Volatile.Read(ref isCalculated))
            {
                lock (locker)
                {
                    if (Volatile.Read(ref isCalculated))
                    {
                        return value;
                    }

                    value = supplier();
                    Volatile.Write(ref isCalculated, true);
                    supplier = null;
                }
            }

            return value;
        }

        private readonly object locker = new object();
        private bool isCalculated = false;
        private Func<T> supplier;
        private T value;
    }
}
