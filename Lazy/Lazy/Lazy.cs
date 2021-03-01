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
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }

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
        private readonly bool isCalculated;
        private readonly Func<T> supplier;
    }
}
