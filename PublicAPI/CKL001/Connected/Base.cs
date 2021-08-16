using System;
using System.Collections.Generic; 
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Collections;
using System.IO.Ports;
using System.Diagnostics;

namespace PublicAPI.CKL001.Connected
{
    internal class Base: IDisposable
    {
        private PublicAPI.CKL001.Others.delegateMessageReceived dMsgReceived;

        protected PublicAPI.CKL001.Others.RingBuffer receivedRingBuffer;
        protected object SyncObject;
        public bool isConnected = false;
        protected byte[] readTempBuffer;
        public Stopwatch stopwatch;

        protected Base()
        {
            stopwatch = new Stopwatch();
            receivedRingBuffer = new PublicAPI.CKL001.Others.RingBuffer(1024*2);
            SyncObject = new object();
            readTempBuffer = new byte[512];
        }
        public void ReceivedCombineMethod(PublicAPI.CKL001.Others.delegateMessageReceived msgReceived) { dMsgReceived += msgReceived; }
        public void RececivedRemoveMethod(PublicAPI.CKL001.Others.delegateMessageReceived msgReceived) { dMsgReceived -= msgReceived; }

        protected void CallDelegateReceived(PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase messageObject)
        {
            try
            {
                if (this.dMsgReceived != null)
                {
                    this.dMsgReceived(messageObject);
                }
            }
            catch { }
        }
        protected void ReceivedDataSplit_AddToThreadPool()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReceivedDataSplit));
            }
            catch { }
        }
        [CompilerGenerated]
        protected void ReceivedDataSplit(object state)
        {
            while (this.isConnected)
            {
                byte[] receivedBytes = null;
                lock (SyncObject)
                {
                    try
                    {
                        if (receivedRingBuffer.DataCount < 8)
                        {
                            Monitor.Wait(SyncObject);
                            continue;
                        }
                        if (receivedRingBuffer[0] != 0x5A || receivedRingBuffer[1] != 0x08 || readTempBuffer[7] != 0x0D)
                        {
                            receivedRingBuffer.Clear(1);
                            continue;
                        }
                        receivedBytes = new byte[8];
                        receivedRingBuffer.ReadFromRingBuffer(receivedBytes, 0, 8);
                        receivedRingBuffer.Clear(8);
                        Monitor.Pulse(SyncObject);
                    }
                    
                    catch { }
            }
            if (receivedBytes != null)
            {
                PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase msg = new PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase(receivedBytes);
                if (msg.CheckData())
                {
                    CallDelegateReceived(msg);
                }
            }
            }
        }
        public virtual bool Connected(string readerName,string _8n1="8:n:1")
        {
            return false;
        }
  
        public virtual void Send(byte[] sendBytes)
        { 
        }
        public virtual void Send(PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase message)
        {
            if (message == null)
                return;            
            this.Send(message.FrameMsg);
        }
        public virtual void Closed()
        { }
        public virtual void Dispose()
        { }
    }
}
