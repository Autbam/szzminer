using Microsoft.Win32;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using szzminer.Tools;
using szzminer.Views;

namespace szzminer.Class
{
    class Functions
    {
        public static string dllPath;
        public static List<string> BUSID = new List<string>();
        public static List<string> Hashrate = new List<string>();
        public static List<string> Accepted = new List<string>();
        public static List<string> Rejected = new List<string>();

        /// <summary>
        /// 读取dll文件并拿取内核的信息
        /// </summary>
        public static void getMinerInfo()
        {
            var asm = Assembly.LoadFile(dllPath);
            var type = asm.GetType("DisplayDll.Display");
            var instance = asm.CreateInstance("DisplayDll.Display");
            var method = type.GetMethod("getMinerInfo");
            method.Invoke(instance, null);
            method = type.GetMethod("getBUSID");
            BUSID = (List<string>)method.Invoke(instance, null);
            method = type.GetMethod("getHashrate");
            Hashrate = (List<string>)method.Invoke(instance, null);
            method = type.GetMethod("getAccepted");
            Accepted = (List<string>)method.Invoke(instance, null);
            method = type.GetMethod("getRejected");
            Rejected = (List<string>)method.Invoke(instance, null);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg">日志内容</param>
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
        /// <summary>
        /// 停止挖矿后表格内容归零
        /// </summary>
        /// <param name="uIDataGridView"></param>
        public static void afterStopMiner(ref UIDataGridView uIDataGridView)
        {
            for (int i = 0; i < uIDataGridView.Rows.Count; i++)
            {
                uIDataGridView.Rows[i].Cells[2].Value = "0";
                uIDataGridView.Rows[i].Cells[3].Value = "0";
                uIDataGridView.Rows[i].Cells[4].Value = "0";
            }
        }
        /// <summary>
        /// 从远端拿取币种，钱包矿池信息
        /// </summary>
        public static void getMiningInfo()
        {
            if (File.Exists(Application.StartupPath + "\\config\\miner.ini"))
            {
                File.Delete(Application.StartupPath + "\\config\\miner.ini");
            }
            if (File.Exists(Application.StartupPath + "\\config\\miningpool.ini"))
            {
                File.Delete(Application.StartupPath + "\\config\\miningpool.in");
            }
            DownloadFile.downloadIniFile("https://szzminer.bj.bcebos.com/miner.ini", "\\config\\miner.ini");
            DownloadFile.downloadIniFile("https://szzminer.bj.bcebos.com/miningpool.ini", "\\config\\miningpool.ini");
        }
        /// <summary>
        /// 读取miner.ini信息
        /// </summary>
        /// <param name="coin"></param>
        public static void loadCoinIni(ref UIComboBox coin)
        {
            IniHelper.setPath(Application.StartupPath + "\\config\\" + "\\miner.ini");
            List<string> coins = IniHelper.ReadSections();
            foreach (string c in coins)
            {
                coin.Items.Add(c);
            }
        }
        static Ping ping = new System.Net.NetworkInformation.Ping();
        public static void pingMiningpool(string url, ref UILabel poolping)
        {
            //ping矿池
            Regex regex = new Regex(@"(stratum\+tcp)://(?<domain>[^(:|/]*)");
            var matchs = regex.Match(url);
            var u = matchs.Groups["domain"].Value;
            if (u == "")
            {
                regex = new Regex(@"(?<domain>[^(:|/]*)");
                matchs = regex.Match(url);
                u = matchs.Groups["domain"].Value;
            }
            PingReply reply = ping.Send(u);
            long pingms = reply.RoundtripTime;
            if (reply.Status == IPStatus.Success)
            {
                poolping.Text = pingms.ToString() + "ms";
                if (pingms >= 0 && pingms <= 100)
                {
                    poolping.ForeColor = Color.Lime;
                }
                else if (pingms > 100 && pingms <= 200)
                {
                    poolping.ForeColor = Color.Orange;
                }
                else
                {
                    poolping.ForeColor = Color.Red;
                }
            }
            else
            {
                poolping.Text = "超时";
                poolping.ForeColor = Color.Red;
            }
        }
        /// <summary>
        /// 检查内核是否存在，若不存在则下载内核压缩包并解压
        /// </summary>
        /// <param name="minerName">内核名称</param>
        /// <param name="url">下载链接</param>
        public static void checkMinerAndDownload(string minerName,string url)
        {
            if (Directory.Exists(Application.StartupPath + "\\miner\\"+minerName))
            {
                return;
            }
            else
            {
                DownloadForm downloadForm = new DownloadForm(url,minerName+".zip");
                downloadForm.ShowDialog();
                Thread.Sleep(1000);//防止还没释放就崩溃
                ZipFile.ExtractToDirectory(Application.StartupPath + "\\miner\\" + minerName + ".zip", Application.StartupPath + "\\miner\\");
                Thread.Sleep(200);//防止还没释放就崩溃
                File.Delete(Application.StartupPath + "\\miner\\" + minerName + ".zip");
                //ZipFile.CreateFromDirectory(Application.StartupPath + "\\miner\\", Application.StartupPath + "\\miner\\" + minerName + ".zip");
            }
        }
        public static void closeUAC()
        {
            string path = Application.ExecutablePath;
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey rk2 = rk.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            rk2.SetValue("EnableLUA", "0");
            rk2.Close();
            rk.Close();
        }
    }
}
