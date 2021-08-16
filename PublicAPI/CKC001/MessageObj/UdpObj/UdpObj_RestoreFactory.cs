using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_RestoreFactory:UdpObjBase
    {
        public UdpObj_RestoreFactory()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_RESTOREFACTORY;
        }
    }
}
