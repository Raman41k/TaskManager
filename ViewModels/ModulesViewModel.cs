using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Infrastructure.Commands;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    class ModulesViewModel : BaseViewModel
    {

        private List<Module> modules;
        public List<Module> Modules { get => modules; set { Set(ref modules, value); OnPropertyChanged(); } }


        private Module _selectedModule;
        public Module SelectedModule
        {
            get { return _selectedModule; }
            set { _selectedModule = value; }
        }


       
        public ICommand CopyToClipboardCommand{ get; set; }
        private void CopyToClipboardCommandExecute(object obj)
        {
            System.Windows.Clipboard.SetText(SelectedModule.ModulePath);
        }
        private bool CanCopyToClipboardCommandExecute(object obj)
        {
            if (SelectedModule == null) return false;
            if (SelectedModule.ModulePath == "-") return false;
            return true;
        }

        public ModulesViewModel():this(null)
        {

        }
        public ModulesViewModel(List<Module> modules)
        {
            


            CopyToClipboardCommand = new LambdaCommand(CopyToClipboardCommandExecute, CanCopyToClipboardCommandExecute);
           
            Modules = modules;
        }

    }
}
