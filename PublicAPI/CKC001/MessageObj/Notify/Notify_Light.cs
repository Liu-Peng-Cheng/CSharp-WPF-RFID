using PublicAPI.CKC001.Connected;
using PublicAPI.CKC001.MessageObj.MsgObj;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class Notify_Light: NotifyBase
    {
        internal Notify_Light(MsgObjBase msg,string ip)
        {
            base.NotifyAdditiveAttributeA(msg,ip);
        }
    }
}
