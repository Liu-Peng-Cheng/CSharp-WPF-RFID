using System;
using System.Collections.Generic;
 
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace PublicAPI.CKL001.Connected
{
    internal class TypeSerial:Base
    {
        private SerialPort serialPort;
        private static Dictionary<string, Parity> dictionaryParity;
        static TypeSerial()
        {
            dictionaryParity = new Dictionary<string, Parity>{
            { "n", Parity.None },
            { "o", Parity.Odd  },
            { "e", Parity.Even },
            { "m", Parity.Mark },
            { "s", Parity.Space}
            };
        }
        public override void Dispose()
        {
            this.Closed();
        }
        [CompilerGenerated]
        private void ReceivedMsg(object objected, SerialDataReceivedEventArgs e)
        {
            base.stopwatch.Start();
            if (base.isConnected)
            {
                try
                {
                    int receivedcount = serialPort.Read(base.readTempBuffer, 0, base.readTempBuffer.Length);
                    if (receivedcount > 0)
                    {
                        lock (base.SyncObject)
                        {
                            int waitetimeMilliseconds = 1000;
                            while ((receivedcount + base.receivedRingBuffer.DataCount) > 1024 * 5)
                            {
                                Monitor.Wait(base.SyncObject, waitetimeMilliseconds);
                            }
                            base.receivedRingBuffer.WriteToRingBuffer(base.readTempBuffer, 0, receivedcount);
                            Monitor.PulseAll(base.SyncObject);
                        }
                    }
                }
                catch { }
            }        
        }
        public override bool Connected(string readName,string _8n1="8:n:1")
        {
            try
            {
                if (serialPort != null)
                    return false;
                if (2 == readName.Split(new char[] { ':' }).Length && 3 == _8n1.Split(new char[] { ':'}).Length)
                {
                    readName = readName + ":" + _8n1;
                }
                else
                    return false;
                this.serialPort = new SerialPort();
                string[] serialinfo = readName.Split(new char[] { ':' });
                serialPort.WriteTimeout = 2000;
                serialPort.PortName = serialinfo[0].ToUpper();
                serialPort.BaudRate = int.Parse(serialinfo[1]);
                serialPort.DataBits = int.Parse(serialinfo[2]);
                serialPort.Parity = dictionaryParity[serialinfo[3].ToLower()];
                switch (int.Parse(serialinfo[4]))
                {
                    case 0:
                        serialPort.StopBits = StopBits.None;
                        break;
                    case 1:
                        serialPort.StopBits = StopBits.One;
                        break;
                    case 2:
                        serialPort.StopBits = StopBits.Two;
                        break;
                    case 3:
                        serialPort.StopBits = StopBits.OnePointFive;
                        break;
                }
                serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceivedMsg);
                serialPort.Open();
                base.isConnected = true;
                base.ReceivedDataSplit_AddToThreadPool();
                return true;
            }
            catch (Exception e)
            {
                base.isConnected = false;
                this.Closed();
                return false;
            }
        }

        public override void Send(byte[] sendBytes)
        {
            TypeSerial connected;
            Monitor.Enter(connected = this);
            try
            {
                this.serialPort.Write(sendBytes, 0, sendBytes.Length);
            }
            catch { }
            finally
            {
                Monitor.Exit(connected);
            }
        }

        public override void Send(PublicAPI.CKL001.MessageObj.MsgObj.MsgObjBase msg)
        {
            if (msg == null)
                return;
            msg.CmdPacked();

            this.Send(msg.CmdToBytes());
        }
        public override void Closed()
        {
            try
            {
                base.isConnected = false;
                if ((this.serialPort != null) && this.serialPort.IsOpen)
                {
                    this.serialPort.Close();
                    this.serialPort.BaseStream.Close();
                    this.serialPort = null;
                }
                lock (base.SyncObject)
                {
                    Monitor.PulseAll(base.SyncObject);
                }
            }
            catch { }

        }
    }
}
