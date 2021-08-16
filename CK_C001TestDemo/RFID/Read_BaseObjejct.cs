using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDotnet.Reader.Api.DAL;
using GDotnet.Reader.Api.Protocol.Gx;

namespace CK_C001TestDemo.RFID
{
    public class Read_BaseObjejct
    {
        GDotnet.Reader.Api.DAL.GClient con;
        string epc;
        string tid;
        uint ant;

        public GClient Con { get => con; set => con = value; }
        public string Epc { get => epc; set => epc = value; }
        public string Tid { get => tid; set => tid = value; }
        public uint Ant { get => ant; set => ant = value; }
    }
}
