namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public  class MsgObj_LED_LightWorkMode:MsgObjBase
    {
        bool isAutoSwitch;
        /// <summary>
        /// true 开关锁自动开关灯  false用户自己控制开关灯
        /// </summary>
        public bool setLightIsAutoSwitch { internal get => isAutoSwitch; set => isAutoSwitch = value; }
        public MsgObj_LED_LightWorkMode()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.LED;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eLED.SetLightLedMode;
        }
        internal override void SendPacked()
        {
            base.CmdData = new byte[1] { (byte)(isAutoSwitch ? 0x00 : 0x01),};
        }
    }
}
