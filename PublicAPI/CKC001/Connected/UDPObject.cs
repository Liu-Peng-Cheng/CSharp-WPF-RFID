
namespace PublicAPI.CKC001.Connected
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Net.Sockets;
    using System.Net;
    using System.Threading;
    using PublicAPI.CKC001.Others;
    using System.Diagnostics;
    using System.Security.Cryptography.X509Certificates;

    public class UDPObject
    {       
        UdpClient UDP;
        public delegateUDPMulticast dNotifyUDP;
        public Dictionary<byte[], IPAddress> DevMsg;
        private byte[] pcid;
        private byte[] devSN;
        private IPEndPoint IPEndPoint1;
        private Thread rcvThread;
        private bool isRcv;

        public UDPObject()
        {
            DevMsg = new Dictionary<byte[], IPAddress>();
            pcid = DataConverts.GetPCID();
            IPEndPoint1 = new IPEndPoint(IPAddress.Broadcast, 1983);
            isRcv = false;
        }

        public void Send(PublicAPI.CKC001.MessageObj.UdpObj.UdpObjBase udp,int timeout=50)
        {
            udp.PCID = pcid;
            udp.DevSN = devSN == null ? new byte[4] : devSN;
            udp.CmdToBytes();
            Send(udp.FrameData, timeout);
        }

        public void Send(byte[] msg,int timeout)
        {
            if (msg != null)
            {
                this.UDP = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                isRcv = true;
                UDP.Send(msg, msg.Length, IPEndPoint1);
                rcvThread = new Thread(new ThreadStart(Receive));
                rcvThread.IsBackground = true;
                rcvThread.Start();
                while (stopwatch.ElapsedMilliseconds <= timeout) ;
                isRcv = false;
            }
        }

        private void Receive( )
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            while (isRcv)
            {
                byte[] udpReceiveBytes = UDP.Receive(ref remoteEP);
                if (udpReceiveBytes != null)
                {
                    MessageObj.UdpObj.UdpObjBase udpObj = new MessageObj.UdpObj.UdpObjBase(udpReceiveBytes);
                    if (dNotifyUDP!=null)
                        dNotifyUDP(udpObj, remoteEP.Address);
                }
            }
            UDP.Client.Close();
        }


    }
}
