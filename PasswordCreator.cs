using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace instdb
{
    /// <summary>
    /// 创建密码
    /// </summary>
    public class PasswordCreator
    {
        /// <summary>
        /// 生成随机密码
        /// </summary>
        /// <param name="len">密码长度</param>
        /// <returns></returns>
        public static string CreateNewPassword(int len)
        {
            var array = new List<char>(56);
            for (int i = 48; i <= 57; i++)
            {
                char c = (char)i;
                array.Add(c);
            }
            for (int i = 65; i < 90; i++)
            {
                char c = (char)i;
                array.Add(c);
            }
            for (int i = 97; i < 122; i++)
            {
                char c = (char)i;
                array.Add(c);
            }

            var sb = new StringBuilder(len);

            var guid = Guid.NewGuid();
            byte[] bts = guid.ToByteArray();
            var seed = BitConverter.ToInt32(bts, 0);
            var pwdRandom = new Random(seed);

            for (int i = 0; i < len; i++)
            {
                int index = pwdRandom.Next(0, array.Count - 1);
                char c = array[index];
                sb.Append(c);
            }

            string returnStr = sb.ToString();
            return returnStr;
        }

    }

    /// <summary>
    /// 密码存储
    /// </summary>
    public class PasswordStoreage
    {
        public static bool SavePasswordToFile(string fileName, string psw)
        {
            try
            {
                string dir = Path.GetDirectoryName(fileName);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    byte[] bts = Encoding.ASCII.GetBytes(psw);
                    fs.Write(bts, 0, bts.Length);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
