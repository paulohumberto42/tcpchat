using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpChat.Networking.Shared;

namespace TcpChat.Networking.Server
{
    public class NetworkServer : INetworkServer, IDisposable
    {
        private readonly ConcurrentDictionary<string, ClientSession> sessions;
        private TcpListener tcpListener;
        private IConnectionValidator connectionValidator;

        private bool disposedValue;
        
        public NetworkServer()
        {
            this.sessions = new ConcurrentDictionary<string, ClientSession>();
        }

        public event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;
        
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

        public void Start(IPAddress address, int port, IConnectionValidator connectionValidator)
        {
            this.Stop();

            this.tcpListener = new TcpListener(address, port);
            this.connectionValidator = connectionValidator;
            this.sessions.Clear();
            this.tcpListener.Start();
            this.WaitForNewClients();
        }

        public void Stop()
        {
            this.tcpListener?.Stop();
            this.tcpListener = null;
            this.connectionValidator = null;
        }

        public bool SendMessage(string sessionId, byte[] message)
        {
            if (this.sessions.TryGetValue(sessionId, out ClientSession session))
            {
                return this.SendMessage(session, message);
            }

            return false;
        }

        private bool SendMessage(ClientSession session, byte[] message)
        {
            try
            {
                session.SendMessage(message);
                return true;
            }
            catch (Exception)
            {
                this.Disconnect(session);
                return false;
            }
        }

        private void WaitForNewClients()
        {
            this.tcpListener.BeginAcceptTcpClient(this.OnClientConnected, null);
        }

        private void OnClientConnected(IAsyncResult result)
        {
            var client = this.tcpListener.EndAcceptTcpClient(result);

            var loginResult = Handshake(client, out string sessionId);

            if (loginResult.Success)
            {
                // Caso já exista uma conexão para a sessão, ela é derrubada
                if (this.sessions.TryRemove(sessionId, out ClientSession existingSession))
                {
                    existingSession.Disconnect();
                }

                var session = this.sessions.GetOrAdd(sessionId, new ClientSession(sessionId, client));
                session.WaitForMessage(this.OnMessageReceived);
                this.ClientConnected?.Invoke(this, new ClientConnectedEventArgs(sessionId));
            }
            else
            {
                client.Client.Close();
            }

            this.WaitForNewClients();
        }

        private ConnectionResult Handshake(TcpClient client, out string sessionId)
        {
            byte[] buffer = new byte[client.SendBufferSize];
            int length = client.Client.Receive(buffer);
            
            var loginMessage = new byte[length];
            Array.Copy(buffer, loginMessage, length);

            var loginResult = this.connectionValidator.ValidateConnection(loginMessage, out sessionId);
            
            client.Client.Send(BitConverter.GetBytes(loginResult.Success));
            client.Client.Send(loginResult.Data);

            return loginResult;
        }

        private void OnMessageReceived(IAsyncResult result)
        {
            var session = (ClientSession)result.AsyncState;

            try
            {
                var message = session.ReceiveMessage(result);
                this.ClientMessageReceived?.Invoke(this, new ClientMessageReceivedEventArgs(session.SessionId, message));
                session.WaitForMessage(this.OnMessageReceived);
            }
            catch (Exception)
            {
                this.Disconnect(session);
            }
        }

        private void Disconnect(ClientSession session)
        {
            session.Disconnect();
            if (this.sessions.TryRemove(session.SessionId, out _))
            {
                this.ClientDisconnected?.Invoke(this, new ClientDisconnectedEventArgs(session.SessionId));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Stop();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
