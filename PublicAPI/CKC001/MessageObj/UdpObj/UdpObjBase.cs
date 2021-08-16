using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Management;
using System.Security.Cryptography;
using PublicAPI.CKC001.Others;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{

    public class UdpObjBase
    {
        byte[] header;
        eCmdType cmdType;
        byte[] pcID;
        byte[] devSN;
        byte cmdResult;
        eCmdTag cmdTag;
        ushort cmdLen;
        byte[] cmdData;
        byte[] frameData;

        //帧头
        internal byte[] Header { get => header;  set => header = value; }
        //命令类型
        public eCmdType CmdType { get => cmdType; internal set => cmdType = value; }
        //PCID-自己设定
        internal byte[] PCID { get => pcID;  set => pcID = value; }
        //设备序列号-板子提供
        public byte[] DevSN { get => devSN; internal set => devSN = value; }
        //操作返回结果
        public byte CmdResult { get => cmdResult; internal set => cmdResult = value; }
        //命令Tag
        public eCmdTag CmdTag { get => cmdTag; internal set => cmdTag = value; }
        //命令长度
        internal ushort CmdLen { get => cmdLen;  set => cmdLen = value; }
        //命令类型
        public byte[] CmdData { get => cmdData; internal set => cmdData = value; }
        //一帧UDP
        public byte[] FrameData { get => frameData; internal set => frameData = value; }


        public UdpObjBase()
        {
            header = new byte[2] { 0x16, 0x98 };           
            cmdLen = 0;
            cmdData = null;
        }

        public UdpObjBase(byte[] param )
        {
            frameData = param;
            int flag = 0;
            header = DataConverts.ReadBytes(param, flag, 2);
            flag += 2;
            cmdType = (eCmdType)param[flag++];
            pcID = DataConverts.ReadBytes(param, flag, 4);
            flag += 4;
            devSN = DataConverts.ReadBytes(param, flag, 4);
            flag += 4;
            cmdResult = param[flag++];
            cmdTag = (eCmdTag)param[flag++];
            cmdLen = DataConverts.Bytes_To_Ushort(DataConverts.ReadBytes(param, flag, 2));
            flag += 2;
            if (cmdLen > 0)
            {
                cmdData = DataConverts.ReadBytes(param, flag, cmdLen);
            } 
        }


        /// <summary>
        /// 设置属性
        /// </summary>
        internal virtual void SetParameter() { }

        /// <summary>
        /// 把属性打包成数组
        /// </summary>
        /// <returns></returns>
        internal void CmdToBytes()
        {
            SetParameter();
            frameData = new byte[14+cmdLen];
            int flag = 0;
            frameData[flag++] = 0x16;
            frameData[flag++] = 0x98;
            frameData[flag++] = (byte)cmdType;
            Array.Copy(pcID, 0, frameData, flag, 4);
            flag += 4;
            Array.Copy(devSN, 0, frameData, flag, 4);
            flag += 4;
            frameData[flag++] = (byte)cmdTag;
            Array.Copy(PublicAPI.CKC001.Others.DataConverts.Int_To_Bytes(CmdLen), 0, frameData, flag, 2);            
            if (cmdLen > 0)
            {
                Array.Copy(cmdData, 0, frameData, flag, CmdLen);
            }
        }

     

    }
}
