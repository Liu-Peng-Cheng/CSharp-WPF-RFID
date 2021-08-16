namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public  class MsgObj_Lock_OpenLock:MsgObjBase
    {
        public MsgObj_Lock_OpenLock()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Lock;
            base.CmdTag = 0x01;
        }
        internal override void SendPacked()
        {
           
        }
    }
}
