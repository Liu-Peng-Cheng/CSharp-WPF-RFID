using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CK_C001TestDemo.Networkconfig
{
    /// <summary>
    /// WindowSetPassword.xaml 的交互逻辑
    /// </summary>
    public partial class WindowSetPassword : Window
    {
        public uint mZtnid = 0;
        public int mPcid = 0;
        public WindowSetPassword(uint ztnd, int pcid)
        {
            mZtnid = ztnd;
            mPcid = pcid;
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox_new.Password.Length == 0)
            {
                MessageBox.Show("请输入新密码！");
                return;
            }
            if (passwordBox_new.Password != passwordBox_confirm.Password)
            {
                MessageBox.Show("两次输入不一致！");
                return;
            }
            IntPtr content = Marshal.AllocHGlobal(100);
            Marshal.Copy(System.Text.Encoding.ASCII.GetBytes(passwordBox.Password), 0, content, passwordBox.Password.Length);

            Marshal.FreeHGlobal(content);
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                button_Click(sender, null);
            }
        }

    }
}
