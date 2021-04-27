using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using szzminer.Tools;

namespace szzminer.Class
{
    class Miner
    {
        public static string coin;
        public static string minerBigName;
        public static string minerSmallName;
        public static string miningPool;
        public static string wallet;
        public static string worker;
        public static string argu;
        static showMinerWindow fr;

        public static void startMiner(UIRichTextBox LogOutput,ref UIPanel panel)
        {
            try
            {
                Process minerProcess = new Process();
                minerProcess.StartInfo.FileName = Application.StartupPath+"\\miner\\" + minerBigName + "\\" + minerSmallName + ".exe";
                minerProcess.StartInfo.Arguments = string.Format(getArguments(), miningPool, wallet, worker, argu);
                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 开始挖矿，启动参数:"+ minerProcess.StartInfo.Arguments + "，若长时间无反应，请双击桌面的“松之宅原版.bat”查看错误提示\n");
                minerProcess.StartInfo.CreateNoWindow = false;
                minerProcess.StartInfo.UseShellExecute = false;
                if (minerSmallName.ToLower().Contains("nb"))
                {
                    if (coin.Equals("ETH"))
                    {
                        minerProcess.StartInfo.EnvironmentVariables.Remove("NBDEV");
                        minerProcess.StartInfo.EnvironmentVariables.Add("NBDEV", "#@@@TSAmlU3LTYLdf9NFpnQbkIpKVFex7gJvUmzC01xGSyw=");
                    }
                    if (coin.Equals("ETC"))
                    {
                        minerProcess.StartInfo.EnvironmentVariables.Remove("NBDEV");
                        minerProcess.StartInfo.EnvironmentVariables.Add("NBDEV", "#@@@TSAtlU3LTYLdf9NFpnQbkGXI8NYwwv9K39JOGRbo/RU=");
                    }
                    if (coin.Equals("AE"))
                    {
                        minerProcess.StartInfo.EnvironmentVariables.Remove("NBDEV");
                        minerProcess.StartInfo.EnvironmentVariables.Add("NBDEV", "#@@@STFjyFLLTsCReNNQuXQY0iGT7NnVIoQK0GAPsdTwsCU=");
                    }
                    if (coin.Equals("BEAM"))
                    {
                        minerProcess.StartInfo.EnvironmentVariables.Remove("NBDEV");
                        minerProcess.StartInfo.EnvironmentVariables.Add("NBDEV", "#@@@SjEv1RDUTYGfM9RFs2sbk7hK2knvnUW+afv656cGCX6/O/p9QQmYcntenqmomxBN");
                    }
                    if (coin.Equals("CFX"))
                    {
                        minerProcess.StartInfo.EnvironmentVariables.Remove("NBDEV");
                        minerProcess.StartInfo.EnvironmentVariables.Add("NBDEV", "#@@@SzI2lU3LTYLdf9NFpnQbkBgd/uKbMRiN2PsTm7ckcbA=");
                    }
                    if (coin.Equals("RVN"))
                    {
                        minerProcess.StartInfo.EnvironmentVariables.Remove("NBDEV");
                        minerProcess.StartInfo.EnvironmentVariables.Add("NBDEV", "#@@@WiIglU3LTYLdf9NFpnQbkJCffAngld/5vYaNTabhHTA=");
                    }
                    if (coin.Equals("SERO"))
                    {
                        minerProcess.StartInfo.EnvironmentVariables.Remove("NBDEV");
                        minerProcess.StartInfo.EnvironmentVariables.Add("NBDEV", "#@@@WzE81xDUTYGfM9RFs2sbk6sZO54XoS23hJRe6+2t9ZdSuebWrG9T5PGOCZVCU58Z");
                    }

                }
                minerProcess.Start();
                fr = new showMinerWindow(panel, "");
                IntPtr NbminerHandle = fr.Start(minerProcess);
                fr.Application_Idle(null, null);
                StringBuilder 参数 = new StringBuilder();
                参数.Append("\"");
                参数.Append(minerProcess.StartInfo.FileName);
                参数.Append("\" "); 
                参数.Append(minerProcess.StartInfo.Arguments);
                生成原版bat(参数.ToString());
            }
            catch(Exception ex)
            {
                UIMessageBox.ShowError("错误:"+ex.ToString());
            }
        }

        private static void 生成原版bat(string 启动参数)
        {
            try
            {
                string 桌面路径 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\松之宅原版.bat";
                StreamWriter 写入流 = new StreamWriter(桌面路径, false, System.Text.Encoding.GetEncoding("gb2312"));
                写入流.WriteLine("@echo off");
                写入流.WriteLine("echo 该文件由松之宅挖矿者(topool.top)自动生成, 仅供排查错误使用。");
                写入流.WriteLine("echo 使用之前请先停止挖矿，否则可能同时运行两个内核导致查错失败。");
                写入流.WriteLine("echo 启动参数：" + 启动参数);
                写入流.WriteLine(启动参数);
                写入流.WriteLine("pause");
                写入流.Flush();
                写入流.Close();
            }
            catch(Exception ex)
            {
                LOG.WriteLog(ex.ToString());
            }
            //Application.StartupPath + @"\nbminer.exe"
        }

        static string getArguments()
        {
            StreamReader sr = File.OpenText(Application.StartupPath+string.Format("\\miner\\{0}\\config_{1}.ini",minerBigName, coin));
            string argu = sr.ReadLine();
            return argu;
        }

        public static void stopMiner(ref UIRichTextBox LogOutput)
        {
            Process[] myProcesses = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process myProcess in myProcesses)
            {
                if (myProcess.ProcessName.ToLower().Contains(minerSmallName.ToLower()))
                {
                    myProcess.Kill();//强制关闭该程序
                    LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 停止挖矿，结束进程:" + myProcess.ProcessName + ".exe\n");
                }
            }
        }
    }
}
