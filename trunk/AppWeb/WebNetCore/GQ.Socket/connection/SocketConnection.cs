using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GQ.Socket.connection
{
    public delegate void DataReceivedEventHandler(SocketConnection sender, DataReceivedEventArgs e);
    public delegate void SocketDisconnectedEventHandler(SocketConnection sender, EventArgs e);

    public class SocketConnection : IDisposable, ISocket
    {

        #region Private members
        protected byte[] dataBuffer;                                  // buffer to hold the data we are reading
        protected bool readingData;                                   // are we in the proccess of reading data or not

        #endregion

        /// <summary>
        /// An event that is triggered whenever the connection has read some data from the client
        /// </summary>
        public event DataReceivedEventHandler DataReceived;

        public event SocketDisconnectedEventHandler Disconnected;

        /// <summary>
        /// Guid for the connection - thouhgt it might be usable in some way
        /// </summary>
        public System.Guid GUID { get; private set; }

        /// <summary>
        /// Gets the socket used for the connection
        /// </summary>
        public System.Net.Sockets.Socket ConnectionSocket { get; private set; }

        #region Constructors
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="connection">The socket on which to esablish the connection</param>
        /// <param name="webSocketOrigin">The origin from which the server is willing to accept connections, usually this is your web server. For example: http://localhost:8080.</param>
        /// <param name="webSocketLocation">The location of the web socket server (the server on which this code is running). For example: ws://localhost:8181/service. The '/service'-part is important it could be '/somethingelse' but it needs to be there.</param>
        public SocketConnection(System.Net.Sockets.Socket socket)
            : this(socket, 256)
        {

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="connection">The socket on which to esablish the connection</param>
        /// <param name="webSocketOrigin">The origin from which the server is willing to accept connections, usually this is your web server. For example: http://localhost:8080.</param>
        /// <param name="webSocketLocation">The location of the web socket server (the server on which this code is running). For example: ws://localhost:8181/service. The '/service'-part is important it could be '/somethingelse' but it needs to be there.</param>
        /// <param name="bufferSize">The size of the buffer used to receive data</param>
        public SocketConnection(System.Net.Sockets.Socket socket, int bufferSize)
        {
            ConnectionSocket = socket;
            dataBuffer = new byte[bufferSize];
            GUID = System.Guid.NewGuid();
        }
        #endregion

        /// <summary>
        /// Invoke the DataReceived event, called whenever the client has finished sending data.
        /// </summary>
        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(this, e);
        }

        public virtual string IP
        {
            get
            {
                string result = "";
                try
                {
                    result = IPAddress.Parse(((IPEndPoint)ConnectionSocket.RemoteEndPoint).Address.ToString()).ToString();
                }
                catch // (Exception e)
                {
                    result = "0.0.0.0";
                }
                return result;
            }
        }

        public virtual string Port
        {
            get
            {
                string result = "";
                try
                {
                    result = ((IPEndPoint)ConnectionSocket.RemoteEndPoint).Port.ToString();
                }
                catch //(Exception e)
                {
                    result = "-1";
                }
                return result;
            }
        }

        /// <summary>
        /// Listens for incomming data
        /// </summary>
        /// 
        public void Listen()
        {
            Listen(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reintentos"></param>
        protected void Listen(int reintentos)
        {
            if (ConnectionSocket == null)
                return;
            try
            {
                ConnectionSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, 0, Read, null);
            }
            catch
            {
                if (reintentos < 3)
                {
                    Listen(reintentos + 1);
                }
                else
                {
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Send a string to the client
        /// </summary>
        /// <param name="str">the string to send to the client</param>
        public virtual void Send(string str)
        {
            Send(Encoding.ASCII.GetBytes(str));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public virtual void Send(byte[] data)
        {
            if (ConnectionSocket == null)
                return;
            if (ConnectionSocket.Connected && data?.LongLength > 0)
            {
                try
                {
                    ConnectionSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), ConnectionSocket);
                }
                catch
                {
                    OnDisconnected();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected void SendCallback(IAsyncResult value)
        {
            try
            {
                System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket)value.AsyncState;
                socket.EndSend(value);
            }
            catch { }
        }

        /// <summary>
        /// reads the incomming data and triggers the DataReceived event when done
        /// </summary>
        protected virtual void Read(IAsyncResult ar)
        {
            if (ConnectionSocket == null)
                return;
            int sizeOfReceivedData = -1;
            try
            {
                sizeOfReceivedData = ConnectionSocket.EndReceive(ar);
            }
            catch { }

            if (sizeOfReceivedData > 0)
            {
                // we are no longer reading data
                readingData = false;
                OnDataReceived(new DataReceivedEventArgs(sizeOfReceivedData, dataBuffer));
                Listen();
            }
            else // the socket is closed
            {
                OnDisconnected();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnDisconnected()
        {
            if (Disconnected != null)
                Disconnected(this, EventArgs.Empty);
        }


        #region cleanup
        /// <summary>
        /// Closes the socket
        /// </summary>
        public void Close()
        {
            if (ConnectionSocket != null)
            {
                if (ConnectionSocket.Connected)
                    ConnectionSocket.Disconnect(true);
                ConnectionSocket.Shutdown(SocketShutdown.Both);
                ConnectionSocket.Close();
            }
        }

        /// <summary>
        /// Closes the socket
        /// </summary>
        public void Dispose()
        {
            try
            {
                DataReceived = null;
                Disconnected = null;
                Close();
                if (ConnectionSocket != null)
                {
                    ConnectionSocket.Dispose();
                    ConnectionSocket = null;
                }
            }
            catch { }
        }
        #endregion
    }
}
