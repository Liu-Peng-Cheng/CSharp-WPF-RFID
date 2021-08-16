using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class Notify_DevOnline: NotifyBase
    {
        byte[] ip;
        byte[] mac;
        byte[] devType;
        byte devAddress;

        public byte[] getDevIPByte { get => ip; }
        internal byte[] setDevIPByte { set => ip = value; }
        public string getDevIPBStr { get => ip==null?"":(ip[0] + "." + ip[1] + "." + ip[2] + "." + ip[3]); }

        public byte[] getMacByte { get => mac; }
        internal byte[] setMacByte { set => mac = value; }
        public string getMacStr { get => mac==null?"":(mac[0].ToString("X2") + ":" + mac[1].ToString("X2") + ":" + mac[2].ToString("X2") + ":" + mac[3].ToString("X2") + ":" + mac[4].ToString("X2") + ":" + mac[5].ToString("X2")); }

        public byte[] getDevType { get => devType; }
        internal byte[] setDevType { set => devType = value; }
        public string getTypeStr { get => devType == null?"":DataConverts.Bytes_To_HexStr(devType); }

        public byte getDevAddress { get => devAddress; }
        internal byte setDevAddress { set => devAddress = value; }
        internal Notify_DevOnline(MsgObjBase msg, string ip)
        {
            base.NotifyAdditiveAttributeA(msg, ip);
        }
    }
}
