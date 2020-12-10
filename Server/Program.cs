using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var echoServer = new EchoServer();
            echoServer.Start();

            Console.WriteLine("Echo Server is Echoing");
            Console.ReadLine();
        }
    }
}
