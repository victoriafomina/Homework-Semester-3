using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNUnitWeb.Models
{
    public class TestsLaunchesHistoryViewModel
    {
        public List<AssemblyViewModel> Assemblies { get; set; } = null;

        public List<TestViewModel> Tests { get; set; } = null;
    }
}
