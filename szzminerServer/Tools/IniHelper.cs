using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace szzminerServer.Tools
{

    class IniHelper
    {
        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        private static String filePath;
        #region API函数声明

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(string section, string key,
            string def, Byte[] retVal, int size, string filePath);

        #endregion

        public static void setPath(string path)
        {
            filePath = path;
        }
        public static List<string> ReadSections()
        {
            return ReadSections(filePath);
        }

        public static List<string> ReadSections(string iniFilename)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(null, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        public static List<string> ReadKeys(String SectionName)
        {
            return ReadKeys(SectionName, filePath);
        }

        public static List<string> ReadKeys(string SectionName, string iniFilename)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(SectionName, null, null, buf, buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }
        /// <summary>
        /// 读取ini文件
        /// </summary>
        /// <param name="Section">名称</param>
        /// <param name="Key">关键字</param>
        /// <param name="defaultText">默认值</param>
        /// <param name="iniFilePath">ini文件地址</param>
        /// <returns></returns>
        public static string GetValue(string Section, string Key, string defaultText)
        {
            if (File.Exists(filePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, defaultText, temp, 1024, filePath);
                return temp.ToString();
            }
            else
            {
                return defaultText;
            }
        }

        public static string GetValue(string Section, string Key, string defaultText, string filePath)
        {
            //MessageBox.Show(filePath);
            if (File.Exists(filePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                long s = GetPrivateProfileString(Section, Key, defaultText, temp, 1024, filePath);
                //MessageBox.Show(s.ToString());
                return temp.ToString();
            }
            else
            {
                return defaultText;
            }
        }
        /// <summary>
        /// 写入ini文件
        /// </summary>
        /// <param name="Section">名称</param>
        /// <param name="Key">关键字</param>
        /// <param name="defaultText">默认值</param>
        /// <param name="iniFilePath">ini文件地址</param>
        /// <returns></returns>
        public static bool SetValue(string Section, string Key, string Value, string iniFilePath)
        {
            var pat = Path.GetDirectoryName(iniFilePath);
            if (Directory.Exists(pat) == false)
            {
                Directory.CreateDirectory(pat);
            }
            if (File.Exists(iniFilePath) == false)
            {
                File.Create(iniFilePath).Close();
            }
            long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
            if (OpStation == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
