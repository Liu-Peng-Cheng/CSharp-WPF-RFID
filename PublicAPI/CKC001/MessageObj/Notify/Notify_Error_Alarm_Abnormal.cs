using PublicAPI.CKC001.MessageObj.MsgObj;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class Notify_Error_Alarm_Abnormal: NotifyBase
    {
        internal Notify_Error_Alarm_Abnormal(MsgObjBase msg, string ip)
        {
            base.NotifyAdditiveAttributeA(msg, ip);
        }
    }
}
