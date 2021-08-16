using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK_C001TestDemo.MsgObj
{
    public class ConObj:ViewModelBase
    {
        private bool oper;
        private int num;
        private bool status;
        private string link;
        private string devIP;
        private string _lock;
        private string light;
        private string alarmLED;
        private string vena;
        private string hF;
        private string rfid;
        private string qRCode;
        private string humiture;
        private string beep;
        private byte addressNum;
        private string devMAC;
        private string devSN;
        private string devType;

        public bool Operate { get=> oper; set { oper = value;OnPropertyChanged("Operate"); } }
        public int Num { get=>num; set { num = value;OnPropertyChanged("Num"); } }
        public bool Status { get => status; set { status = value;  } } 
        public string IsConnected { get => link; set { link = value; OnPropertyChanged("IsConnected"); } } 
        public string DevIP { get => devIP; set { devIP = value; OnPropertyChanged("DevIP"); } } 
        public string Lock { get => _lock; set { _lock = value; OnPropertyChanged("Lock"); } }
        public string Light { get => light; set { light = value; OnPropertyChanged("Light"); } }
        public string AlarmLED { get => alarmLED; set { alarmLED = value; OnPropertyChanged("AlarmLED"); } }
        public string Vena { get => vena; set { vena = value; OnPropertyChanged("Vena"); } }
        public string HF { get => hF; set { hF = value; OnPropertyChanged("HF"); } }
        public string RFID { get => rfid; set { rfid = value; OnPropertyChanged("RFID"); } }
        public string QRCode { get => qRCode; set { qRCode = value; OnPropertyChanged("QRCode"); } }
        public string Humiture { get => humiture; set { humiture = value; OnPropertyChanged("Humiture"); } }
        public string Beep { get => beep; set { beep = value; OnPropertyChanged("Beep"); } }
        public byte AddressNum { get => addressNum; set { addressNum = value; OnPropertyChanged("AddressNum"); } }
        public string DevMAC { get => devMAC; set { devMAC = value; OnPropertyChanged("DevMAC"); } }
        public string DevSN { get => devSN; set { devSN = value; OnPropertyChanged("DevSN"); } }
        public string DevType { get => devType; set { devType = value; OnPropertyChanged("DevType"); } }
        public PublicAPI.CKC001.Connected.ClientObject DevClient { get; set; }

    }
}
