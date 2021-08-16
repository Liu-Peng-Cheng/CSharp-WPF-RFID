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

namespace CK_C001TestDemo.MainTabControl
{
    /// <summary>
    /// MainTabControlPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainTabControlPage : Page
    {
        //CK-C001页面
        public Test.TestPage testPage;
        //CK-C001的日志页面
        public Log.LogPage logPage;
        //锁控板连接对象
        public PublicAPI.CKL001.Connected.ClientObject LockConnectedObject;
        //RFID读写器连接对象
        public GDotnet.Reader.Api.DAL.GClient RfidConnectedObject;
        public PublicAPI.CKC001.Connected.ServerObject Server;

        public MainTabControlPage()
        {
            InitializeComponent();
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Pack://application:,,,/PublicResource;Component/styles/TableControl_Main.xaml", UriKind.RelativeOrAbsolute) });
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = lang.Languages.ResourceChsUri });

            //配置
            this.FrameSetting.SetValue(Page.ContentProperty, new Networkconfig.NetworkconfigPage(this));
            //测试页
            this.FrameTest.SetValue(Page.ContentProperty,testPage =  new Test.TestPage(this));
            //日志页
            this.FrameLog.SetValue(Page.ContentProperty,logPage = new Log.LogPage(this));
            //附加页-读写器
            this.FrameAppendRFID.SetValue(Page.ContentProperty, new RFID.LoginPage(this));
            //附加页-锁控板
            this.FrameAppendLock.SetValue(Page.ContentProperty, new LockControl.LoginPage(this));
            
        }
    }
}
