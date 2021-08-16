using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_SetGateWay:UdpObjBase
    {
        string gateWay;
        public UdpObj_SetGateWay()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_GATEWAY;
        }
        
        public string GateWay {  set => gateWay = value; }

        internal override void SetParameter()
        {
            base.CmdLen = 4;
            var temp = Regex.Matches(gateWay, @"\d+");
            int temp1 = temp.Count;
            base.CmdData = new byte[temp1];
            for (int i = 0; i < temp1; i++)
            {
                base.CmdData[i] = byte.Parse(temp[i].ToString());
            }
        }
    }
}
