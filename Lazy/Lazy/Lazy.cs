using System;

namespace Lazy
{
    /// <summary>
    /// Implements ILazy interface. Lets make late initialization.
    /// </summary>
    public class Lazy<T> : ILazy<T>
    {
        /// <summary>
        /// Creates an object of the class Lazy.
        /// </summary>
        /// <param name="supplier"></param>
        public Lazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }

        /// <returns>value of the Lazy object</returns>
        public T Get()
        {
            if (!isCalculated)
            {
                value = supplier();
            }

            return value;
        }

        private T value;
        private bool isCalculated;
        private Func<T> supplier;
    }
}
