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
        public Lazy(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }

            this.supplier = supplier;
        }

        /// <summary>Returns value of the object.</summary>
        public T Get()
        {
            if (!isCalculated)
            {
                value = supplier();
                isCalculated = true;
                supplier = null;
            }

            return value;
        }

        private T value;
        private bool isCalculated;
        private Func<T> supplier;
    }
}
