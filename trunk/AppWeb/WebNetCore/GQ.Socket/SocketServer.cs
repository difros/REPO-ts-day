using GQ.Socket.connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GQ.Socket
{
    public delegate void DataChangeEventHandler(SocketServer sender);
    public delegate void AddClientEventHandler(SocketConnection sender);
    public delegate void RemoveClientEventHandler(SocketConnection sender);
    public delegate void DataReciveEventHandler(SocketServer server, SocketConnection sender, DataReceivedEventArgs e);

    public class SocketServer : ISocket, IDisposable
    {
        public class ClienteSocketBlock
        {
            public bool IsBlock { get; set; } = false;
            public int Intentos { get; set; } = 0;
            public long Timer { get; set; } = DateTime.Now.AddMinutes(1).Ticks;
        }

        public event AddClientEventHandler AddClient;
        public event RemoveClientEventHandler RemoveClient;
        public event DataChangeEventHandler DataChange;
        public event DataReciveEventHandler DataRecive;
        public event EventHandler DataSent;

        public Dictionary<string, ClienteSocketBlock> BlockClient { get; set; }

        private System.Net.Sockets.Socket ListenerSocker = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public delegate void CallbackRecive(string message);

        public List<SocketConnection> Connections { get; private set; }

        public int bufferSize { get; set; }

        private byte[] _buffer = new byte[1024];

        public int port { get; private set; }

        public System.Guid GUID { get; private set; }

        public bool UseWebSock { get; set; } = false;

        public bool UseBlockList { get; set; } = false;

        public SocketServer(int _port)
        {
            this.port = _port;
            startService();
        }

        private void startService()
        {
            bufferSize = 2048;
            UseWebSock = false;
            Connections = new List<SocketConnection>();
            BlockClient = new Dictionary<string, ClienteSocketBlock>();
            GUID = System.Guid.NewGuid();
            _buffer = new byte[bufferSize];
            SetUpServer();
        }

        private void SetUpServer(int countReintentos = 0)
        {
            try
            {
                if (ListenerSocker == null)
                    return;
                ListenerSocker.Bind(new IPEndPoint(IPAddress.Any, port));
                ListenerSocker.Listen(0x7FFFFFFF);
                ListenerSocker.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception e)
            {
                Log.Log.GetLog().Error(this, "SetUpServer", e);
                Thread.Sleep(5000);
                if (countReintentos > 10)
                {
                    Log.Log.GetLog().Info(this, "Fin Reintentos - SetUpServer - " + countReintentos);
                    return;
                }
                Log.Log.GetLog().Info(this, "Reintentando - SetUpServer - " + countReintentos);
                SetUpServer(countReintentos + 1);
            }
        }

        private void AcceptCallback(IAsyncResult value)
        {
            try
            {
                if (ListenerSocker == null)
                {
                    return;
                }

                System.Net.Sockets.Socket socket = ListenerSocker.EndAccept(value);

                if (UseBlockList)
                {
                    string ip = IP(socket);
                    if (BlockClient.ContainsKey(ip))
                    {
                        if (BlockClient[ip].IsBlock)
                        {
                            if (BlockClient[ip].Timer > DateTime.Now.Ticks)
                            {
                                Log.Log.GetLog().Info(this, "AcceptCallback  Disconnect ip : " + ip);
                                socket.Disconnect(true);
                            }
                            else
                            {
                                BlockClient[ip] = new ClienteSocketBlock();
                            }
                        }
                    }
                }

                if (socket.Connected)
                {
                    getClientType(socket);
                }
            }
            catch (Exception e)
            {
                Log.Log.GetLog().Error(this, "AcceptCallback", e);
            }
            finally
            {
                try
                {
                    if (ListenerSocker != null)
                        ListenerSocker.BeginAccept(new AsyncCallback(AcceptCallback), null);
                }
                catch (Exception e)
                {
                    Log.Log.GetLog().Error(this, "AcceptCallback - ListenerSocker.BeginAccept", e);

                    try
                    {
                        close();
                    }
                    catch (Exception ex)
                    {
                        Log.Log.GetLog().Error(this, "AcceptCallback - ListenerSocker.BeginAccept - close", ex);
                    }

                    Thread.Sleep(2000);

                    try
                    {
                        startService();
                    }
                    catch (Exception ex)
                    {
                        Log.Log.GetLog().Error(this, "AcceptCallback - ListenerSocker.BeginAccept - startService", ex);
                    }
                }
            }

            OnDataChange();
        }

        private void getClientType(System.Net.Sockets.Socket socket)
        {
            if (UseWebSock)
            {
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ClientTypeCallback), socket);
            }
            else
            {
                SocketConnection sc = new SocketConnection(socket, bufferSize);
                sc.Disconnected += new SocketDisconnectedEventHandler(ClientDisconnected);
                sc.DataReceived += new DataReceivedEventHandler(DataReceivedFromClient);

                Connections.Add(sc);

                OnAddClient(sc);

                OnDataChange();

                sc.Listen();
            }
        }

        private void ClientTypeCallback(IAsyncResult value)
        {
            SocketConnection sc = null;

            System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket)value.AsyncState;
            int received = socket.EndReceive(value);
            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);
            string text = Encoding.ASCII.GetString(dataBuf);
            /*
            //// PERMITE ONECCION WEBSOCKET
            if (new Regex("^GET").IsMatch(text))
            {
                WebSocketConnection wsc = new WebSocketConnection(socket, bufferSize);
                wsc.Disconnected += new SocketDisconnectedEventHandler(ClientDisconnected);
                wsc.DataReceived += new DataReceivedEventHandler(DataReceivedFromClient);
                wsc.WebSocketKey = new Regex("Sec-WebSocket-Key: (.*)").Match(text).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"; // GUID;

                wsc.Send(Encoding.ASCII.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                    + "Connection: Upgrade" + Environment.NewLine
                    + "Upgrade: websocket" + Environment.NewLine
                    + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                        SHA1.Create().ComputeHash(
                            Encoding.UTF8.GetBytes(wsc.WebSocketKey)
                        )
                    ) + Environment.NewLine
                    + Environment.NewLine));

                Connections.Add(wsc);
                OnAddClient(wsc);
                sc = wsc;
            }
            else
            {*/
            sc = new SocketConnection(socket, bufferSize);
            sc.Disconnected += new SocketDisconnectedEventHandler(ClientDisconnected);
            sc.DataReceived += new DataReceivedEventHandler(DataReceivedFromClient);
            Connections.Add(sc);
            OnAddClient(sc);
            //}

            OnDataChange();

            sc.Listen();
        }

        private void ShakeHands(System.Net.Sockets.Socket conn)
        {
            using (var stream = new NetworkStream(conn))
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                string r = null;
                while (r != "")
                {
                    r = reader.ReadLine();
                }

                // send handshake to the client
                writer.WriteLine("HTTP/1.1 101 Web Socket Protocol Handshake");
                writer.WriteLine("Upgrade: WebSocket");
                writer.WriteLine("Connection: Upgrade");
                //writer.WriteLine("WebSocket-Origin: " + webSocketOrigin);
                //writer.WriteLine("WebSocket-Location: " + webSocketLocation);
                writer.WriteLine("");
            }
        }

        void ClientDisconnected(SocketConnection sender, EventArgs e)
        {
            try
            {
                if (Connections != null)
                {
                    addBlockSock(sender);
                    Connections.Remove(sender);
                    OnRemoveClient(sender);
                    OnDataChange();
                }
            }
            catch (Exception ex)
            {
                Log.Log.GetLog().Error(this, "ClientDisconnected", ex);
            }
        }

        void DataReceivedFromClient(SocketConnection sender, DataReceivedEventArgs e)
        {
            try
            {
                OnDataRecive(sender, e);
                OnDataChange();
            }
            catch (Exception ex)
            {
                Log.Log.GetLog().Error(this, "DataReceivedFromClient", ex);
            }
        }

        public void Send(byte[] buffer)
        {
            try
            {
                if (ListenerSocker == null)
                    return;
                ListenerSocker.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), this);
            }
            catch (Exception ex)
            {
                Log.Log.GetLog().Error(this, "Send", ex);
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                OnDataSent(this, new EventArgs());
            }
        }

        private void OnDataRecive(SocketConnection sender, DataReceivedEventArgs e)
        {
            DataRecive?.Invoke(this, sender, e);
        }

        private void OnAddClient(SocketConnection sender)
        {
            AddClient?.Invoke(sender);
        }

        private void OnRemoveClient(SocketConnection sender)
        {
            RemoveClient?.Invoke(sender);
        }

        private void OnDataChange()
        {
            DataChange?.Invoke(this);
        }

        private void OnDataSent(object sender, EventArgs e)
        {
            DataSent?.Invoke(sender, e);
        }

        public virtual string IP(System.Net.Sockets.Socket ConnectionSocket)
        {
            string result = "";
            try
            {
                result = IPAddress.Parse(((IPEndPoint)ConnectionSocket.RemoteEndPoint).Address.ToString()).ToString();
            }
            catch
            {
                result = "0.0.0.0";
            }
            return result;
        }

        private void addBlockSock(SocketConnection sender)
        {
            try
            {
                if (UseBlockList)
                {
                    string ip = sender.IP;

                    if (!BlockClient.ContainsKey(ip))
                    {
                        BlockClient.Add(ip, new ClienteSocketBlock());
                    }
                    if (!BlockClient[ip].IsBlock)
                    {
                        if (BlockClient[ip].Timer < DateTime.Now.Ticks)
                            BlockClient[ip].Intentos = 0;

                        BlockClient[ip].Intentos++;
                        BlockClient[ip].Timer = DateTime.Now.AddMinutes(1).Ticks;

                        if (BlockClient[ip].Intentos > 6)
                        {
                            Log.Log.GetLog().Info(this, "addBlockSock :" + ip);
                            BlockClient[ip].IsBlock = true;
                            BlockClient[ip].Intentos = 0;
                            BlockClient[ip].Timer = DateTime.Now.AddMinutes(5).Ticks;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Log.GetLog().Error(this, "addBlockSock", e);
            }
        }

        public void close()
        {
            Dispose();
            Log.Log.GetLog().Info(this, "close");
        }

        public void Dispose()
        {
            try
            {
                AddClient = null;
                RemoveClient = null;
                DataChange = null;
                DataRecive = null;
                DataSent = null;

                if (ListenerSocker != null)
                {
                    ListenerSocker.Shutdown(SocketShutdown.Both);
                    if (ListenerSocker.Connected)
                    {
                        ListenerSocker.Disconnect(true);
                    }
                    ListenerSocker.Close();
                    ListenerSocker.Dispose();

                }
                for (int i = 0; i < Connections.Count; i++)
                {
                    Connections[i].Close();
                    Connections[i].Dispose();
                }
            }
            catch { }

            ListenerSocker = null;
            Connections = null;

            Log.Log.GetLog().Info(this, "Dispose");

            GC.SuppressFinalize(this);
        }
    }
}

