namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_HeartBeat:MsgObjBase
    {
        public MsgObj_HeartBeat()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.HartBeat;
            base.CmdTag = 0x02;
        }
    }
}
