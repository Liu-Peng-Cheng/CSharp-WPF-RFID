namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    /// <summary>
    /// 应答指静脉的采集次数上报
    /// </summary>
    public class MsgObj_Finger_GatherTemplatesACK:MsgObjBase
    {
        /// <summary>
        /// 第几次采集
        /// </summary>
        public byte setAckCollectCount {internal get; set; }
        public MsgObj_Finger_GatherTemplatesACK()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Finger;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eFinger.GatherTemplateACK;
            base.CmdData = new byte[1];
        }
        internal override void SendPacked()
        {
            CmdData[0] = setAckCollectCount;
        }
    }
}
