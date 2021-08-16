using System;
using System.Collections.Generic; 
using System.Text;

namespace PublicAPI.CKL001.MessageObj.MsgObj
{
    public class MsgObj_LED_Switch:MsgObjBase
    {
        private PublicAPI.CKL001.Others.eNum led;
        private bool isSwitchLED;
        public PublicAPI.CKL001.Others.eNum SetSwitchLEDNum { set => led = value; }
        public bool IsSwitchLED {  set => isSwitchLED = value; }

        public MsgObj_LED_Switch()
        {
        }
        public override void CmdPacked()
        {
            base.CmdType = (byte)(isSwitchLED ? 0x04 : 0x05);
            base.Status = PublicAPI.CKL001.Others.DataConvert.Ushort_To_Bytes((ushort)led);
        }

    }
}
