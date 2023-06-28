using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Infrastructure.Commands;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels
{

    class ProcessesViewModel : BaseViewModel
    {
        private TaskService TaskService { get; }
        public Action<List<Module>> OpenUC { get; }

        private List<Process_> _processes;
        public List<Process_> Processes
        {
            get { return _processes; }
            set { Set(ref _processes, value); OnPropertyChanged(); }
        }



        private Process_ _selectedProcess;
        public Process_ SelectedProcess
        {
            get { return _selectedProcess; }
            set { Set(ref _selectedProcess, value); OnPropertyChanged(); }
        }

        #region Stop process command
        public ICommand StopProcessCommand { get; set; }
        private void StopProcessExecution(object obj)
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(SelectedProcess.Name, ".");
            foreach (var proc in processes)
            {
                proc.Kill();
            }
        }
        private bool CanStopProcessExecute(object obj)
        {
            if (SelectedProcess == null) return false;
            if (SelectedProcess.ProcessUser == "SYSTEM") return false;
            else return true;
        }
        #endregion


        #region Copy path command
        public ICommand CopyToClipboardCommand { get; set; }
        private void CopyToClipboardCommandExecute(object obj)
        {
            System.Windows.Clipboard.SetText(SelectedProcess.Path);
        }
        private bool CanCopyToClipboardCommandExecute(object obj)
        {
            if (SelectedProcess == null) return false;
            if (SelectedProcess.Path == "-") return false;
            return true;
        }

        #endregion


        #region Check modules command
        public ICommand CheckModulesCommand { get; set; }
        private void CheckModulesCommandExecution(object obj)
        {
            OpenUC.Invoke(SelectedProcess.Modules);
        }

        private bool CanCheckModulesCommandExecute(object obj)
        {
            if (SelectedProcess == null) return false;
            if (SelectedProcess.Modules == null || SelectedProcess.Modules.Count == 0) return false;
            return true;
        }
        #endregion
        public ProcessesViewModel():this(null, null)
        {

        }
        public ProcessesViewModel(TaskService taskService, Action<List<Module>> openUC)
        {

            StopProcessCommand = new LambdaCommand(StopProcessExecution, CanStopProcessExecute);
            CopyToClipboardCommand = new LambdaCommand(CopyToClipboardCommandExecute, CanCopyToClipboardCommandExecute);
            CheckModulesCommand = new LambdaCommand(CheckModulesCommandExecution, CanCheckModulesCommandExecute);

            TaskService = taskService;
            OpenUC = openUC;
            //Processes = TaskService.GetListProcesses();

            Task.Run(async () =>
            {
                while (true)
                {
                    Processes = TaskService.GetListProcesses();
                    await Task.Delay(1000);
                }
            });


        }

        
    }
}
