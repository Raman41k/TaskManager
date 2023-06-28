using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Services
{
    internal class TaskService
    {
        public List<Process_> GetListProcesses()
        {
            string str = "-";  
            Process[] processes = Process.GetProcesses();

            List<Process_> process_s = new List<Process_>();

            foreach (Process process in processes)
            {
                var proc = new Process_
                {
                    ID = process.Id,
                    Name = process.ProcessName
                };
                try
                {
                    proc.Path = process.MainModule.FileName;
                }
                catch (Exception)
                {
                    proc.Path = str;
                }

                try
                {
                    proc.ModuleName = process.MainModule.ModuleName;
                }
                catch (Exception)
                {
                    proc.ModuleName = str;
                }

                try
                {
                    proc.UsedMemory = process.WorkingSet64 / 1024;
                }
                catch (Exception)
                {               
                }

                try
                {
                    proc.State = process.Responding;
                }
                catch (Exception)
                {                    
                }
                try
                {
                    proc.UsedCPU = this.GetProcessCpuUsage(process);
                }
                catch (Exception)
                {                    
                }

                try
                {
                    proc.ProcessUser = this.GetProcessUserName(process.Id);
                }
                catch (Exception)
                {       
                }
                try
                {
                    for (int i = 0; i < process.Modules.Count; i++)
                    {
                        var module = new Module
                        {
                            ModuleName = process.Modules[i].ModuleName,
                            ModulePath = process.Modules[i].FileName,
                            MemorySize = process.Modules[i].ModuleMemorySize / 1024
                        };
                        proc.Modules.Add(module);
                    }
                }
                catch (Exception)
                {
                }

                process_s.Add(proc);
            }
            return process_s;
        }
        private string GetProcessUserName(int id)
        {
            string userName = string.Empty;
            try
            {
                string query = "SELECT * FROM Win32_Process WHERE ProcessId = " + id;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", query);

                foreach (ManagementObject obj in searcher.Get())
                {
                    string[] ownerInfo = new string[2];
                    obj.InvokeMethod("GetOwner", (object[])ownerInfo);
                    userName = ownerInfo[0];
                    break;
                }
            }
            catch (Exception)
            {             
            }
            return userName;
        }
        private double GetProcessCpuUsage(Process process)
        {
            TimeSpan cpuTime = process.TotalProcessorTime;
            TimeSpan wallClockTime = DateTime.Now - process.StartTime;
            double cpuUsage = (cpuTime.TotalMilliseconds / wallClockTime.TotalMilliseconds) * 100;

            return Math.Round(cpuUsage, 2);
        }


        public List<StartupProgram> GetStartupApplications()
        {
            List<StartupProgram> startupApplications = new List<StartupProgram>();    
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"))
            {
                foreach (string valueName in key.GetValueNames())
                {
                    string applicationPath = key.GetValue(valueName) as string;
                    if (!string.IsNullOrEmpty(applicationPath))
                    {
                        try
                        {
                            if (applicationPath.Contains("\""))
                                applicationPath = applicationPath.Substring(applicationPath.IndexOf("\\") - 2, applicationPath.LastIndexOf("\"") - 1);
                            string name = Path.GetFileNameWithoutExtension(applicationPath);
                            string description = GetFileProperty(applicationPath, "FileDescription");
                            string publisher = GetFileProperty(applicationPath, "CompanyName");

                            startupApplications.Add(new StartupProgram { Name = name, Description = description, Publisher = publisher, Path = applicationPath });

                        }
                        catch (Exception)
                        { }
                    }
                }
            }

            string startupFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            foreach (string filePath in Directory.GetFiles(startupFolderPath))
            {
                string name = Path.GetFileNameWithoutExtension(filePath);
                string description = GetFileProperty(filePath, "FileDescription");
                string publisher = GetFileProperty(filePath, "CompanyName");
                startupApplications.Add(new StartupProgram { Name = name, Description = description, Publisher = publisher, Path = filePath });
            }
            return startupApplications;
        }

        private string GetFileProperty(string filePath, string propertyName)
        {
            string propertyValue = string.Empty;
            try
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
                switch (propertyName)
                {
                    case "FileDescription":
                        propertyValue = versionInfo.FileDescription;
                        break;
                    case "CompanyName":
                        propertyValue = versionInfo.CompanyName;
                        break;
                       
                }
            }
            catch (Exception)
            {
                
            }

            return propertyValue;
        }



        public List<Service> GetServices()
        {
            ServiceController[] services = ServiceController.GetServices();
            List<Service> serviceslist = new List<Service>();
            foreach (ServiceController service in services)
            {
                try
                {
                    serviceslist.Add(new Service
                    {
                        Name = service.ServiceName,
                        Description = GetServiceDescription(service.ServiceName),
                        Status = service.Status.ToString(),
                        StartupType = service.StartType.ToString()
                    });
                }
                catch (Exception)
                {
                    
                }
            }
            return serviceslist;
        }
        static string GetServiceDescription(string serviceName)
        {
            string serviceDescription = string.Empty;

            try
            {
                ServiceController sc = new ServiceController(serviceName);
                serviceDescription = sc.DisplayName;
            }
            catch (Exception)
            {
            }

            return serviceDescription;
        }
        public TaskService()
        {
        }
    }
}
