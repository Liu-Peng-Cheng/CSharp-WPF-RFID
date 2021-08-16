namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_RFID_StopRead:MsgObjBase
    {
        public MsgObj_RFID_StopRead()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.RFID;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eRFID.StopReadTags;
        }
    }
}
