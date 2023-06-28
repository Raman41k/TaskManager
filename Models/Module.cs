namespace TaskManager.Models
{
    internal class Module : ViewModels.BaseViewModel
    {
        public string ModuleName { get; set; }
        public string ModulePath { get; set; }
        public int MemorySize { get; set; }
    }
}