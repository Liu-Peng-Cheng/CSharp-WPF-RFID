namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObj_Finger_DeleteSingleTemplate:MsgObjBase
    {
        byte[] userID;
        /// <summary>
        /// 6 byte
        /// </summary>
        public byte[] setUserIDByte { set => userID = value; }
        /// <summary>
        /// String.Length = 12
        /// </summary>
        public string setUserIDStr { set {
                while (value.Length < 12) value = "0" + value;
                userID = PublicAPI.CKC001.Others.DataConverts.HexStr_To_Bytes(value); } }

        public MsgObj_Finger_DeleteSingleTemplate()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Finger;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eFinger.DeleteSingleTemplate;
        }

        internal override void SendPacked()
        {
            if (userID != null)
                base.CmdData = userID;
            else
                userID = new byte[6];
        }
    }
}
