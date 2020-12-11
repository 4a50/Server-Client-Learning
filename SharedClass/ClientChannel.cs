using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass
{
    public class ClientChannel<TProtocol, TMessageType> : ChannelClass<TProtocol, TMessageType>
        where TProtocol : Protocol<TMessageType>, new()
    {
        public async Task ConnectAsync(IPEndPoint endpoint)
        {
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(endpoint).ConfigureAwait(false);
            Attach(socket);

        }
    }
}
