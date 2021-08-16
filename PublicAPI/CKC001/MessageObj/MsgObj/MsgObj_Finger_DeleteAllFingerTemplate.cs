namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_Finger_DeleteAllFingerTemplate:MsgObjBase
    {
        public MsgObj_Finger_DeleteAllFingerTemplate()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Finger;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eFinger.DeleteAllTemplate;
        }
    }
}
