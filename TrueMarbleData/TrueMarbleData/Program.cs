using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace TrueMarbleData
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //TMDataControllerImpl tm = new TMDataControllerImpl();
                //ServiceHost host;
                NetTcpBinding tcpBinding = new NetTcpBinding();//Creation of NetTcpBinding Object.
                tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;//Relaxing the limits of the NetTcpBinding object.
                tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

                System.Console.WriteLine("Welcome to TrueMarble Data Server Application! \nPress Enter to exit\n\n");

                ServiceHost host = new ServiceHost(typeof(TMDataControllerImpl));
                host.AddServiceEndpoint(typeof(ITMDataController), tcpBinding, "net.tcp://localhost:65000/TMData"); //Access via the interface class.
                host.Open();//Enter listening state ready for client requests.
                System.Console.ReadLine(); //Block waiting for client requests.
                host.Close();
            }
            catch (FaultException ex)
            {
                Console.WriteLine("An exception occured: {0}", ex.Message);
            }
        }
    }
}
