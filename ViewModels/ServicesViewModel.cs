using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels
{
    class ServicesViewModel : BaseViewModel
    {
        private readonly TaskService TaskService;

        #region Services
        private List<Service> _services;
        public List<Service> Services
        {
            get { return _services; }
            set { Set(ref _services, value); OnPropertyChanged(); }
        }

        #endregion

        public ServicesViewModel(): this(null)
        {

        }
        public ServicesViewModel(TaskService taskService)
        {
            TaskService = taskService;

            Task.Run(() =>
            {
                Services = TaskService.GetServices();
            });
        }
    }
}
