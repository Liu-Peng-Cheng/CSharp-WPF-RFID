using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_SetDevName:UdpObjBase
    {
        string name;
        public string SetName {  set => name = value; }

        public UdpObj_SetDevName()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_NAME;
        }

        internal override void SetParameter()
        {
            byte[] temp = Encoding.ASCII.GetBytes(name);
            base.CmdLen = (ushort)temp.Length ;
            base.CmdData = temp;
        }

    }
}
