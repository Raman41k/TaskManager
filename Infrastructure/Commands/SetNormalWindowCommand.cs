using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManager.Infrastructure.Commands
{
    internal class SetNormalWindowCommand : Command
    {
        public override bool CanExecute(object parameter)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                return true;
            return false;
        }

        public override void Execute(object parameter)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

    }
}
