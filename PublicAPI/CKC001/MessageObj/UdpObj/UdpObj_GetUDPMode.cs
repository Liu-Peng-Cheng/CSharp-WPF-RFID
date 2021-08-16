using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_GetUDPMode:UdpObjBase
    {
        public UdpObj_GetUDPMode()
        {
            base.CmdType = eCmdType.GetParameter;
            base.CmdTag = eCmdTag.TAG_UDP;
        }
    }
}
