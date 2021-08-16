using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_GetGateWay : UdpObjBase
    {
        public UdpObj_GetGateWay()
        {
            base.CmdType = eCmdType.GetParameter;
            base.CmdTag = eCmdTag.TAG_GATEWAY;
        }
    }
}
