namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_Humiture_SetWorkingMode : MsgObjBase
    {
        byte[] workMode;
        public byte[] setWorkMode{ set => workMode = value; }

        public MsgObj_Humiture_SetWorkingMode()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Humiture;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eHumiture.SetHumitureMode;
        }
        internal override void SendPacked()
        {
            base.CmdData = workMode;
        }

    }
}
