using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using PublicAPI.CKC001.MessageObj;
using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.MessageObj.Notify;
using PublicAPI.CKC001.Others;
using Newtonsoft.Json.Linq;

namespace PublicAPI.CKC001.Connected
{
    public class ClientObject
    {
        public delegateNotifyLock dNotifyLock;
        public delegateNotifyLight dNotifyLight;
        public deleageNotifyFinger dNotifyFinger;
        public delegateNotifyHF dNotifyHF;
        public delegateNotifyRQCode dNotifyRQCode;
        public delegateNotifyRFID dNotifyRFID;
        public delegateNotifyAbnormal dNotifyAbnormal;
        public delegateNotifyHeartBeat dHeartBeat;
        public delegateNotifyDeviceOnline dNotifyDeviceOnline;
        public delegateNotifyHumiture dNotifyHumiture;
        public delegateNotifyContainerNum dNotifyContainerNum;
        public delegateNotifyLog dNotifyLog;

        internal PublicAPI.CKC001.Connected.communication.Base _base;
        private Dictionary<string, ManualReset> dictionary;
        public string DevIP { get { return _base.DevIP; } }
        private byte AddressNum = 0;
        private byte[] SerialNum = null;
        public ClientObject()
        {
            dictionary = new Dictionary<string, ManualReset>();
        }
        public bool Closed()
        {
            try
            {
                if (_base != null)
                {
                    _base.Closed();
                    _base.ReceivedRemoveMethod(new delegateMessageReceived(this.SortMessage));
                    _base = null;
                    return true;
                }
            }
            catch { }
            return false;
        }

        internal bool Connected(PublicAPI.CKC001.Connected.communication.Base connectedType)
        {
            if (connectedType != null)
            {
                this._base = connectedType;
                this._base.ReceivedCombineMethod(new delegateMessageReceived(this.SortMessage));
                if (this._base.IsReceived)
                {
                    return true;
                }
                try
                {
                    this._base.Closed();
                    this._base.ReceivedRemoveMethod(new delegateMessageReceived(this.SortMessage));
                }
                catch { return false; }
            }
            return false;
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        public void SendAsync(MsgObjBase msgObj)
        {
            if (msgObj != null)
            {
                msgObj.AddressNum = this.AddressNum;
                msgObj.SerialNum = this.SerialNum==null?new byte[4]:this.SerialNum;
                msgObj.SendPacked();
                this._base.Send(msgObj);
                LogOnNotify(false, msgObj);
            }
        }
        /// <summary>
        /// 异步发送
        /// </summary>
        public void SendAsync(byte[] cmdBytes)
        {
            if (cmdBytes != null)
            {
                this._base.Send(cmdBytes);
                try
                {
                    LogOnNotify(false,new MsgObjBase(cmdBytes));
                }
                catch
                { }
            }
        }

        private void LogOnNotify(bool isRecv, MsgObjBase msg)
        {
            if (dNotifyLog != null)
            {
                dNotifyLog(new Notify_Log(msg, this.DevIP) { getIsRecv = isRecv });
            }
        }

        private void SortMessage(MsgObjBase msg)
        {
            try
            {
                if (msg == null) return;
                LogOnNotify(true, msg);
                switch (msg.CmdType)
                {
                    case eCmdType.Lock:
                        if (dNotifyLock != null)
                            dNotifyLock(new MessageObj.Notify.Notify_Lock(msg, this.DevIP) {
                                getLockStatus = msg.CmdData,
                            });
                        break;
                    case eCmdType.LED:
                        if (dNotifyLight != null)
                            dNotifyLight(new MessageObj.Notify.Notify_Light(msg, this.DevIP) {
                            
                            });
                        break;
                    case eCmdType.Finger:
                        MessageObj.Notify.Notify_Finger notify_Finger = new Notify_Finger(msg, this.DevIP);
                        switch (msg.CmdTag) 
                        {
                            case 0x02:
                                notify_Finger.getNotifyVerifyResult = new FingerChildrenVerify_1_N() {
                                    setUserID = DataConverts.ReadBytes(msg.CmdData, 0, 6),
                                    setFingerID = msg.CmdData[6],
                                    setVerifyResult = msg.CmdData[7],
                                };
                                break;
                            case 0x04:
                                FingerChildrenAckGatherResult tempFingerGather = new FingerChildrenAckGatherResult();
                                tempFingerGather.setGatherCount = msg.CmdData[0];
                                tempFingerGather.setResult = msg.CmdData[1];
                                if (msg.CmdData[0] == 0x03 && msg.CmdData[1] == 0x00)
                                {
                                    tempFingerGather.setFingerTemplatesByte = DataConverts.ReadBytes(msg.CmdData, 2, msg.CmdData.Length - 2);
                                }
                                notify_Finger.getNotifyGatherResult = tempFingerGather;

                                this.SendAsync(new MsgObj_Finger_GatherTemplatesACK()
                                {
                                    setAckCollectCount = msg.CmdData[0],
                                }) ;
                                break;
                        }
                        if (dNotifyFinger != null)
                            dNotifyFinger(notify_Finger);
                        break;
                    case eCmdType.HFCard:
                        if (dNotifyHF != null)
                            dNotifyHF(new Notify_HFCard(msg, this.DevIP) {
                                getIDNumByte = (msg.CmdTag ==0x01)?  msg.CmdData:null,
                                getUserDataByte = (msg.CmdTag ==0x02)?  msg.CmdData:null,
                            });
                        break;
                    case eCmdType.QRCode:
                        if (dNotifyRQCode != null) 
                            dNotifyRQCode(new Notify_QRCode(msg, this.DevIP) {
                                setRQCodeByte = msg.CmdData,
                            });
                        break;
                    case eCmdType.RFID:
                        Notify_RFID notify_RFID = new Notify_RFID(msg, this.DevIP) { };
                        if (msg.CmdData != null)
                        {
                            Tags tags = null;
                            switch ((eRFID)msg.CmdTag)
                            {
                                case eRFID.NotifyReadData:
                                    string jsonStr = DataConverts.Bytes_To_ASCII(msg.CmdData);
                                    JObject ob = JObject.Parse(jsonStr);
                                    tags = new Tags();
                                    tags.all_tag_num = (int)ob["all_tag_num"];
                                    tags.add_tag_num = (int)ob["add_tag_num"];
                                    tags.loss_tag_num = (int)ob["delete_tag_num"];
                                    if (tags.add_tag_num > 0)
                                    {
                                        tags.add_tag_list = new List<_tag>();
                                        foreach (JObject jo in ob["add_tag_list"])
                                        {
                                            tags.add_tag_list.Add(new _tag() { epc = jo["epc"].ToString() });
                                        }
                                    }
                                    if (tags.loss_tag_num > 0)
                                    {
                                        tags.loss_tag_list = new List<_tag>();
                                        foreach (JObject jo in ob["delete_tag_list"])
                                        {
                                            tags.loss_tag_list.Add(new _tag() { epc = jo["epc"].ToString()});
                                        }
                                    }
                                    break;
                                case eRFID.GetAllTags:
                                    string jsonStr2 = DataConverts.Bytes_To_ASCII(msg.CmdData);
                                    Console.WriteLine(jsonStr2);
                                    JObject ob2 = JObject.Parse(jsonStr2);
                                    Console.WriteLine(ob2);
                                    tags = new Tags();
                                    tags.all_tag_num = (int)ob2["all_tag_num"];
                                    tags.all_tag_list = new List<_tag>();
                                    foreach (JObject jo in ob2["tag_list"])
                                    {
                                        tags.all_tag_list.Add(new _tag() { epc = jo["epc"].ToString()});
                                    }
                                    break;
                            }
                            notify_RFID.setTags = tags;
                        }
                        if (dNotifyRFID != null)
                            dNotifyRFID(notify_RFID);
                        break;
                    case eCmdType.Warnning:
                        if (dNotifyAbnormal != null)
                            dNotifyAbnormal(new Notify_Error_Alarm_Abnormal(msg, this.DevIP) { 
                            
                            });
                        break;
                    case eCmdType.HartBeat:
                        this.SendAsync(new MsgObj_HeartBeat());
                        if (dHeartBeat != null)
                            dHeartBeat(new Notify_HeartBeat(msg, this.DevIP) {
                            
                            });
                        break;
                    case eCmdType.Online:
                        {                           
                            SerialNum = msg.SerialNum;
                            AddressNum = msg.AddressNum;
                            if (dNotifyDeviceOnline != null)
                                dNotifyDeviceOnline(new Notify_DevOnline(msg, this.DevIP){
                                    setDevAddress = this.AddressNum,
                                        setDevIPByte = DataConverts.ReadBytes(msg.CmdData, 0, 4),
                                        setMacByte = DataConverts.ReadBytes(msg.CmdData, 4, 6),
                                        setDevType = DataConverts.ReadBytes(msg.CmdData, 10, msg.CmdData.Length - 10),
                                });
                            break;
                        }
                    case eCmdType.Humiture:
                        Notify_Humiture notify_Humiture = new Notify_Humiture(msg, this.DevIP) { };
                        switch ((eHumiture)msg.CmdTag)
                        {
                            case eHumiture.NotifyHumiture:
                                ushort tempUshort1 = DataConverts.Bytes_To_Ushort(new byte[2] { msg.CmdData[0], msg.CmdData[1] });
                                notify_Humiture.setTemperature = Math.Round(((double)tempUshort1 / 10), 1);
                                tempUshort1 = DataConverts.Bytes_To_Ushort(new byte[2] { msg.CmdData[2], msg.CmdData[3] });
                                notify_Humiture.setHumidity = Math.Round(((double)tempUshort1 / 10), 1);
                                break;
                        }
                        if (dNotifyHumiture != null)
                            dNotifyHumiture(notify_Humiture);
                        break;
                    case eCmdType.Address:
                        if (dNotifyContainerNum != null)
                            dNotifyContainerNum(new Notify_ContainerNum(msg, this.DevIP){
                                getNewAddress = this.AddressNum = msg.AddressNum,
                            }) ;
                        break;
                }
            }
            catch { }
        }

        public void NotifyDisconnected(delegateDisconnected disconnected)
        {
            try
            {
                if (disconnected != null)
                {
                    _base.DisconnectedCombineMethod(disconnected);
                }
            }
            catch(Exception ex)
            { }
        }
    }
}
