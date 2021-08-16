using System;
using System.Collections.Generic; 
using System.Text;

namespace PublicAPI.CKL001.MessageObj.MsgObj
{
    public class MsgObj_Lock_Open:MsgObjBase
    {
        private PublicAPI.CKL001.Others.eNum openLock;
        public PublicAPI.CKL001.Others.eNum SetLockNum { set => openLock = value; }

        public MsgObj_Lock_Open()
        {
            base.CmdType = 0x01;
        }
        public override void CmdPacked()
        {
            base.Status = PublicAPI.CKL001.Others.DataConvert.Ushort_To_Bytes((ushort)openLock);
        }

    }
}
