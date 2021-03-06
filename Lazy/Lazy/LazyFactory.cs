using System;

namespace Lazy
{
    /// <summary>
    /// The LazyFactory class creates objects of the class Lazy.
    /// </summary>
    public class LazyFactory<T>
    {
        /// <summary>
        /// Creates Lazy instance (non thread-safe).
        /// </summary>
        public static Lazy<T> Create(Func<T> supplier) => new Lazy<T>(supplier);

        /// <summary>
        /// Creates LazyTheadSafe instance.
        /// </summary>
        public static LazyThreadSafe<T> CreateThreadSafe(Func<T> supplier) =>
                new LazyThreadSafe<T>(supplier);
    }
}
