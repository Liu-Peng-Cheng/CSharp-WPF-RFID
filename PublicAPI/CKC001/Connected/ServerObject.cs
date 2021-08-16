using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
using PublicAPI.CKC001.Others;
using PublicAPI.CKC001.MessageObj;

namespace PublicAPI.CKC001.Connected
{
    public class ServerObject
    {
        private PublicAPI.CKC001.Connected.communication.TypeTcpServer server;
        private Dictionary<string, ClientObject> SocketList;
        public delegateTcpClientConnected dClientConnected;

        public ServerObject()
        {
            SocketList = new Dictionary<string, ClientObject>();
        }

        private void Connected(PublicAPI.CKC001.Connected.communication.TypeTcpClient tcpClient)
        {
            if (tcpClient != null)
            {
                ClientObject client = new ClientObject() {
                    //ReaderName = tcpClient.ipAddress.ToString(),//此处获取IP
                };
                if (client.Connected(tcpClient))
                {
                    this.CallTcpClientConnected(client);
                    lock (this.SocketList)
                    {
                        string tempIP = tcpClient.ipAddress.ToString();
                        if (!this.SocketList.ContainsKey(tempIP))
                        {
                            this.SocketList.Add(tempIP, client);
                        }
                        else
                        {
                            this.SocketList[tempIP].Closed();
                            this.SocketList[tempIP]._base = tcpClient;
                        }
                    }
                    return;
                }
                client.Closed();
                tcpClient.Closed();
            }
        }

        public bool Open(int portNum)
        {
            if (this.server != null)
            {
                return false;
            }
            this.server = new PublicAPI.CKC001.Connected.communication.TypeTcpServer();
            this.server.delegateInternal = new delegateInternalTcpClientConnected( this.Connected);
            if (!this.server.Open(portNum))
            {
                this.Closed();
                return false;
            }
            return true;
        }

        public void CallTcpClientConnected(ClientObject client)
        {
            if (this.dClientConnected != null)
            {
                this.dClientConnected(client);
            }
        }

        public bool IsListening
        {
            get
            {
                if (this.server == null)
                    return false;
                return this.server.IsListen;
            }
        }

        public void Closed()
        {
            try
            {
                lock (this.SocketList)
                {
                    foreach (KeyValuePair<string, ClientObject> vue in this.SocketList)
                    {
                        vue.Value.Closed();
                    }
                    this.SocketList.Clear();
                }
                if (this.server != null)
                {
                    this.server.Closed();
                    this.server = null;
                }
            }
            catch
            { }            
        }

        public void CloseClient(string ClientIP)
        {
            if (!string.IsNullOrEmpty(ClientIP))
            {
                lock (this.SocketList)
                {
                    if (this.SocketList.ContainsKey(ClientIP))
                    {
                        this.SocketList[ClientIP].Closed();
                        this.SocketList.Remove(ClientIP);
                    }
                }

            }
        }

    }
}
