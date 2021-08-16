using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_SetIP:UdpObjBase
    {
        string ip;
        public UdpObj_SetIP()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_IP;
        }

        public string SetIp { set => ip = value; }

        internal override void SetParameter()
        {
            base.CmdLen = 4;
            var temp = Regex.Matches(ip, @"\d+");
            int temp1 = temp.Count;
            base.CmdData = new byte[temp1];
            for (int i = 0; i < temp1; i++)
            {
                base.CmdData[i] = byte.Parse(temp[i].ToString());
            }
        }

    }
}
