using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Net;


namespace CK_C001TestDemo.Networkconfig
{
    using PublicAPI.CKC001.Others;
    using System.Net;
    using System.Net.Sockets;
    using System.Text.RegularExpressions;
    using System.Threading;
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NetworkconfigPage
    {
        private Thread mListDevThread = null;//枚举连接的设备线程

        PublicAPI.CKC001.Connected.UDPObject udp;

        public NetworkconfigPage(MainTabControl.MainTabControlPage mainTab)
        {
            InitializeComponent();
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = lang.Languages.ResourceChsUri });
            button_list_dev.Click += Button_list_dev_Click;
            dataGrid.IsReadOnly = true;
            udp = new PublicAPI.CKC001.Connected.UDPObject();
            udp.dNotifyUDP += Notify_UDP;
            this.button_list_dev.Click += Button_list_dev_Click1;           
        }
        private void Notify_UDP(PublicAPI.CKC001.MessageObj.UdpObj.UdpObjBase udp,IPAddress ip)
        {
            Console.WriteLine("Rcv:[" + ip.ToString() + "]" + DataConverts.Bytes_To_HexStr(udp.FrameData));
        }
        private void Button_list_dev_Click1(object sender, RoutedEventArgs e)
        {
            udp.Send(new PublicAPI.CKC001.MessageObj.UdpObj.UdpObj_WakeUpAllDev()) ;
        }

        //显示网络数据日志
        private void OnTftpReport(int msg)
        {
            Dispatcher.Invoke(new Action(() => {
                progressBar_UpLoad.Value = msg;
            }));
        }

        //显示网络数据日志
        private void OnShowLog(string cmd)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                textBlock1.Text += cmd + "\r\n";
                ScrollViewer.ScrollToBottom();
            }));
        }

        //刷新列表
        private void Button_list_dev_Click(object sender, RoutedEventArgs e)
        {

        }
    

           

    }
}
