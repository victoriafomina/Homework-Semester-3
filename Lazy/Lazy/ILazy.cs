

namespace Lazy
{
    /// <summary>
    /// ILazy interface. Lets implement late initialization.
    /// </summary>
    public interface ILazy<T>
    {
        T Get();
    }
}
