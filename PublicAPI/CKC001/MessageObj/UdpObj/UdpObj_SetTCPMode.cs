using PublicAPI.CKL001.Others;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_SetTCPMode:UdpObjBase
    {
        string ip;
        ushort port;

        public UdpObj_SetTCPMode()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_TCP;
        }

        public string Ip { get => ip; set => ip = value; }
        public ushort Port { get => port; set => port = value; }

        internal override void SetParameter()
        {
            base.CmdLen = 0x82;
            base.CmdData = new byte[0x82];
            var temp = Regex.Matches(ip, @"\d+");
            int temp1 = temp.Count;
            string StrIP = "";
            for (int i = 0; i < temp1; i++)
            {
                StrIP+= temp[i].ToString()+".";
            }
            StrIP = StrIP.TrimEnd('.');
            byte[] byteIP = Encoding.ASCII.GetBytes(StrIP);
            Array.Copy(byteIP, 0, base.CmdData, 0, byteIP.Length);
            byte[] lenByte = DataConvert.Ushort_To_Bytes(port);
            base.CmdData[64] = lenByte[0];
            base.CmdData[65] = lenByte[1];
        }

    }
}
