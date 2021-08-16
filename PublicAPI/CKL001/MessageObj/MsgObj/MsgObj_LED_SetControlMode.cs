using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKL001.MessageObj.MsgObj
{
    public class MsgObj_LED_SetControlMode:MsgObjBase
    {
        private bool isAutoSwitchLight;
        public bool IsAutoSwitchLight { set => isAutoSwitchLight = value; }

        public MsgObj_LED_SetControlMode()
        {
            base.CmdType = 0x06;
        }
        public override void CmdPacked()
        {
            base.Status = new byte[2] { 0x00, (byte)(isAutoSwitchLight ? 01 : 02) };
        }

    }
}
