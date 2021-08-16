using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK_C001TestDemo.MsgObj
{
    public class DisplayConDev:ViewModelBase
    {
        private string devIP;
        private byte devAddress;
        private string userName;
        private string useType;
        private string borrowAndReturn;
        private PublicAPI.CKC001.Connected.ClientObject client;
        bool isReading;
        
        public string DevIP { get => devIP; set { devIP = value; OnPropertyChanged("DevIP"); } }
        public byte DevAddress { get => devAddress; set { devAddress = value; OnPropertyChanged("DevAddress"); } }
        public string UserName { get => userName; set { userName = value; OnPropertyChanged("UserName"); } }
        public string UseType { get => useType; set { useType = value; OnPropertyChanged("UseType"); } }
        public string BorrowAndReturn { get => borrowAndReturn; set { borrowAndReturn = value; OnPropertyChanged("BorrowAndReturn"); } }

        public PublicAPI.CKC001.Connected.ClientObject Client { get => client; set => client = value; }
        public bool IsReading { get => isReading; set => isReading = value; }
    }
}
