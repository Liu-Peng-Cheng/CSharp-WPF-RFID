using PublicAPI.CKC001.MessageObj.MsgObj;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PublicAPI.CKC001.Others
{
    internal class ManualReset
    {
        public ManualResetEvent ResetEvent;
        public MsgObjBase msgObj;
        public ManualReset( bool status)
        {
            ResetEvent = new ManualResetEvent(status);
        }
    }
}
