using System;
using System.Collections.Generic;
 
using System.Text;

namespace PublicAPI.CKL001.Others
{
    internal delegate void delegateMessageReceived(PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase messageMethod);
    public delegate void delegateNotifyMessage(PublicAPI.CKL001.MessageObj.Notify.Notify_Message msgRcv);
}
