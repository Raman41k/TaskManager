//using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using TaskManager.Infrastructure.Commands;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels
{
    class AutostartsViewModel : BaseViewModel
    {
        private readonly TaskService taskService;

        #region FilePath
        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { Set(ref filePath, value); OnPropertyChanged(); }
        }
        #endregion

        #region StartupPrograms
        private List<StartupProgram> _startupPrograms;
        public List<StartupProgram> StartupPrograms
        {
            get { return _startupPrograms; }
            set { Set(ref _startupPrograms, value); OnPropertyChanged(); }
        }
        #endregion

        #region Selected program
        private StartupProgram _selectedProgram;

        public StartupProgram SelectedProgram
        {
            get { return _selectedProgram; }
            set { Set(ref _selectedProgram, value); OnPropertyChanged(); }
        }




        #endregion

        #region OpenFileDialogCommand
        public ICommand OpenFileDialogCommand{ get; set; }

        private void OpenFileDialogCommandExecution(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                FilePath = openFileDialog.FileName;
            }
        }

        #endregion

        #region Add program to startup
        public ICommand AddToStartupCommand { get; set; }

        private void AddToStartupCommandExecution(object obj)
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
             ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                string name = Path.GetFileNameWithoutExtension(FilePath);
                key.SetValue(name, FilePath);
                StartupPrograms = taskService.GetStartupApplications();
            }
        }
        private bool CanAddToStartupCommandExecute(object obj)
        {
            if (string.IsNullOrEmpty(FilePath) || string.IsNullOrWhiteSpace(FilePath))
                return false;
            if (IsInStartup(Path.GetFileNameWithoutExtension(FilePath), FilePath))
                return false;
            return true;
        }

        #endregion


        #region Remove program from startup
        public ICommand RemoveFromStartupCommand { get; set; }

        private void RemoveFromStartupCommandExecution(object obj)
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue(SelectedProgram.Name, false);
                StartupPrograms = taskService.GetStartupApplications();
            }
        }
        private bool CanRemoveFromStartupCommandExecute(object obj)
        {
            if (SelectedProgram is null)
                return false;
           
            return true;
        }



        private static bool IsInStartup(string AppTitle, string AppPath)
        {
            Microsoft.Win32.RegistryKey rk;
            string value;
            try
            {
                rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                value = rk.GetValue(AppTitle)?.ToString();
            
                if (value == null)
                {
                    return false;
                }
                else if (!value.ToLower().Equals(AppPath.ToLower()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }

            try
            {
                rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                value = rk.GetValue(AppTitle).ToString();
                if (value == null)
                {
                    return false;
                }
                else if (!value.ToLower().Equals(AppPath.ToLower()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        #endregion


        public AutostartsViewModel() :this(null)
        {

        }
        public AutostartsViewModel(TaskService taskService)
        {

            OpenFileDialogCommand = new LambdaCommand(OpenFileDialogCommandExecution);
            AddToStartupCommand = new LambdaCommand(AddToStartupCommandExecution, CanAddToStartupCommandExecute);
            RemoveFromStartupCommand = new LambdaCommand(RemoveFromStartupCommandExecution, CanRemoveFromStartupCommandExecute);

            this.taskService = taskService;

            Task.Run(() =>
            {
                StartupPrograms = taskService.GetStartupApplications();
            });


        }
        
    }
}
