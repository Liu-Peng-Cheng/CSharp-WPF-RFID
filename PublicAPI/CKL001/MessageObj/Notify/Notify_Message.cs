using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKL001.MessageObj.Notify
{
    public class Notify_Message
    {
        private byte cmdType;
        private byte address;
        private byte[] status;
        private string frameDataStr;
        private bool isRcv;

        public byte CmdType { get => cmdType;internal  set => cmdType = value; }
        public byte Address { get => address; internal set => address = value; }
        public byte[] Status { get => status; internal set => status = value; }
        public string FrameDataStr { get => frameDataStr; internal set => frameDataStr = value; }
        public bool IsRcv { get => isRcv;internal set => isRcv = value; }

        public Notify_Message(PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase msg)
        {
            cmdType = msg.CmdType;
            address = msg.Address;
            status = msg.Status;
            frameDataStr = PublicAPI.CKL001.Others.DataConvert.Bytes_To_HexStr(msg.FrameMsg);
        }
    }
}
