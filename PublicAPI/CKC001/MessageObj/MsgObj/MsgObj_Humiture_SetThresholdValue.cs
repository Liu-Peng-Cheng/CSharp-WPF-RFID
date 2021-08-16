namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_Humiture_SetThresholdValue:MsgObjBase
    {
        byte[] bytes;

        public byte[] setThresholdValue { internal get => bytes; set => bytes = value; }

        public MsgObj_Humiture_SetThresholdValue()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Humiture;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eHumiture.SetThresholdValue;
        }
        internal override void SendPacked()
        {
            base.CmdData = bytes;
        }
    }
}
