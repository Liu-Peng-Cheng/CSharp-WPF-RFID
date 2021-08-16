namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_HF_ReadIDNum:MsgObjBase
    {
        public MsgObj_HF_ReadIDNum()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.HFCard;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eHFCard.GetIdCardNum;
        }
    }
}
