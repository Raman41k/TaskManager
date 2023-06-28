using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    class StartupProgram : ViewModels.BaseViewModel
    {    
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Path { get; set; }
    }
}
