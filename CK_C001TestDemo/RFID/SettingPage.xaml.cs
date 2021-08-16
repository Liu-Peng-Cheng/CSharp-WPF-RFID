using CK_C001TestDemo.MainTabControl;
using System;
using System.Collections.Generic;
using System.Configuration;
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

using System.Configuration;
using System.Runtime.CompilerServices;

namespace CK_C001TestDemo.RFID
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page
    {
        MainTabControlPage mainTab;
        byte antNum =0;
        public SettingPage( MainTabControlPage mainTab)
        {
            InitializeComponent();
            this.Loaded += SettingPage_Loaded;
            this.Btn_getBaseSpeed.Click += Btn_getBaseSpeed_Click;
            this.Btn_setBaseSpeed.Click += Btn_setBaseSpeed_Click;
            this.Btn_getFilter.Click += Btn_getFilter_Click;
            this.Btn_setFilter.Click += Btn_setFilter_Click;
            this.Btn_getFrequency.Click += Btn_getFrequency_Click;
            this.Btn_setFrequency.Click += Btn_setFrequency_Click;
            this.Btn_Break.Click += Btn_Break_Click;
            this.Btn_Read.Click += Btn_Read_Click;
            this.mainTab = mainTab;
            this.Combox_BaseSpeed.ItemsSource = new string[] { "0 |Tari=25us,FM0,LHF=40KH", "1 |密集模式", "2 |Tari=25us,Miller4,LHF=300KH", "3 |快速模式", "4 |Tari=20us,Miller2,LHF=320KH", "10|Tari=12.5us,Miller4,LHF=250KH", "11|Tari=6.25us,Miller4,LHF=250KH", "12|Tari=12.5us,Miller4,LHF=300KH", "13|Tari=6.25us,Miller4,LHF=300KH", "Auto" };
            this.Combox_Session.ItemsSource = new byte[] { 0, 1, 2, 3, 4 };
            this.Combox_QValue.ItemsSource = new string[] { "0|单标签","1","2","3", "4|多标签","5","6","7", "8","9","10","11", "12","13","14","15"};
            this.Combox_InventoryType.ItemsSource = new string[] { "A面盘存", "B面盘存", "双面盘存" };
            this.Combox_Frequency.ItemsSource = new string[] { "GB920_925", "GB840_845", "GB840_845|920_925", "FCC902_928", "ETSI866_868", "JP916.8_920.4", "TW922.25_927.75","ID923.125_925.125", "RUS866.6_867.4", "TEST_AUTO802.75_998.75" };

            GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetCapabilities rfidCapabilities = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetCapabilities();
            this.mainTab.RfidConnectedObject.SendSynMsg(rfidCapabilities);
            if (rfidCapabilities.RtCode == 0)
            {
                antNum = rfidCapabilities.AntennaCount;
            }
            else
            {
                antNum = 16;
            }
            for (byte i = 0; i < antNum; i++)
            {
                CheckBox ckAntNum = new CheckBox()
                {
                    Content = "Ant" + (i + 1).ToString("00"),
                    FontSize = 13,
                    Margin = new Thickness(20, 10, 20, 10),
                };
                this.WrapPanel_UserAnt.RegisterName("CheckBox_Ant" + (i + 1).ToString("00"), ckAntNum);
                this.WrapPanel_UserAnt.Children.Add(ckAntNum);
                CheckBox ckAntPower = new CheckBox()
                {
                    Content = "Ant" + (i + 1).ToString("00"),
                    FontSize = 13,
                    Margin = new Thickness(20, 5, 10, 5),
                    VerticalAlignment = VerticalAlignment.Center,
                };
                this.WrapPanel_SetPower.RegisterName("CkeckBox_Power" + (i + 1).ToString("00"), ckAntPower);
                this.WrapPanel_SetPower.Children.Add(ckAntPower);
                ComboBox cbAntPower = new ComboBox()
                {
                    ItemsSource = new byte[] { 33, 30, 27, 25, 20, 15, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 16, 17, 18, 19, 21, 22, 23, 24, 26, 28, 29, 31, 32 },
                    FontSize = 13,
                    Width = 70,
                    Height = 21,
                    Margin = new Thickness(10, 5, 40, 5),
                };
                this.WrapPanel_SetPower.RegisterName("ComboBox_Power" + (i + 1).ToString("00"), cbAntPower);
                this.WrapPanel_SetPower.Children.Add(cbAntPower);
            }
            Button btnSetAntNum = new Button()
            {
                Name = "Btn_SetAntNum",
                Content = "设置天线",
                FontSize = 13,
                Width = 90,
                Height = 27,
                Margin = new Thickness(280, 10, 0, 0),
            };
            btnSetAntNum.Click += BtnSetAntNum_Click;
            this.WrapPanel_UserAnt.Children.Add(btnSetAntNum);
            Button btnGetAntPower = new Button()
            {
                Name = "Btn_SetAntNum",
                Content = "查询功率",
                FontSize = 13,
                Width = 90,
                Height = 27,
                Margin = new Thickness(285, 10, 0, 0),
            };
            btnGetAntPower.Click += BtnGetAntPower_Click;
            this.WrapPanel_SetPower.Children.Add(btnGetAntPower);
            Button btnSetAntPower = new Button()
            {
                Name = "Btn_SetAntNum",
                Content = "设置功率",
                FontSize = 13,
                Width = 90,
                Height = 27,
                Margin = new Thickness(285, 10, 0, 0),
            }; btnSetAntPower.Click += BtnSetAntPower_Click;
            this.WrapPanel_SetPower.Children.Add(btnSetAntPower);
        }

        private void Btn_Read_Click(object sender, RoutedEventArgs e)
        {
            try { this.mainTab.FrameAppendRFID.RemoveBackEntry(); } catch { }
            this.mainTab.FrameAppendRFID.Content = new RFID.ReadPage(this.mainTab);
        }

        private void Btn_Break_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainTab.RfidConnectedObject.Close())
            {
                try { this.mainTab.FrameAppendRFID.RemoveBackEntry(); } catch { }
                this.mainTab.FrameAppendRFID.Content = new RFID.LoginPage(this.mainTab);
            }
            else
            {
                MessageBox.Show("断开失败请稍后再试");
            }
        }
        //设置频段
        private void Btn_setFrequency_Click(object sender, RoutedEventArgs e)
        {
            GDotnet.Reader.Api.Protocol.Gx.MsgBaseSetFreqRange freqRange = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseSetFreqRange()
            {
                FreqRangeIndex =(byte) this.Combox_Frequency.SelectedIndex,
            };
            this.mainTab.RfidConnectedObject.SendSynMsg(freqRange);
            if (freqRange.RtCode != 0)
            {
                MessageBox.Show("设置工作频段失败");
            }
        }
        //查询频段
        private void Btn_getFrequency_Click(object sender, RoutedEventArgs e)
        {
            GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetFreqRange freqRange = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetFreqRange();
            this.mainTab.RfidConnectedObject.SendSynMsg(freqRange);
            if (freqRange.RtCode == 0)
            {
                this.Combox_Frequency.SelectedIndex = freqRange.FreqRangeIndex;
            }
            else
            {
                MessageBox.Show("获取工作频段失败");
            }
        }
        //设置过滤
        private void Btn_setFilter_Click(object sender, RoutedEventArgs e)
        {
            GDotnet.Reader.Api.Protocol.Gx.MsgBaseSetTagLog filter = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseSetTagLog()
            {
                RepeatedTime = string.IsNullOrEmpty(this.TextBox_UploadInterval.Text)?(ushort)0: ushort.Parse(this.TextBox_UploadInterval.Text),
                RssiTV = string.IsNullOrEmpty(this.TextBox_RSSI.Text) ? (byte)0 : byte.Parse(this.TextBox_UploadInterval.Text),
            };
            this.mainTab.RfidConnectedObject.SendSynMsg(filter);
            if (filter.RtCode != 0)
            {
                MessageBox.Show("设置标签过滤参数失败");
            }
        }
        
        //查询过滤
        private void Btn_getFilter_Click(object sender, RoutedEventArgs e)
        {
            GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetTagLog filter = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetTagLog();
            this.mainTab.RfidConnectedObject.SendSynMsg(filter);
            if (filter.RtCode == 0)
            {
                this.TextBox_UploadInterval.Text = filter.RepeatedTime.ToString();
                this.TextBox_RSSI.Text = filter.RssiTV.ToString();
            }
            else
            {
                MessageBox.Show("获取标签过滤参数失败");
            }
        }
        //设置基带参数
        private void Btn_setBaseSpeed_Click(object sender, RoutedEventArgs e)
        {
            GDotnet.Reader.Api.Protocol.Gx.MsgBaseSetBaseband setBaseband = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseSetBaseband()
            {
                BaseSpeed = (byte)this.Combox_BaseSpeed.SelectedIndex,
                Session = (byte)this.Combox_Session.SelectedIndex,
                QValue = (byte)this.Combox_QValue.SelectedIndex,
                InventoryFlag = (byte)this.Combox_InventoryType.SelectedIndex,
            };
            this.mainTab.RfidConnectedObject.SendSynMsg(setBaseband);
            if (setBaseband.RtCode != 0)
            {
                MessageBox.Show("设置基带参数失败");
            }
        }
        //查询基带参数
        private void Btn_getBaseSpeed_Click(object sender, RoutedEventArgs e)
        {
            GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetBaseband getBaseband = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetBaseband();
            this.mainTab.RfidConnectedObject.SendSynMsg(getBaseband);
            if (getBaseband.RtCode == 0)
            {
                this.Combox_BaseSpeed.SelectedIndex = getBaseband.BaseSpeed;
                this.Combox_Session.SelectedIndex = getBaseband.Session;
                this.Combox_QValue.SelectedIndex = getBaseband.QValue;
                this.Combox_InventoryType.SelectedIndex = getBaseband.InventoryFlag;
            }
            else
            {
                MessageBox.Show("获取基带参数失败");
            }
        }
        //页面加载
        private void SettingPage_Loaded(object sender, RoutedEventArgs e)
        {
            string ant = ConfigerSetting.GetAppSetting("KeyAntNum");
            for (int i = 0; i < antNum; i++)
            {
                CheckBox ck = this.WrapPanel_UserAnt.FindName("CheckBox_Ant" + (i + 1).ToString("00")) as CheckBox;
                if(ck!=null)
                    ck.IsChecked = ant[i] == '1';                
            }
            BtnGetAntPower_Click(null, null);
            Btn_getFrequency_Click(null, null);
            Btn_getFilter_Click(null, null);
            Btn_getBaseSpeed_Click(null, null);
        }
        //设置功率
        private void BtnSetAntPower_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<byte, byte> power = new Dictionary<byte, byte>();
            for (byte i = 0; i < antNum; i++)
            {
                CheckBox ck = this.WrapPanel_SetPower.FindName("CkeckBox_Power" + (i + 1).ToString("00")) as CheckBox;
                if (ck != null && ck.IsChecked == true)
                {
                    ComboBox cb = this.WrapPanel_SetPower.FindName("ComboBox_Power" + (i + 1).ToString("00")) as ComboBox;
                    if (cb != null && cb.SelectedIndex != -1)
                    {
                        power.Add((byte)(i + 1), byte.Parse(cb.SelectedItem.ToString()));
                    }
                }
            }
            if (power.Count > 0)
            {
                GDotnet.Reader.Api.Protocol.Gx.MsgBaseSetPower setPower = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseSetPower() {
                    DicPower = power,
                };
                this.mainTab.RfidConnectedObject.SendSynMsg(setPower);
                if (setPower.RtCode != 0)
                {
                    MessageBox.Show("设置功率失败");
                }
            }
        }
        //查询功率
        private void BtnGetAntPower_Click(object sender, RoutedEventArgs e)
        {
            GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetPower getPower = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseGetPower();
            this.mainTab.RfidConnectedObject.SendSynMsg(getPower);
            if (getPower.RtCode == 0)
            {
                foreach (KeyValuePair<byte, byte> entry in getPower.DicPower)
                {
                    CheckBox ck = this.WrapPanel_SetPower.FindName("CkeckBox_Power" + entry.Key.ToString("00")) as CheckBox;
                    ComboBox cb = this.WrapPanel_SetPower.FindName("ComboBox_Power" + entry.Key.ToString("00")) as ComboBox;
                    cb.SelectedValue = entry.Value;
                }
            }
            else
            {
                MessageBox.Show("查询功率失败");
            }            
        }
        //设置工作天线
        private void BtnSetAntNum_Click(object sender, RoutedEventArgs e)
        {
            string ant = "";
            for (int i = 0; i < 16; i++)
            {
                CheckBox ck = this.WrapPanel_UserAnt.FindName("CheckBox_Ant" + (i + 1).ToString("00")) as CheckBox;
                if (ck != null && ck.IsChecked == true)
                    ant += "1";
                else
                    ant += "0";
            }
            ConfigerSetting.SaveAppSetting("KeyAntNum", ant);
        }



    }
}
