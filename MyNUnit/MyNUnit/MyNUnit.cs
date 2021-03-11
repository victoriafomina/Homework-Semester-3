﻿using MyNUnit.Attributes;
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
    public class MyNUnit
    {
        /// <summary>
        /// Tests results.
        /// </summary>
        private ConcurrentDictionary<Type, ConcurrentQueue<TestMethodInfo>> testsResults;

        /// <summary>
        /// Methods to test.
        /// </summary>
        private ConcurrentDictionary<Type, MyNUnitLogic> testedMethods;

        /// <summary>
        /// Runs tests.
        /// </summary>
        /// <returns>the result of test running</returns>
        public Dictionary<Type, List<TestMethodInfo>> Run(string path)
        {
            testsResults = new ConcurrentDictionary<Type, ConcurrentQueue<TestMethodInfo>>();
            testedMethods = new ConcurrentDictionary<Type, MyNUnitLogic>();
            RunTestsByPath(path);

            return GetReportAboutTests();
        }

        /// <summary>
        /// Runs tests by path.
        /// </summary>
        private void RunTestsByPath(string path)
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
        private Dictionary<Type, List<TestMethodInfo>> GetReportAboutTests()
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
        private IEnumerable<Type> GetClasses(string path)
        {
            var assemblies = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories)
                    .Concat(Directory.EnumerateFiles(path, "*.exe", SearchOption.AllDirectories)).ToList();
            assemblies.RemoveAll(assemblyPath => assemblyPath.Contains("\\MyNUnit.dll"));
            assemblies.RemoveAll(assemblyPath => assemblyPath.Contains("\\MyNUnit.exe"));
            assemblies.RemoveAll(assemblyPath => assemblyPath.Contains("\\Attributes.dll"));

            var classes = assemblies.AsParallel()
                .Select(Assembly.LoadFrom)
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.IsClass);

            return classes;
        }

        /// <summary>
        /// Loads tested methods.
        /// </summary>
        private void ClassTests(Type type) => testedMethods.TryAdd(type, new MyNUnitLogic(type));

        /// <summary>
        /// Runs all the tests.
        /// </summary>
        private void RunTests()
        {
            Parallel.ForEach(testedMethods.Keys, type =>
            {
                var taskClass = Task.Run(() =>
                {
                    testsResults.TryAdd(type, new ConcurrentQueue<TestMethodInfo>());

                    var tasksMethods = new List<Task>();

                    foreach (var beforeClassMethod in testedMethods[type].BeforeClassTests)
                    {
                        RunNonTestMethod(beforeClassMethod, null);
                    }

                    Parallel.ForEach(testedMethods[type].Tests, testedMethod =>
                    {
                        RunTestMethod(type, testedMethod);
                    });

                    foreach (var afterClassMethod in testedMethods[type].AfterClassTests)
                    {
                            RunNonTestMethod(afterClassMethod, null);
                    }
                });
            });
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
        private void RunTestMethod(Type type, MethodInfo method)
        {
            CheckMethodCorrectness(method);

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
                CheckMethodCorrectness(beforeTest);
                RunNonTestMethod(beforeTest, instance);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                method.Invoke(instance, null);
                stopwatch.Stop();
                var timeElapsed = stopwatch.Elapsed;
                var testMethodInfo = new TestMethodInfo(method.Name);
                var passedTest = attribute.ExpectedException == null;
                testMethodInfo.SetInfoPassedTest(passedTest, attribute.ExpectedException, null, timeElapsed);
                testsResults[type].Enqueue(testMethodInfo);
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                var timeElapsed = stopwatch.Elapsed;
                var testMethodInfo = new TestMethodInfo(method.Name);
                var passedTest = e.InnerException.GetType() == attribute.ExpectedException;
                testMethodInfo.SetInfoPassedTest(passedTest, attribute.ExpectedException, e.GetType(), timeElapsed);
                testsResults[type].Enqueue(testMethodInfo);
            }

            foreach (var afterTest in testedMethods[type].AfterTests)
            {
                CheckMethodCorrectness(afterTest);
                RunNonTestMethod(afterTest, instance);
            }
        }

        /// <summary>
        /// Checks if the method is valid for testing.
        /// </summary>
        private void CheckMethodCorrectness(MethodInfo method)
        {
            if (method.IsStatic)
            {
                throw new FormatException("Method can not be static!");
            }

            if (method.GetParameters().Length > 0)
            {
                throw new FormatException("Method can not have parameters!");
            }

            if (method.ReturnType != typeof(void))
            {
                throw new FormatException("Method can not have a return value!");
            }
        }

        /// <summary>
        /// Runs test that is needed for testing but is not marked with the TestAttribute.
        /// </summary>
        private void RunNonTestMethod(MethodInfo method, object instance) =>
                method.Invoke(instance, null);

        /// <summary>
        /// Prints tests results.
        /// </summary>
        public void PrintResult()
        {
            Console.WriteLine($"Number of classes to test: {testedMethods.Keys.Count}\n");

            var methodsCount = 0;

            foreach (var testedClass in testedMethods.Keys)
            {
                methodsCount += testedMethods[testedClass].TestsCount;
            }

            Console.WriteLine($"Number of methods to test: {methodsCount}");

            foreach (var testedClass in testsResults.Keys)
            {
                var test = testsResults;

                foreach (var testInfo in testsResults[testedClass])
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine($"Class: {testedClass}\n");
                    Console.WriteLine($"Method: {testInfo.Name}\n");

                    if (testInfo.Ignored)
                    {
                        Console.WriteLine($"Ignored {testInfo.Name}, message: {testInfo.IgnoreMessage}");
                        continue;
                    }

                    if (testInfo.ExpectedException == null && testInfo.ThrownException != null)
                    {
                        Console.WriteLine($"Unexpected exception: {testInfo.ThrownException}");
                    }

                    if (testInfo.ExpectedException != null && testInfo.ThrownException != null)
                    {
                        Console.WriteLine($"Expected exception: {testInfo.ExpectedException}");
                        Console.WriteLine($"Thrown exception: {testInfo.ThrownException}");
                    }

                    Console.WriteLine($"Time to test: {testInfo.Time}\n");

                    if (testInfo.Passed)
                    {
                        Console.WriteLine($"Passed {testInfo.Name} test");
                    }
                    else
                    {
                        Console.WriteLine($"Did not pass {testInfo.Name} test");
                    }
                }
            }
        }
    }
}