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
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace CK_C001TestDemo.Networkconfig
{
    /// <summary>
    /// WindowPassword.xaml 的交互逻辑
    /// </summary>
    public partial class WindowPassword : Window
    {
        public uint mZtnid = 0;
        public int mPcid = 0;
        public WindowPassword(uint ztnd, int pcid)
        {
            mZtnid = ztnd;
            mPcid = pcid;
            InitializeComponent();
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            IntPtr content = Marshal.AllocHGlobal(100);
            Marshal.Copy(System.Text.Encoding.ASCII.GetBytes(passwordBox.Password), 0, content, passwordBox.Password.Length);
     
            Marshal.FreeHGlobal(content);
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                button_ok_Click(sender, null);
            }
        }

    }
}
