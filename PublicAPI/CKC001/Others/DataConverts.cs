using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Globalization;

namespace PublicAPI.CKC001.Others
{
    public class DataConverts
    {
        #region 校验函数

        /// <summary>
        /// 校验：校验(不包含帧头和帧尾校验位)并获得校验数组
        /// </summary>
        public static byte[] Make_CRC16(byte[] puchMsg)
        {
            int wCRCin = 0x0000; //初值
            int wCPoly = 0x1021; //多项式
            int wChar = 0;
            int k = 0;
            int usDataLen = puchMsg.Length;
            while (usDataLen-- > 0)
            {
                wChar = puchMsg[k++];
                wCRCin ^= wChar << 8;
                for (int i = 0; i < 8; i++)
                {
                    if ((wCRCin & 0x8000) > 0)
                        wCRCin = (UInt16)((wCRCin << 1) ^ wCPoly);
                    else
                        wCRCin = (UInt16)(wCRCin << 1);
                }
            }
            byte[] bytes = Int_To_Bytes(wCRCin);
            return new byte[] { bytes[2], bytes[3] };
        }
        public static byte[] Make_CRC16(string msg)
        {
            byte[] puchMsg = HexStr_To_Bytes(msg);
            return Make_CRC16(puchMsg);
        }
        #endregion



        /// <summary>
        /// 转换1：   字符串16   ->   数组10(不要求4byte ,2char = 1byte)
        /// </summary>
        public static byte[] HexStr_To_Bytes(string hexstr)
        {
            hexstr = hexstr.Replace(@"\s", "").Replace(" ", "");
            if (hexstr.Length % 2 != 0)
                hexstr = hexstr.Insert(0, "0");
            byte[] returnBytes = new byte[hexstr.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexstr.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary>
        /// 转换2：   int/ushort10   ->   数组10    如10 = {0A ，00}
        /// </summary>
        public static byte[] Int_To_Bytes(int i)
        {
            byte[] _temp = new byte[]
            {
                (byte) (0xff & i),
                (byte)((0xff00 & i)>>8),
                (byte)((0xff0000 & i)>>16),
                (byte)((0xff000000 & i)>>24)
            };
            Array.Reverse(_temp);
            return _temp;
        }

        /// <summary>
        /// 转换2：   int/ushort10   ->   数组10    如10 = {0A ，00}
        /// </summary>
        public static byte[] Int_To_Bytes(uint i)
        {
            byte[] _temp = new byte[]
            {
                (byte) (0xff & i),
                (byte)((0xff00 & i)>>8),
                (byte)((0xff0000 & i)>>16),
                (byte)((0xff000000 & i)>>24)
            };
            Array.Reverse(_temp);
            return _temp;
        }
        /// <summary>
        /// 转换2：   int/ushort10   ->   数组10    如10 = {00 ，0A}
        /// </summary>
        public static byte[] Int_To_Bytes(ushort i)
        {
            byte[] _temp = new byte[]
            {
                (byte) (0xff & i),
                (byte)((0xff00 & i)>>8),
            };
            Array.Reverse(_temp);
            return _temp;
        }

        /// <summary>
        /// 转换3：   数组10   ->   字符串16   (偏移N位，截取长度为len)
        /// </summary>
        public static string Bytes_To_HexStr(byte[] bytes, int off, int len)
        {
            string str = string.Empty;
            if (bytes == null)
            {
                return str;
            }
            if ((off + len) > bytes.Length)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = off; i < (off + len); i++)
            {
                stringBuilder.Append(bytes[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 转换4：   字符串2   ->   int/ushort/ulong10
        /// </summary>
        public static ulong BinStr_To_Dec(string strBinary)
        {
            ulong num = 0L;
            strBinary = strBinary.Replace(" ", "");
            if (strBinary.Length > 0x40)
            {
                throw new Exception("String is longer than 64 bits, less than 64 bits is required");
            }
            for (int i = strBinary.Length; i > 0; i--)
            {
                if ((strBinary[i - 1] != '1') && (strBinary[i - 1] != '0'))
                {
                    throw new Exception("String is not in binary string format");
                }
                ulong num3 = (strBinary[i - 1] == '1') ? ((ulong)1L) : ((ulong)0L);
                num += num3 << (strBinary.Length - i);
            }
            return num;
        }

        /// <summary>
        /// 转换5：   字符串2   ->   字符串8
        /// </summary>
        public static string BinStr_To_OctStr(string strBinary)
        {
            StringBuilder builder = new StringBuilder();
            string str = strBinary;
            while (str.Length > 0)
            {
                int startIndex = (str.Length > 4) ? (str.Length - 4) : 0;
                int length = (str.Length > 4) ? 4 : str.Length;
                string str2 = str.Substring(startIndex, length);
                str = str.Remove(startIndex, length);
                string str3 = Convert.ToString((long)BinStr_To_Dec(str2), 0x10);
                builder = builder.Insert(0, str3);
            }
            return builder.ToString().ToUpper();
        }

        /// <summary>
        /// 转换6：   数组bool   ->   数组10
        /// </summary>
        public static byte[] BoolBytes_To_Bytes(bool[] bit_array)
        {
            int num = bit_array.Length % 8;
            int num2 = (num > 0) ? ((bit_array.Length + 8) - num) : bit_array.Length;
            int num3 = num2 / 8;
            bool[] destinationArray = new bool[num2];
            Array.Copy(bit_array, 0, destinationArray, (num > 0) ? (8 - num) : 0, bit_array.Length);
            byte[] buffer = new byte[num3];
            for (int i = 0; i < num3; i++)
            {
                byte num5 = 0;
                for (int j = 0; j < 8; j++)
                {
                    num5 = (byte)(num5 << 1);
                    num5 = (byte)(num5 + (destinationArray[(i * 8) + j] ? ((byte)1) : ((byte)0)));
                }
                buffer[i] = num5;
            }
            return buffer;
        }
        /// <summary>
        /// 转换7：   数组10   ->   字符串16
        /// </summary>
        public static string Bytes_To_HexStr(byte[] byte_array)
        {
            string str = string.Empty;
            try
            {
                for (int i = 0; i < byte_array.Length; i++)
                {
                    str = str + string.Format("{0:X2}", byte_array[i]);
                }
            }
            catch
            {
            }
            return str;
        }

        /// <summary>
        /// 转换9：   uint/10   ->    字符串2     strLen为多少位2进制
        /// </summary>
        public static string Dec_To_BinStr(ulong dec, int strLen)
        {
            string str = Convert.ToString((long)dec, 2);
            if (str.Length > strLen)
            {
                throw new Exception("Converted string is longer than expected!");
            }
            int length = str.Length;
            return str.PadLeft(strLen, '0');
        }

        /// <summary>
        /// 转换10：   字符   ->   字符串2
        /// </summary>
        public static string Char_To_BinStr(char char_0)
        {
            switch (char_0)
            {
                case '0':
                    return "0000";

                case '1':
                    return "0001";

                case '2':
                    return "0010";

                case '3':
                    return "0011";

                case '4':
                    return "0100";

                case '5':
                    return "0101";

                case '6':
                    return "0110";

                case '7':
                    return "0111";

                case '8':
                    return "1000";

                case '9':
                    return "1001";

                case 'A':
                case 'a':
                    return "1010";

                case 'B':
                case 'b':
                    return "1011";

                case 'C':
                case 'c':
                    return "1100";

                case 'D':
                case 'd':
                    return "1101";

                case 'E':
                case 'e':
                    return "1110";

                case 'F':
                case 'f':
                    return "1111";
            }
            throw new Exception("Input is not a  Hex. string");
        }

        /// <summary>
        ///    转换11：   字符串16   ->   字符串2
        /// </summary>
        public static string HexStr_To_BinStr(string strHex)
        {
            string str = string.Empty;
            int length = strHex.Length;
            try
            {
                for (int i = 0; i < length; i++)
                {
                    str = str + Char_To_BinStr(strHex[i]);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        /// <summary>
        /// 转换12：   长整型   ->   字符串16
        /// </summary>
        public static string Dec_To_HexStr(long lControlWord)
        {
            string str = lControlWord.ToString("X");
            string str2 = "";
            int num = 8 - str.Length;
            if (num < 0)
            {
                return null;
            }
            for (int i = 0; i < num; i++)
            {
                str2 = str2 + "0";
            }
            return (str2 + str);
        }

        /// <summary>
        ///    转换13：   字符串16   ->   单词
        /// </summary>
        public static string HexStr_To_Word(string strHexString)
        {
            byte[] buffer = HexStr_To_Bytes(strHexString);
            int length = buffer.Length;
            char[] chArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                chArray[i] = (char)buffer[i];
            }
            return new string(chArray);
        }

        /// <summary>
        ///    转换14：   单词   ->   字符串16
        /// </summary>
        public static string Word_To_HexStr(string strString)
        {
            char[] chArray = strString.ToCharArray();
            int length = chArray.Length;
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = (byte)chArray[i];
            }
            return Bytes_To_HexStr(buffer);
        }

        /// <summary>
        ///    转换15：   字符串换行
        /// </summary>
        public static string Indent(string to_indent)
        {
            char[] separator = new char[] { '\n' };
            string[] strArray = to_indent.Replace("\r", "").Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string str2 = "";
            foreach (string str3 in strArray)
            {
                str2 = str2 + "  " + str3 + "\r\n";
            }
            return str2;
        }

        /// <summary>
        /// 转换16：   IP地址   ->   long
        /// </summary>
        public static long Ip_To_Long(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] strArray = ip.Split(separator);
            return ((((long.Parse(strArray[0]) << 0x18) | (long.Parse(strArray[1]) << 0x10)) | (long.Parse(strArray[2]) << 8)) | long.Parse(strArray[3]));
        }

        /// <summary>
        /// 转换17：   long   ->   IP地址
        /// </summary>
        public static string Long_To_Ip(long ipInt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append((long)((ipInt >> 0x18) & 0xffL)).Append(".");
            builder.Append((long)((ipInt >> 0x10) & 0xffL)).Append(".");
            builder.Append((long)((ipInt >> 8) & 0xffL)).Append(".");
            builder.Append((long)(ipInt & 0xffL));
            return builder.ToString();
        }
        /// <summary>
        ///    转换18：   Long   ->   日期
        /// </summary>
        public static string Long_To_Date(ulong ulong_0)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long ticks = time.Ticks + ((long)(((ulong)10L) * ulong_0));
            time = new DateTime(ticks, DateTimeKind.Utc);
            return string.Format("{00}.{1:00000}", time.ToString("G"), ulong_0 % ((ulong)0xf4240L));
        }

        /// <summary>
        /// 转换18：   日期   ->   Long
        /// </summary>
        public static ulong Date_To_Long(string string_0)
        {
            try
            {
                DateTime time;
                DateTime.TryParse(string_0, null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out time);
                DateTime time2 = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return (ulong)((time.Ticks - time2.Ticks) / 10L);
            }
            catch
            {
                return 0L;
            }
        }

        /// <summary>
        ///  转换19：   数组10   ->   字符串     （特定格式）
        /// </summary>
        public static string Bytes_To_TypeStr(byte[] bytes, string format)
        {
            string ret = string.Empty;
            foreach (byte mybyte in bytes)
            {
                ret += mybyte.ToString("X2") + format;
            }
            ret = ret.Substring(0, ret.Length);
            return ret;
        }

        /// <summary>
        ///    转换20：   Copy  Some Bytes  
        /// </summary>
        public static byte[] ReadBytes(byte[] respose, int index, int length)
        {
            byte[] revs = new byte[length];
            for (int i = 0; i < length; i++)
            {
                revs[i] = respose[index + i];
            }
            return revs;
        }

        /// <summary>
        ///    转换21：   数组16   -   Ushort     如：{00，0A}  = 10（仅用于2个元素的数组转换，其他出错）
        /// </summary>
        public static ushort Bytes_To_Ushort(byte[] hexByes)
        {
            ushort _temp = 0;
            Array.Reverse(hexByes);
            for (int i = 0; i < hexByes.Length; i++)
            {
                _temp += (ushort)((0xff & hexByes[i]) << (8 * i));
            }
            return _temp;
        }
        /// <summary>
        ///    转换21：   数组16   -   Long    如：{00，0A}  = 10
        /// </summary>
        /// 
        public static long Bytes_To_Long(byte[] hexByes)
        {
            long _temp = 0;
            if (hexByes.Length > 8)
                return 0;
            Array.Reverse(hexByes);
            for (int i = 0; i < hexByes.Length; i++)
            {
                _temp += (long)((0xff & hexByes[i]) << (8 * i));
            }
            return _temp;
        }
        public static uint Bytes_To_Uint(byte[] hexByes)
        {
            uint _temp = 0;
            if (hexByes.Length > 4)
                return 0;
            Array.Reverse(hexByes);
            for (int i = 0; i < hexByes.Length; i++)
            {
                _temp += (uint)((0xff & hexByes[i]) << (8 * i));
            }
            return _temp;
        }
        /// <summary>
        /// 转换22：   数组10加数组10    ->    合并成超大的数组
        /// </summary>
        public static byte[] Bytes_Bytes_To_BytesPlus(byte[] firstBytes, byte[] LastBytes)
        {

            byte[] plusBytes;
            if (firstBytes == null && LastBytes != null)
            {
                plusBytes = LastBytes;
                return plusBytes;
            }
            if (LastBytes == null && firstBytes != null)
            {
                plusBytes = firstBytes;
                return plusBytes;
            }
            plusBytes = new byte[firstBytes.Length + LastBytes.Length];
            if (LastBytes == null && firstBytes == null)
                return null;
            Array.Copy(firstBytes, 0, plusBytes, 0, firstBytes.Length);
            Array.Copy(LastBytes, 0, plusBytes, firstBytes.Length, LastBytes.Length);
            return plusBytes;
        }

        /// <summary>
        /// 转换23：  字符串10   ->   数组10 (U32)
        /// </summary>
        public static byte[] Text_Bytes_To_Bytes(string strBytes)
        {
            if (strBytes == "" || strBytes == null)
                return null;
            string strBytes1 = strBytes.Replace(" ", "");
            while (strBytes1.Length % 4 != 0)
            {
                strBytes1 += "0";
            }

            byte[] Bytes10 = new byte[strBytes1.Length / 2];
            for (int i = 0; i < Bytes10.Length; i++)
            {
                Bytes10[i] = Convert.ToByte(strBytes1.Substring(i * 2, 2).Replace(" ", ""), 16);
            }
            return Bytes10;
        }

        public static string[] Doubles_T0_Strs(double[] doubleList)
        {
            if (doubleList == null)
                return new string[] { };
            int dataLength = doubleList.Length;
            string[] strList = new string[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                strList[i] = doubleList[i].ToString("f3");
            }
            return strList;
        }

        /// <summary>
        /// 十进制字符串转十进制数字（2位或3位的，太多了，累）
        /// </summary>
        public static byte DecStr_To_DecByte(string decString)
        {
            if ((decString == "" || decString == null) && (decString.Length > 3))
                return 0;
            //decString.Replace(" ", "");
            int lengh = decString.Length;
            byte Num = 0;
            Num += Convert.ToByte(decString.Substring(0, lengh), 10);
            return Num;
        }

        /// <summary>
        /// 十六进制 ASCII   转字符串
        /// </summary>
        public static string Bytes_To_ASCII(byte[] hexBytes)
        {
            int len = hexBytes.Length;
            string strCharacter = "";
            if (len == 0)
                return null;
            for (int i = 0; i < len; i++)
            {
                if (hexBytes[i] >= 0 && hexBytes[i] <= 255)
                {
                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    byte[] byteArray = new byte[] { (byte)hexBytes[i] };
                    strCharacter += asciiEncoding.GetString(byteArray);
                }
                else
                    return "Bu..B.Bug,You got it!";
            }
            return strCharacter;
        }

        public static string HexStr_To_ASCII(byte hexBytes)
        {
            string strCharacter = "";
            if (hexBytes >= 0 && hexBytes <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                strCharacter = asciiEncoding.GetString(new byte[] { hexBytes });
            }
            else
                return null;
            return strCharacter;
        }

        /// <summary>
        /// 计算持续时间  毫秒
        /// </summary>
        public static long Begin_MillisecondsTime_End(DateTime beginTime, DateTime endTime)
        {
            TimeSpan endSpan = new TimeSpan(endTime.Ticks);
            TimeSpan beginSpan = new TimeSpan(beginTime.Ticks);
            return (long)endSpan.Subtract(beginSpan).Duration().TotalMilliseconds;
        }

        /// <summary>
        ///  计算持续时间  秒
        /// </summary>
        public static long Begin_SecondTime_End(DateTime beginTime, DateTime endTime)
        {
            TimeSpan endSpan = new TimeSpan(endTime.Ticks);
            TimeSpan beginSpan = new TimeSpan(beginTime.Ticks);
            return (long)endSpan.Subtract(beginSpan).Duration().TotalSeconds;
        }

        /// <summary>
        /// 计算运行时间  毫秒
        /// </summary>
        public static long Run_MillisecondsTime(DateTime beginTime)
        {
            return Begin_MillisecondsTime_End(beginTime, DateTime.Now);
        }

        /// <summary>
        /// 计算运行时间   秒
        /// </summary>
        public static long Run_SecondTime(DateTime beginTime)
        {
            return Begin_SecondTime_End(beginTime, DateTime.Now);
        }

        public static DateTime Milliseconds_UTC_To_DateTime(long utcTime)
        {
            DateTime time = new DateTime(0x7b2, 1, 1);
            return time.Add(new TimeSpan(utcTime * 0x2710L));
        }
        public static DateTime Second_UTC_To_DateTime(long utcTime)
        {
            DateTime time = new DateTime(0x7b2, 1, 1);
            return time.Add(new TimeSpan(utcTime * 0x989680L));
        }
        /// <summary>
        /// 把秒数转换为    0秒   00:00:00
        /// </summary>
        public static string LongSecond_To_TimeStr(long senondTime)
        {
            string time = "";
            long hour = senondTime / 3600;
            long min = senondTime % 3600 / 60;
            long sec = senondTime % 60;

            if (hour < 10)
            {
                time += "0" + hour.ToString();
            }
            else
            {
                time += hour.ToString();
            }
            time += ":";
            if (min < 10)
            {
                time += "0" + min.ToString();
            }
            else
            {
                time += min.ToString();
            }
            time += ":";
            if (sec < 10)
            {
                time += "0" + sec.ToString();
            }
            else
            {
                time += sec.ToString();
            }
            return time;
        }

        public static BitArray BytesArray_To_BitArray(byte[] data)
        {
            if (data == null)
                return null;
            BitArray array = new BitArray(data.Length * 8);
            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        array[(i * 8) + j] = ((data[i] >> (7 - j)) & 1) == 1;
                    }
                }
            }
            catch
            {
            }
            return array;
        }
        public static BitArray Byte_To_BitArray(byte data)
        {
            BitArray array = new BitArray(8);
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    array[i] = ((data >> (7 - i)) & 1) == 1;
                }
            }
            catch
            {
            }
            return array;
        }
        public static BitArray Int_To_BitArray(uint val, int length)
        {
            BitArray array = new BitArray(length);
            string str = Convert.ToString((long)val, 2).PadLeft(length, '0');
            for (int i = 0; i < length; i++)
            {
                array[i] = (str[i] == '1');
            }
            return array;
        }



        public static object CalculateValue(ref BitArray bit_array, ref int cursor, int len)
        {
            ulong num = 0L;
            try
            {
                for (int i = 0; i < len; i++)
                {
                    num = num << 1;
                    if (cursor >= bit_array.Length)
                    {
                        return 0;
                    }
                    num += bit_array[cursor] ? ((ulong)1L) : ((ulong)0L);
                    cursor++;
                }
                return num;
            }
            catch
            {
            }
            return num;
        }


 
        public static byte[] GetPCID()
        {
            try
            {
                string temp = "";
                System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_Processor");
                System.Management.ManagementObjectCollection moc = mc.GetInstances();
                foreach (System.Management.ManagementObject mo in moc)
                {
                    temp += mo.Properties["ProcessorId"].Value.ToString();
                }
                byte[] bytes = PublicAPI.CKC001.Others.DataConverts.HexStr_To_Bytes(temp);
                Array.Reverse(bytes);
                moc = null;
                mc = null;
                if (bytes.Length >= 4)
                {
                    return new byte[4] { bytes[0], bytes[1], bytes[2], bytes[3] };
                }
            }
            catch
            {
            }
            return new byte[4] { 0x00, 0x11, 0x22, 0x33 };
        }



    }
}
