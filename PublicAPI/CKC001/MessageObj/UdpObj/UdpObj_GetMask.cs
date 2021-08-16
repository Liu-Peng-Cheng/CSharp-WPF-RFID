using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public  class UdpObj_GetMask:UdpObjBase
    {
        public UdpObj_GetMask()
        {
            base.CmdType = eCmdType.GetParameter;
            base.CmdTag = eCmdTag.TAG_NETMASK;
        }
    }
}
