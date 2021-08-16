using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public  class UdpObj_SetDNS:UdpObjBase
    {
        string dns;        
        public UdpObj_SetDNS()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_DNS;
            
        }
        internal override void SetParameter()
        {
            base.CmdLen = 0x04;
            var temp = Regex.Matches(dns, @"\d+");
            int temp1 = temp.Count;
            base.CmdData = new byte[temp1];
            for (int i = 0; i < temp1; i++)
            {
                base.CmdData[i] = byte.Parse(temp[i].ToString());
            }
        }
        public string DNS { set => dns = value; }
    }
}
