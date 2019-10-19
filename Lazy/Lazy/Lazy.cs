using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy
{
    /// <summary>
    /// 
    /// </summary>
    public class Lazy<T> : ILazy<T>
    {
        public Lazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }

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
