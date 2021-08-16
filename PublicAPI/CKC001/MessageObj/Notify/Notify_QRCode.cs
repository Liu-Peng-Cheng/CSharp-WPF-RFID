using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class Notify_QRCode: NotifyBase
    {
        byte[] RQCode;
        public byte[] getRQCodeByte { get => RQCode; }
        internal byte[] setRQCodeByte {  set => RQCode = value; }
        public string getRQCodeStr { get =>RQCode==null?"": DataConverts.Bytes_To_ASCII(RQCode);  }

        internal Notify_QRCode(MsgObjBase msg, string ip)
        {
            base.NotifyAdditiveAttributeA(msg, ip);
        }
    }
}
