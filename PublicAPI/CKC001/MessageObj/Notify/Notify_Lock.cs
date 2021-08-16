using System;
using System.Collections.Generic;
using System.Text;
using PublicAPI.CKC001.MessageObj.MsgObj;
using PublicAPI.CKC001.Others;
namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class Notify_Lock:NotifyBase
    {
        byte[] lockStatus;

        /// <summary>
        ///byte[0]电子锁，byte[1]磁吸锁            1 为开，2 为关闭；如果为 0 表示没有配置该锁
        /// </summary>
        public byte[] getLockStatus { get => lockStatus;internal set => lockStatus = value; }
        public Notify_Lock(MsgObjBase msg,string ip)
        {
            base.NotifyAdditiveAttributeA(msg,ip);
        }
    }
}
