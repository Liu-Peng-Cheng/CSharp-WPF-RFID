using CK_C001TestDemo.Log;
using CK_C001TestDemo.MainTabControl;
using CK_C001TestDemo.MsgObj;
using CK_C001TestDemo.RFID;
using CK_C001TestDemo.Test;
using log4net;
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

namespace CK_C001TestDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.pageContainer.Content= new MainTabControl.MainTabControlPage();
            this.MouseLeftButtonDown += WindowMove;
            this.BtnShutdown.MouseLeftButtonDown += BtnShutdown_MouseDown;
        }
         
        private void BtnShutdown_MouseDown(object sender, MouseButtonEventArgs e)=>this.Close();

        private void WindowMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try { this.DragMove(); } catch { }
            }
        }
    }
}
