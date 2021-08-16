using PublicAPI.CKC001.Connected;
using PublicAPI.CKC001.MessageObj;
using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.MessageObj.Notify;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PublicAPI.CKC001.Others
{
    public delegate void delegateMessageReceived(MsgObjBase msg);
    public delegate void delegateDisconnected(string msg);
    public delegate void delegateConnected();

    public delegate void delegateTcpDisconnected(string str);
    //public delegate void delegateTcpClientConnected(Others.SocketObject client);
    public delegate void delegateTcpClientConnected(ClientObject client);
    internal delegate void delegateInternalTcpClientConnected(PublicAPI.CKC001.Connected.communication.TypeTcpClient client);

    //上报
    public delegate void delegateNotifyDeviceOnline(Notify_DevOnline msg);
    public delegate void delegateNotifyLock(Notify_Lock msg);
    public delegate void delegateNotifyHF(Notify_HFCard msg);
    public delegate void deleageNotifyFinger(Notify_Finger msg);
    public delegate void delegateNotifyRQCode(Notify_QRCode msg);
    public delegate void delegateNotifyHumiture(Notify_Humiture msg);
    public delegate void delegateNotifyRFID(Notify_RFID msg);
    public delegate void delegateNotifyHeartBeat(Notify_HeartBeat msg);
    public delegate void delegateNotifyLight(Notify_Light msg);
    public delegate void delegateNotifyAbnormal(Notify_Error_Alarm_Abnormal msg);
    public delegate void delegateNotifyContainerNum(Notify_ContainerNum msg);
    public delegate void delegateNotifyLog(Notify_Log msg);

    public delegate void delegateUDPMulticast(PublicAPI.CKC001.MessageObj.UdpObj.UdpObjBase udp,IPAddress address);

}
