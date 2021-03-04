using MyNUnit.Attributes;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace MyNUnit
{
    /// <summary>
    /// Implements all the methods that are necessary for testing.
    /// </summary>
    public class MyNUnitLogic
    {
        /// <summary>
        /// Initializes an instance of MyNUnitLogic class.
        /// </summary>
        public MyNUnitLogic(Type type)
        {
            BeforeClassTests = new ConcurrentQueue<MethodInfo>();
            BeforeTests = new ConcurrentQueue<MethodInfo>();
            Tests = new ConcurrentQueue<MethodInfo>();
            AfterTests = new ConcurrentQueue<MethodInfo>();
            AfterClassTests = new ConcurrentQueue<MethodInfo>();

            QueueTestedClassMethods(type);
        }
        /// <summary>
        /// Queue of the methods that are marked by an attribute AfterAttribute.
        /// </summary>
        public ConcurrentQueue<MethodInfo> AfterTests;

        /// <summary>
        /// Queue of the methods that are marked by an attribute AfterClassAttribute.
        /// </summary>
        public ConcurrentQueue<MethodInfo> AfterClassTests;

        /// <summary>
        /// Queue of the methods that are marked by an attribute BeforeAttribute.
        /// </summary>
        public ConcurrentQueue<MethodInfo> BeforeTests;

        /// <summary>
        /// Queue of the methods that are marked by an attribute BeforeClassAttribute.
        /// </summary>
        public ConcurrentQueue<MethodInfo> BeforeClassTests;

        /// <summary>
        /// Queue of the methods that are marked by an attribute TestAttribute.
        /// </summary>
        public ConcurrentQueue<MethodInfo> Tests;

        /// <summary>
        /// Count of the tested methods.
        /// </summary>
        public int TestsCount => Tests.Count;

        /// <summary>
        /// Adds method to the queue.
        /// </summary>
        private void AddMethodToQueue(MethodInfo method, ConcurrentQueue<MethodInfo> queue) => queue.Enqueue(method);

        /// <summary>
        /// Fills the queues of the tested methods.
        /// </summary>
        /// <param name="type">The class whose methods will be tested.</param>
        public void QueueTestedClassMethods(Type type)
        {
            foreach (var method in type.GetMethods())
            {
                Task task = Task.Run(() => {
                    if (method.GetCustomAttribute<BeforeClassAttribute>() != null)
                    {
                        if (!method.IsStatic)
                        {
                            throw new FormatException("Methods marked by BeforeClassAttribute must be static!");
                        }

                        AddMethodToQueue(method, BeforeClassTests);
                    }
                    else if (method.GetCustomAttribute<BeforeAttribute>() != null)
                    {
                        AddMethodToQueue(method, BeforeTests);
                    }
                    else if (method.GetCustomAttribute<TestAttribute>() != null)
                    {
                        AddMethodToQueue(method, Tests);
                    }
                    else if (method.GetCustomAttribute<AfterAttribute>() != null)
                    {
                        AddMethodToQueue(method, AfterTests);
                    }
                    else if (method.GetCustomAttribute<AfterClassAttribute>() != null)
                    {
                        if (!method.IsStatic)
                        {
                            throw new FormatException("Methods marked by AfterClassAttribute must be static!");
                        }

                        AddMethodToQueue(method, AfterClassTests);
                    }
                });
            }
        }
    }
}