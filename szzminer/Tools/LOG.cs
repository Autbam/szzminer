using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szzminer.Tools
{
    class LOG
    {
        public static void WriteLog(string msg)//写日志
        {
            try
            {
                string path = Path.Combine("./log");
                if (!Directory.Exists(path))//判断是否有该文件 
                    Directory.CreateDirectory(path);
                string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//生成日志文件 
                if (!File.Exists(logFileName))//判断日志文件是否为当天
                {
                    FileStream fs;
                    fs = File.Create(logFileName);//创建文件
                    fs.Close();
                }
                StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流
                writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + msg + "\n");
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                string path = Path.Combine("./log");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                if (!File.Exists(logFileName))//判断日志文件是否为当天
                {
                    FileStream fs;
                    fs = File.Create(logFileName);//创建文件
                    fs.Close();
                }
                StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流
                writer.WriteLine(DateTime.Now.ToString("日志记录错误HH:mm:ss") + "\r\n " + e.Message + " " + msg);
                writer.WriteLine("--------------------------------分割线--------------------------------");
                writer.Flush();
                writer.Close();
            }

        }
    }
}
