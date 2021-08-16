using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace PublicAPI.CKC001.MessageObj
{
    public class RingBuffer
    {
        [CompilerGenerated]
        private byte[] ringBuffer;
        [CompilerGenerated]
        private int writeFlag;
        [CompilerGenerated]
        private int readFlag;
        [CompilerGenerated]
        private int ringBufferLength;
        private byte[] ReceiveRingBuffer
        {
            [CompilerGenerated]
            get { return ringBuffer; }
            [CompilerGenerated]
            set { ringBuffer = value; }
        }
        public int DataCount
        {
            [CompilerGenerated]
            get { return ringBufferLength; }
            [CompilerGenerated]
            set { ringBufferLength = value; }
        }
        private int ReadFlag
        {
            [CompilerGenerated]
            get { return readFlag; }
            [CompilerGenerated]
            set { readFlag = value; }
        }
        private int WriteFlag
        {
            [CompilerGenerated]
            get { return writeFlag; }
            [CompilerGenerated]
            set { writeFlag = value; }
        }
        public RingBuffer(int bufferSize)
        {
            DataCount = 0;
            WriteFlag = 0;
            ReadFlag = 0;
            ReceiveRingBuffer = new byte[bufferSize];
        }

        public void Clear()
        {
            DataCount = 0;
        }
        public void Clear(int clearLength)
        {
            if (clearLength >= DataCount)
            {
                DataCount = 0;
                ReadFlag = 0;
                WriteFlag = 0;
            }
            else
            {
                if ((ReadFlag + clearLength) >= ReceiveRingBuffer.Length)
                {
                    ReadFlag = ReadFlag + clearLength - ReceiveRingBuffer.Length;
                }
                else
                {
                    ReadFlag += clearLength;
                }
                DataCount -= clearLength;
            }
        }
        public int GetDataCount()
        {
            return DataCount;
        }
        public int GetRemainCount()
        {
            return ReceiveRingBuffer.Length - DataCount;
        }
        public void ReadFromRingBuffer(byte[] readWhere, int offset, int readCount)
        {
            if (readCount > DataCount)
            {
                throw new Exception("读越界");
            }
            if ((ReadFlag + readCount) < ReceiveRingBuffer.Length)
            {
                Array.Copy(ReceiveRingBuffer, ReadFlag, readWhere, offset, readCount);
            }
            else
            {
                int readLength1 = (ReadFlag + readCount) - ReceiveRingBuffer.Length;
                int readLength2 = readCount - readLength1;
                Array.Copy(ReceiveRingBuffer, ReadFlag, readWhere, offset, readLength2);
                offset += readLength2;
                if (readLength1 != 0)
                {
                    Array.Copy(ReceiveRingBuffer, 0, readWhere, offset, readLength1);
                }
            }
        }

        public void WriteToRingBuffer(byte[] buffer)
        {
            try
            {
                WriteToRingBuffer(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void WriteToRingBuffer(byte[] receivedBuffer, int offset, int receiveDataCount)
        {
            int remainBuffer = GetRemainCount();
            if (remainBuffer >= receiveDataCount)
            {
                if ((WriteFlag + receiveDataCount) < ReceiveRingBuffer.Length)
                {
                    Array.Copy(receivedBuffer, offset, ReceiveRingBuffer, WriteFlag, receiveDataCount);
                    WriteFlag += receiveDataCount;
                    DataCount += receiveDataCount;
                }
                else
                {
                    int WriteLength1 = (WriteFlag + receiveDataCount) - ReceiveRingBuffer.Length;
                    int WriteLength2 = receiveDataCount - WriteLength1;
                    Array.Copy(receivedBuffer, offset, ReceiveRingBuffer, WriteFlag, WriteLength2);
                    WriteFlag = 0;
                    offset += WriteLength2;
                    if (WriteLength1 != 0)
                    {
                        Array.Copy(receivedBuffer, offset, ReceiveRingBuffer, WriteFlag, WriteLength1);
                        WriteFlag += WriteLength1;
                    }
                    DataCount += receiveDataCount;
                }
            }
            else
            {
                throw new Exception("写指针追上读指针啦，你咋处理这么慢呢？我把数据丢了[/手动滑稽]");
            }
        }

        public byte this[int index]
        {
            get
            {
                if (index >= DataCount)
                {
                    throw new Exception("访问数组越界");
                }
                if ((ReadFlag + index) < ReceiveRingBuffer.Length)
                {
                    return ReceiveRingBuffer[ReadFlag + index];
                }
                else
                {
                    return ReceiveRingBuffer[ReadFlag + index - ReceiveRingBuffer.Length];
                }
            }
        }
    }
}
