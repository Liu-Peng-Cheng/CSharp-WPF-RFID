using PublicAPI.CKC001.MessageObj.MsgObj;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAPI.CKC001.MessageObj.Notify
{
    public class Notify_Humiture: NotifyBase
    {
        double temperature;
        double humidity;

        internal double setTemperature { set => temperature = value; }
        public double getTemperatureFload { get => temperature; }
        public string getTemperatureStr { get => temperature + "℃"; }

        internal double setHumidity { set => humidity = value; }
        public double getHumidityFloat { get => humidity; }
        public string getHumidityStr { get => humidity + "%RH"; }

        internal Notify_Humiture(MsgObjBase msg, string ip)
        {
            base.NotifyAdditiveAttributeA(msg, ip);
        }
    }
}
