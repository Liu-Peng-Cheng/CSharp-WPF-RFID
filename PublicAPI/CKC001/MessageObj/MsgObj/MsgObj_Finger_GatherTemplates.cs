namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    /// <summary>
    /// 开始采集指静脉特征
    /// </summary>
    public class MsgObj_Finger_GatherTemplates:MsgObjBase
    {
        public MsgObj_Finger_GatherTemplates()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Finger;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eFinger.GatherTemplate;
        }
    }
}
