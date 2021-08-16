using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public  class UdpObj_SetNetWorkingMode:UdpObjBase
    {
        bool isTCP = true;
        //public bool IsTCP { set => isTCP = value; }

        internal override void SetParameter()
        {
            base.CmdLen = 0x01;
            base.CmdData = new byte[1] { (byte)(isTCP ? 0x00 : 0x01) };
        }

        public UdpObj_SetNetWorkingMode()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_WMODE;
        }
    }
}
