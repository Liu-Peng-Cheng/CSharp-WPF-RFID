using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public class DevIPSN
    {
        string ip;
        IPAddress iPAddress;
        string snStr;
        byte[] snBytes;

        public string Ip { get => ip; set => ip = value; }
        public IPAddress IPAddress { get => iPAddress; set => iPAddress = value; }
        public string SnStr { get => snStr; set => snStr = value; }
        public byte[] SnBytes { get => snBytes; set => snBytes = value; }

    }

    }
