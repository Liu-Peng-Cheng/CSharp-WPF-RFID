using CK_C001TestDemo.MainTabControl;
using GDotnet.Reader.Api.Protocol.Gx;
using PublicAPI.CKC001.MessageObj;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CK_C001TestDemo.RFID
{
    /// <summary>
    /// ReadPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReadPage : Page
    {
        MainTabControlPage mainTab;
        delegate void BindTagAsync();
        ObservableCollection<TagViewObject> _tags;
        List<TagViewObject> tempTags;
        GDotnet.Reader.Api.Protocol.Gx.MsgBaseInventoryEpc _read6C;
        System.Timers.Timer _timer;//计算读取时间
        System.Timers.Timer currentSpeed;//计算平均读取速度的时间参考
        System.Timers.Timer tempTimer;//异步处理心跳
        System.Diagnostics.Stopwatch _stopwatch;
        long _countTemp ;//保存上一秒读取的总次数，为了计算平均速度
        long countReadTime;
        Counts _counts;

        ~ReadPage()
        {
            try
            {
                if (mainTab.RfidConnectedObject != null)
                {
                    mainTab.RfidConnectedObject.SendUnsynMsg(new MsgBaseStop());
                    mainTab.RfidConnectedObject.Close();
                }
            }
            catch { }
        }

        public ReadPage(MainTabControlPage mainTab)
        {
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = lang.Languages.ResourceChsUri });
            InitializeComponent();
            this.mainTab = mainTab;
            DataGrid_General.ItemsSource = _tags = new ObservableCollection<TagViewObject>();
            tempTags = new List<TagViewObject>();
            _read6C = new GDotnet.Reader.Api.Protocol.Gx.MsgBaseInventoryEpc();
            _countTemp = 0;
            countReadTime = 0;
            _counts = new Counts();
            _timer = new System.Timers.Timer();
            currentSpeed = new System.Timers.Timer(1000);
            tempTimer = new System.Timers.Timer(1);
            _stopwatch = new System.Diagnostics.Stopwatch();

            this.Loaded += ReadPage_Loaded;

            BtnRead.Click += BtnRead_Click;
            BtnStop.Click += BtnStop_Click;
            RdBtn_Once.Checked += RdBtn_Once_Checked;
            RdBtn_While.Checked += RdBtn_While_Checked;
            RdBtn_Time.Checked += RdBtn_Time_Checked;
            CkBox_TID.Checked += CkBox_TID_Checked;
            CkBox_TID.Unchecked += CkBox_TID_Unchecked;
            CkBox_User.Checked += CkBox_User_Checked;
            CkBox_User.Unchecked += CkBox_User_Unchecked;
            CkBox_Reserve.Checked += CkBox_Reserve_Checked;
            CkBox_Reserve.Unchecked += CkBox_Reserve_Unchecked;

            BtnShowColumn.Click += BtnShowColumn_Click;
            BtnClear.Click += BtnClear_Click;
            popupTID.Closed += PopupTID_Closed;
            popupUser.Closed += PopupUser_Closed;
            popupReserve.Closed += PopupReserve_Closed;
            popupShow.Closed += PopupShow_Closed;

            Child_TxtBox_ReadTime.TextChanged += Child_TxtBox_ReadTime_TextChanged;
            _timer.Elapsed += _timer_Elapsed;
            currentSpeed.Elapsed += CurrentSpeed_Elapsed;
            tempTimer.Elapsed += TempTimer_Elapsed;

            Btn_Break.Click += Btn_Break_Click;
            Btn_Setting.Click += Btn_Setting_Click;
            Txt_TagNum.DataContext = _counts;
            Txt_TotalCount.DataContext = _counts;
            Txt_Speed.DataContext = _counts;

        }
        //进入设置
        private void Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            try { this.mainTab.FrameAppendRFID.RemoveBackEntry(); } catch { }
            this.mainTab.FrameAppendRFID.Content = new RFID.SettingPage(this.mainTab);  
            
        }
        //断开返回
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

        private void ReadPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.mainTab.RfidConnectedObject.OnEncapedTagEpcLog = null;
            this.mainTab.RfidConnectedObject.OnEncapedTagEpcLog = new GDotnet.Reader.Api.DAL.delegateEncapedTagEpcLog(GeneralReportTag);
            string[] loop = ConfigerSetting.GetAppSetting("KeyReadLoop").Split(new char[] { ',' });
            switch (loop[0])
            {
                case "0":
                    RdBtn_Once.IsChecked = true;
                    break;
                case "1":
                    RdBtn_While.IsChecked = true;
                    break;
            }
            Child_TxtBox_ReadTime.Text = loop[1];
            string[] tid = ConfigerSetting.GetAppSetting("KeyTID").Split(new char[] { ',' });
            switch (tid[0])
            {
                case "0": Child_RdBtn_TIDAutoLength.IsChecked = true; break;
                case "1": Child_RdBtn_TIDManualLength.IsChecked = true; break;
            }
            Child_TxtBox_TIDLength.Text = tid[1];

            string[] user = ConfigerSetting.GetAppSetting("KeyUser").Split(new char[] { ',' });
            Child_TxtBox_UserStartAddress.Text = user[0];
            Child_TxtBox_UserReadLength.Text = user[1];

            string[] reserve = ConfigerSetting.GetAppSetting("KeyReserve").Split(new char[] { ',' });
            Child_TxtBox_ReserveStartAddress.Text = reserve[0];
            Child_TxtBox_ReserveReadLength.Text = reserve[1];

            //列表显示选项
            string listShow = ConfigerSetting.GetAppSetting("KeyShowList");
            for (int i = 0; i < listShow.Length; i++)
            {
                if (listShow[i] == '1')
                    DataGrid_General.Columns[i].Visibility = Visibility.Visible;
                else
                    DataGrid_General.Columns[i].Visibility = Visibility.Hidden;
            }
            
            string tempAnt = ConfigerSetting.GetAppSetting("KeyAntNum");
            for (int i = 0; i < 16; i++)
            {
                _read6C.AntennaEnable |= (uint)(((tempAnt[i] == '1') ? 0x0001 : 0x000) << i);
            }
        }

        //异步处理心跳
        private void TempTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate {
                while (tempTags.Count > 0)
                {
                    try {
                        TagViewObject rvm = _tags.First(p => p.Key == tempTags[0].Key);
                        switch (tempTags[0].Ant)
                        {
                            case 1:rvm.Ant1 += 1;break;
                            case 2:rvm.Ant2 += 1;break;
                            case 3:rvm.Ant3 += 1;break;
                            case 4:rvm.Ant4 += 1;break;
                            case 5:rvm.Ant5 += 1;break;
                            case 6:rvm.Ant6 += 1;break;
                            case 7:rvm.Ant7 += 1;break;
                            case 8:rvm.Ant8 += 1;break;
                            case 9:rvm.Ant9 += 1;break;
                            case 10:rvm.Ant10 += 1;break;
                            case 11:rvm.Ant11 += 1;break;
                            case 12:rvm.Ant12 += 1;break;
                            case 13:rvm.Ant13 += 1;break;
                            case 14:rvm.Ant14 += 1;break;
                            case 15:rvm.Ant15 += 1;break;
                            case 16:rvm.Ant16 += 1;break;
                        }
                        rvm.Total = rvm.Total + 1;
                        rvm.RSSI = tempTags[0].RSSI;
                        rvm.Frequency = tempTags[0].Frequency;
                        rvm.PcValue = tempTags[0].PcValue;
                        rvm.Phase = tempTags[0].Phase;
                        tempTags.RemoveAt(0);
                    } catch {
                        _counts.TagNum++;
                        tempTags[0].Num = _tags.Count + 1;
                        _tags.Add(tempTags[0]);
                        tempTags.RemoveAt(0);
                    }
                }
                tempTimer.Stop();
            }));
        }

        //计算平均读取速度的时间参考
        private void CurrentSpeed_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _counts.TagSpeed = _counts.ReadTotal - _countTemp;
            _countTemp = _counts.ReadTotal;
        }

        //计算读取时间
        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TxtTime.Dispatcher.BeginInvoke(new Action(delegate {
                this.TxtTime.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", _stopwatch.Elapsed.Hours, _stopwatch.Elapsed.Minutes, _stopwatch.Elapsed.Seconds, _stopwatch.Elapsed.Milliseconds);
                if (this.RdBtn_Time.IsChecked == true)
                {
                    if (_stopwatch.ElapsedMilliseconds >= countReadTime)
                    {
                        BtnStop_Click(null, null);
                    }
                }
            }));

        }

        //限定读取时间输入数字
        private void Child_TxtBox_ReadTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            //屏蔽中文输入和非法字符粘贴输入
            TextBox textBox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!Double.TryParse(textBox.Text, out num))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }
        }

        //
        private void PopupShow_Closed(object sender, EventArgs e)
        {
            string listShow = "";
            for (int i = 0; i < 26; i++)
            {
                CheckBox ck = (CheckBox)this.FindName("chk" + i);
                if (ck.IsChecked == true)
                {
                    DataGrid_General.Columns[i].Visibility = Visibility.Visible;
                    listShow += "1";
                }
                else
                {
                    DataGrid_General.Columns[i].Visibility = Visibility.Hidden;
                    listShow += "0";
                }
            }
            ConfigerSetting.SaveAppSetting("KeyShowList", listShow);
        }

        //设置Reserve参数
        private void PopupReserve_Closed(object sender, EventArgs e)
        {
            _read6C.ReadReserved = new ParamEpcReadReserved()
            {
                Start = (byte)(Child_TxtBox_ReserveStartAddress.Text == "" ? 0 : byte.Parse(Child_TxtBox_ReserveStartAddress.Text)),
                Len = (byte)(Child_TxtBox_ReserveReadLength.Text == "" ? 4 : byte.Parse(Child_TxtBox_ReserveReadLength.Text))
            };
            string cc = Child_TxtBox_ReserveStartAddress.Text + "," + Child_TxtBox_ReserveReadLength.Text;
            ConfigerSetting.SaveAppSetting("KeyReserve", cc);
        }

        //设置User参数
        private void PopupUser_Closed(object sender, EventArgs e)
        {
            _read6C.ReadUserdata = new ParamEpcReadUserdata()
            {
                 Start = (byte)(Child_TxtBox_UserStartAddress.Text == "" ? 0 : byte.Parse(Child_TxtBox_UserStartAddress.Text)),
                Len = (byte)(Child_TxtBox_UserReadLength.Text == "" ? 4 : byte.Parse(Child_TxtBox_UserReadLength.Text))
            };
            string cc = Child_TxtBox_UserStartAddress.Text + "," + Child_TxtBox_UserReadLength.Text;
            ConfigerSetting.SaveAppSetting("KeyUser", cc);
        }

        //设置TID参数
        private void PopupTID_Closed(object sender, EventArgs e)
        {
            _read6C.ReadTid = new ParamEpcReadTid()
            {
                Mode = (byte)((Child_RdBtn_TIDAutoLength.IsChecked == true) ? eParamTidMode.Auto : eParamTidMode.Fixed),
                Len = (Child_TxtBox_TIDLength.Text == "") ? (byte)6 : byte.Parse(Child_TxtBox_TIDLength.Text)
            };
            string cc = (Child_RdBtn_TIDAutoLength.IsChecked == true ? "0," : "1,") + Child_TxtBox_TIDLength.Text;
            ConfigerSetting.SaveAppSetting("KeyTID", cc);
        }

        //设置读取时间
        private void SaveReadTimes()
        {
            countReadTime = long.Parse(Child_TxtBox_ReadTime.Text) *1000;
            ConfigerSetting.SaveAppSetting("KeyReadLoop", "1," + Child_TxtBox_ReadTime.Text);
        }

        //清空列表
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            _tags.Clear();
            _counts.TagNum = 0;
            _counts.ReadTotal = 0;
            _counts.TagSpeed = 0;
            _countTemp = 0;
        }

        //显示列表选项
        private void BtnShowColumn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < DataGrid_General.Columns.Count; i++)
            {
                if (DataGrid_General.Columns[i].Visibility == Visibility.Visible)
                {
                    CheckBox ck = (CheckBox)this.FindName("chk" + i);
                    if(ck!=null)
                        ck.IsChecked = true;
                }
            }
            popupShow.IsOpen = true;
        }

        //取消读取Reserve
        private void CkBox_Reserve_Unchecked(object sender, RoutedEventArgs e)
        {
            _read6C.ReadReserved = null;
            DataGrid_General.Columns[4].Visibility = Visibility.Hidden;
        }

        //读取Reserve
        private void CkBox_Reserve_Checked(object sender, RoutedEventArgs e)
        {
            popupReserve.IsOpen = true;
            DataGrid_General.Columns[4].Visibility = Visibility.Visible;
        }

        //取消读取User
        private void CkBox_User_Unchecked(object sender, RoutedEventArgs e)
        {
            _read6C.ReadUserdata = null;
            DataGrid_General.Columns[3].Visibility = Visibility.Hidden;
        }

        //读取User
        private void CkBox_User_Checked(object sender, RoutedEventArgs e)
        {
            popupUser.IsOpen = true;
            DataGrid_General.Columns[3].Visibility = Visibility.Visible;
        }

        //取消读取TID
        private void CkBox_TID_Unchecked(object sender, RoutedEventArgs e)
        {
            _read6C.ReadTid = null;
            DataGrid_General.Columns[2].Visibility = Visibility.Hidden;
        }

        //读取TID
        private void CkBox_TID_Checked(object sender, RoutedEventArgs e)
        {
            popupTID.IsOpen = true;
            DataGrid_General.Columns[2].Visibility = Visibility.Visible;
        }

        //计时读取
        private void RdBtn_Time_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox rd = (CheckBox)sender;
            if (rd.IsChecked == true)
                _read6C.InventoryMode = 1;
        }

        //循环读取
        private void RdBtn_While_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rd = (RadioButton)sender;
            if (rd.IsChecked == true)
                _read6C.InventoryMode = 1;
            string[] cc = ConfigerSetting.GetAppSetting("KeyReadLoop").Split(new char[] { ',' });
            cc[0] = "1";
            ConfigerSetting.SaveAppSetting("KeyReadLoop", cc[0] + "," + cc[1]);
        }

        //单次读取
        private void RdBtn_Once_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rd = sender as RadioButton;
            if (rd.IsChecked == true)
            {
                _read6C.InventoryMode = 0;
                string[] cc = ConfigerSetting.GetAppSetting("KeyReadLoop").Split(new char[] { ',' });
                cc[0] = "0";
                ConfigerSetting.SaveAppSetting("KeyReadLoop", cc[0] + "," + cc[1]);
            }
        }

        //停止读卡
        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            this.mainTab.RfidConnectedObject.SendUnsynMsg(new MsgBaseStop());
            _timer.Stop();
            _stopwatch.Stop();
            currentSpeed.Stop();
        }

        //读卡
        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            SaveReadTimes();
            BtnClear_Click(null, null);
            this.mainTab.RfidConnectedObject.SendUnsynMsg(new MsgBaseStop());
            this.mainTab.RfidConnectedObject.SendUnsynMsg(_read6C);
            if (RdBtn_While.IsChecked == true)
            {
                currentSpeed.Start();
                _timer.Start();
                _stopwatch.Restart();
            }
        }

      
        private void Menu_Clear(object sender, RoutedEventArgs e)
        {
            //清空列表
          
        }
        private void SaveUserData(string key, string value)
        {
            try {
                ConfigurationManager.AppSettings.Remove(key);
                ConfigurationManager.AppSettings.Add(key,value);
                ConfigurationManager.RefreshSection("appSettings");
            } catch { }
        }
        private string GetUserData(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


        public void GeneralReportTag(GDotnet.Reader.Api.DAL.EncapedLogBaseEpcInfo msg)
        {
            if (msg == null)
                return;
            Txt_TotalCount.Dispatcher.BeginInvoke(new Action(() =>
            {
                _counts.ReadTotal++;
            }));
            tempTags.Add(new TagViewObject(msg.logBaseEpcInfo));
            tempTimer.Start();
        }

        private ContextMenu ContextMenus(Dictionary<string, RoutedEventHandler> list)
        {
            ContextMenu cm = new ContextMenu();
            //cm.Style = style;
            foreach (var dc in list)
            {
                MenuItem menu = new MenuItem();
                menu.Header = dc.Key;
                menu.Click += dc.Value;
                cm.Items.Add(menu);
            }
            return cm;
        }

    }
}
