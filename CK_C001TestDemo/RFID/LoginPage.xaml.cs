using CK_C001TestDemo.MainTabControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        MainTabControlPage mainTab;
        List<string> ListIP = new List<string>();

        public LoginPage( MainTabControlPage mainTab)
        {
            InitializeComponent();
            this.mainTab = mainTab;
            this.Button_Connected.Click += Button_Connected_Click;
            this.Loaded += LoginPage_Loaded;
            this.TextBox_IP.ItemsSource = ListIP;
        }

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.mainTab.RfidConnectedObject = new GDotnet.Reader.Api.DAL.GClient();
            if (ListIP.Count <= 0)
            {
                isRcv = true;
                new DelegateRunGetIP(RunGetIP).BeginInvoke(new AsyncCallback(CallBackFun), null);
            }
        }

        private delegate void DelegateRunGetIP();
        bool isRcv;
        private void RunGetIP()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread thread = new Thread(new ThreadStart(GetIP));
            thread.IsBackground = true;
            thread.Start();
            while (stopwatch.ElapsedMilliseconds <= 3000) ;
            isRcv = false;
        }
        private void CallBackFun(IAsyncResult result)
        { }
        private void Button_Connected_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBox_IP.Text)||string.IsNullOrEmpty(this.TextBox_Port.Text)) return;
            string rfifStr = this.TextBox_IP.Text + ":" + this.TextBox_Port.Text;
            GDotnet.Reader.Api.Protocol.Gx.eConnectionAttemptEventStatusType status;
            if (this.mainTab.RfidConnectedObject.OpenTcp(rfifStr,3000, out status))
            {
                try { this.mainTab.FrameAppendRFID.RemoveBackEntry(); } catch { }
                this.mainTab.FrameAppendRFID.Content = new RFID.ReadPage(this.mainTab);
            }
            else
            {
                MessageBox.Show(string.Format("{0}连接失败{1}如果用的是CK-C001板，IP就是搜到的板子的IP，端口号固定8160",rfifStr,Environment.NewLine));
            }
        }

        public  void GetIP()
        {
            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            byte[] wakeUp = new byte[14];
            wakeUp[0] = 0x16;
            wakeUp[1] = 0x98;
            wakeUp[2] = 0x04;
            client.Send(wakeUp, 14, new IPEndPoint(IPAddress.Broadcast, 1983));
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);            
            while (isRcv)
            {
                byte[] rcv = client.Receive(ref remoteEP);
                if (!ListIP.Contains(remoteEP.Address.ToString()))
                {
                    Console.WriteLine("获取到的IP：" + remoteEP);
                    ListIP.Add(remoteEP.Address.ToString());
                }
            }
        }

    }
}
