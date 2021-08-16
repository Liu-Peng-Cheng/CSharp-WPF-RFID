using System;
using System.Collections.Generic; 
using System.Text;

namespace PublicAPI.CKL001.MessageObj.MsgObj
{
    public class MsgObj_Lock_GetStatus:MsgObjBase
    {

        public MsgObj_Lock_GetStatus()
        {
            base.CmdType = 0x03;
        }

        public override void CmdPacked()
        {
            base.Status = new byte[2] { 0xFF,0xFF };
        }


    }
}
