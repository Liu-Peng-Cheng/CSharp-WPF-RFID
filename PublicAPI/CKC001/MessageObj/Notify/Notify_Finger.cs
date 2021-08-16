using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public  class Notify_Finger: NotifyBase
    {

        /// <summary>
        /// 1:N验证结果上报
        /// </summary>
        public FingerChildrenVerify_1_N getNotifyVerifyResult { get; internal set; }

        /// <summary>
        /// 指静脉采集上报
        /// </summary>
        public FingerChildrenAckGatherResult getNotifyGatherResult { get;internal set; }

        internal Notify_Finger(MsgObjBase msg, string ip)
        {
            base.NotifyAdditiveAttributeA(msg, ip);
        }
    }

}
