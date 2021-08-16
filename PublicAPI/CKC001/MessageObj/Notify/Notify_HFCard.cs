using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class Notify_HFCard : NotifyBase
    {
        byte[] idNum;
        public byte[] getIDNumByte { get => idNum; internal set => idNum = value; }
        public string getIDNumStr { get =>idNum==null?"": DataConverts.Bytes_To_HexStr(idNum); }

        byte[] userData;
        public byte[] getUserDataByte { get => userData; internal set => userData = value; }
        public string getUserDataStr { get =>userData==null?"": DataConverts.Bytes_To_HexStr(userData); }

        internal Notify_HFCard(MsgObjBase msg, string ip)
        {
            base.NotifyAdditiveAttributeA(msg, ip);
        }
    }
}
