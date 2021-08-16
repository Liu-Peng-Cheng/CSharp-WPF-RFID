namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    using System;
    /// <summary>
    /// 把采集到的模板写入设备（指静脉模板下发）
    /// </summary>
    public class MsgObj_Finger_WriteTempleToDevice: MsgObjBase
    {
        byte[] userID;
        byte fingerID;
        byte[] templates;

        internal byte[] getUserID { get => userID;  }
        public byte[] setUserID { set => userID = value; }
        public string setUserIDStr { set {
                while (value.Length < 12) value = "0" + value;
                userID = PublicAPI.CKC001.Others.DataConverts.HexStr_To_Bytes(value);
            } }
        public byte setFingerID { set => fingerID = value; }
        public byte[] setFingerTemple { set => templates = value; }
        public string setTemplateStr { set {
                if (value.Length == 3072) templates = PublicAPI.CKC001.Others.DataConverts.HexStr_To_Bytes(value);
                else templates = null;
            } }

        public MsgObj_Finger_WriteTempleToDevice()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Finger;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eFinger.WriteTemplateToDevice;
        }
        internal override void SendPacked()
        {
            base.CmdData = new byte[1543];
            int flag = 0;
            Array.Copy(userID, 0, base.CmdData, flag, userID.Length);
            flag += 6;
            base.CmdData[flag++] = fingerID;
            Array.Copy(templates, 0, base.CmdData, flag, templates.Length);
        }
    }
}
