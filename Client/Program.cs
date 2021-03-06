﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using SharedClass;

namespace Client
{

    public class MyMessage
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }



    class Program
    {

        ////static = does not need to be instantiated.  async = other things can be done while this works.  Task = Async Operation
        ////the <T> denotes a generic type, doesn't fit into the normal class types.  T is arbitrary
        //// Params: NetworkStream, T message
        //static async Task SendAsync<T>(NetworkStream networkStream, T message)
        //{
        //    //Setting to vars using the two properites of Encode.
        //    //Invoking the method Encode.            
        //    var (header, body) = Encode(message);
        //    Console.WriteLine("Header: " + header + "\tBody: " + body);
        //    //This will send out the header information (Length of the body to follow)
        //    //                             buffer, offset, length(count)
        //    await networkStream.WriteAsync(header, 0, header.Length).ConfigureAwait(false);
        //    //After the header information is sent out send the body of informationa out.
        //    await networkStream.WriteAsync(body, 0, body.Length).ConfigureAwait(false);
        //}

        //static async Task<T> ReceiveAsync<T>(NetworkStream networkStream)
        //{
        //    //Reads the first four bytes coming in which is the header with the body length.
        //    var headerBytes = await ReadAsync(networkStream, 4);
        //    //converts the ReadAsync headerBytes to an int of length of the body.
        //    var bodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(headerBytes));
        //    //Reads the remaining data in the network stream. The header removed the first 4 bytes from it.
        //    var bodyBytes = await ReadAsync(networkStream, bodyLength);
        //    //Run Decode and return that result.  Which is XML
        //    return Decode<T>(bodyBytes);
        //}
        ////This method returns a tuple (header, body) are the elements of that tuple.
        //static (byte[] header, byte[] body) Encode<T>(T message)
        //{
        //    Console.WriteLine("meassayge: " + message);
        //    //XML serializer
        //    var xs = new XmlSerializer(typeof(T));
        //    // StringBuilder
        //    var sb = new StringBuilder();
        //    //StringWriter is adding to the sb object
        //    var sw = new StringWriter(sb);
        //    //Converts to XML using 'StringWriter' with the data from 'message'
          
        //    xs.Serialize(sw, message);
        //   //Converts the string sb to a byte array bodyBytes[]
        //    var bodyBytes = Encoding.UTF8.GetBytes(sb.ToString());
        //    //Ordering of bytes among different computers can be different, so we convert to HostToNetwork to standardize it.
                        
        //    var headerBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(bodyBytes.Length));
        //   // Console.WriteLine(IPAddress.HostToNetworkOrder(bodyBytes.Length).ToString());
        //    //tuple return
        //    return (headerBytes, bodyBytes);
        //}

        //static T Decode<T>(byte[] body)
        //{
        //    //converts the byte array to a string
        //    var str = Encoding.UTF8.GetString(body);
        //    //put the stringReader using str
        //    var sr = new StringReader(str);
        //    //instantiates a new XMlSerializer for conversion into XML
        //    var xs = new XmlSerializer(typeof(T));
        //    //converts sr and returns XML using the xs object.
        //    return (T)xs.Deserialize(sr);       
        
        //}

        //static async Task<byte[]> ReadAsync(NetworkStream networkStream, int bytesToRead)
        //{
        //    var buffer = new byte[bytesToRead];
        //    var bytesRead = 0;
        //    while (bytesRead < bytesToRead)
        //    {
        //        var bytesReceived = await networkStream.ReadAsync(buffer, bytesRead, (bytesToRead - bytesRead)).ConfigureAwait(false);
        //        if (bytesReceived == 0)
        //            throw new Exception("Socket Closed");
        //        bytesRead += bytesReceived;
        //    }
        //    return buffer;
        //}

        static async Task Main(string[] args)
        {

            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();

            //Sets the endpoint, or where to connect to.

            //We can set an IPv4 address by string.  IPAddress ipAddr = IPAddress.Parse("111.11.1.12");
            //Can Set the PORT as an INT.  Each Client can have it's unique port.
            var endpoint = new IPEndPoint(IPAddress.Loopback, 3333);
            //var channel = new ClientChannel<JsonMessageProtocol, JObject>();
            var channel = new ClientChannel<XmlMessageProtocol, XDocument>();

            channel.OnMessage(OnMessage);

            await channel.ConnectAsync(endpoint).ConfigureAwait(false);


            //instantiating a basic object to format the message.  string and an int.
            var myMessage = new MyMessage
            {
                IntProperty = 2222,
                StringProperty = "Hello World"
            };

            Console.WriteLine("Sending");
            Print(myMessage);

            await channel.SendAsync(myMessage).ConfigureAwait(false);
            
            //Console.ReadLine();

        }
        static Task OnMessage(JObject jObject)
        {
            Console.WriteLine("Revcieved JObject Message");
            Print(Convert(jObject));
            return Task.CompletedTask;
        }
        static Task OnMessage(XDocument xDocument)
        {
            Console.WriteLine("Revcieved XDocument Message");
            Print(Convert(xDocument));
            return Task.CompletedTask;
        }
        static MyMessage Convert(JObject jObject)
            => (jObject as JObject).ToObject(typeof(MyMessage)) as MyMessage;
        static MyMessage Convert(XDocument xmlDocument)
            => new XmlSerializer(typeof(MyMessage)).Deserialize(new StringReader((xmlDocument as XDocument).ToString())) as MyMessage;
        static void Print(MyMessage m) => Console.WriteLine($"MyMessage.IntProperty = {m.IntProperty}, MyMessage.StringProperty = {m.StringProperty}");
    }
}