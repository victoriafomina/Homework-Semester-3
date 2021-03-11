using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNUnitWeb.Models
{
    public class TestingInfoViewModel
    {
        public List<AssemblyViewModel> Assemblies { get; set; }

        public List<TestViewModel> Tests { get; set; }
    }
}
