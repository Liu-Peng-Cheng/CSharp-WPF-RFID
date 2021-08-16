using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.UdpObj
{
    public enum eCmdType
    { 
        GetParameter = 0x02, 
        SetParameter = 0x03,
        PasswordVerify = 0x04
    }
    public enum eCmdTag
    {
        /// <summary>
        ///唤醒所有设备
        /// </summary>
        TAG_NULL = 0x00,
        /// <summary>
        ///设备复位
        /// </summary>
        TAG_RESET = 0x01,
        /// <summary>
        /// 恢复出厂设置
        /// </summary>
        TAG_RESTOREFACTORY=0x02,
        /// <summary>
        ///重新上电
        /// </summary>
        TAG_POWERUPTIME = 0x04,
        /// <summary>
        ///名称 
        /// </summary>
        TAG_NAME = 0x07,
        /// <summary>
        ///MAC 地址
        /// </summary>
        TAG_MAC = 0x0B,
        /// <summary>
        ///dhcp
        /// </summary> 
        TAG_DHCP = 0x0C,
        /// <summary>
        ///ip 
        /// </summary>
        TAG_IP = 0x0D,
        /// <summary>
        ///netmask 
        /// </summary>
        TAG_NETMASK = 0x0E,
        /// <summary>
        ///gate way 
        /// </summary>
        TAG_GATEWAY = 0x0F,
        /// <summary>
        ///dns 
        /// </summary>
        TAG_DNS = 0x10,
        /// <summary>
        ///网络工作模式  
        /// </summary>
        TAG_WMODE = 0x12,
        /// <summary>
        ///tcp
        /// </summary>
        TAG_TCP = 0X14,
        /// <summary>
        ///udp 
        /// </summary>
        TAG_UDP = 0x15,             
    }

}
