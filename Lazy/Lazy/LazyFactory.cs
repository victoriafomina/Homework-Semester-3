using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy
{
    /// <summary>
    /// The LazyFactory class creates objects of the class Lazy.
    /// </summary>
    public class LazyFactory<T>
    {
        public static Lazy<T> Create(Func<T> supplier)
        {
            return new Lazy<T>(supplier);
        }

        public static LazyThreadSafe<T> CreateThreadSafe(Func<T> supplier, T value)
        {
            return new LazyThreadSafe<T>(supplier);
        }
          
    }
}
