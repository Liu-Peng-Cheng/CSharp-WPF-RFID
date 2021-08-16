using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.Others;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PublicAPI.CKC001.Connected.communication
{
    internal class TypeTcpClient:Base
    {
        internal IPAddress ipAddress;
        internal int ipPort;
        
        public bool TcpPingIpAddress(IPAddress iPAddress)
        {
            Ping ping = new Ping();
            PingOptions options = new PingOptions { DontFragment = true };
            string pingTest = "JustForTest.";
            byte[] bytes = Encoding.ASCII.GetBytes(pingTest);
            return (ping.Send(iPAddress, 1000, bytes, options).Status == IPStatus.Success);
        }

        public void TcpSocketKeepAlive(Socket socket)
        {
            try
            {
                byte[] array = new byte[Marshal.SizeOf(0) * 3];
                BitConverter.GetBytes((uint)1).CopyTo(array, 0);
                BitConverter.GetBytes((uint)0xbb8).CopyTo(array, Marshal.SizeOf(0));
                BitConverter.GetBytes(0xbb8).CopyTo(array, (int)(Marshal.SizeOf(0) * 2));
                socket.IOControl(IOControlCode.KeepAliveValues, array, null);
            }
            catch
            {
            }
        }

        public void ReceivedData_AddToThreadPool()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.Received));
        }

        [CompilerGenerated]
        private void Received(object object0)
        {
            bool isSend =true;
            while (base.IsReceived)
            {
                try
                {
                    if (base.socket.Poll(5, SelectMode.SelectRead))
                    {
                        if (isSend)
                        {
                            isSend = false;
                            base.CallDelegateDiconnected(this.DevIP);
                            base.dDisconnected = null;
                        }
                        continue;
                    }
                    int count = base.socket.Receive(this.readTempBuffer);
                    if (count > 0)
                    {
                        lock (base.SyncLock)
                        {
                            while ((count + this.receivedRingBuffer.DataCount) > 1024 * 1024)
                            {
                                Monitor.Wait(base.SyncLock, 10000);
                            }
                            this.receivedRingBuffer.WriteToRingBuffer(this.readTempBuffer, 0, count);
                            Monitor.PulseAll(base.SyncLock);
                        }
                        isSend = true;
                    }
                    continue;
                }
                catch (SocketException se)
                {
                    if (isSend)
                    {
                        isSend = false;
                        base.CallDelegateDiconnected(this.DevIP);
                        base.dDisconnected = null;
                        Console.WriteLine(string.Format("客户端：[{0}]异常：" + se.Message, this.DevIP));
                    }
                }
                catch (Exception ex){
                    Thread.Sleep(200);
                    Console.WriteLine(string.Format("客户端：[{0}]异常："+ ex.Message, this.DevIP)) ;
                }
            }
        }

        internal override bool Open(string readerName)
        {
            try
            {
                if ((base.socket != null) && base.socket.Connected)
                {
                    return true;
                }
                string[] strArray = readerName.Split(new char[':']);
                if (strArray.Length != 2)
                {
                    return false;
                }
                base.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.ipAddress = IPAddress.Parse(strArray[0]);
                this.ipPort = int.Parse(strArray[1]);
                if (!this.TcpPingIpAddress(this.ipAddress))
                { throw new Exception(string.Format("Ping {0} TimeOut.", this.ipAddress)); }
                base.socket.Connect(this.ipAddress, this.ipPort);
                this.TcpSocketKeepAlive(this.socket);
                base.IsReceived = true;
                this.ReceivedData_AddToThreadPool();
                base.ReceivedDataSplit_AddToThreadPool();
                
                return true;
            }
            catch
            {
                this.Closed();
                return false;
            }
        }

        internal override bool Open(Socket _socket)
        {
            try
            {
                if (_socket == null) return false;
                base.socket = _socket;
                base.DevIP = _socket.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
                this.TcpSocketKeepAlive(base.socket);
                base.IsReceived = true;
                this.ReceivedData_AddToThreadPool();
                base.ReceivedDataSplit_AddToThreadPool();

                return true;
            }
            catch
            {
                this.Closed();
                return false;
            }
        }

        internal override void Send(MsgObjBase msg)
        {
            if (msg != null)
            {
                this.Send(msg.CmdToBytes());
            }
        }

        internal override void Send(byte[] sendBytes)
        {
            TypeTcpClient TCPclient;
            Monitor.Enter(TCPclient = this);
            try
            {
                base.socket.Send(sendBytes);
            }
            catch
            { }
            finally
            {
                Monitor.Exit(TCPclient);
            }
        }

        internal override void Closed()
        {
            try
            {
                base.IsReceived = false;
                if (base.socket != null)
                {
                    base.socket.Close();
                    base.socket = null;
                }
                lock (base.SyncLock)
                {
                    Monitor.PulseAll(base.SyncLock);
                }
            }
            catch { }
        }

    }
}
