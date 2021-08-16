using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Collections.ObjectModel;
using CK_C001TestDemo.MsgObj;
using log4net;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

namespace CK_C001TestDemo.Test
{

    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class TestPage : Page
    {
        MainTabControl.MainTabControlPage mainTab;
        ObservableCollection<ConObj> ClientList;

        public TestPage(MainTabControl.MainTabControlPage mainTab)
        {
            InitializeComponent();
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = lang.Languages.ResourceChsUri });
            this.mainTab = mainTab;
            this.Loaded += Page1_Loaded;
            this.BtnOpenLock.Click += BtnOpenLock_Click;
            this.BtnLightLedOpen.Click += BtnLightLedOpen_Click;
            this.BtnLightLedClose.Click += BtnLightLedClose_Click;
            this.BtnSwitchLedMode.Click += BtnSwitchLedMode_Click;
            this.BtnAlarmLedOpen.Click += BtnAlarmLedOpen_Click;
            this.BtnAlarmLedClose.Click += BtnAlarmLedClose_Click;
            this.BtnRFIDRead.Click += BtnRFIDRead_Click;
            this.BtnRFIDClearTemp.Click += BtnRFIDClearTemp_Click;
            this.BtnFingerRegist.Click += BtnFingerRegist_Click;
            this.BtnFingerRegistCancle.Click += BtnFingerRegistCancle_Click;
            this.BtnFingerDelete.Click += BtnFingerDelete_Click;
            this.BtnFingerWriteToDev.Click += BtnFingerWriteToDev_Click;
            this.CheckBoxAutoLock.Click += CheckBoxAutoLock_Click;
            this.DataGridConList.ItemsSource = ClientList = new ObservableCollection<ConObj>();
            string ListnPort = ConfigurationManager.AppSettings.Get("KeyListenPort");
            this.mainTab.Server = new PublicAPI.CKC001.Connected.ServerObject();
            if (this.mainTab.Server.Open(int.Parse(ListnPort)))
            {
                this.mainTab.Server.dClientConnected += new PublicAPI.CKC001.Others.delegateTcpClientConnected(OnClientConnected);
            }
        }

        private void CheckBoxAutoLock_Click(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (ck.IsChecked == true)
            {
                this.BtnOpenLock.IsEnabled = false;
            }
            else
            {
                this.BtnOpenLock.IsEnabled = true;
            }
        }

        private void CallNotify(PublicAPI.CKC001.Connected.ClientObject co)
        {
            co.dNotifyLog += OnLog;
            co.NotifyDisconnected(new PublicAPI.CKC001.Others.delegateDisconnected(OnDiscon));
            co.dNotifyDeviceOnline += OnDevOnline;
            co.dNotifyLock += OnLock;
            co.dNotifyLight += OnLight;
            co.dNotifyHF += OnHFCard;
            co.dNotifyRQCode += OnRQCode;
            co.dHeartBeat += OnHeartBeat;
            co.dNotifyAbnormal += OnAbnormal;
            co.dNotifyHumiture += OnHumiture;
            co.dNotifyRFID += OnRFID;
            co.dNotifyFinger += OnFinger;
            co.dNotifyContainerNum += OnAddress;
        }

        #region Log
        private void OnLog(PublicAPI.CKC001.MessageObj.Notify.Notify_Log notify)
        {
            string temp = "";
            temp = string.Format(" [{0}]-[{1}] {2}:<- ",this.Resources["103000"], notify.getDevIp,notify.getIsRecv==true?this.Resources["105006"]: this.Resources["105005"]);
            temp += SortCmd(notify.getCmdType, notify.getCmdTag)+" "+notify.getFrameData;
            this.mainTab.logPage.TxtLog.Dispatcher.BeginInvoke(new Action(delegate
            {
                this.mainTab.logPage.TxtLog.Text += DateTime.Now.ToString("G") + temp +Environment.NewLine;
            }));
            if(notify.getCmdType!= PublicAPI.CKC001.Others.eCmdType.HartBeat)
                LogManager.GetLogger("").Info(temp);
        }
        private void Log(string ip, string msg)
        {
            string temp = "";
            temp = string.Format(" [{0}]-[{1}] {2}:-> ", this.Resources["103000"], ip, this.Resources["105007"])+msg;
            this.mainTab.logPage.TxtLog.Dispatcher.BeginInvoke(new Action(delegate
            {
                this.mainTab.logPage.TxtLog.Text += DateTime.Now.ToString("G") + temp + Environment.NewLine;
            }));
            LogManager.GetLogger("").Info(temp);
        }
        #endregion

        #region Online
        private void OnDevOnline(PublicAPI.CKC001.MessageObj.Notify.Notify_DevOnline notify)
        {
            DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    ConObj co = ClientList.First(p => p.DevIP == notify.getDevIp);
                    if (co != null)
                    {
                        co.AddressNum = notify.getDevAddress;
                        co.DevSN = notify.getDevSN;
                        co.DevMAC = notify.getMacStr;
                        co.DevType = notify.getTypeStr;
                    }
                }
                catch { }
            }));
        }
        #endregion

        #region Lock
        private void BtnOpenLock_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_Lock_OpenLock());
        }
        private void OnLock(PublicAPI.CKC001.MessageObj.Notify.Notify_Lock notify)
        {
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    ConObj co = ClientList.First(p => p.DevSN == notify.getDevSN);
                    if (co != null )
                    {
                        co.Lock = SortCmd(PublicAPI.CKC001.Others.eCmdType.Lock, notify.getCmdTag);
                    }
                } catch { }
            }));
        }
        #endregion

        #region Light
        bool isSetLightAuto = true;
        private void BtnLightLedClose_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_LED_LightSwitch() {  setLightStatus = false,});
        }

        private void BtnLightLedOpen_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_LED_LightSwitch() { setLightStatus = true, });
        }

        private void BtnSwitchLedMode_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_LED_LightWorkMode() {  setLightIsAutoSwitch = isSetLightAuto, });
            isSetLightAuto = !isSetLightAuto;
        }

        private void BtnAlarmLedClose_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_LED_AralmSwitch() { setAlarmStatus =false, });
        }

        private void BtnAlarmLedOpen_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_LED_AralmSwitch() { setAlarmStatus = true, });
        }

        private void OnLight(PublicAPI.CKC001.MessageObj.Notify.Notify_Light notify)
        {
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try {
                    ConObj co = ClientList.First(p => p.DevSN == notify.getDevSN);
                    if (co != null)
                    {
                        switch (notify.getCmdTag)
                        {
                            case 0x01:
                            case 0x02:
                            case 0x05:
                                co.Light = SortCmd(PublicAPI.CKC001.Others.eCmdType.LED, notify.getCmdTag);
                                break;
                            case 0x03:
                            case 0x04:
                                co.AlarmLED = SortCmd(PublicAPI.CKC001.Others.eCmdType.LED,notify.getCmdTag);
                                break;
                        }
                    }
                } catch { }
            }));
        }

        #endregion

        #region HF
        private void OnHFCard(PublicAPI.CKC001.MessageObj.Notify.Notify_HFCard notify)
        {
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    ConObj co = ClientList.First(p => p.DevSN == notify.getDevSN);
                    if (co != null && notify.getReturnResult == PublicAPI.CKC001.Others.eReturnResult.Succeed)
                    {
                        if (string.IsNullOrEmpty(notify.getIDNumStr) || string.IsNullOrEmpty(notify.getUserDataStr))
                        {
                            co.HF = notify.getIDNumStr + notify.getUserDataStr;
                        }
                        else
                        {
                            co.HF = SortCmd(PublicAPI.CKC001.Others.eCmdType.HFCard, notify.getCmdTag);
                        }
                    }
                }
                catch { }
            }));
        }
        #endregion

        #region RQCode
        private void OnRQCode(PublicAPI.CKC001.MessageObj.Notify.Notify_QRCode notify)
        {
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    ConObj co = ClientList.First(p => p.DevSN == notify.getDevSN);
                    if (co != null && notify.getReturnResult == PublicAPI.CKC001.Others.eReturnResult.Succeed)
                    {
                        co.QRCode =  notify.getRQCodeStr;
                    }
                }
                catch { }
            }));
        }
        #endregion

        #region HeartBeat
        private void OnHeartBeat(PublicAPI.CKC001.MessageObj.Notify.Notify_HeartBeat notify)
        {
            //收到心跳，不用应答接口已自动应答了心跳
            //如果需要其他操作可在此处理
        }
        #endregion

        #region Abnormal

        private void OnAbnormal(PublicAPI.CKC001.MessageObj.Notify.Notify_Error_Alarm_Abnormal notify)
        {
            Console.WriteLine(notify.getFrameData);
        }

        #endregion

        #region Humiture

        
        private void OnHumiture(PublicAPI.CKC001.MessageObj.Notify.Notify_Humiture notify)
        {
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    ConObj co = ClientList.First(p => p.DevSN == notify.getDevSN);
                    if (co != null && notify.getReturnResult == PublicAPI.CKC001.Others.eReturnResult.Succeed)
                    {
                        if (notify.getHumidityFloat == 0 && notify.getTemperatureFload == 0)
                        {
                            co.Humiture = SortCmd(PublicAPI.CKC001.Others.eCmdType.Humiture, notify.getCmdTag);
                        }
                        else
                        {
                            co.Humiture = notify.getTemperatureStr + " | " + notify.getHumidityStr;
                        }
                    }
                }
                catch { }
            }));
        }

        #endregion

        #region RFID

        private void BtnRFIDClearTemp_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_RFID_ClearTempTags());
        }

        private void BtnRFIDGetAllTags_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_RFID_GetAllTags());
        }

        private void BtnRFIDStop_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_RFID_StopRead());
        }

        private void BtnRFIDRead_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_RFID_ReadTags() {  setReadTimes =byte.Parse(ConfigerSetting.GetAppSetting("KeyReadTimes")),});
        }

        private void OnRFID(PublicAPI.CKC001.MessageObj.Notify.Notify_RFID notify)
        {
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    ConObj co = ClientList.First(p => p.DevSN == notify.getDevSN);
                    if (co != null && notify.getReturnResult == PublicAPI.CKC001.Others.eReturnResult.Succeed)
                    {
                        switch (notify.getCmdTag)
                        {
                            case 0x01:
                                co.RFID = this.Resources["103076"].ToString()  + notify.GetTags.all_tag_num +" - "+ this.Resources["103077"] + notify.GetTags.add_tag_num + " - " + this.Resources["103078"] + notify.GetTags.loss_tag_num;
                                break;
                            case 0x08:
                                co.RFID = this.Resources["103076"].ToString() + notify.GetTags.all_tag_num;
                                break;
                            default:
                                co.RFID = SortCmd(PublicAPI.CKC001.Others.eCmdType.RFID, notify.getCmdTag);
                                break;
                        }
                    }
                }
                catch { }
            }));
        }
        #endregion

        #region Finger

        public static string getRandom(int length)
        {
            byte[] random = new Byte[length / 2];
            // 使用加密服务提供程序 (CSP) 提供的实现来实现加密随机数生成器 (RNG)。无法继承此类
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(random);
            StringBuilder sb = new StringBuilder(length);
            int i;
            for (i = 0; i < random.Length; i++)
            {
                // 以16进制格式输出
                sb.Append(String.Format("{0:X2}", random[i]));
            }
            return sb.ToString();
        }

        byte[] fingerTemple;


        private void BtnFingerWriteToDev_Click(object sender, RoutedEventArgs e)
        {

            if (fingerTemple == null || fingerTemple.Length != 1536)
            {
                this.TxtFingerTip.Text = "请点击注册，先采集指静脉模板";
                return;
            }
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_Finger_WriteTempleToDevice { setUserIDStr = getRandom(12), setFingerID = 0, setFingerTemple = fingerTemple, });
        }

        private void BtnFingerDelete_Click(object sender, RoutedEventArgs e)
        {
            
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_Finger_DeleteAllFingerTemplate() { });
        }


        private void BtnFingerRegistCancle_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_Finger_GatherTemplateCancel() { });
        }

        private void BtnFingerRegist_Click(object sender, RoutedEventArgs e)
        {
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_Finger_GatherTemplates() { });
            this.TxtFingerTip.Text = "采集开始，请放置手指";
        }

        private void OnFinger(PublicAPI.CKC001.MessageObj.Notify.Notify_Finger notify)
        {
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    this.TxtFingerTip.Text = SortCmd(PublicAPI.CKC001.Others.eCmdType.Finger, notify.getCmdTag);
                    ConObj co = ClientList.First(p => p.DevSN == notify.getDevSN);
                    if (co != null && notify.getReturnResult == PublicAPI.CKC001.Others.eReturnResult.Succeed)
                    {
                        switch (notify.getCmdTag)
                        {
                            //指静脉数据采集
                            case 0x01:
                                break;
                                //1：N验证
                            case 0x02:
                                co.Vena = VerifyResult1_N(notify.getNotifyVerifyResult.getVerifyResult)+"-用户ID："+ notify.getNotifyVerifyResult.getUserIDStr+"-手指ID："+notify.getNotifyVerifyResult.getFingerID;
                                break;
                                //指静脉模板写入设备
                            case 0x03:
                                break;
                                //指静脉采集上报
                            case 0x04:
                                co.Vena = GatherResult(notify.getNotifyGatherResult.getGatherCount,notify.getNotifyGatherResult.getResult);
                                if (notify.getNotifyGatherResult.getResult== 0x00)
                                {
                                    if (notify.getNotifyGatherResult.getGatherCount==0x03)
                                        fingerTemple = notify.getNotifyGatherResult.getFingerTemplatesByte;
                                }
                                break;
                                //下发确认
                            case 0x05:
                                break;
                                //删除模板
                            case 0x06:
                                break;
                                //应答采集信息
                            case 0x07:
                                break;
                                //删除所有模板
                            case 0x08:
                                break;
                                //移开手指
                            case 0x09:
                                break;
                                //停止注册
                            case 0x0A:
                                break;
                        }
                    }
                }
                catch { }
            }));
        }

        private string VerifyResult1_N(byte result)
        {
            switch (result)
            {
                case 0x00:return this.Resources["105008"].ToString();
                case 0x01:return this.Resources["105009"].ToString();
                case 0x02:return this.Resources["105010"].ToString();
                case 0x07:return this.Resources["105011"].ToString();
                case 0x10:return this.Resources["105012"].ToString();
                case 0x11:return this.Resources["105013"].ToString();
            }
            return "Unknown";
        }

        private string GatherResult(byte ackCount,byte result)
        { 
            switch(result)
            {
                case 0x00:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105014"];
                case 0x01:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105015"];
                case 0x02:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105016"];
                case 0x03:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105017"];
                case 0x04:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105018"];
                case 0x05:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105019"];
                case 0x06:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105020"];
                case 0x07:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105021"];
                case 0x08:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105022"];
                case 0x09:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105023"];
                case 0x10:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105024"];
                case 0x11:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105025"];
                case 0x12:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105026"];
                case 0x13:return string.Format(this.Resources["105028"].ToString(), ackCount) + this.Resources["105027"];
            }
            return "Unknown";
        }

        #endregion


        #region Address
        private void OnAddress(PublicAPI.CKC001.MessageObj.Notify.Notify_ContainerNum notify)
        {
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    ConObj co = ClientList.First(p => p.DevSN == notify.getDevSN);
                    if (co != null && notify.getReturnResult == PublicAPI.CKC001.Others.eReturnResult.Succeed)
                    {
                        co.AddressNum = notify.getNewAddress;
                    }
                }
                catch { }
            }));
        }
        #endregion

        private void Send(PublicAPI.CKC001.MessageObj.MsgObj.MsgObjBase mb)
        {
            if (ClientList.Count <= 0) return;
            foreach (ConObj co in ClientList)
            {
                if (co.Operate && co.Status)
                {
                    co.DevClient.SendAsync(mb);
                }
            }
        }

        private string SortCmd(PublicAPI.CKC001.Others.eCmdType cmdType,byte cmdTag)
        {
            switch (cmdType)
            {
                case PublicAPI.CKC001.Others.eCmdType.Lock:
                    switch (cmdTag)
                    {
                        case 0x01: return this.Resources["103007"].ToString(); 
                        case 0x02: return this.Resources["103038"].ToString(); 
                        case 0x03: return this.Resources["103039"].ToString(); 
                        case 0x04: return this.Resources["103030"].ToString(); 
                    }
                    break;
                case PublicAPI.CKC001.Others.eCmdType.LED:
                    switch (cmdTag)
                    {
                        case 0x01: return this.Resources["103010"].ToString(); 
                        case 0x02: return this.Resources["103011"].ToString(); 
                        case 0x03: return this.Resources["103032"].ToString(); 
                        case 0x04: return this.Resources["103033"].ToString(); 
                        case 0x05: return this.Resources["103040"].ToString(); 
                    }
                    break;
                case PublicAPI.CKC001.Others.eCmdType.Finger:
                    switch (cmdTag)
                    {
                        case 0x01: return this.Resources["103041"].ToString(); 
                        case 0x02: return this.Resources["103042"].ToString(); 
                        case 0x03: return this.Resources["103043"].ToString(); 
                        case 0x04: return this.Resources["103044"].ToString(); 
                        case 0x05: return this.Resources["103045"].ToString(); 
                        case 0x06: return this.Resources["103046"].ToString(); 
                        case 0x07: return this.Resources["103047"].ToString(); 
                        case 0x08: return this.Resources["103048"].ToString(); 
                        case 0x09: return this.Resources["103049"].ToString(); 
                        case 0x0A: return this.Resources["103050"].ToString(); 
                    }
                    break;
                case PublicAPI.CKC001.Others.eCmdType.HFCard:
                    switch (cmdTag)
                    { 
                        case 0x01: return this.Resources["103051"].ToString(); 
                        case 0x02: return this.Resources["103052"].ToString(); 
                        case 0x03: return this.Resources["103053"].ToString(); 
                        case 0x04: return this.Resources["103054"].ToString(); 
                        case 0x05: return this.Resources["103055"].ToString(); 
                    }
                    break;
                case PublicAPI.CKC001.Others.eCmdType.QRCode:
                    switch (cmdTag)
                    {
                        default: return this.Resources["103029"].ToString(); 
                    }
                case PublicAPI.CKC001.Others.eCmdType.RFID:
                    switch (cmdTag)
                    {
                        case 0x01: return this.Resources["103056"].ToString(); 
                        case 0x02: return this.Resources["103057"].ToString(); 
                        case 0x03: return this.Resources["103058"].ToString(); 
                        case 0x08: return this.Resources["103059"].ToString(); 
                        case 0x09: return this.Resources["103060"].ToString(); 
                    }
                    break;
                case PublicAPI.CKC001.Others.eCmdType.Warnning:
                    switch (cmdTag)
                    {
                        default: return this.Resources["103061"].ToString(); 
                    }
                case PublicAPI.CKC001.Others.eCmdType.HartBeat:
                    switch (cmdTag)
                    {
                        default: return this.Resources["103062"].ToString(); 
                    }
                case PublicAPI.CKC001.Others.eCmdType.Online:
                    switch (cmdTag)
                    {
                        default: return this.Resources["103063"].ToString(); 
                    }
                case PublicAPI.CKC001.Others.eCmdType.Humiture:
                    switch (cmdTag)
                    {
                        case 0x01: return this.Resources["103064"].ToString(); 
                        case 0x02: return this.Resources["103065"].ToString(); 
                        case 0x03: return this.Resources["103066"].ToString(); 
                        case 0x04: return this.Resources["103067"].ToString(); 
                    }
                    break;
                case PublicAPI.CKC001.Others.eCmdType.Address:
                    switch (cmdTag)
                    {
                        case 0x01: return this.Resources["103068"].ToString(); 
                    }
                    break;
            }
            return "Unknown";
        }
     
        private void Page1_Loaded(object sender, RoutedEventArgs e)
        {
       
        }

        private void OnClientConnected(PublicAPI.CKC001.Connected.ClientObject client)
        {
            CallNotify(client);
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                ConObj temp = null;
                try
                {
                    temp = ClientList.First(p => p.DevIP == client.DevIP);
                }
                catch { }
                if (temp == null)
                {
                    temp = new ConObj() 
                    { 
                        Operate= true,
                        Num = ClientList.Count + 1,
                        Status = true,
                        IsConnected=this.Resources["103036"].ToString(),
                        DevIP = client.DevIP,
                        DevClient = client,
                    };
                    ClientList.Add(temp);
                }
                else
                {
                    temp.Operate = true;
                    temp.Status = true;
                    temp.IsConnected = this.Resources["103036"].ToString();
                    temp.DevClient = client;
                }
            }));            
        }

        public void OnDiscon(string msg)
        {            
            Log(msg, string.Format(this.Resources["105002"].ToString(), msg));
            this.DataGridConList.Dispatcher.BeginInvoke(new Action(delegate {
                try
                {
                    ConObj co = ClientList.First(p => p.DevIP == msg);
                    if (co != null)
                    {
                        ClientRemoveNotify(co.DevClient);
                        co.Operate = false;
                        co.Status = false;
                        co.IsConnected= this.Resources["103037"].ToString();
                        if(co.DevClient!=null)
                            co.DevClient.Closed();
                        co.DevClient = null;
                    }
                }
                catch { }
            }));
        }


        private void ClientRemoveNotify(PublicAPI.CKC001.Connected.ClientObject client)
        {
            if (client == null) return;
            client.dNotifyLock = null;
            client.dNotifyLight = null;
            client.dNotifyFinger = null;
            client.dNotifyHF = null;
            client.dNotifyRQCode = null;
            client.dNotifyRFID = null;
            client.dNotifyAbnormal = null;
            client.dHeartBeat = null;
            client.dNotifyDeviceOnline = null;
            client.dNotifyHumiture = null;
            client.dNotifyContainerNum = null;
        }

        public void Close( )
        {
            this.mainTab.Server.dClientConnected -= new PublicAPI.CKC001.Others.delegateTcpClientConnected(OnClientConnected);
            this.mainTab.Server.dClientConnected = null;
            try
            {
                lock (ClientList)
                {
                    foreach (ConObj co in ClientList)
                    {
                        ClientRemoveNotify(co.DevClient);
                        co.Operate = false;
                        co.Status = false;
                        if(co.DevClient!=null)
                            co.DevClient.Closed();
                        co.DevClient = null;
                        ClientList.Remove(co);
                    }
                }
            }
            catch { }
            finally { this.mainTab.Server.Closed(); }
        }

        /// <summary>
        /// 设置柜号
        /// </summary>
        private void DataGridConList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ConObj co = e.Row.Item as ConObj;
            Send(new PublicAPI.CKC001.MessageObj.MsgObj.MsgObj_ContainerSetAddress() { setNewAddressNum = co.AddressNum ,});
        }

   
    }
    
}
