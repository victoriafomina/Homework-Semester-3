using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Collections.Concurrent;
using MyNUnit.Attributes;
using System.Diagnostics;

namespace MyNUnit
{
    /// <summary>
    /// Class was implemented for running tests and getting the test running information.
    /// </summary>
    public static class MyNUnit
    {
        /// <summary>
        /// Запуск тестов с выводом результатов на экран
        /// </summary>
        public static void Run(string path)
        {
            results = new ConcurrentDictionary<Type, ConcurrentBag<TestMethodInfo>>();
            testedMethods = new ConcurrentDictionary<Type, MyNUnitLogic>();
            RunTestsByPath(path);

            PrintReport();
        }

        /// <summary>
        /// Запуск тестов с сохранением результатов в виде словаря
        /// </summary>
        public static Dictionary<Type, List<TestMethodInfo>> RunTestsAndGetReport(string path)
        {
            results = new ConcurrentDictionary<Type, ConcurrentBag<TestMethodInfo>>();
            testedMethods = new ConcurrentDictionary<Type, MyNUnitLogic>();
            RunTestsByPath(path);

            return GetReportAboutTests();
        }

        /// <summary>
        /// Результаты тестов
        /// </summary>
        private static ConcurrentDictionary<Type, ConcurrentBag<TestMethodInfo>> results;

        /// <summary>
        /// Методы для тестирования
        /// </summary>
        private static ConcurrentDictionary<Type, MyNUnitLogic> testedMethods;

        /// <summary>
        /// Runs tests by path.
        /// </summary>
        private static void RunTestsByPath(string path)
        {
            var classes = GetClasses(path);

            Parallel.ForEach(classes, someClass => {
                QueueClassTests(someClass);
            });

            RunAllTests();
        }

        /// <summary>
        /// Gets the report about tests.
        /// </summary>
        private static Dictionary<Type, List<TestMethodInfo>> GetReportAboutTests()
        {
            var result = new Dictionary<Type, List<TestMethodInfo>>();

            foreach (var type in results.Keys)
            {
                result.Add(type, new List<TestMethodInfo>());

                foreach (var testInfo in results[type])
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

            var classes = assemblies
                .Select(Assembly.LoadFrom)
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.IsClass);

            return classes;
        }

        /// <summary>
        /// Загрузка методов для тестирования переданного класса в очередь
        /// </summary>
        private static void QueueClassTests(Type type) => testedMethods.TryAdd(type, new MyNUnitLogic(type));

        /// <summary>
        /// Исполнение всех тестов
        /// </summary>
        private static void RunAllTests()
        {
            Parallel.ForEach(testedMethods.Keys, type =>
            {
                results.TryAdd(type, new ConcurrentBag<TestMethodInfo>());

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
        }

        /// <summary>
        /// Исполение переданного тестового метода
        /// </summary>
        private static void RunTestMethod(Type type, MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<TestAttribute>();
            var passed = false;
            Type thrownException = null;
            var emptyConstructor = type.GetConstructor(Type.EmptyTypes);

            if (emptyConstructor == null)
            {
                throw new FormatException($"The class doesn't have a constructor without parameters");
            }

            var instance = emptyConstructor.Invoke(null);

            if (attribute.Ignored)
            {
                var ignored = new TestMethodInfo(method.Name);
                ignored.SetInfoIgnoredTest(attribute.IgnoreMessage);
                results[type].Add(ignored);

                return;
            }

            foreach (var beforeTestMethod in testedMethods[type].BeforeTests)
            {
                RunNonTestMethod(beforeTestMethod, instance);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                method.Invoke(instance, null);

                if (attribute.ExpectedException == null)
                {
                    passed = true;
                    stopwatch.Stop();
                }
            }
            catch (Exception testException)
            {
                thrownException = testException.InnerException.GetType();

                if (thrownException == attribute.ExpectedException)
                {
                    passed = true;
                    stopwatch.Stop();
                }
            }
            finally
            {
                stopwatch.Stop();
                var ellapsedTime = stopwatch.Elapsed;
                var testMethodInfo = new TestMethodInfo(method.Name);
                testMethodInfo.SetInfoPassedTest(passed, attribute.ExpectedException, thrownException, ellapsedTime);
                results[type].Add(testMethodInfo);
            }

            foreach (var afterTestMethod in testedMethods[type].AfterTests)
            {
                RunNonTestMethod(afterTestMethod, instance);
            }
        }

        /// <summary>
        /// Исполнение метода, который требуется для тестирования, но не помеченного аттрибутом [Test]
        /// </summary>
        private static void RunNonTestMethod(MethodInfo method, object instance) =>
                method.Invoke(instance, null);

        /// <summary>
        /// Распечатывает результаты тестирования
        /// </summary>
        private static void PrintReport()
        {
            Console.WriteLine("Testing report:");
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Found classes to test: {testedMethods.Keys.Count}");

            var allMethodsCount = 0;

            foreach (var testedClass in testedMethods.Keys)
            {
                allMethodsCount += testedMethods[testedClass].TestsCount;
            }

            Console.WriteLine($"Found methods to test (total): {allMethodsCount}");

            foreach (var someClass in results.Keys)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Class: {someClass}");

                var test = results;

                foreach (var testInfo in results[someClass])
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine($"Tested method: {testInfo.Name}()");

                    if (testInfo.Ignored)
                    {
                        Console.WriteLine($"Ignored {testInfo.Name}() with message: {testInfo.IgnoreMessage}");
                        continue;
                    }

                    if (testInfo.ExpectedException != null || testInfo.ThrownException != null)
                    {
                        if (testInfo.ExpectedException == null)
                        {
                            Console.WriteLine($"Unexpected exception: {testInfo.ThrownException}");
                        }
                        else
                        {
                            Console.WriteLine($"Expected exception: {testInfo.ExpectedException}");
                            Console.WriteLine($"Thrown exception: {testInfo.ThrownException}");
                        }
                    }

                    Console.WriteLine($"Ellapsed time: {testInfo.Time}");

                    if (testInfo.Passed)
                    {
                        Console.WriteLine($"Passed {testInfo.Name}() test");
                    }
                    else
                    {
                        Console.WriteLine($"Failed {testInfo.Name}() test");
                    }
                }
            }

            Console.WriteLine("-----------------------------");
        }
    }
}