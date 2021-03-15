using Microsoft.AspNetCore.Mvc;
using MyNUnitWeb.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MyNUnitWeb.Controllers
{
    /// <summary>
    /// Controller class.
    /// </summary>
    public class HomeController : Controller
    {
        private Repository repository;
        private MyNUnit.MyNUnit testRunner;
        private readonly string pathToFolderWithTests = Directory.GetCurrentDirectory() + "\\Tests";

        /// <summary>
        /// Initializes an instance of home conroller class.
        /// </summary>
        public HomeController(Repository repository)
        {
            this.repository = repository;
            testRunner = new MyNUnit.MyNUnit();
        }

        /// <summary>
        /// Renders index page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Renders privacy page.
        /// </summary>
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
            var testsToRender = new List<TestViewModel>();

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
                        testsToRender.Add(test);
                    }
                }
            }

            await repository.SaveChangesAsync();

            return View("TestsLaunchesInfo", testsToRender);
        }

        /// <summary>
        /// Loads assembly to the server.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LoadAssembly(IFormFile file)
        {
            if (file != null)
            {
                using var stream = new FileStream($"{Path.Combine(pathToFolderWithTests, file.FileName)}", FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return View("LoadAssembly");
        }

        /// <summary>
        /// Loads an assembly page.
        /// </summary>
        public IActionResult LoadAssemblyPage()
        {
            return View("LoadAssembly");
        }

        /// <summary>
        /// Renders page with the test run history.
        /// </summary>
        public IActionResult TestsLaunchesHistory()
        {
            var testsLaunchesHistory = new TestsLaunchesHistoryViewModel { Tests = repository.Tests.ToList(), Assemblies = repository.Assemblies.ToList() };

            return View("TestsLaunchesHistory", testsLaunchesHistory);
        }

        /// <summary>
        /// Renders an error page.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
