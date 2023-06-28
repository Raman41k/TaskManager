using System.Collections.Generic;

namespace TaskManager.Models
{
    class Process_ : ViewModels.BaseViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ModuleName { get; set; }
        public string ProcessUser { get; set; }
        public bool State { get; set; }
        public int ID { get; set; }
        public long UsedMemory { get; set; }
        public double UsedCPU { get; set; }
        public List<Module> Modules { get; set; } = new List<Module>();
    }
}
