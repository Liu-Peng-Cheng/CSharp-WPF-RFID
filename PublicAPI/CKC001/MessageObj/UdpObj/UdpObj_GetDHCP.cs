using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_GetDHCP:UdpObjBase
    {
        public UdpObj_GetDHCP()
        {
            base.CmdType = eCmdType.GetParameter;
            base.CmdTag = eCmdTag.TAG_DHCP;
        }
    }
}
