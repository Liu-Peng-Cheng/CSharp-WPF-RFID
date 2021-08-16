using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.Others
{
    public enum eReturnResult
    { 
        /// <summary>
        /// 失败
        /// </summary>
        Failed=0x00,
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0x01,
        /// <summary>
        /// 指令不存在
        /// </summary>
        NoSuchCMD = 0x02,
        /// <summary>
        /// CRC校验错误
        /// </summary>
        ErrorCRC = 0x03,
        /// <summary>
        /// 魔数
        /// </summary>
        ErrorMagic = 0x04,
        /// <summary>
        /// 长度错误
        /// </summary>
        ErrorLen = 0x05,
        /// <summary>
        /// 设备编号错误
        /// </summary>
        ErrorDevNum = 0x06,
        /// <summary>
        /// 空数据
        /// </summary>
        ErrorNullData = 0x07,
        /// <summary>
        /// json错误
        /// </summary>
        ErrorJson = 0x08,
        /// <summary>
        /// 超时
        /// </summary>
        TimeOut = 0x09,
        /// <summary>
        /// 其他错误
        /// </summary>
        OthersError = 0x0A,

        /// <summary>
        /// 通信失败，没有发送成功
        /// </summary>
        NonResponse = 0xFF
    }

    public enum eCmdType
    {
        /// <summary>
        /// 锁
        /// </summary>
        Lock = 0x01,
        /// <summary>
        /// 灯
        /// </summary>
        LED = 0x02,
        /// <summary>
        /// 指静脉
        /// </summary>
        Finger = 0x03,
        /// <summary>
        /// 高频
        /// </summary>
        HFCard = 0x04,
        /// <summary>
        /// 二维码
        /// </summary>
        QRCode = 0x05,
        /// <summary>
        /// 超高频
        /// </summary>
        RFID = 0x06,
        /// <summary>
        /// 故障报警数据
        /// </summary>
        Warnning = 0x07,
        /// <summary>
        /// 心跳
        /// </summary>
        HartBeat = 0x08,
        /// <summary>
        /// 设备上线
        /// </summary>
        Online = 0x09,
        /// <summary>
        /// 温湿度
        /// </summary>
        Humiture = 0x0A,
        /// <summary>
        /// 设置柜号
        /// </summary>
        Address = 0x0B,

    }

    internal enum eLock
    {
        /// 开锁
        /// </summary>
        OpenLock = 0x01,
        /// <summary>
        /// 状态反馈
        /// </summary>
        NotifyStatus = 0x02,
        /// <summary>
        /// 关锁
        /// </summary>
        CloseLock = 0x03,
        /// <summary>
        /// 锁一直开着
        /// </summary>
        LockKeepOpen = 0x04
    }

    internal enum eLED
    {
        /// <summary>
        /// 打开照明灯
        /// </summary>
        OpenLightLED = 0x01,
        /// <summary>
        /// 关闭照明灯
        /// </summary>
        CloseLightLED = 0x02,
        /// <summary>
        /// 打开报警灯
        /// </summary>
        OpenAlarmLED = 0x03,
        /// <summary>
        /// 关闭报警灯
        /// </summary>
        CloseALarmLED = 0x04,
        /// <summary>
        /// 设置照明灯工作模式
        /// </summary>
        SetLightLedMode = 0x05
    }

    internal enum eFinger
    {
        /// <summary>
        /// 指静脉模板数据采集
        /// </summary>
        GatherTemplate = 0x01,
        /// <summary>
        /// 1：N验证结果上报
        /// </summary>
        Verify_1_N = 0x02,
        /// <summary>
        /// 指静脉模板写入设备
        /// </summary>
        WriteTemplateToDevice = 0x03,
        /// <summary>
        /// 指静脉采集上报
        /// </summary>
        NotifyGatherTemplate = 0x04,
        /// <summary>
        /// 指静脉模板写入设备确认回复
        /// </summary>
        ReplyYes_WriteTemplateToDevice = 0x05,
        /// <summary>
        /// 删除指静脉模板
        /// </summary>
        DeleteSingleTemplate = 0x06,
        /// <summary>
        /// 服务端应答收到采集信息
        /// </summary>
        GatherTemplateACK = 0x07,
        /// <summary>
        /// 删除手指
        /// </summary>
        DeleteAllTemplate = 0x08,
        /// <summary>
        /// 请移开手指
        /// </summary>
        PlaceMoveFinger =0x09,
        /// <summary>
        /// 取消注册
        /// </summary>
        CancelGatherTemplate = 0x0A,

    }

    internal enum eHFCard
    {
        /// <summary>
        /// 上报ID号
        /// </summary>
        NotifyIdCardNum = 0x01,
        /// <summary>
        /// 上报用户区数据
        /// </summary>
        NotifyUserData = 0x02,
        /// <summary>
        /// 读取用户区数据
        /// </summary>
        ReadUserData = 0x03,
        /// <summary>
        /// 配置读取模式
        /// </summary>
        SetReadMode = 0x04,
        /// <summary>
        /// 获取ID号
        /// </summary>
        GetIdCardNum = 0x05
    }

    internal enum eQRCode
    {
        /// <summary>
        /// 上报二维码数据
        /// </summary>
        NotifyQRCodeData = 0x01
    }

    internal enum eRFID
    {
        /// <summary>
        /// 盘点数据上报
        /// </summary>
        NotifyReadData = 0x01,
        /// <summary>
        /// 启动盘点
        /// </summary>
        StartReadTags = 0x02,
        /// <summary>
        /// 结束盘点
        /// </summary>
        StopReadTags = 0x03,
        ///// <summary>
        ///// 设置盘点模式
        ///// </summary>
        //SetReadMode = 0x04,
        ///// <summary>
        ///// 异常数据上报
        ///// </summary>
        //NotifyAbnormalData = 0x05,
        ///// <summary>
        ///// 设置功率
        ///// </summary>
        //SetPower = 0x06,
        ///// <summary>
        ///// 查询功率
        ///// </summary>
        //GetPower = 0x07,
        /// <summary>
        /// 获取所有标签
        /// </summary>
        GetAllTags = 0x08,
        /// <summary>
        /// 清空标签缓存
        /// </summary>
        ClearTempTags = 0x09
    }

    internal enum eHumiture
    {
        /// <summary>
        /// 温湿度上报
        /// </summary>
        NotifyHumiture = 0x01,
        /// <summary>
        /// 启动风机
        /// </summary>
        StartTheFan = 0x02,
        /// <summary>
        /// 设置工作模式
        /// </summary>
        SetHumitureMode = 0x03,
        /// <summary>
        /// 设置阈值
        /// </summary>
        SetThresholdValue = 0x04
    }

    internal enum eHartBeat
    {
        /// <summary>
        /// 客户端心跳上报
        /// </summary>
        DeviceHartBeat = 0x01,
        /// <summary>
        /// 服务端心跳应答
        /// </summary>
        SoftHartBeat = 0x02
    }

    internal enum eAddress
    { 
        setAddress=0x01,
    }

}
