using PublicAPI.CKC001.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class FingerChildrenAckGatherResult
    {
        byte gatherCount;
        byte result;
        byte[] templates;

        public byte getGatherCount { get => gatherCount; }
        internal byte setGatherCount { set => gatherCount = value; }

        public byte getResult { get => result; }
        internal byte setResult { set => result = value; }

        public byte[] getFingerTemplatesByte { get => templates; }
        internal byte[] setFingerTemplatesByte { set => templates = value; }
        public string getFingerTemplatesStr { get => templates==null?"":DataConverts.Bytes_To_HexStr(templates); }

       
    }
}
