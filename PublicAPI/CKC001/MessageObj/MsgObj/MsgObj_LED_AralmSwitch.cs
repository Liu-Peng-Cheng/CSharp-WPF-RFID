namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_LED_AralmSwitch:MsgObjBase
    {
        bool lightStatus;
        /// <summary>
        /// true 开报警  false关报警灯
        /// </summary>
        public bool setAlarmStatus { internal get => lightStatus; set => lightStatus = value; }
        public MsgObj_LED_AralmSwitch()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.LED;
        }
        internal override void SendPacked()
        {
            base.CmdTag = (byte)(lightStatus ? 0x03 : 0x04);
        }
    }
}
