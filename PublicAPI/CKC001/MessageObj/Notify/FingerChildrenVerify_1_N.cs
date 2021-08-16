using PublicAPI.CKC001.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class FingerChildrenVerify_1_N
    {
        protected byte[] userID;
        protected byte fingerID;
        protected byte result;

        internal byte[] setUserID { set => userID = value; }
        public byte[] getUserID { get => userID; }
        public string getUserIDStr { get => userID==null?"":DataConverts.Bytes_To_HexStr(userID); }

        internal byte setFingerID { set => fingerID = value; }
        public byte getFingerID { get => fingerID; }

        internal byte setVerifyResult { set => result = value; }
        public byte getVerifyResult { get => result; }
    }
}
