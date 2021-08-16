using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_GetNetWorkingMode:UdpObjBase
    {
        public UdpObj_GetNetWorkingMode()
        {
            base.CmdType = eCmdType.GetParameter;
            base.CmdTag = eCmdTag.TAG_WMODE;
        }
    }
}
