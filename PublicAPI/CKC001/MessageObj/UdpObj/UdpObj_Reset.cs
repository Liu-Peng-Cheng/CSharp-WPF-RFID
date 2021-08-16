using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_Reset:UdpObjBase
    {
        public UdpObj_Reset()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_RESET;
        }
    }
}
