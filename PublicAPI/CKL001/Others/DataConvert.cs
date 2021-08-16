using System;
using System.Collections.Generic;
 
using System.Text;

namespace PublicAPI.CKL001.Others
{
    public class DataConvert
    {
        /// <summary>
        /// BCC校验
        /// </summary>
        /// <param name="data">需要校验的数据</param>
        /// <returns></returns>
        public static byte BccCheck(byte[] data)
        {
            byte CheckCode = 0;
            int len = data.Length;
            for (int i = 0; i < len; i++)
            {
                CheckCode ^= data[i];
            }
            return CheckCode;
        }

        /// <summary>
        /// 转换2：   ushort   ->   数组    如10 = {00 ，0A}
        /// </summary>
        public static byte[] Ushort_To_Bytes(ushort i)
        {
            return new byte[]
            {
                (byte)((0xff00 & i)>>8),
                (byte) (0xff & i),
            };
        }

        /// <summary>
        ///    转换3：   数组16   -   Ushort     如：{00，0A}  = 10（仅用于2个元素的数组转换，其他出错）
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
        ///    转换4：   Copy  Some Bytes  
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
        /// 转换：   数组10   ->   字符串16 
        /// </summary>
        public static string Bytes_To_HexStr(byte[] bytes)
        {
            string str = "";
            if (bytes == null)
            {
                return str;
            }
            foreach (byte bt in bytes)
            {
                str += bt.ToString("X2");
            }
            return str;
        }
    }
}
