namespace PublicAPI.CKC001.MessageObj.MsgObj
{
    public  class MsgObj_ContainerSetAddress:MsgObjBase
    {
        byte address;

        public byte setNewAddressNum {set => address = value; }

        public MsgObj_ContainerSetAddress()
        {
            base.CmdType = PublicAPI.CKC001.Others.eCmdType.Address;
            base.CmdTag = (byte)PublicAPI.CKC001.Others.eAddress.setAddress;
        }
        internal override void SendPacked()
        {
            base.CmdData = new byte[1] { address };
        }
    }
}
