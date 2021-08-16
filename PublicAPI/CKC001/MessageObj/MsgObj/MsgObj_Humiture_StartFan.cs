namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_Humiture_StartFan : MsgObjBase
    {
        public MsgObj_Humiture_StartFan()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Humiture;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eHumiture.StartTheFan;
        }
    }
}
