using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyNUnitWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyNUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace MyNUnitWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment environment;
        private Repository repository;
        private MyNUnit.MyNUnit testRunner;
        private TestsViewModel testsToRender;
        private readonly string pathToFolderWithTests = Directory.GetCurrentDirectory() + "\\Tests";

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, Repository repository)
        {
            _logger = logger;
            this.repository = repository;
            this.environment = environment;
            testRunner = new MyNUnit.MyNUnit();
            testsToRender = new TestsViewModel();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Launches the tests and renders the result.
        /// </summary>
        public async Task<IActionResult> RunTests()
        {
            repository.Tests.RemoveRange(repository.Tests);
            repository.Assemblies.RemoveRange(repository.Assemblies);
            repository.SaveChanges();

            var classesWithTestMethods = testRunner.Run(pathToFolderWithTests);
            var assembliesContainingTheirClasses = testRunner.GetAssembliesWithTheirClasses(pathToFolderWithTests);

            var assemblies = assembliesContainingTheirClasses.Keys;
            var testsInAssembly = new ConcurrentBag<TestViewModel>();

            foreach (var assembly in assemblies)
            {
                var classes = assembliesContainingTheirClasses[assembly];
                testsInAssembly.Clear();

                var assemblyViewModel = new AssemblyViewModel
                {
                    Name = assembly,
                };

                foreach (var someClass in classes)
                {
                    var tests = classesWithTestMethods[someClass];

                    foreach (var testMethod in tests)
                    {
                        var test = new TestViewModel
                        {
                            Name = testMethod.Name,
                            Ignored = testMethod.Ignored,
                            IgnoreMessage = testMethod.IgnoreMessage,
                            ElapsedTime = testMethod.Time,
                            Passed = testMethod.Passed,
                            TestLaunchTime = DateTime.Now,
                            ClassName = someClass.ToString(),
                        };

                        testsInAssembly.Add(test);
                        testsToRender.Tests.Add(test);
                        repository.Tests.Add(test);
                        assemblyViewModel.Tests.Add(test);
                    }
                }

                var assemblyInstance = repository.Assemblies.FirstOrDefault(x => x.Name == assembly);

                repository.TestLaunchesHistory.Add(assemblyViewModel);

                if (assemblyInstance == null)
                {
                    repository.Assemblies.Add(assemblyViewModel);
                }
            }

            await repository.SaveChangesAsync();

            return View("TestsLaunchesInfo", testsToRender);
        }

        /// <summary>
        /// Loads page with test run history.
        /// </summary>
        public IActionResult TestsLaunchesHistory() => View("TestsLaunchesHistory", repository.TestLaunchesHistory.ToList());

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
