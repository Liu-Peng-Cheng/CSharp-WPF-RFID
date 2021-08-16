using CK_C001TestDemo.MainTabControl;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CK_C001TestDemo.LockControl
{
    /// <summary>
    /// CKL001Page.xaml 的交互逻辑
    /// </summary>
    public partial class CKL001Page : Page
    {
        MainTabControlPage mainTab;
        byte[,] AllStatus;//0x11 0x锁灯
        System.Timers.Timer timer;
        int allNum;
        public CKL001Page( MainTabControlPage mainTab)
        {
            InitializeComponent();
            this.mainTab = mainTab;
            this.ComboBox_Address.SelectionChanged += ComboBox_Address_SelectionChanged;
            AllStatus = new byte[16,12] ;
            this.mainTab.LockConnectedObject.Address = 0xFF;
            this.RadioButton_Auto.Checked += RadioButton_Auto_Manual_Checked;
            this.RadioButton_Manual.Checked += RadioButton_Auto_Manual_Checked;
            this.Button_AllOpen.Click += Button_AllOpen_Click;
            this.GroupBox_Lock.MouseDown += GroupBox_Lock_MouseDown;
            this.TextBox.MouseDoubleClick += TextBox_MouseDoubleClick;
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.TextBox.Text = "";
        }

        private void GroupBox_Lock_MouseDown(object sender, MouseButtonEventArgs e)
        {        
            PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_Lock_GetStatus getStatus = new PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_Lock_GetStatus();
            this.mainTab.LockConnectedObject.Send(getStatus);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int temp = allNum++;
            if (temp < 13)
            {
                this.CheckBox_OpenAllLock.Dispatcher.BeginInvoke(new Action(delegate {
                    if (this.CheckBox_OpenAllLock.IsChecked == true)
                    {
                        this.mainTab.LockConnectedObject.Send(new PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_Lock_Open() { SetLockNum = (PublicAPI.CKL001.Others.eNum)(0x0001 << (temp - 1)) });
                    }
                    else
                    {
                        allNum = 13;
                    }
                }));
                return;
            }
            else if (temp < 25)
            {
                this.GroupBox_All.Dispatcher.BeginInvoke(new Action(delegate
                {
                    if (RadioButton_Manual.IsChecked == true)
                    {
                        if (CheckBox_OpenAllLight.IsChecked == true)
                        {
                            this.mainTab.LockConnectedObject.Send(new PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_LED_Switch()
                            {
                                IsSwitchLED = RadioButton_AllOpenLed.IsChecked == true,
                                SetSwitchLEDNum = (PublicAPI.CKL001.Others.eNum)(0x0001 << (temp - 13))
                            });
                            return;
                        }
                    }
                    allNum = 0xF0;                    
                }));
            }
            else
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
                TextBox.Dispatcher.BeginInvoke(new Action(delegate {
                    TextBox.Text += "[" + DateTime.Now + "] 操作完毕" + Environment.NewLine;
                }));
            }
        }
        private void Button_AllOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckBox_OneByOne.IsChecked == true)
                {
                    string numStr = System.Text.RegularExpressions.Regex.Replace(this.TextBox_DelayTime.Text, @"[^0-9]", "");
                    int num = int.Parse(numStr);
                    if (timer != null) timer.Stop();
                    timer = new System.Timers.Timer(num);
                    timer.Elapsed += Timer_Elapsed;
                    allNum = 1;
                    timer.Start();
                }
                else
                {
                    if (this.CheckBox_OpenAllLock.IsChecked == true)
                    {
                        this.mainTab.LockConnectedObject.Send(new PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_Lock_Open { SetLockNum = (PublicAPI.CKL001.Others.eNum)0xFF });
                        System.Threading.Thread.Sleep(10);
                    }
                    if (this.RadioButton_Manual.IsChecked == true && this.CheckBox_OpenAllLight.IsChecked == true)
                    {
                        System.Threading.Thread.Sleep(10);
                        this.mainTab.LockConnectedObject.Send(new PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_LED_Switch()
                        {
                            IsSwitchLED = RadioButton_AllOpenLed.IsChecked == true,
                            SetSwitchLEDNum = (PublicAPI.CKL001.Others.eNum)0xFF
                        });
                    }   
                }
            }
            catch {  }
           
        }


        private void RadioButton_Auto_Manual_Checked(object sender, RoutedEventArgs e)
        {
            SwitchLightMode(this.RadioButton_Manual.IsChecked == true);
            this.mainTab.LockConnectedObject.Send(new PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_LED_SetControlMode() { IsAutoSwitchLight = this.RadioButton_Auto.IsChecked == true, });
        }

        private void SwitchLightMode(bool isManual)
        {
            this.CheckBox_OpenAllLight.IsEnabled = isManual;
            this.RadioButton_AllOpenLed.IsEnabled = isManual;
            this.RadioButton_AllCloseLed.IsEnabled = isManual;
            this.GroupBox_Light.IsEnabled = isManual;
            this.GroupBox_All.IsEnabled = true;
            this.GroupBox_Lock.IsEnabled = true;
        }

        private void ComboBox_Address_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = this.ComboBox_Address.SelectedIndex;
            if (selectedIndex == -1)
            {
                return;
            }
            else if (selectedIndex == 0)
            {
                this.mainTab.LockConnectedObject.Address = 0xFF;
                foreach (Button btn in this.WrapPanel_ButtonLock.Children)
                {
                    btn.Content = btn.Content.ToString().Replace("关", "开");
                    btn.IsEnabled = true;
                }
                foreach (Button btn in this.WrapPanel_ButtonLight.Children)
                {
                    btn.Content = btn.Content.ToString().Replace("关", "开");
                    btn.IsEnabled = true;
                }
            }
            else if (selectedIndex == 17)
            {
                if (this.mainTab.LockConnectedObject.Close())
                {
                    try { this.mainTab.FrameAppendLock.RemoveBackEntry(); } catch { }
                    this.mainTab.FrameAppendLock.Content = new LockControl.LoginPage(this.mainTab);
                }
                else
                {
                    MessageBox.Show("断开连接失败");
                }
            }
            else
            {
                this.mainTab.LockConnectedObject.Address = (byte)(selectedIndex - 1);
                foreach (Button btn in this.WrapPanel_ButtonLock.Children)
                {
                    string num = System.Text.RegularExpressions.Regex.Replace(btn.Name, @"[^0-9]", "");
                    byte btnNum = byte.Parse(num);
                    if ((AllStatus[this.mainTab.LockConnectedObject.Address, btnNum-1] & 0x10) == 0x10)
                    {
                        btn.Content = btn.Content.ToString().Replace("开", "关");
                        btn.IsEnabled = false;
                    }
                    else
                    {
                        btn.Content = btn.Content.ToString().Replace("关", "开");
                        btn.IsEnabled = true;
                    }
                }
                foreach (Button btn in this.WrapPanel_ButtonLight.Children)
                {
                    string num = System.Text.RegularExpressions.Regex.Replace(btn.Name, @"[^0-9]", "");
                    byte btnNum = byte.Parse(num);
                    if ((AllStatus[this.mainTab.LockConnectedObject.Address, btnNum-1] & 0x01) == 0x01)
                    {
                        btn.Content = btn.Content.ToString().Replace("开", "关");
                    }
                    else
                    {
                        btn.Content = btn.Content.ToString().Replace("关", "开");
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> tempLock = new List<string>();
            List<string> tempLight = new List<string>();
            List<string> tempAddress = new List<string>();
            tempAddress.Add("广播地址");
            for (byte i = 0; i < 16; i++)
            {
                tempAddress.Add(i.ToString());
                if (i < 12)
                {
                    Button lockBtn = new Button()
                    {
                        Name = "Btn_Lock" + (i+1).ToString("00"),
                        Content = "柜号" + (i + 1).ToString("00")+"开门",
                        FontSize =12,
                        Width = 110,
                        Height = 30,
                        Margin = new Thickness(20, 10, 20, 10),
                    };
                    lockBtn.Click += LockBtn_Click;
                    this.WrapPanel_ButtonLock.Children.Add(lockBtn);
                    Button ledBtn = new Button()
                    {
                        Name = "Btn_LED" + (i + 1).ToString("00"),
                        Content = "柜号" + (i + 1).ToString("00")+"开灯",
                        Width =110,
                        Height =30,
                        FontSize = 12,
                        Margin = new Thickness(20, 10, 20, 10),
                    };
                    ledBtn.Click += LedBtn_Click;
                    this.WrapPanel_ButtonLight.Children.Add(ledBtn);
                }
            }
            tempAddress.Add("断开连接");
            this.ComboBox_Address.ItemsSource = tempAddress;
            this.ComboBox_Address.SelectedIndex = 0;
            this.mainTab.LockConnectedObject.dNotifyMessage += ProcessMessage;
            this.GroupBox_All.IsEnabled = false;
            this.GroupBox_Lock.IsEnabled = false;
            this.GroupBox_Light.IsEnabled = false;
            this.TextBox.Text += 
                "\r\n    提示：\r\n\t1、请选择 '灯控模式' 后继续；\r\n\t2、关锁后不会上报锁状态，如需要获取锁状态：请单击 '锁控' 框 。\r\n" +
                "\t3、广播地址是可以同时控制所有锁控板的那种地址；\r\n\t4、如串口选用错误，请选择地址最后一个 '断开连接'，返回重新连接。\r\n\t" +
                "5、双击清除日志内容。\r\n\r\n";
        }

        private void LedBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Light_Lock_ButtonClick(false,btn, 0x01, 0x10);
        }

        private void LockBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Light_Lock_ButtonClick(true,btn, 0x10, 0x01);
        }

        public void Light_Lock_ButtonClick(bool isLock, Button btn, byte open, byte close)
        {
            bool isOpen = btn.Content.ToString().Contains("开");
            string num = System.Text.RegularExpressions.Regex.Replace(btn.Name, @"[^0-9]", "");
            byte btnNum = byte.Parse(num);
            if (isLock)
            {
                this.mainTab.LockConnectedObject.Send(new PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_Lock_Open()
                {
                    SetLockNum = (PublicAPI.CKL001.Others.eNum)(0x0001 << (btnNum - 1)),
                });
            }
            else
            {
                this.mainTab.LockConnectedObject.Send(new PublicAPI.CKL001.MessageObj.MsgObj.MsgObj_LED_Switch()
                {
                    IsSwitchLED = isOpen,
                    SetSwitchLEDNum = (PublicAPI.CKL001.Others.eNum)(0x0001 << (btnNum - 1)),
                });
            }  
        }

        private void ProcessMessage(PublicAPI.CKL001.MessageObj.Notify.Notify_Message notify_Msg)
        {
            if (notify_Msg.IsRcv)
            {
                bool[] statusBytes = new bool[12];            
                switch (notify_Msg.CmdType)
                {
                    case 0x01:
                    case 0x02:
                    case 0x03:
                        for (int i = 0; i < 12; i++)
                        {
                            this.GroupBox_Lock.Dispatcher.BeginInvoke(new Action(delegate
                            {
                                Button btn = FindName("Btn_Lock" + (i + 1).ToString("00")) as Button;
                                if (((i < 8 ? (notify_Msg.Status[1] >> i) : (notify_Msg.Status[0] >> (i - 8))) & 0x0001) == 0x0001)
                                {
                                    AllStatus[notify_Msg.Address, i] |= 0x10;
                                    btn.Content = btn.Content.ToString().Replace("开", "关");
                                    btn.IsEnabled = false;
                                }
                                else
                                {
                                    AllStatus[notify_Msg.Address, i] &= 0x01;
                                    btn.Content = btn.Content.ToString().Replace("关", "开");
                                    btn.IsEnabled = true;
                                }
                            }));
                        }
                        break;
                    case 0x04:
                    case 0x05:
                        for (int i = 0; i < 12; i++)
                        {
                            this.GroupBox_Lock.Dispatcher.BeginInvoke(new Action(delegate
                            {
                                Button btn = FindName("Btn_LED" + (i + 1).ToString("00")) as Button;
                                if (((i < 8 ? (notify_Msg.Status[1] >> i) : (notify_Msg.Status[0] >> (i - 8))) & 0x0001) == 0x0001)
                                {
                                    if (notify_Msg.CmdType == 0x04)
                                    {
                                        AllStatus[notify_Msg.Address, i] |= 0x01;
                                        btn.Content = btn.Content.ToString().Replace("开", "关");
                                    }
                                    else
                                    {
                                        AllStatus[notify_Msg.Address, i] &= 0x10;
                                        btn.Content = btn.Content.ToString().Replace("关", "开");
                                    }
                                }
                            }));
                        }
                        break;
                    case 0x06:
                        this.GroupBox_All.Dispatcher.BeginInvoke(new Action(delegate {
                            if (notify_Msg.Status[1] == 0x01)
                            {
                                this.RadioButton_Auto.IsChecked = true;
                            }
                            else if (notify_Msg.Status[1] == 0x02)
                            {
                                this.RadioButton_Manual.IsChecked = true;
                            }
                        }));                        
                        break;
                }
            }
            TextBox.Dispatcher.BeginInvoke(new Action(delegate
            {
                TextBox.Text+= string.Format("[{0}] -[{1}]- [{2}]-：{3}{4}", DateTime.Now, notify_Msg.IsRcv ? "接收" : "发送", TypeToStr(notify_Msg.CmdType), notify_Msg.FrameDataStr, Environment.NewLine);
            }));
        }


        private string TypeToStr(byte cmdType)
        {
            string temp = "";
            switch (cmdType)
            {
                case 0x01:
                    temp = "开锁的指令";
                    break;
                case 0x02:
                    temp = "锁状态反馈";
                    break;
                case 0x03:
                    temp = "查询锁状态";
                    break;
                case 0x04:
                    temp = "开启照明灯";
                    break;
                case 0x05:
                    temp = "关闭照明灯";
                    break;
                case 0x06:
                    temp = "设置灯模式";
                    break;
                default:
                    temp = "未知的指令";
                    break;
            }
            return temp;
        }

    }
}
