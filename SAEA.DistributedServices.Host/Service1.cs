using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.DistributedServices.Host
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DistributedHelper.TansactionServiceInit();
        }

        protected override void OnStop()
        {
        }

        public void Run()
        {
            ServiceBase[] ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            Run(ServicesToRun);
        }
    }
}
