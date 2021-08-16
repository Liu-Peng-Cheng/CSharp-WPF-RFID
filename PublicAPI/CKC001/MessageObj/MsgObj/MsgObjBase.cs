
using PublicAPI.CKC001.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public class MsgObjBase
    {
        private byte[] headFlag;
        private byte addressNum;
        private byte[] serialNum;
        private eCmdType cmdType;
        private byte cmdTag;
        private eReturnResult ereturn;
        private byte[] cmdLenByte;
        private byte[] cmdData;
        private byte[] crc;
        private byte[] frameData;

        internal byte[] HeadFlag { get => headFlag; set => headFlag = value; }
        internal byte AddressNum { get => addressNum; set => addressNum = value; }
        internal byte[] SerialNum { get => serialNum; set => serialNum = value; }
        internal eCmdType CmdType { get => cmdType; set => cmdType = value; }
        internal byte CmdTag { get => cmdTag; set => cmdTag = value; }
        internal eReturnResult eReturn { get => ereturn;  set => ereturn = value; }
        //public ushort CmdLen { get => cmdLen; set => cmdLen = value; }
        internal byte[] CmdLenByte { get => cmdLenByte; set => cmdLenByte = value; }
        internal byte[] CmdData { get => cmdData; set => cmdData = value; }
        internal byte[] Crc { get => crc; set => crc = value; }
        internal byte[] FrameData { get => frameData; set => frameData = value; }

        public MsgObjBase()
        {
            headFlag = new byte[2] { 0x17,0x99};
            addressNum = 0x00;
            cmdData = null;
            ereturn = eReturnResult.NonResponse;
            serialNum = new byte[4];
        }
        public MsgObjBase(byte[] receivedData)
        {
            frameData = receivedData;
            int flag = 0;
            headFlag = new byte[2] { receivedData[flag], receivedData[flag + 1] };
            flag += 2;
            addressNum = receivedData[flag++];
            serialNum = new byte[4] { receivedData[flag], receivedData[flag + 1], receivedData[flag+2], receivedData[flag + 3] };
            flag += 4;
            cmdType = (eCmdType)receivedData[flag++];
            cmdTag = receivedData[flag++];
            ereturn = (eReturnResult)receivedData[flag++];
            cmdLenByte = new byte[2] { receivedData[flag], receivedData[flag+1] };
            flag += 2;
            ushort len = DataConverts.Bytes_To_Ushort(cmdLenByte);
            if (len > 0)
            {
                cmdData = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    cmdData[i] = receivedData[flag + i];
                }
            }
            else
                cmdData = null;
            flag += len;
            crc = new byte[2] { receivedData[flag], receivedData[flag + 1] };
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        internal virtual void SendPacked() { }

        /// <summary>
        /// 把属性打包成数组
        /// </summary>
        /// <returns></returns>
        internal byte[] CmdToBytes()
        {
            SendPacked();
            List<byte> tempCmd = new List<byte>();
            tempCmd.AddRange(headFlag);
            tempCmd.Add(addressNum);
            tempCmd.AddRange(serialNum);
            tempCmd.Add((byte)cmdType);
            tempCmd.Add(cmdTag);
            if (cmdData == null)
            {
                tempCmd.AddRange(new byte[2] { 0x00, 0x00 });
            }
            else
            {
                tempCmd.AddRange(DataConverts.Int_To_Bytes((ushort)cmdData.Length));
                tempCmd.AddRange(cmdData);
            }
            byte[] tempBytes = new byte[tempCmd.Count];
            frameData = new byte[tempCmd.Count + 2];
            for (int i = 0; i < tempCmd.Count; i++)
            {
                tempBytes[i] = frameData[i] = tempCmd[i];
            }
            byte[] tempCrc = DataConverts.Make_CRC16(tempBytes);
            frameData[frameData.Length - 2] = tempCrc[0];
            frameData[frameData.Length - 1] = tempCrc[1];
            return frameData;
        }
        internal bool CheckCRC()
        {
            if (frameData.Length < 13)
                return false;
            byte[]tempData = DataConverts.ReadBytes(frameData, 0, frameData.Length - 2);
            byte[] temp = DataConverts.Make_CRC16(tempData);
            if (temp[0] == crc[0] && temp[1] == crc[1])
            {
                return true;
            }
            return false;
        }

        internal string ToKey()
        {
            byte[] temp = new byte[7] { addressNum, serialNum[0], serialNum[1], serialNum[2], serialNum[3] , (byte)cmdType , cmdTag };
            return DataConverts.Bytes_To_HexStr(temp);
        }
    }
}
