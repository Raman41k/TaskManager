﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    class Service : ViewModels.BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string StartupType { get; set; }

    }
}
