using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManager.Infrastructure.Commands;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Title
        private string _Title = "Disk Benchmark";
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion

        #region Current view
        private object _currentView;
        public object CurrentView
        {
            get => _currentView; set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private TaskService _service;

        public TaskService Service
        {
            get { return _service; }
            set { _service = value; }
        }


        public ICommand ProcessesCommand { get; set; }
        public ICommand AutostartsCommand { get; set; }
        public ICommand ServicesCommand { get; set; }
        public ICommand AboutCommand { get; set; }


        private void Processes(object obj)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            CurrentView = new ProcessesViewModel(Service, OpenUC);
        }


        private void Autostarts(object obj)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            CurrentView = new AutostartsViewModel(Service);
        }
        private void Services(object obj)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            CurrentView = new ServicesViewModel(Service);
        }
        private void About(object obj)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            CurrentView = new AboutViewModel();
        }
        private void OpenUC(List<Module> modules)
        {     
            CurrentView = new ModulesViewModel(modules);
        }


        public MainWindowViewModel()
        {
            ProcessesCommand = new LambdaCommand(Processes);
            AutostartsCommand = new LambdaCommand(Autostarts);
            ServicesCommand = new LambdaCommand(Services);
            AboutCommand = new LambdaCommand(About);

            Service = new TaskService();

            CurrentView = new ProcessesViewModel(Service, OpenUC);
        }

    }
}
