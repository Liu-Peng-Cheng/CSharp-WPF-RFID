using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class UdpObj_RePower:UdpObjBase
    {
        private string reName;
        public UdpObj_RePower()
        {
            base.CmdType = eCmdType.SetParameter;
            base.CmdTag = eCmdTag.TAG_POWERUPTIME;
        }
        internal override void SetParameter()
        {
            byte[] cc =  Encoding.Default.GetBytes(reName);
        }
        public string ReName { set => reName = value; }
    }
}
