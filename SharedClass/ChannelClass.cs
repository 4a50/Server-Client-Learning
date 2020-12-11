using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedClass
{
    public abstract class ChannelClass<TProtocol, TMessageType> : IDisposable
        where TProtocol : Protocol<TMessageType>, new()
    {
        protected bool _isDisposed = false;

        readonly TProtocol _protocol = new TProtocol();
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        Func<TMessageType, Task> _messageCallback;
        NetworkStream _networkStream;
        Task _recieveLoopTask;


        public void Attach (Socket socket)
        {
            _networkStream = new NetworkStream(socket, true);
            

            _recieveLoopTask = Task.Run(RecieveLoop, _cancellationTokenSource.Token);

        }

        public void OnMessage(Func<TMessageType,Task> callBackHandler) => _messageCallback = callBackHandler;

        public void Close()
        {
            _cancellationTokenSource.Cancel();
            _networkStream?.Close();
        }

        public async Task SendAsync<T>(T message) => await _protocol.SendAsync(_networkStream,  message).ConfigureAwait(false);

        protected virtual async Task RecieveLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                //TODO: Pass Cancellation Toke to Protocol Methods
                var msg = await _protocol.RecieveAsync(_networkStream).ConfigureAwait(false);
                await _messageCallback(msg).ConfigureAwait(false);

            }
        }

        ~ChannelClass() => Dispose(false);


        public void Dispose() => Dispose(true);
        protected void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Close();
                _networkStream?.Dispose();
            }
            if (isDisposing) GC.SuppressFinalize(this);
        }
        
    }
}
