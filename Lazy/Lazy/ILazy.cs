namespace Lazy
{
    /// <summary>
    /// ILazy interface. Implements late initialization.
    /// </summary>
    public interface ILazy<out T>
    {
        /// <returns>value of the object</returns>
        T Get();
    }
}
