namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_HF_SwitchWorkingMode:MsgObjBase
    {
        bool isAutoRead;
        public bool setIsAutoReadHF {internal get => isAutoRead; set => isAutoRead = value; }

        public MsgObj_HF_SwitchWorkingMode()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.HFCard;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eHFCard.SetReadMode;
        }

        internal override void SendPacked()
        {
            base.CmdData = new byte[1] { (byte)(isAutoRead ? 0 : 1) };
        }
    }
}
