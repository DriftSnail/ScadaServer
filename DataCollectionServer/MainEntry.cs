using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ScadaWcfLibrary;
using System.ServiceModel;

namespace ScadaServerHost
{
    public class MainEntry
    {
        static readonly string ip = ConfigurationManager.AppSettings["ServerIP"];
        static readonly string name = ConfigurationManager.AppSettings["ServerName"];
        static readonly ushort port = ushort.Parse(ConfigurationManager.AppSettings["ServerPort"]);
        ScadaServer MyServer;
        private readonly ServiceHost host;
        public SvcManage ManageSvc;

        public MainEntry()
        {            
            ManageSvc = new SvcManage();
            host = new ServiceHost(ManageSvc);
            MyServer = new ScadaServer(name, ip, port, ManageSvc);
        }

        public void Start()
        {
            try
            {
                host.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            MyServer.Start();
            DevInfo dev = new DevInfo(0, 2, string.Empty, false);
            ManageSvc.AddDevice(dev);     //添加一个设备
        }

        public void Stop()
        {
            try
            {
                host.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
