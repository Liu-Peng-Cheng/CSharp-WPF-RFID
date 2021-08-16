using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK_C001TestDemo.MsgObj
{
    public class TagObj:ViewModelBase 
    {
        private int num;
        private string epc;
        private string status;

        public int Num { get => num; set { num = value; OnPropertyChanged("Num"); } }
        public string EPC { get => epc; set { epc = value; OnPropertyChanged("EPC"); } }
        public string Status { get => status; set { status = value; OnPropertyChanged("Status"); } }
    }
}
