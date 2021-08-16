namespace CK_C001TestDemo.lang
{
    using System;
    class Languages
    {
        //public static Uri ResourceEngUri { get => new Uri("/PublicResources;component/lang/en-us.xaml", UriKind.RelativeOrAbsolute); }
        public static Uri ResourceChsUri { get; } = new Uri("/lang/zh-cn.xaml", UriKind.RelativeOrAbsolute);
        public static Uri ResourceEngUri { get => new Uri("/lang/en-us.xaml", UriKind.RelativeOrAbsolute); }
    }
}
