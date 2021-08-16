namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_RFID_ReadTags:MsgObjBase
    {
        byte readTimes;
        public byte setReadTimes { set => readTimes = value; }

        public MsgObj_RFID_ReadTags()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.RFID;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eRFID.StartReadTags;
        }

        internal override void SendPacked()
        {
            base.CmdData = new byte[1] { readTimes };
        }
    }
}
