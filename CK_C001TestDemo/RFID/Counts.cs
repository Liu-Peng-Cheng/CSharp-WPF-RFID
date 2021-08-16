using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK_C001TestDemo.RFID
{
    public class Counts:MsgObj.ViewModelBase
    {
        long tagNum;
        public long TagNum { get => tagNum; set { tagNum = value; OnPropertyChanged("TagNum"); } }
        long readTotal;
        public long ReadTotal { get => readTotal; set { readTotal = value; OnPropertyChanged("ReadTotal"); } }
        long tagSpeed;
        public long TagSpeed { get => tagSpeed; set { tagSpeed = value; OnPropertyChanged("TagSpeed"); } }
    }
}
