using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.Others;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class Notify_RFID: NotifyBase
    {
        Tags tags;
        internal Tags setTags { set => tags = value; }
        public Tags GetTags { get => tags; }
        internal Notify_RFID(MsgObjBase msg, string ip)
        {
            base.NotifyAdditiveAttributeA(msg, ip);
        }
    }
}
