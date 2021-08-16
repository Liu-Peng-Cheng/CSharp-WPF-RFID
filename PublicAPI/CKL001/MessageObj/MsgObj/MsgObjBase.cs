using System;
using System.Collections.Generic; 
using System.Text;

namespace PublicAPI.CKL001.MessageObj.MsgObj
{
    [Serializable]
    public class MsgObjBase
    {
        protected const byte HEAD = 0x5A;

        private byte address;
        private byte cmdType;
        private byte[] status;
        private byte crc;
        protected const byte END = 0x0D;
        private const byte LEN = 0x08;
        private byte[] toBCC;
        private byte[] frameMsg;


        internal byte Address { get => address; set => address = value; }
        internal byte CmdType { get => cmdType; set => cmdType = value; }
        internal byte[] Status { get => status; set => status = value; }
        //public byte CRC { get => crc; set => crc = value; }
        internal byte[] FrameMsg { get => frameMsg; set => frameMsg = value; }

        internal byte[] ToBCC { get => toBCC; set => toBCC = value; }

        public MsgObjBase()
        {
            status = new byte[2];
        }

        public MsgObjBase(byte[] receivedData)
        {
            try
            {
                this.frameMsg = receivedData;
                this.toBCC = new byte[6];
                Array.Copy(receivedData, 0, toBCC, 0, toBCC.Length);
                this.address = receivedData[2];
                this.cmdType = receivedData[3];
                this.status = new byte[2] { receivedData[4],receivedData[5]};
                this.crc = receivedData[6];
            }
            catch 
            { }
        }

        public virtual void CmdPacked()
        {
        }
                
        public byte[] CmdToBytes()
        {
            this.frameMsg = new byte[8];
            this.toBCC = new byte[6];
            toBCC[0] = HEAD;
            toBCC[1] = LEN;
            toBCC[2] = address;
            toBCC[3] = cmdType;
            Array.Copy(status, 0, toBCC, 4, 2);
            Array.Copy(toBCC, 0, frameMsg, 0, toBCC.Length);
            frameMsg[6] = crc = PublicAPI.CKL001.Others.DataConvert.BccCheck(toBCC);
            frameMsg[7] = END;
            return this.frameMsg;
        }

        public bool CheckData()
        {
            if (this.toBCC != null)
            {
                byte tempBcc = PublicAPI.CKL001.Others.DataConvert.BccCheck(toBCC);
                if (tempBcc == this.crc) 
                    return true;  
            }
            return false;
        }

        public byte ToKey()
        {
            if (CmdType  == 0x02)
                return 0x01;
            else
            return cmdType;
        }
    }
}
