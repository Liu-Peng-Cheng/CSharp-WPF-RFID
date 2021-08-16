using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_SetDHCP:UdpObjBase
    {
        bool isAutoGetIP;

        public UdpObj_SetDHCP()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_DHCP;
        }

        public bool IsAutoGetIP { set => isAutoGetIP = value; }

        internal override void SetParameter()
        {
            base.CmdLen = 0x04;
            base.CmdData = new byte[4] { (byte)(isAutoGetIP ? 0x01 : 0x00), 0x00, 0x00,0x00};
        }
    }
}
