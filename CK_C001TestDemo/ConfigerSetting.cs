
namespace CK_C001TestDemo
{
    using System.Configuration;

    public class ConfigerSetting
    {
        public static ConfigerSetting configManager;
        private static System.Configuration.Configuration config = null;

        static ConfigerSetting()
        {
            if (configManager == null)
            {
                configManager = new ConfigerSetting();
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
        }


        /// <summary>
        /// 添加键值
        /// </summary>
        public static void AddAppSetting(string key, string value)
        {
            config.AppSettings.Settings.Add(key, value);
            config.Save();
        }
        /// <summary>
        /// 修改、保存键值
        /// </summary>
        public static void SaveAppSetting(string key, string value)
        {
            try
            {
                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, value);
                config.Save();
            }
            catch
            { }
        }
        /// <summary>
        /// 获取键值
        /// </summary>
        public static string GetAppSetting(string key)
        {
            return config.AppSettings.Settings[key].Value;
        }
        /// <summary>
        /// 移除键值
        /// </summary>
        public static void DelAppSetting(string key)
        {
            try
            {
                config.AppSettings.Settings.Remove(key);
                config.Save();
            }
            catch { }
        }

    }

}
