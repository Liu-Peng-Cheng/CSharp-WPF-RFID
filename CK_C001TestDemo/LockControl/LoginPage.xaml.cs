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
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        MainTabControlPage mainTab;
        public LoginPage(MainTabControlPage mainTab)
        {
            InitializeComponent();
            this.mainTab = mainTab;
            this.Button_Connected.Click += Button_Connected_Click;
        }

        private void Button_Connected_Click(object sender, RoutedEventArgs e)
        {
            if (this.ComboBox_COM.SelectedIndex == -1) return;
            string lcokStr = this.ComboBox_COM.SelectedItem.ToString() + ":" + this.ComboBox_Baudrate.SelectedItem.ToString();
            if (this.mainTab.LockConnectedObject.Open(lcokStr))
            {
                try { this.mainTab.FrameAppendLock.RemoveBackEntry(); } catch { }
                this.mainTab.FrameAppendLock.Content = new LockControl.CKL001Page(this.mainTab);
            }
            else
            {
                MessageBox.Show(lcokStr + "连接失败");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> temp = PublicAPI.AssistClass.GetCOM();
            this.ComboBox_COM.ItemsSource = temp;
            this.ComboBox_COM.SelectedIndex = temp.Count > 0 ? 0 : -1;
            this.ComboBox_Baudrate.ItemsSource = new int[] { 9600, 115200 };
            this.ComboBox_Baudrate.SelectedIndex = 1;
            this.mainTab.LockConnectedObject = new PublicAPI.CKL001.Connected.ClientObject();
        }
    }
}
