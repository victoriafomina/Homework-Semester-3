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
using Microsoft.EntityFrameworkCore;

namespace MyNUnitWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment environment;
        private Repository repository;
        private MyNUnit.MyNUnit testRunner;
        private readonly string pathToFolderWithTests = Directory.GetCurrentDirectory() + "\\Tests";

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, Repository repository)
        {
            _logger = logger;
            this.repository = repository;
            this.environment = environment;
            testRunner = new MyNUnit.MyNUnit();
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
            var classesWithTestMethods = testRunner.Run(pathToFolderWithTests);
            var assembliesContainingTheirClasses = testRunner.GetAssembliesWithTheirClasses(pathToFolderWithTests);

            var assemblies = assembliesContainingTheirClasses.Keys;

            foreach (var assemblyFullPath in assemblies)
            {
                var classes = assembliesContainingTheirClasses[assemblyFullPath];

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

                        var assemblyName = Path.GetFileName(assemblyFullPath);
                        var assemblyEntity = repository.Assemblies.FirstOrDefault(x => x.Name == assemblyName);

                        if (assemblyEntity == null)
                        {
                            var assemblyViewModel = new AssemblyViewModel
                            {
                                Name = assemblyName,
                            };

                            test.Assembly = assemblyViewModel;
                            repository.Add(assemblyViewModel);
                        }
                        else
                        {
                            test.Assembly = assemblyEntity;
                        }

                        repository.Tests.Add(test);
                    }
                }
            }

            await repository.SaveChangesAsync();
            return View("TestsLaunchesInfo", repository.Tests.ToList());
        }

        /// <summary>
        /// Loads page with the test run history.
        /// </summary>
        public IActionResult TestsLaunchesHistory()
        {
            var testsLaunchesHistory = new TestsLaunchesHistoryViewModel { Tests = repository.Tests.ToList(), Assemblies = repository.Assemblies.ToList() };

            return View("TestsLaunchesHistory", testsLaunchesHistory);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
