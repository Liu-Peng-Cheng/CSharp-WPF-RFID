using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public  class UdpObj_SetMask:UdpObjBase
    {
        string masek;
        public UdpObj_SetMask()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_NETMASK;
        }

        public string Masek {  set => masek = value; }

        internal override void SetParameter()
        {
            base.CmdLen = 4;
            var temp = Regex.Matches(masek, @"\d+");
            int temp1 = temp.Count;
            base.CmdData = new byte[temp1];
            for (int i = 0; i < temp1; i++)
            {
                base.CmdData[i] = byte.Parse(temp[i].ToString());
            }
        }
    }
}
