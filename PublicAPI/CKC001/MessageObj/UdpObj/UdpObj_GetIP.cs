using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_GetIP : UdpObjBase
    {
        public UdpObj_GetIP()
        {
            base.CmdType = eCmdType.GetParameter;
            base.CmdTag = eCmdTag.TAG_IP;
        }
    }
}
