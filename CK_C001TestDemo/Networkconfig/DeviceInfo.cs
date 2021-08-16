using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK_C001TestDemo.Networkconfig
{


    public enum TAG
    {
        // TAG_NULL 只用于验证密码
        TAG_NULL = 0,
        // Command below are only used in get or set command.
        TAG_RESET=1,//重启
        TAG_RETURNFACTORY=2, //恢复出厂设置
        TAG_POWERUPTIME=3,   //设备启动时间（未验证）
        TAG_SAVE=4,//保存配置
        TAG_MODE=5, //设备类型
        TAG_SERIALNUM=6,//序列号
        TAG_NAME=7,//设备名称 
        TAG_FIRMVER=8,//固件版本
        TAG_PSWD_STATUS=9,//密码启用状态
        TAG_PASSWD=10,//passwd
        TAG_MAC=11,//mac地址
        TAG_DHCP=12,//dhcp状态
        TAG_IP=13,//ip地址
        TAG_NETMASK=14,//子网掩码
        TAG_GATEWAY=15,//网关
        TAG_DNS=16,//dns
        TAG_TRANSMIT=17,//上传模式
        TAG_NETMODE=18,//上传协议
        TAG_LPORT=19,//l port
        TAG_SERVINFO=20,//上传服务器TCP地址
        TAG_UDP=21,//上传服务器UDP地址
        TAG_BAUDRATE=22,//波特率
        TAG_DATABITS=23,//数据位
        TAG_STOPBITS=24,//停止位
        TAG_PARITY=25,//校验位

        TAG_PANID=26,
        TAG_CHANNEL=27,
        TAG_SHORTADDR=28,
        TAG_NETWAY=29,
        TAG_ROUTEDEEP=30,
        TAG_AIRSPEED=31,
        TAG_DATAOUTSTYLE=32,
        TAG_TXPOWER=33,//发射功率

        TAG_BYTESSENT=34,//bytes sent
        TAG_BYTESRECVED=35,//bytes recved

        TAG_FRAMESPLIT=36,//frame split
        TAG_REMOTEPORT=37,//上传数据端口
        TAG_TTLMODE=38,//TTL模式
        TAG_UPDATE_LOCAL=39,//本地更新 ，指通过应该是 39
        TAG_UPDATE_ZIGBEE=40,// 指通过 ZigBeeZigBeeZigBeeZigBeeZigBeeZigBee远程更新 远程更新
        TAG_IMAGE_NOTIFY=41,// 发送一次这个命令，网关以间隔 1秒的形式发送 5次更新广播
        TAG_TTL_UP_TIME=42, // 设置 TTL TTL主动上传时间,数据段： 1个字,以分钟为单位
        TAG_FIND_NODE=43,
        TAG_NOTIFY_SPECIFY=44, //指定短地址，需带两字节的短地址。 
        TAG_UPDATE_STATUE=45,//更新状态
    };



    // 命令类型
    public enum CMD_TYPE
    {
        CMD_VERIFY_PSWD = 1, //校验密码
        CMD_GET,//读取参数
        CMD_SET,//设置参数
        CMD_ENUM,//枚举设备
    };
    public enum PSWD
    {
        PSWD_DISABLED = 0,
        PSWD_ENABLED,
        PSWD_STATUS_NUM,
    };
    public enum DHCP
    {
        DHCP_DISABLED = 0,
        DHCP_ENABLED,
        DHCP_STSTUS_NUM,
    };

    // Net Mode
    public enum NET_MODE
    {
        TCP_CLIENT = 0,
        UDP,
        MODE_NUM,
    };
    // BaudRate
    public enum BAUD_RATE
    {
        BR2400 = 0,
        BR4800,
        BR9600,
        BR38400,
        BR57600,
        BR115200,
        BR230400,
        BR460800,
        BR921600,
        BR_NUM,
    };

    // Flow control
    public enum FLOW_CONTROL
    {
        FCNONE = 0,
        FCXONXOFF,
        FCHARDWARE,
        FC_NUM,
    };

 
    //zigbee net way
    public enum ZIGBEE_NET_WAY
    {
        MESH = 0,
        TREE,
        START,
    };
    //zigbee air speed
    public enum ZIGBEE_AIR_SPEED
    {
        AIRSPEED_250K = 0,
        AIRSPEED_30K,
    };
    //zigbee route deep
    public enum ZIGBEE_ROUTE_DEEP
    {
        ROUTEDEEP_1 = 1,
        ROUTEDEEP_2,
        ROUTEDEEP_3,
        ROUTEDEEP_4,
        ROUTEDEEP_5,
        ROUTEDEEP_6,
        ROUTEDEEP_7,
    };
    //zigbee out style 
    public enum ZIGBEE_OUT_STYLE
    {
        OUTSTYLE_NO = 0,

    };

    public enum ZIGBEE_CHANNEL
    {
        channeLANY = 0,
        channeL11 = 11,
        channeL12,
        channeL13,
        channeL14,
        channeL15,
        channeL16,
        channeL17,
        channeL18,
        channeL19,
        channeL20,
        channeL21,
        channeL22,
        channeL23,
        channeL24,
        channeL25,
        channeL26,
    };

    public enum TX_POWER
    {
        DBm8 = 8,
        DBm9,
        DBm10,
        DBm11,
        DBm12,
        DBm13,
        DBm14,
        DBm15,
        DBm16,
        DBm17,
        DBm18,
        DBm19,
        DBm20,
        DBm21,
        DBm22,
    }

}