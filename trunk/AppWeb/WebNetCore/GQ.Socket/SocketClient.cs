using System;
using System.Net.Sockets;
using System.Threading;

namespace GQ.Socket
{
    public class SocketClient : ISocket, IDisposable
    {
        public delegate void DataClientReceivedEventHandler(object sender, DataReceivedEventArgs e);
        public delegate void CallbackRecive(string message);

        public event EventHandler ClientConnected;
        public event EventHandler ClientDisconnected;
        public event DataClientReceivedEventHandler DataReceived;
        public event EventHandler DataSent;

        private System.Net.Sockets.Socket _ListenerSocker = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public System.Net.Sockets.Socket ListenerSocker { get { return _ListenerSocker; } private set { _ListenerSocker = value; } }

        public Uri uri { get; private set; }

        public System.Guid GUID { get; private set; }

        private bool isDispose = false;

        public SocketClient(Uri _uri)
        {
            GUID = System.Guid.NewGuid();
            this.uri = _uri;
        }

        private int countReintentosSetUpServer = 0;
        public void Connect()
        {
            isDispose = false;
            try
            {
                if (ListenerSocker == null)
                    ListenerSocker = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ListenerSocker.SendTimeout = 2000;
                ListenerSocker.ReceiveTimeout = 2000;
                ListenerSocker.BeginConnect(uri.Host, uri.Port, new AsyncCallback(ConnectCallback), this);
            }
            catch
            {
                Thread.Sleep(2000);
                if (countReintentosSetUpServer < 10)
                {
                    Connect();
                    countReintentosSetUpServer++;
                }
                else
                    OnClientDisconnected(this, new EventArgs());
            }
        }

        private int countReintentosListen = 0;
        private void Listen()
        {
            try
            {
                if (ListenerSocker != null)
                    ListenerSocker.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, this);
            }
            catch
            {
                Thread.Sleep(2000);
                if (countReintentosListen < 10)
                {
                    Listen();
                    countReintentosListen++;
                }
                else
                    OnClientDisconnected(this, new EventArgs());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (isDispose)
                return;
            int sizeOfReceivedData = -1;
            try
            {
                sizeOfReceivedData = ListenerSocker.EndReceive(ar);
            }
            catch { }

            if (sizeOfReceivedData > 0)
            {
                OnDataReceived(this, new DataReceivedEventArgs(sizeOfReceivedData, _buffer));
                Listen();
            }
            else // the socket is closed
            {

            }
        }

        private void ConnectCallback(IAsyncResult value)
        {
            if (isDispose)
                return;
            if (ListenerSocker != null && ListenerSocker.Connected)
            {
                OnClientConnected(this, new EventArgs());
                Listen();
            }
            else
            {
                Connect();
            }
        }

        public void Send(byte[] _buffer)
        {
            if (isDispose)
                return;
            try
            {
                if (ListenerSocker != null)
                    ListenerSocker.BeginSend(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), this);
            }
            catch
            {
                OnClientDisconnected(this, new EventArgs());
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            if (isDispose)
                return;
            if (ar.IsCompleted)
            {
                OnDataSent(this, new EventArgs());
            }
        }

        private byte[] _buffer = new byte[1024];

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (isDispose)
                return;
            if (DataReceived != null)
                DataReceived(sender, e);
        }

        private void OnDataSent(object sender, EventArgs e)
        {
            if (isDispose)
                return;
            if (DataSent != null)
                DataSent(sender, e);
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            if (isDispose)
                return;
            if (ClientConnected != null)
                ClientConnected(sender, e);
        }

        private void OnClientDisconnected(object sender, EventArgs e)
        {
            if (isDispose)
                return;
            if (ClientDisconnected != null)
                ClientDisconnected(sender, e);
        }

        public void Dispose()
        {
            try
            {
                if (isDispose)
                    return;
                if (ListenerSocker != null)
                {
                    try
                    {
                        if (ListenerSocker.Connected)
                        {
                            ListenerSocker.Disconnect(false);
                        }
                    }
                    catch { }

                    ListenerSocker.Close();
                    ListenerSocker.Dispose();
                }
            }
            catch { }

            ListenerSocker = null;

            ClientConnected = null;
            DataReceived = null;
            DataSent = null;
            isDispose = true;
        }
    }
}
