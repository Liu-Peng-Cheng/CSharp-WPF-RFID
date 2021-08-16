using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_WakeUpAllDev:UdpObjBase
    {
        public UdpObj_WakeUpAllDev()
        {
            base.CmdType = eCmdType.PasswordVerify;
            base.CmdTag = eCmdTag.TAG_NULL;
        }
    }
}
