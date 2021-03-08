using MyNUnit.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyNUnit
{
    /// <summary>
    /// Class was implemented for running tests and getting the test running information.
    /// </summary>
    public static class MyNUnit
    {
        /// <summary>
        /// Tests results.
        /// </summary>
        private static ConcurrentDictionary<Type, ConcurrentQueue<TestMethodInfo>> testsResults;

        /// <summary>
        /// Methods to test.
        /// </summary>
        private static ConcurrentDictionary<Type, MyNUnitLogic> testedMethods;

        /// <summary>
        /// Runs tests.
        /// </summary>
        /// <returns>the result of test running</returns>
        public static Dictionary<Type, List<TestMethodInfo>> Run(string path)
        {
            testsResults = new ConcurrentDictionary<Type, ConcurrentQueue<TestMethodInfo>>();
            testedMethods = new ConcurrentDictionary<Type, MyNUnitLogic>();
            RunTestsByPath(path);

            return GetReportAboutTests();
        }

        /// <summary>
        /// Runs tests by path.
        /// </summary>
        private static void RunTestsByPath(string path)
        {
            var classes = GetClasses(path);

            Parallel.ForEach(classes, someClass =>
            {
                ClassTests(someClass);
            });

            RunTests();
        }

        /// <summary>
        /// Gets the report about tests.
        /// </summary>
        private static Dictionary<Type, List<TestMethodInfo>> GetReportAboutTests()
        {
            var result = new Dictionary<Type, List<TestMethodInfo>>();

            foreach (var type in testsResults.Keys)
            {
                result.Add(type, new List<TestMethodInfo>());

                foreach (var testInfo in testsResults[type])
                {
                    result[type].Add(testInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all the classes from the assemblies by path.
        /// </summary>
        private static IEnumerable<Type> GetClasses(string path)
        {
            var assemblies = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories)
                    .Concat(Directory.EnumerateFiles(path, "*.exe", SearchOption.AllDirectories)).ToList();
            assemblies.RemoveAll(assemblyPath => assemblyPath.Contains("\\MyNUnit.dll"));
            assemblies.RemoveAll(assemblyPath => assemblyPath.Contains("\\MyNUnit.exe"));
            assemblies.RemoveAll(assemblyPath => assemblyPath.Contains("\\Attributes.dll"));

            var classes = assemblies
                .Select(Assembly.LoadFrom)
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.IsClass);

            return classes;
        }

        /// <summary>
        /// Loads tested methods.
        /// </summary>
        private static void ClassTests(Type type) => testedMethods.TryAdd(type, new MyNUnitLogic(type));

        /// <summary>
        /// Runs all the tests.
        /// </summary>
        private static void RunTests()
        {
            var tasks = new List<Task>();

            foreach (var type in testedMethods.Keys)
            {
                var task = Task.Run(() =>
                {
                    testsResults.TryAdd(type, new ConcurrentQueue<TestMethodInfo>());

                    foreach (var beforeClassMethod in testedMethods[type].BeforeClassTests)
                    {
                        RunNonTestMethod(beforeClassMethod, null);
                    }

                    foreach (var testMethod in testedMethods[type].Tests)
                    {
                        RunTestMethod(type, testMethod);
                    }

                    foreach (var afterClassMethod in testedMethods[type].AfterClassTests)
                    {
                        RunNonTestMethod(afterClassMethod, null);
                    }
                });

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <summary>
        /// Gets instance by invocating a constructor.
        /// </summary>
        private static object GetInstanceByInvocationConstructor(Type type)
        {
            var emptyConstructor = type.GetConstructor(Type.EmptyTypes);

            if (emptyConstructor == null)
            {
                throw new FormatException($"The class doesn't have a constructor without parameters");
            }

            return emptyConstructor.Invoke(null);
        }

        /// <summary>
        /// Runs the test method.
        /// </summary>
        private static void RunTestMethod(Type type, MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<TestAttribute>();

            if (attribute.Ignored)
            {
                var ignored = new TestMethodInfo(method.Name);
                ignored.SetInfoIgnoredTest(attribute.IgnoreMessage);
                testsResults[type].Enqueue(ignored);

                return;
            }

            var instance = GetInstanceByInvocationConstructor(type);

            foreach (var beforeTest in testedMethods[type].BeforeTests)
            {
                RunNonTestMethod(beforeTest, instance);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                method.Invoke(instance, null);
                stopwatch.Stop();
            }
            catch (Exception e)
            {
                if (e.InnerException.GetType() == attribute.ExpectedException)
                {
                    stopwatch.Stop();
                    var timeElapsed = stopwatch.Elapsed;
                    var testMethodInfo = new TestMethodInfo(method.Name);
                    testMethodInfo.SetInfoPassedTest(true, attribute.ExpectedException, e.GetType(), timeElapsed);
                    testsResults[type].Enqueue(testMethodInfo);
                }
            }
            finally
            {
                stopwatch.Stop();
                var timeElapsed = stopwatch.Elapsed;
                var testMethodInfo = new TestMethodInfo(method.Name);
                testMethodInfo.SetInfoPassedTest(true, attribute.ExpectedException, null, timeElapsed);
                testsResults[type].Enqueue(testMethodInfo);
            }

            foreach (var afterTest in testedMethods[type].AfterTests)
            {
                RunNonTestMethod(afterTest, instance);
            }
        }

        /// <summary>
        /// Runs test that is needed for testing but is not marked with the TestAttribute.
        /// </summary>
        private static void RunNonTestMethod(MethodInfo method, object instance) =>
                method.Invoke(instance, null);
    }
}