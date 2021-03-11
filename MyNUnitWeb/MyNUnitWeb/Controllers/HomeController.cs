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

namespace MyNUnitWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment environment;
        private Repository repository;
        private MyNUnit.MyNUnit testsRunner;
        private TestsViewModel testsToRender;
        private readonly string pathToFolderWithTests = Directory.GetCurrentDirectory() + "\\Tests";

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, Repository repository)
        {
            _logger = logger;
            this.repository = repository;
            this.environment = environment;
            testsRunner = new MyNUnit.MyNUnit();
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
        public IActionResult RunTests()
        {
            var classesWithTestMethods = testsRunner.Run(pathToFolderWithTests);
            var classesAssemblies = testsRunner.AssembliesClasses(pathToFolderWithTests);

            var assemblies = classesAssemblies.Keys;

            foreach (var assembly in assemblies)
            {
                var classes = classesAssemblies[assembly];
                var testsInAssembly = new List<TestViewModel>();

                foreach (var someClass in classes)
                {
                    var tests = classesWithTestMethods[someClass];

                    Parallel.ForEach(tests, testMethod =>
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
                        repository.Tests.Add(test);
                    });
                }

                var assemblyInstance = repository.Assemblies.FirstOrDefault(x => x.Name == assembly);

                if (assemblyInstance == null)
                {
                    var assemblyViewModel = new AssemblyViewModel
                    {
                        Name = assembly,
                        Tests = testsInAssembly,
                    };

                    repository.Assemblies.Add(assemblyViewModel);
                    repository.SaveChanges();
                }
            }

            return View("TestsLaunchesInfo", testsToRender);
        }

        /// <summary>
        /// Loads page with test run history.
        /// </summary>
        public IActionResult TestsLaunchesHistory() => View("TestingInfoViewModel", repository.Assemblies.ToList());

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
