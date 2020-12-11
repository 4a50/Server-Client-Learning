using Newtonsoft.Json.Linq;
using SharedClass;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ClientInterfacingExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    
        
            static async Task getConnected(ipAddress)
        {
            string ipAddress = "192.168.1.14";
            Console.WriteLine("Setting Up Connection");
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), 3333);
            var channel = new SharedClass.ClientChannel<JsonMessageProtocol, JObject>();
            channel.OnMessage();

                   
        }

        static Task OnMessage(JObject jObject)
        {

        }
    }
    }

}
