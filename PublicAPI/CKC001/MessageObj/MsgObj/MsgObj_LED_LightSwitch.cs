namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_LED_LightSwitch:MsgObjBase
    {
        bool lightStatus;
        /// <summary>
        /// true 开灯  false关灯
        /// </summary>
        public bool setLightStatus {internal  get => lightStatus; set => lightStatus = value; }
        public MsgObj_LED_LightSwitch()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.LED;            
        }
        internal override void SendPacked()
        {
            base.CmdTag = (byte)(lightStatus ? 0x01:0x02);
        }
    }
}
