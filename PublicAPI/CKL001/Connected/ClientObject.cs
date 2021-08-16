using System;
using System.Collections.Generic;
 
using System.Text;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace PublicAPI.CKL001.Connected
{
    public class ClientObject
    {
        private PublicAPI.CKL001.Others.delegateMessageReceived dMsgReceived;
        public bool LinkStatus;
        private Base BaseConnect;
        public string ReaderName { get; private set; }

        public byte Address { get; set; }
        public PublicAPI.CKL001.Others.delegateNotifyMessage dNotifyMessage;


        public ClientObject()
        {
            dMsgReceived += this.SortMessage;
        }
        public bool Close()
        {
            try
            {
                if (BaseConnect != null)
                {
                    LinkStatus = false;
                    BaseConnect.isConnected = false;
                    BaseConnect.Closed();
                    BaseConnect.RececivedRemoveMethod(dMsgReceived);
                   // BaseConnect.RececivedRemoveMethod(new PublicAPI.CKL001.Others.delegateMessageReceived(this.SortMessage));
                    BaseConnect = null;
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool Open(string readerName,string _8n1="8:n:1")
        {
            BaseConnect = new TypeSerial();
            BaseConnect.ReceivedCombineMethod(dMsgReceived);
            //BaseConnect.ReceivedCombineMethod(new PublicAPI.CKL001.Others.delegateMessageReceived(this.SortMessage));
            if (BaseConnect.Connected(readerName,_8n1))
            {
                this.ReaderName = readerName;
                BaseConnect.isConnected = true;
                LinkStatus = true;
                return true;
            }
            else
            {
                this.Close();
                return false;
            }
        }

        public void Send(PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase msgObject)
        {
            if (msgObject != null)
            {
                msgObject.Address = this.Address;
                msgObject.CmdPacked();
                msgObject.CmdToBytes();
                if (dNotifyMessage != null)
                {
                    dNotifyMessage(new MessageObj.Notify.Notify_Message(msgObject)
                    {
                        IsRcv = false,
                    });
                }
                this.BaseConnect.Send(msgObject);
            }
        }
        public void Send(byte[] msgBytes)
        {
            if (msgBytes != null)
            {
                if (dNotifyMessage != null)
                {
                    dNotifyMessage(new MessageObj.Notify.Notify_Message(new MessageObj.MsgObj.MsgObjBase(msgBytes))
                    {
                        IsRcv = false,
                    });
                }
                this.BaseConnect.Send(msgBytes);
            }
        }

        private void SortMessage(PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase msgObject)
        {
            if (msgObject == null)
                return;
            if (dNotifyMessage != null)
            {
                dNotifyMessage(new MessageObj.Notify.Notify_Message(msgObject)
                {
                    IsRcv = true,
                });
            }
        }
    }
}
