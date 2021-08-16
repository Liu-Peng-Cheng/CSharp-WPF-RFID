using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CK_C001TestDemo.RFID
{
    public class TagViewObject:MsgObj.ViewModelBase
    {
        public TagViewObject(GDotnet.Reader.Api.Protocol.Gx.LogBaseEpcInfo tag)
        {            
            epc = tag.Epc;
            tid = tag.Tid;
            key = epc + "|" + tid;
            user = tag.Userdata;
            reserve = tag.Reserved;
            rssi = tag.Rssi;
            pcvaule = tag.Pc;
            frequency = tag.FrequencyPoint;
            phase = tag.Phase;
            ant = tag.AntId;
            total = 1L;
            ant1 = 0L;
            ant2 = 0L;
            ant3 = 0L;
            ant4 = 0L;
            ant5 = 0L;
            ant6 = 0L;
            ant7 = 0L;
            ant8 = 0L;
            ant9 = 0L;
            ant10 = 0L;
            ant11 = 0L;
            ant12 = 0L;
            ant13 = 0L;
            ant14 = 0L;
            ant15 = 0L;
            ant16 = 0L;
            switch (tag.AntId)
            {
                case 1: ant1 = 1L; break;
                case 2: ant2 = 1L; break;
                case 3: ant3 = 1L; break;
                case 4: ant4 = 1L; break;
                case 5: ant5 = 1L; break;
                case 6: ant6 = 1L; break;
                case 7: ant7 = 1L; break;
                case 8: ant8 = 1L; break;
                case 9: ant9 = 1L; break;
                case 10: ant10 = 1L; break;
                case 11: ant11 = 1L; break;
                case 12: ant12 = 1L; break;
                case 13: ant13 = 1L; break;
                case 14: ant14 = 1L; break;
                case 15: ant15 = 1L; break;
                case 16: ant16 = 1L; break;
            }
        }
        private string key;
        public string Key { get => key; set { key = value; } }
        private int num;
        public int Num { get => num; set { num = value; OnPropertyChanged("Num"); } }
        private string epc;
        public string EPC { get => epc; set { epc = value; OnPropertyChanged("EPC"); } }
        private string tid;
        public string TID { get => tid; set { tid = value; OnPropertyChanged("TID"); } }
        private string user;
        public string User { get => user; set { user = value; OnPropertyChanged("User"); } }
        private string reserve;
        public string Reserve { get => reserve; set { reserve = value; OnPropertyChanged("Reserve"); } }
        private long total;
        public long Total { get => total; set { total = value; OnPropertyChanged("Total"); } }

        private long ant;
        public long Ant { get => ant; set { ant = value; } }
        private long ant1;
        public long Ant1 { get => ant1; set { ant1 = value; OnPropertyChanged("Ant1"); } }
        private long ant2;
        public long Ant2 { get => ant2; set { ant2 = value; OnPropertyChanged("Ant2"); } }
        private long ant3;
        public long Ant3 { get => ant3; set { ant3 = value; OnPropertyChanged("Ant3"); } }
        private long ant4;
        public long Ant4 { get => ant4; set { ant4 = value; OnPropertyChanged("Ant4"); } }
        private long ant5;
        public long Ant5 { get => ant5; set { ant5 = value; OnPropertyChanged("Ant5"); } }
        private long ant6;
        public long Ant6 { get => ant6; set { ant6 = value; OnPropertyChanged("Ant6"); } }
        private long ant7;
        public long Ant7 { get => ant7; set { ant7 = value; OnPropertyChanged("Ant7"); } }
        private long ant8;
        public long Ant8 { get => ant8; set { ant8 = value; OnPropertyChanged("Ant8"); } }
        private long ant9;
        public long Ant9 { get => ant9; set { ant9 = value; OnPropertyChanged("Ant9"); } }
        private long ant10;
        public long Ant10 { get => ant10; set { ant10 = value; OnPropertyChanged("Ant10"); } }
        private long ant11;
        public long Ant11 { get => ant11; set { ant11 = value; OnPropertyChanged("Ant11"); } }
        private long ant12;
        public long Ant12 { get => ant12; set { ant12 = value; OnPropertyChanged("Ant12"); } }
        private long ant13;
        public long Ant13 { get => ant13; set { ant13 = value; OnPropertyChanged("Ant13"); } }
        private long ant14;
        public long Ant14 { get => ant14; set { ant14 = value; OnPropertyChanged("Ant14"); } }
        private long ant15;
        public long Ant15 { get => ant15; set { ant15 = value; OnPropertyChanged("Ant15"); } }
        private long ant16;
        public long Ant16 { get => ant16; set { ant16 = value; OnPropertyChanged("Ant16"); } }

        private uint frequency;
        public uint Frequency { get => frequency; set { frequency = value; OnPropertyChanged("Frequency"); } }
        private byte phase;
        public byte Phase { get => phase; set { phase = value; OnPropertyChanged("Phase"); } }
        private byte rssi;
        public byte RSSI { get => rssi; set { rssi = value; OnPropertyChanged("RSSI"); } }
        private ushort pcvaule;
        public ushort PcValue { get => pcvaule; set { pcvaule = value; OnPropertyChanged("PcValue"); } }
        private string tagtype;
        public string TagType { get => tagtype; set { tagtype = value; OnPropertyChanged("TagType"); } }


    }
}
