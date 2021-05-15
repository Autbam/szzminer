using Microsoft.Win32;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using szzminer.Class;
using szzminer.Class.RemoteClass;
using szzminer.Tools;
using szzminer_overclock.AMD;

namespace szzminer.Views
{
    public partial class MainForm : UIForm
    {
        Thread MinerStatusThread;
        Thread getGpusInfoThread;
        Thread noDevfeeThread;
        public const double currentVersion = 1.22;
        bool isMining = false;
        public static string MinerStatusJson;
        public static string MinerStatusJson2;
        GoogleAnalyiticsSDK GA = new GoogleAnalyiticsSDK();
        System.DateTime TimeNow = new DateTime();
        TimeSpan TimeCount = new TimeSpan();
        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private UdpClient udpcRecv = null;

        private IPEndPoint localIpep = null;

        /// <summary>
        /// 开关：在监听UDP报文阶段为true，否则为false
        /// </summary>
        private bool IsUdpcRecvStart = false;
        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        private Thread thrRecv;
        private void StartReceive()
        {
            try
            {
                if (!IsUdpcRecvStart) // 未监听的情况，开始监听
                {
                    localIpep = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 19465); // 本机IP和监听端口号
                    udpcRecv = new UdpClient(localIpep);
                    thrRecv = new Thread(ReceiveMessage);
                    thrRecv.IsBackground = true;
                    thrRecv.Start();
                    IsUdpcRecvStart = true;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                //Application.Exit();
            }
        }
        private void StopReceive()
        {
            if (IsUdpcRecvStart)
            {
                thrRecv.Abort(); // 必须先关闭这个线程，否则会异常
                IsUdpcRecvStart = false;
                udpcRecv.Close();
            }
        }
        public void getMinerJson()
        {
            RemoteMinerStatus remoteMinerStatus = new RemoteMinerStatus();
            remoteMinerStatus.function = "minerStatus";
            remoteMinerStatus.if_mining = isMining;
            remoteMinerStatus.Worker = InputWorker.Text;
            remoteMinerStatus.Coin = SelectCoin.Text;
            remoteMinerStatus.MinerCore = SelectMiner.Text;
            remoteMinerStatus.IP = NetCardDriver.getIP();
            remoteMinerStatus.MAC = NetCardDriver.getMAC();
            remoteMinerStatus.Pool = InputMiningPool.Text;
            remoteMinerStatus.Wallet = InputWallet.Text;
            remoteMinerStatus.Hashrate = TotalHashrate.Text;
            remoteMinerStatus.Accepted = Convert.ToInt32(TotalSubmit.Text);
            remoteMinerStatus.Rejected = Convert.ToInt32(TotalReject.Text);
            remoteMinerStatus.Power = Convert.ToInt32(TotalPower.Text.Split(' ')[0]);
            remoteMinerStatus.xmrPool = xmrPool.Text;
            remoteMinerStatus.xmrWallet = xmrWallet.Text;
            remoteMinerStatus.xmrHashrate = xmrhash.Text;
            remoteMinerStatus.xmrAccept = xmrshare.Text;
            remoteMinerStatus.xmrReject = xmr_reject.Text;
            List<DevicesItem> devicesItemList = new List<DevicesItem>();
            for (int i = 0; i < GPUStatusTable.Rows.Count; i++)
            {
                DevicesItem devicesItem = new DevicesItem();
                devicesItem.idbus = Convert.ToString(GPUStatusTable.Rows[i].Cells[0].Value);
                devicesItem.name = Convert.ToString(GPUStatusTable.Rows[i].Cells[1].Value);
                devicesItem.Hashrate = Convert.ToString(GPUStatusTable.Rows[i].Cells[2].Value);
                devicesItem.accept = Convert.ToString(GPUStatusTable.Rows[i].Cells[3].Value);
                devicesItem.reject = Convert.ToString(GPUStatusTable.Rows[i].Cells[4].Value);
                devicesItem.power = Convert.ToString(GPUStatusTable.Rows[i].Cells[5].Value);
                devicesItem.temp = Convert.ToString(GPUStatusTable.Rows[i].Cells[6].Value);
                devicesItem.fan = Convert.ToString(GPUStatusTable.Rows[i].Cells[7].Value);
                devicesItem.coreclock = Convert.ToString(GPUStatusTable.Rows[i].Cells[8].Value);
                devicesItem.memoryclock = Convert.ToString(GPUStatusTable.Rows[i].Cells[9].Value);
                devicesItemList.Add(devicesItem);
            }
            remoteMinerStatus.Devices = devicesItemList;
            MinerStatusJson = JsonConvert.SerializeObject(remoteMinerStatus);
        }
        
        public void getMinerJson2()
        {
            Miner2 miner2 = new Miner2();
            miner2.Miningpool = InputMiningPool.Text;
            miner2.Coin = SelectCoin.Text;
            miner2.Core = SelectMiner.Text;
            miner2.Wallet = InputWallet.Text;
            miner2.Worker = InputWorker.Text;
            miner2.Argu = InputArgu.Text;
            miner2.Delay = Timeout.Text;
            miner2.Time = RunningTime.Text;
            miner2.IP = NetCardDriver.getIP();
            miner2.MAC = NetCardDriver.getMAC();
            List<MinerItem> devicesItemList = new List<MinerItem>();
            List<OverclockItem> overclockItems = new List<OverclockItem>();
            for (int i = 0; i < GPUStatusTable.Rows.Count; i++)
            {
                OverclockItem overclockItem = new OverclockItem();
                MinerItem devicesItem = new MinerItem();
                devicesItem.Busid = Convert.ToString(GPUStatusTable.Rows[i].Cells[0].Value);
                devicesItem.Name = Convert.ToString(GPUStatusTable.Rows[i].Cells[1].Value);
                devicesItem.Hashrate = Convert.ToString(GPUStatusTable.Rows[i].Cells[2].Value);
                devicesItem.Accept = Convert.ToString(GPUStatusTable.Rows[i].Cells[3].Value);
                devicesItem.Reject = Convert.ToString(GPUStatusTable.Rows[i].Cells[4].Value);
                devicesItem.Power = Convert.ToString(GPUStatusTable.Rows[i].Cells[5].Value);
                devicesItem.Coretemp = Convert.ToString(GPUStatusTable.Rows[i].Cells[6].Value);
                devicesItem.Fan = Convert.ToString(GPUStatusTable.Rows[i].Cells[7].Value);
                devicesItem.Coreclock = Convert.ToString(GPUStatusTable.Rows[i].Cells[8].Value);
                devicesItem.Memclock = Convert.ToString(GPUStatusTable.Rows[i].Cells[9].Value);
                devicesItemList.Add(devicesItem);
                overclockItem.Busid = Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value);
                overclockItem.Power = Convert.ToString(GPUOverClockTable.Rows[i].Cells[2].Value);
                overclockItem.Templimit = Convert.ToString(GPUOverClockTable.Rows[i].Cells[3].Value);
                overclockItem.Core = Convert.ToString(GPUOverClockTable.Rows[i].Cells[4].Value);
                overclockItem.CoreV = Convert.ToString(GPUOverClockTable.Rows[i].Cells[5].Value);
                overclockItem.Memory = Convert.ToString(GPUOverClockTable.Rows[i].Cells[6].Value);
                overclockItem.MemV = Convert.ToString(GPUOverClockTable.Rows[i].Cells[7].Value);
                overclockItem.Fan = Convert.ToString(GPUOverClockTable.Rows[i].Cells[8].Value);
                overclockItems.Add(overclockItem);
            }
            miner2.Miner = devicesItemList;
            miner2.Overclock = overclockItems;
            MinerStatusJson2 = JsonConvert.SerializeObject(miner2);
        }
        private void ReceiveMessage(object obj)
        {
            while (IsUdpcRecvStart)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref localIpep);

                    string reData = Encoding.GetEncoding("gb2312").GetString(bytRecv, 0, bytRecv.Length);
                    RemoteFunction ur = JsonConvert.DeserializeObject<RemoteFunction>(reData);
                    switch (ur.function)
                    {
                        case "startMining":
                            if (ActionButton.Text.Contains("开始"))
                            {
                                uiButton1_Click(null, null);
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控开始挖矿命令\n");
                            }
                            else
                            {
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控开始挖矿命令，但是正在挖矿，不作任何处理\n");
                            }
                            break;
                        case "stopMining":
                            if (ActionButton.Text.Contains("停止"))
                            {
                                uiButton1_Click(null, null);
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控停止挖矿命令\n");
                            }
                            else
                            {
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控停止挖矿命令，但是已经停止，不作任何处理\n");
                            }
                            break;
                        case "startXmr":
                            if (uiButton5.Enabled)
                            {
                                uiButton5_Click(null,null);
                            }
                            else
                            {
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控挖门罗命令，但是已经在挖，不作任何处理\n");
                            }
                            break;
                        case "stopXmr":
                            if (uiButton6.Enabled)
                            {
                                uiButton6_Click(null, null);
                            }
                            else
                            {
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控停止挖门罗命令，但是已经停止，不作任何处理\n");
                            }
                            break;
                        case "changeXmr":
                            if (uiButton6.Enabled)
                            {
                                uiButton6_Click(null, null);
                                changeXmr changeXmr = new changeXmr();
                                changeXmr = JsonConvert.DeserializeObject<changeXmr>(reData);
                                xmrPool.Text = changeXmr.pool;
                                xmrWallet.Text = changeXmr.wallet;
                                xmrArgu.Text = changeXmr.argu;
                                uiButton5_Click(null,null);
                            }
                            else
                            {
                                changeXmr changeXmr = new changeXmr();
                                changeXmr = JsonConvert.DeserializeObject<changeXmr>(reData);
                                xmrPool.Text = changeXmr.pool;
                                xmrWallet.Text = changeXmr.wallet;
                                xmrArgu.Text = changeXmr.argu;
                                uiButton5_Click(null, null);
                            }
                            break;
                        case "changeCoin":
                            if (ActionButton.Text.Contains("停止"))
                            {
                                uiButton1_Click(null, null);
                            }
                            changeCoinClass minerOptions = new changeCoinClass();
                            minerOptions = JsonConvert.DeserializeObject<changeCoinClass>(reData);
                            SelectCoin.Text = minerOptions.coin;
                            SelectMiner.Text = minerOptions.core;
                            SelectMiningPool.Text = minerOptions.miningpool;
                            InputMiningPool.Text = minerOptions.miningpoolurl;
                            InputWallet.Text = minerOptions.wallet;
                            InputArgu.Text = minerOptions.argu;
                            uiButton1_Click(null, null);
                            LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控修改币种命令\n");
                            break;
                        case "shutdown":
                            ExitWindows.Shutdown(true);
                            break;
                        case "reboot":
                            ExitWindows.Reboot(true);
                            break;
                        case "update":
                            updateButton_Click(null,null);
                            break;
                        case "overclock":
                            RemoteOverclock remoteOverclock = new RemoteOverclock();
                            remoteOverclock=JsonConvert.DeserializeObject<RemoteOverclock>(reData);
                            for(int i = 0; i < GPUOverClockTable.Rows.Count; i++)
                            {
                                if (GPUOverClockTable.Rows[i].Cells[1].Value.ToString() == remoteOverclock.OVData.Name)
                                {
                                    GPUOverClockTable.Rows[i].Cells[2].Value = remoteOverclock.OVData.Power;
                                    GPUOverClockTable.Rows[i].Cells[3].Value = remoteOverclock.OVData.TempLimit;
                                    GPUOverClockTable.Rows[i].Cells[4].Value = remoteOverclock.OVData.CoreClock;
                                    GPUOverClockTable.Rows[i].Cells[5].Value = remoteOverclock.OVData.CV;
                                    GPUOverClockTable.Rows[i].Cells[6].Value = remoteOverclock.OVData.MemoryClock;
                                    GPUOverClockTable.Rows[i].Cells[7].Value = remoteOverclock.OVData.MV;
                                    GPUOverClockTable.Rows[i].Cells[8].Value = remoteOverclock.OVData.Fan;
                                }
                            }
                            overClockConfirm_Click(null,null);
                            break;
                        case "setreboot":
                            if (isMining)
                            {
                                uiButton1_Click(null, null);
                                RemoteReboot remoteReboot = new RemoteReboot();
                                remoteReboot = JsonConvert.DeserializeObject<RemoteReboot>(reData);
                                timeRestart.Text = remoteReboot.hourReboot;
                                lowHashrateRestart.Text = remoteReboot.hashrateReboot;
                                uiButton1_Click(null, null);
                            }
                            else
                            {
                                RemoteReboot remoteReboot = new RemoteReboot();
                                remoteReboot = JsonConvert.DeserializeObject<RemoteReboot>(reData);
                                timeRestart.Text = remoteReboot.hourReboot;
                                lowHashrateRestart.Text = remoteReboot.hashrateReboot;
                            }
                            WriteConfig();
                            LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控修改重启条件命令\n");
                            break;
                        case "otherOption":
                            RemoteOtherOptions remoteOtherOptions = new RemoteOtherOptions();
                            remoteOtherOptions = JsonConvert.DeserializeObject<RemoteOtherOptions>(reData);
                            loginStart.Active = remoteOtherOptions.autoLogin;
                            autoMining.Active = remoteOtherOptions.autoMining;
                            autoMiningTime.Text = remoteOtherOptions.autoMiningTime;
                            autoOverclock.Active = remoteOtherOptions.autoOv;
                            WriteConfig();
                            LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控修改其他设置命令\n");
                            break;
                        case "amdOnCalc":
                            {
                                if (!Directory.Exists(Application.StartupPath + "\\bin"))
                                    Directory.CreateDirectory(Application.StartupPath + "\\bin");
                                byte[] Save = szzminer.Properties.Resources.switchradeongpu;
                                FileStream fsObj = new FileStream(Application.StartupPath + "\\bin" + @"\switchradeongpu.exe", FileMode.Create);
                                fsObj.Write(Save, 0, Save.Length);
                                fsObj.Close();
                                Process srg = new Process();
                                srg.StartInfo.FileName = Application.StartupPath + "\\bin" + @"\switchradeongpu.exe";
                                srg.StartInfo.Arguments = "--compute=on --admin --restart";
                                srg.StartInfo.CreateNoWindow = true;
                                srg.StartInfo.UseShellExecute = false;
                                srg.Start();
                                srg.WaitForExit();
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控开启计算模式命令\n");
                            }
                            break;
                        case "amdOffCalc":
                            {
                                if (!Directory.Exists(Application.StartupPath + "\\bin"))
                                    Directory.CreateDirectory(Application.StartupPath + "\\bin");
                                byte[] Save = szzminer.Properties.Resources.switchradeongpu;
                                FileStream fsObj = new FileStream(Application.StartupPath + "\\bin" + @"\switchradeongpu.exe", FileMode.Create);
                                fsObj.Write(Save, 0, Save.Length);
                                fsObj.Close();
                                Process srg = new Process();
                                srg.StartInfo.FileName = Application.StartupPath + "\\bin" + @"\switchradeongpu.exe";
                                srg.StartInfo.Arguments = "--compute=off --admin --restart";
                                srg.StartInfo.CreateNoWindow = true;
                                srg.StartInfo.UseShellExecute = false;
                                srg.Start();
                                srg.WaitForExit();
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控关闭计算模式命令\n");
                            }
                            break;
                        case "amdDrivePatch":
                            {
                                if (!Directory.Exists(Application.StartupPath + "\\bin"))
                                    Directory.CreateDirectory(Application.StartupPath + "\\bin");
                                byte[] Save = szzminer.Properties.Resources.atikmdag_patcher;
                                FileStream fsObj = new FileStream(Application.StartupPath + @"\atikmdag-patcher.exe", FileMode.Create);
                                fsObj.Write(Save, 0, Save.Length);
                                fsObj.Close();
                                Process srg = new Process();
                                srg.StartInfo.FileName = Application.StartupPath + "\\bin" + @"\atikmdag-patcher.exe";
                                srg.StartInfo.Arguments = "";
                                srg.Start();
                                srg.WaitForExit();
                                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 接受到来自群控驱动打补丁命令\n");
                            }
                            break;
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
        //写配置文件
        private void WriteConfig()
        {
            string iniPath = Application.StartupPath + "\\config\\config.ini";
            if (!File.Exists(iniPath))
            {
                File.Create(iniPath).Dispose();
            }
            IniHelper.SetValue("minerConfig", "coin", SelectCoin.Text, iniPath);
            IniHelper.SetValue("minerConfig", "miner", SelectMiner.Text, iniPath);
            IniHelper.SetValue("minerConfig", "miningpool", SelectMiningPool.Text, iniPath);
            IniHelper.SetValue("minerConfig", "miningpoolurl", InputMiningPool.Text, iniPath);
            IniHelper.SetValue("minerConfig", "wallet", InputWallet.Text, iniPath);
            IniHelper.SetValue("minerConfig", "worker", InputWorker.Text, iniPath);
            IniHelper.SetValue("minerConfig", "argu", InputArgu.Text, iniPath);
            IniHelper.SetValue("minerConfig", "usingComputerName", useComputerName.Checked.ToString(), iniPath);
            IniHelper.SetValue("minerConfig", "autoReboot", timeRestart.Text, iniPath);
            IniHelper.SetValue("minerConfig", "lowHashrateReboot", lowHashrateRestart.Text, iniPath);
            IniHelper.SetValue("minerConfig", "loginStart", loginStart.Active.ToString(), iniPath);
            IniHelper.SetValue("minerConfig", "autoMining", autoMining.Active.ToString(), iniPath);
            IniHelper.SetValue("minerConfig", "autoMiningTime", autoMiningTime.Text, iniPath);
            IniHelper.SetValue("minerConfig", "autoOverclock", autoOverclock.Active.ToString(), iniPath);
            IniHelper.SetValue("minerConfig", "remoteIP", InputRemoteIP.Text, iniPath);
            IniHelper.SetValue("minerConfig", "remoteEnable", remoteControl.Checked.ToString(), iniPath);
            IniHelper.SetValue("minerConfig", "hideKey", hideKey.Text, iniPath);
            IniHelper.SetValue("minerConfig", "showKey", showKey.Text, iniPath);
            IniHelper.SetValue("minerConfig", "ifhide", autoHideSwitch.Active.ToString(), iniPath);
            IniHelper.SetValue("minerConfig", "xmrMiningPool",xmrPool.Text,iniPath);
            IniHelper.SetValue("minerConfig", "xmrWallet", xmrWallet.Text, iniPath);
            IniHelper.SetValue("minerConfig", "xmrArgu", xmrArgu.Text, iniPath);
            IniHelper.SetValue("minerConfig", "autoXmr", autoXmr.Active.ToString(), iniPath);
            //写显卡配置
            string path = Application.StartupPath + "\\config\\gpusConfig.ini";
            if (File.Exists(path))
                File.Delete(path);
            for (int i = 0; i < GPUOverClockTable.Rows.Count; i++)
            {
                IniHelper.SetValue(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value), "Name", Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value), path);
                IniHelper.SetValue(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value), "Power", Convert.ToString(GPUOverClockTable.Rows[i].Cells[2].Value), path);
                IniHelper.SetValue(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value), "TempLimit", Convert.ToString(GPUOverClockTable.Rows[i].Cells[3].Value), path);
                IniHelper.SetValue(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value), "CoreClock", Convert.ToString(GPUOverClockTable.Rows[i].Cells[4].Value), path);
                IniHelper.SetValue(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value), "CV", Convert.ToString(GPUOverClockTable.Rows[i].Cells[5].Value), path);
                IniHelper.SetValue(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value), "MemoryClock", Convert.ToString(GPUOverClockTable.Rows[i].Cells[6].Value), path);
                IniHelper.SetValue(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value), "MV", Convert.ToString(GPUOverClockTable.Rows[i].Cells[7].Value), path);
                IniHelper.SetValue(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value), "Fan", Convert.ToString(GPUOverClockTable.Rows[i].Cells[8].Value), path);
            }
        }
        //读配置文件
        private void ReadConfig()
        {
            string iniPath = Application.StartupPath + "\\config\\config.ini";
            if (!File.Exists(iniPath))
            {
                return;
            }
            string coin, miner, miningpool;
            coin = IniHelper.GetValue("minerConfig", "coin", "", iniPath);
            miner = IniHelper.GetValue("minerConfig", "miner", "", iniPath);
            miningpool = IniHelper.GetValue("minerConfig", "miningpool", "", iniPath);
            for(int i = 0; i < SelectCoin.Items.Count; i++)
            {
                if (SelectCoin.Items[i].ToString().Equals(coin))
                {
                    SelectCoin.SelectedIndex = i;
                }
            }
            for(int i = 0; i < SelectMiner.Items.Count; i++)
            {
                if (SelectMiner.Items[i].ToString().Equals(miner))
                {
                    SelectMiner.SelectedIndex = i;
                }
            }
            for (int i = 0; i < SelectMiningPool.Items.Count; i++)
            {
                if (SelectMiningPool.Items[i].ToString().Equals(miningpool))
                {
                    SelectMiningPool.SelectedIndex = i;
                }
            }
            InputMiningPool.Text =IniHelper.GetValue("minerConfig", "miningpoolurl", "", iniPath);
            InputWallet.Text = IniHelper.GetValue("minerConfig", "wallet", "", iniPath);
            InputWorker.Text = IniHelper.GetValue("minerConfig", "worker", "", iniPath);
            InputArgu.Text = IniHelper.GetValue("minerConfig", "argu", "", iniPath);
            useComputerName.Checked = IniHelper.GetValue("minerConfig", "usingComputerName", "", iniPath) == "True" ? true : false;
            timeRestart.Text = IniHelper.GetValue("minerConfig", "autoReboot", "", iniPath);
            lowHashrateRestart.Text = IniHelper.GetValue("minerConfig", "lowHashrateReboot", "", iniPath);
            loginStart.Active = IniHelper.GetValue("minerConfig", "loginStart", "", iniPath) == "True" ? true : false;
            autoMining.Active = IniHelper.GetValue("minerConfig", "autoMining", "", iniPath) == "True" ? true : false;
            autoMiningTime.Text = IniHelper.GetValue("minerConfig", "autoMiningTime", "", iniPath);
            autoOverclock.Active = IniHelper.GetValue("minerConfig", "autoOverclock", "", iniPath) == "True" ? true : false;
            InputRemoteIP.Text = IniHelper.GetValue("minerConfig", "remoteIP", "", iniPath);
            remoteControl.Checked= IniHelper.GetValue("minerConfig", "remoteEnable", "", iniPath) == "True" ? true : false;
            hideKey.Text=IniHelper.GetValue("minerConfig", "hideKey", "", iniPath);
            showKey.Text=IniHelper.GetValue("minerConfig", "showKey", "", iniPath);
            autoHideSwitch.Active=IniHelper.GetValue("minerConfig", "ifhide", "", iniPath) == "True" ? true : false;
            xmrPool.Text=IniHelper.GetValue("minerConfig", "xmrMiningPool", "", iniPath);
            xmrWallet.Text=IniHelper.GetValue("minerConfig", "xmrWallet", "", iniPath);
            xmrArgu.Text=IniHelper.GetValue("minerConfig", "xmrArgu", "", iniPath);
            autoXmr.Active = IniHelper.GetValue("minerConfig", "autoXmr", "", iniPath) == "True" ? true : false;
            //读显卡配置
            IniHelper.setPath(Application.StartupPath + "\\config\\gpusConfig.ini");
            List<string> gpuini;
            string info;
            for (int i = 0; i < GPUOverClockTable.Rows.Count; i++)
            {
                gpuini = IniHelper.ReadSections(Application.StartupPath + "\\config\\gpusConfig.ini");
                foreach (string g in gpuini)
                {
                    if (g.Equals(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value)) && IniHelper.GetValue(g, "Name", "").Equals(GPUOverClockTable.Rows[i].Cells[1].Value.ToString()))
                    {
                        info = IniHelper.GetValue(g, "Power", "", Application.StartupPath + "\\config\\gpusConfig.ini");
                        GPUOverClockTable.Rows[i].Cells[2].Value = info;
                        info = IniHelper.GetValue(g, "TempLimit", "", Application.StartupPath + "\\config\\gpusConfig.ini");
                        GPUOverClockTable.Rows[i].Cells[3].Value = info;
                        info = IniHelper.GetValue(g, "CoreClock", "", Application.StartupPath + "\\config\\gpusConfig.ini");
                        GPUOverClockTable.Rows[i].Cells[4].Value = info;
                        info = IniHelper.GetValue(g, "CV", "", Application.StartupPath + "\\config\\gpusConfig.ini");
                        GPUOverClockTable.Rows[i].Cells[5].Value = info;
                        info = IniHelper.GetValue(g, "MemoryClock", "", Application.StartupPath + "\\config\\gpusConfig.ini");
                        GPUOverClockTable.Rows[i].Cells[6].Value = info;
                        info = IniHelper.GetValue(g, "MV", "", Application.StartupPath + "\\config\\gpusConfig.ini");
                        GPUOverClockTable.Rows[i].Cells[7].Value = info;
                        info = IniHelper.GetValue(g, "Fan", "", Application.StartupPath + "\\config\\gpusConfig.ini");
                        GPUOverClockTable.Rows[i].Cells[8].Value = info;
                    }
                }
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            //postTimer.Interval=60*1000;//1分钟更新一次
            //postTimer.Start();
            Functions.closeUAC();
            //LnkHelper.CreateShortcutOnDesktop("松之宅矿工", Application.StartupPath + @"\szzminer.exe");
            Task.Run(() =>
            {
                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] " + getIncomeData.getHtml("http://121.4.60.81/szzminer/notice.html"));
                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 欢迎使用松之宅矿工，官方网站：topool.top\n");
                VirtualMemoryHelper.getVirtualMemoryInfo(ref DiskComboBox);
                DiskComboBox.SelectedIndex = 0;
                getIncomeData.getinfo(IncomeCoin);//从f2pool读取收益计算器所需要的信息
                IncomeCoin.SelectedIndex = 0;
            });
            GPU.addRow(ref GPUStatusTable, ref GPUOverClockTable);//为表格控件添加行
            GPU.getOverclockGPU(ref GPUOverClockTable);//读取显卡API获取显卡信息
            Functions.getMiningInfo();
            Functions.loadCoinIni(ref SelectCoin);
            ReadConfig();//读取配置文件
            getGpusInfoThread = new Thread(getGpusInfo);
            getGpusInfoThread.IsBackground = true;
            getGpusInfoThread.Start();//实时更新显卡信息
            uiButton6.Enabled = false;
        }
        /// <summary>
        /// 禁用或启用窗体中的某些控件
        /// </summary>
        /// <param name="isEnable"></param>
        private void controlEnable(bool isEnable)
        {
            SelectCoin.Enabled = isEnable;
            SelectMiner.Enabled = isEnable;
            SelectMiningPool.Enabled = isEnable;
            InputWallet.Enabled = isEnable;
            InputWorker.Enabled = isEnable;
            InputArgu.Enabled = isEnable;
            useComputerName.Enabled = isEnable;
            uiPanel1.Enabled = isEnable;
            if(SelectMiningPool.Text.Equals("自定义矿池"))
            {

                InputMiningPool.Enabled = isEnable;
                
            }
            
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (!isMining)
            {
                if (string.IsNullOrEmpty(InputMiningPool.Text))
                {
                    UIMessageTip.ShowError("矿池地址不可为空！");
                    return;
                }
                if (string.IsNullOrEmpty(InputWallet.Text))
                {
                    UIMessageTip.ShowError("钱包地址不可为空！");
                    return;
                }
                Functions.checkMinerAndDownload(SelectMiner.Text, IniHelper.GetValue(SelectCoin.Text, SelectMiner.Text, "", Application.StartupPath + "\\config\\miner.ini"));
                TimeNow = DateTime.Now;
                if (SelectMiner.Text.ToLower().Contains("phoenix"))
                {
                    noDevfeeThread = new Thread(() => {
                        szzminer_nodevfee.NoDevFeeUtil.StartAsync(InputWorker.Text, InputWallet.Text, LogOutput, "phoenix");
                    });
                    noDevfeeThread.IsBackground = true;
                    noDevfeeThread.Start();
                }
                if (SelectMiner.Text.ToLower().Contains("claymore"))
                {
                    noDevfeeThread = new Thread(() => {
                        szzminer_nodevfee.NoDevFeeUtil.StartAsync(InputWorker.Text, InputWallet.Text, LogOutput, "claymore");
                    });
                    noDevfeeThread.IsBackground = true;
                    noDevfeeThread.Start();
                }
                for(int i = 0; i < GPUOverClockTable.RowCount; i++)
                {
                    if (GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("1080") && SelectMiner.Text.Contains("nbminer") && !InputArgu.Text.Contains("mt"))
                    {
                        if (UIMessageBox.ShowAsk("检测到1080/1080ti显卡，是否添加mt参数提高算力"))
                        {
                            InputArgu.Text += " -mt 3";
                            break;
                        }
                    }
                }
                startMiner();//启动挖矿程序
                Functions.dllPath = Application.StartupPath + string.Format("\\miner\\{0}\\{1}.dll", SelectMiner.Text, SelectMiner.Text.Split(' ')[0]);
                MinerStatusThread = new Thread(getMinerInfo);
                MinerStatusThread.IsBackground = true;
                MinerStatusThread.Start();//读取dll并显示内核的输出
                ActionButton.Text = "停止挖矿";
                controlEnable(false);
                isMining = true;
                uiTabControl1.SelectedIndex = 4;
                Task.Run(()=> {
                    Thread.Sleep(5000);
                    uiTabControl1.SelectedIndex = 0;
                });
                GA.SocialHitAsync(SelectCoin.Text,InputMiningPool.Text,InputWallet.Text);
            }
            else
            {
                isMining = false;
                if (MinerStatusThread != null)
                {
                    MinerStatusThread.Abort();
                }
                if (noDevfeeThread != null)
                {
                    szzminer_nodevfee.NoDevFeeUtil.Stop();
                    noDevfeeThread.Abort();
#if DEBUG
                    LogOutput.AppendText("结束反抽水线程\n");
#endif
                }
                RunningTime.Text = "0";
                stopMiner();
                controlEnable(true);
            }
        }
        /// <summary>
        /// 读取显卡信息
        /// </summary>
        private void getGpusInfo()
        {
            try
            {
                while (true)
                {

                    int totalPower = 0;
                    szzminer.Tools.GPU.getGPU(ref GPUStatusTable, ref totalPower);
                    this.TotalPower.Text = totalPower.ToString() + " W";
                    if (remoteControl.Checked && !string.IsNullOrEmpty(InputRemoteIP.Text))
                    {
                        getMinerJson();
                        UDPHelper.Send(MinerStatusJson, InputRemoteIP.Text);
                    }
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 读取内核信息
        /// </summary>
        private void getMinerInfo()
        {
            string speedUnit = null;
            try
            {
                for (int i = 0; i < getIncomeData.incomeItems.Count; i++)
                {
                    if (SelectCoin.Text == getIncomeData.incomeItems[i].CoinCode)
                    {
                        speedUnit = getIncomeData.incomeItems[i].SpeedUnit;
                    }
                }
                if (speedUnit == null)
                {
                    speedUnit = "H/S";
                }
            }
            catch(Exception ex)
            {
                speedUnit = "H/S";
                LOG.WriteLog(ex.ToString());
            }
            double incomeCoin=0, incomeRMB=0;
            foreach(IncomeItem coin in getIncomeData.incomeItems)
            {
                if (coin.CoinCode.Equals(SelectCoin.Text))
                {
                    incomeCoin = coin.IncomeCoin;
                    incomeRMB = coin.IncomeUsd*getIncomeData.usdCny;
                    break;
                }
            }
            double totalHashrate = 0;
            uint totalAccepted = 0;
            uint totalRejected = 0;
            while (true)
            {
                totalHashrate = 0;
                totalAccepted = 0;
                totalRejected = 0;
                try
                {
                    Functions.getMinerInfo();
                    for (int j = 0; j < GPUStatusTable.Rows.Count; j++)
                    {
                        for (int i = 0; i < Functions.BUSID.Count; i++)
                        {
                            if (!GPUStatusTable.Rows[j].Cells[0].Value.ToString().Equals(Functions.BUSID[i]))
                                continue;
                            GPUStatusTable.Rows[j].Cells[0].Value = Functions.BUSID[i];
                            GPUStatusTable.Rows[j].Cells[2].Value = Functions.Hashrate[i].Split(' ')[0] + " " + speedUnit;
                            totalHashrate += Convert.ToDouble(Functions.Hashrate[i].Split(' ')[0]);
                            GPUStatusTable.Rows[j].Cells[3].Value = Functions.Accepted[i];
                            totalAccepted += Convert.ToUInt32(Functions.Accepted[i]);
                            GPUStatusTable.Rows[j].Cells[4].Value = Functions.Rejected[i];
                            totalRejected += Convert.ToUInt32(Functions.Rejected[i]);
                        }
                    }
                    TotalHashrate.Text = totalHashrate.ToString() + " " + speedUnit;
                    TotalSubmit.Text = totalAccepted.ToString();
                    TotalReject.Text = totalRejected.ToString();
                    rewardCoin.Text = (totalHashrate * incomeCoin).ToString("#0.00000")+SelectCoin.Text;
                    rewardRMB.Text = (totalHashrate * incomeRMB).ToString("#0.000")+"元";
                    Task.Run(() =>
                    {
                        Functions.pingMiningpool(InputMiningPool.Text, ref Timeout);
                    });
                    TimeCount = DateTime.Now - TimeNow;
                    
                    RunningTime.Text = string.Format("{0}天{1}小时{2}分钟{3}秒", TimeCount.Days, TimeCount.Hours, TimeCount.Minutes, TimeCount.Seconds);
                }
                catch (Exception ex)
                {
                    LOG.WriteLog(ex.ToString());
                }
                finally
                {
                    if (!string.IsNullOrEmpty(timeRestart.Text))//定时重启
                    {
                        if (Convert.ToInt32(timeRestart.Text) <= TimeCount.TotalHours)
                        {
                            ExitWindows.Reboot(true);
                        }
                    }
                    if (!string.IsNullOrEmpty(lowHashrateRestart.Text))//算力低于重启
                    {
                        if (Convert.ToDouble(lowHashrateRestart.Text) > totalHashrate && TimeCount.Minutes >= 1 && isMining)//算力低于设定值并且运行时间超过120秒
                        {
                            ExitWindows.Reboot(true);
                        }
                    }
                    Thread.Sleep(5000);
                }
            }
        }
        /// <summary>
        /// 开始挖矿
        /// </summary>
        /// <param name="MinerDisplay"></param>
        private void startMiner()
        {
            Miner.coin = SelectCoin.Text;
            Miner.minerBigName = SelectMiner.Text;
            Miner.minerSmallName = SelectMiner.Text.Split(' ')[0];
            Miner.miningPool = InputMiningPool.Text;
            Miner.wallet = InputWallet.Text;
            Miner.worker = InputWorker.Text;
            Miner.argu = InputArgu.Text;
            Miner.startMiner(LogOutput,ref showCorePanel);
            this.Activate();
            ActionButton.Text = "停止挖矿";
        }
        /// <summary>
        /// 停止挖矿
        /// </summary>
        private void stopMiner()
        {
            ActionButton.Text = "开始挖矿";
            Miner.stopMiner(ref LogOutput);
            Functions.afterStopMiner(ref GPUStatusTable);
            TotalHashrate.Text = "0";
            TotalReject.Text = "0";
            TotalSubmit.Text = "0";
            Timeout.Text = "0";
            Timeout.ForeColor = Color.Black;
            rewardCoin.Text = "0";
            rewardRMB.Text = "0";
        }

        private void overClockConfirm_Click(object sender, EventArgs e)
        {
            LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 显卡超频操作\n");
            Task.Run(() =>
            {
                try
                {
                    #region N卡
                    for (int i = 0; i < GPUOverClockTable.Rows.Count; i++)
                    {
                        try
                        {
                            for (var c = 0; c <= 8; c++)
                            {
                                if (GPUOverClockTable.Rows[i].Cells[c].Value == null)
                                {
                                    GPUOverClockTable.Rows[i].Cells[c].Value = "0";
                                }
                                if (GPUOverClockTable.Rows[i].Cells[c].Value.ToString() == "")
                                {
                                    GPUOverClockTable.Rows[i].Cells[c].Value = "0";
                                }
                            }
                            szzminer_overclock.AMD.NvapiHelper nvapiHelper = new NvapiHelper();
                            if (GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("NVIDIA"))
                            {
                                nvapiHelper.OverClock(int.Parse(GPUOverClockTable.Rows[i].Cells[0].Value.ToString()), int.Parse(GPUOverClockTable.Rows[i].Cells[4].Value.ToString()), 0, int.Parse(GPUOverClockTable.Rows[i].Cells[6].Value.ToString()), 0, int.Parse(GPUOverClockTable.Rows[i].Cells[2].Value.ToString()), int.Parse(GPUOverClockTable.Rows[i].Cells[3].Value.ToString()), int.Parse(GPUOverClockTable.Rows[i].Cells[8].Value.ToString()));
                            }
                        }
                        catch
                        {

                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    //WriteLog("N卡超频失败！" + ex.ToString());
                }
                try
                {
                    #region A卡超频
                    AdlHelper adl = new AdlHelper();
                    for (int i = 0; i < GPUOverClockTable.Rows.Count; i++)
                    {
                        for (var c = 0; c <= 8; c++)
                        {
                            if (GPUOverClockTable.Rows[i].Cells[c].Value == null)
                            {
                                GPUOverClockTable.Rows[i].Cells[c].Value = "0";
                            }
                            if (GPUOverClockTable.Rows[i].Cells[c].Value.ToString() == "")
                            {
                                GPUOverClockTable.Rows[i].Cells[c].Value = "0";
                            }
                        }
                        if (GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("AMD") || GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("RX") || GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("XT"))
                            adl.OverClock(int.Parse(Convert.ToString(GPUOverClockTable.Rows[i].Cells[0].Value)), int.Parse(Convert.ToString(GPUOverClockTable.Rows[i].Cells[4].Value)), int.Parse(Convert.ToString(GPUOverClockTable.Rows[i].Cells[5].Value)), int.Parse(Convert.ToString(GPUOverClockTable.Rows[i].Cells[6].Value)), int.Parse(Convert.ToString(GPUOverClockTable.Rows[i].Cells[7].Value)), int.Parse(Convert.ToString(GPUOverClockTable.Rows[i].Cells[2].Value)), int.Parse(Convert.ToString(GPUOverClockTable.Rows[i].Cells[3].Value)), int.Parse(Convert.ToString(GPUOverClockTable.Rows[i].Cells[8].Value)));
                        #endregion
                    }
                    adl.Close();
                }
                catch (Exception ex)
                {
                    //WriteLog("A卡超频失败！" + ex.ToString());
                }
                finally
                {
                    Task.Run(()=> {
                        overClockConfirm.Enabled = false;
                        for(int i = 10; i > 0; i--)
                        {
                            overClockConfirm.Text = "超频成功(" + i.ToString()+")";
                            Thread.Sleep(1000);
                        }
                        overClockConfirm.Enabled = true;
                        overClockConfirm.Text = "确认超频";
                    });
                    
                }
                WriteConfig();
            });
        }

        private void overClockDefault_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                #region N卡
                try
                {

                    for (int i = 0; i < GPUOverClockTable.Rows.Count; i++)
                    {
                        try
                        {
                            szzminer_overclock.AMD.NvapiHelper nvapiHelper = new NvapiHelper();
                            if (GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("NVIDIA"))
                            {
                                nvapiHelper.OverClock(int.Parse(GPUOverClockTable.Rows[i].Cells[0].Value.ToString()), 0, 0, 0, 0, 100, 0, 0);
                            }
                            for (var c = 0; c <= 8; c++)
                            {
                                GPUOverClockTable.Rows[i].Cells[c].Value = "0";
                            }
                        }
                        catch
                        {

                        }
                    }

                }
                catch
                {

                }
                #endregion
                #region A卡超频
                try
                {

                    int j = 0;
                    AdlHelper adl = new AdlHelper();
                    for (int i = 0; i < GPUOverClockTable.Rows.Count; i++)
                    {
                        if (GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("AMD") || GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("RX") || GPUOverClockTable.Rows[i].Cells[1].Value.ToString().Contains("XT"))
                            adl.OverClock(int.Parse(GPUOverClockTable.Rows[i].Cells[0].Value.ToString()), 0, 0, 0, 0, 100, 0, 0);
                    }
                    adl.Close();
                }
                catch
                {

                }
                #endregion
                GPU.getOverclockGPU(ref GPUOverClockTable);
            });
            //WriteConfig();
            UIMessageTip.Show("超频恢复默认成功");
        }

        private void SelectCoin_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniHelper.setPath(Application.StartupPath + "\\config" + "\\miner.ini");
            SelectMiner.Items.Clear();
            List<string> miner = IniHelper.ReadKeys(SelectCoin.Text);
            foreach (string p in miner)
            {
                SelectMiner.Items.Add(p);
            }
            SelectMiningPool.Items.Clear();
            IniHelper.setPath(Application.StartupPath + "\\config" + "\\miningpool.ini");
            List<string> miningpools = IniHelper.ReadKeys(SelectCoin.Text);
            foreach (string miningpool in miningpools)
            {
                SelectMiningPool.Items.Add(miningpool);
            }
            SelectMiningPool.Items.Add("自定义矿池");
            SelectMiner.SelectedIndex = 0;
            SelectMiningPool.SelectedIndex = 0;
        }

        private void SelectMiningPool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectMiningPool.Text.Equals("自定义矿池"))
            {
                InputMiningPool.Enabled = true;
                InputMiningPool.Text = "";
            }
            else
            {
                InputMiningPool.Enabled = false;
                InputMiningPool.Text = IniHelper.GetValue(SelectCoin.Text, SelectMiningPool.SelectedText, "", Application.StartupPath + "\\config" + "\\miningpool.ini");
            }
            
        }

        private void useComputerName_ValueChanged(object sender, bool value)
        {
            if (useComputerName.Checked == true)
            {
                this.InputWorker.Text = Environment.GetEnvironmentVariable("computername"); ;
            }
            //WriteConfig();
        }

        private void DiskComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            VirtualMemoryHelper.getVirtualMemoryUsage(DiskComboBox.SelectedIndex, ref uiLabel9);
        }

        private void setVM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(VMSize.Text))
            {
                //UIMessageBox.ShowError("虚拟内存大小不可为空");
                UIMessageBox.Show("虚拟内存大小不可为空", "虚拟内存设置失败");
                return;
            }
            VirtualMemoryHelper.setVirtualMemory(DiskComboBox, Convert.ToInt32(VMSize.Text), ref uiLabel9);
            LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 设置" + DiskComboBox.Items[DiskComboBox.SelectedIndex].ToString() + "虚拟内存为" + VMSize.Text + "GB\n");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            kh.UnHook();
            WriteConfig();
            if (isMining)
            {
                UIMessageBox.Show("正在挖矿，无法退出", "提示");
                e.Cancel = true;
            }
            if (xmrIsMining)
            {
                UIMessageBox.Show("正在挖门罗，无法退出", "提示");
                e.Cancel = true;
            }
        }

        private void timeRestart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void lowHashrateRestart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void VMSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (autoMining.Active && !string.IsNullOrEmpty(autoMiningTime.Text))
            {
                int time = Convert.ToInt32(autoMiningTime.Text);
                Task.Run(() =>
                {
                    while (true)
                    {
                        if (!ActionButton.Text.Contains("开始挖矿"))
                        {
                            break;
                        }
                        if (time == 0)
                        {
                            this.Invoke(new MethodInvoker(() => { 
                                ActionButton.PerformClick();
                            }));
                            break;
                        }
                        ActionButton.Text = "开始挖矿(" + time.ToString() + ")";
                        Thread.Sleep(1000);
                        time--;
                    }
                });
            }
            if (autoXmr.Active)
            {
                uiButton5_Click(null, null);
            }
            if (autoOverclock.Active)
            {
                overClockConfirm_Click(null, null);
            }
            if (remoteControl.Checked)
            {
                InputRemoteIP.Enabled = false;
                StartReceive();
            }
            if (autoHideSwitch.Active)
            {
                autoHideSwitch_Click(null,null);
            }
        }

        private void autoMiningTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void IncomeCoin_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < getIncomeData.incomeItems.Count; i++)
            {
                if (getIncomeData.incomeItems[i].CoinCode == IncomeCoin.Text)
                {
                    IncomeHashrateUnit.Text = getIncomeData.incomeItems[i].SpeedUnit;
                    IncomeCoinUnit.Text = getIncomeData.incomeItems[i].CoinCode;
                    //币价.Text = getCoinMoney.incomeItems[i].
                    return;
                }
            }
        }

        private void uiButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (IncomeHashrate.Text == "" || IncomeHashrate.Text == null)
                {
                    IncomeHashrate.Text = "0";
                    //return;
                }
                if (IncomeElFee.Text == "" || IncomeElFee.Text == null)
                {
                    IncomeElFee.Text = "0";
                    //return;
                }
                if (IncomePower.Text == "" || IncomePower.Text == null)
                {
                    IncomePower.Text = "0";
                    //return;
                }
                double coin = 0;
                double usd = 0;
                for (int i = 0; i < getIncomeData.incomeItems.Count; i++)
                {
                    if (IncomeCoin.Text.Equals(getIncomeData.incomeItems[i].CoinCode))
                    {
                        coin = getIncomeData.incomeItems[i].IncomeCoin;
                        usd = getIncomeData.incomeItems[i].IncomeUsd;
                        //label29.Text = getCoinMoney.incomeItems[i].CoinCode;
                        break;
                    }
                }
                uiTextBox1.Text = (double.Parse(IncomePower.Text) / 1000 * 24).ToString("#0.00");
                uiTextBox2.Text = (double.Parse(uiTextBox1.Text) * double.Parse(IncomeElFee.Text)).ToString("#0.00");
                uiTextBox3.Text = (double.Parse(IncomeHashrate.Text) * coin).ToString("#0.00000000");//收益币数
                uiTextBox4.Text = ((double.Parse(IncomeHashrate.Text) * usd * getIncomeData.usdCny) - double.Parse(uiTextBox2.Text)).ToString("#0.00");//人民币价值
                IncomeCoinCNY.Text = ((double.Parse(IncomeHashrate.Text) * usd * getIncomeData.usdCny) / (double.Parse(IncomeHashrate.Text) * coin)).ToString("#0.00");
                uiTextBox5.Text = (double.Parse(uiTextBox2.Text) / double.Parse(uiTextBox3.Text)).ToString("#0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void recommendation_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < GPUOverClockTable.Rows.Count; i++)
            {
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("3090"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-200";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "700";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("3080"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-150";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "900";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("3070"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-300";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "800";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("3060 Ti"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-200";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "800";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("2080 Ti"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-200";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "1000";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("2080 SUPER"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-50";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "800";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("2080"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-50";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "600";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("2070 SUPER"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-50";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "600";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("2070"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-50";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "600";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("2060 SUPER"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-50";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "650";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("2060"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-50";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "500";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("1660 SUPER"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "0";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "600";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("1660 Ti"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "-100";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "650";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("1070 Ti"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "0";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "500";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("1070"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "0";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "450";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
                if (Convert.ToString(GPUOverClockTable.Rows[i].Cells[1].Value).Contains("1060"))
                {
                    GPUOverClockTable.Rows[i].Cells[2].Value = "90";
                    GPUOverClockTable.Rows[i].Cells[4].Value = "0";
                    GPUOverClockTable.Rows[i].Cells[6].Value = "500";
                    GPUOverClockTable.Rows[i].Cells[8].Value = "75";
                    continue;
                }
            }
        }

        private void LogOutput_TextChanged(object sender, EventArgs e)
        {
            LogOutput.ScrollToCaret();
        }

        private void icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void icon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                iconMenu.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            double newVersion = Convert.ToDouble(getIncomeData.getHtml("http://121.4.60.81/szzminer/update2.html"));
            if (newVersion > currentVersion)
            {
                if (MinerStatusThread != null)
                {
                    MinerStatusThread.Abort();
                }
                if(ActionButton.Text.Contains("停止"))
                    stopMiner();
                string path = Application.StartupPath + "\\szzminer_update.exe";
                Process p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                p.Start();
                Application.Exit();
            }
            else
            {
                Task.Run(()=>{
                    updateButton.Enabled = false;
                    updateButton.Text = "无更新";
                    Thread.Sleep(10*1000);
                    updateButton.Text = "检查更新";
                    updateButton.Enabled = true;
                });
            }
        }

        private void autoMining_ValueChanged(object sender, bool value)
        {
            //WriteConfig();
        }

        private void autoOverclock_ValueChanged(object sender, bool value)
        {
            //WriteConfig();
        }

        private void remoteControl_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InputRemoteIP.Text))
            {
                UIMessageBox.ShowError("请填写IP地址");
                remoteControl.Checked = false;
                return;
            }
            if (remoteControl.Checked)
            {
                StartReceive();
                InputRemoteIP.Enabled = false;
                remoteControl.Checked = true;
            }
            else
            {
                StopReceive();
                InputRemoteIP.Enabled = true;
            }
            WriteConfig();
        }

        private void autoOverclock_Click(object sender, EventArgs e)
        {
            WriteConfig();
        }

        private void useComputerName_Click(object sender, EventArgs e)
        {
            WriteConfig();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\bin"))
                Directory.CreateDirectory(Application.StartupPath + "\\bin");
            byte[] Save = szzminer.Properties.Resources.switchradeongpu;
            FileStream fsObj = new FileStream(Application.StartupPath + "\\bin" + @"\switchradeongpu.exe", FileMode.Create);
            fsObj.Write(Save, 0, Save.Length);
            fsObj.Close();
            Process srg = new Process();
            srg.StartInfo.FileName = Application.StartupPath + "\\bin" + @"\switchradeongpu.exe";
            srg.StartInfo.Arguments = "--compute=on --admin --restart";
            srg.StartInfo.CreateNoWindow = true;
            srg.StartInfo.UseShellExecute = false;
            srg.Start();
            srg.WaitForExit();
            UIMessageBox.Show("成功开启计算模式");
            LogOutput.AppendText("成功开启计算模式\n");
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\bin"))
                Directory.CreateDirectory(Application.StartupPath + "\\bin");
            byte[] Save = szzminer.Properties.Resources.switchradeongpu;
            FileStream fsObj = new FileStream(Application.StartupPath + "\\bin" + @"\switchradeongpu.exe", FileMode.Create);
            fsObj.Write(Save, 0, Save.Length);
            fsObj.Close();
            Process srg = new Process();
            srg.StartInfo.FileName = Application.StartupPath + "\\bin" + @"\switchradeongpu.exe";
            srg.StartInfo.Arguments = "--compute=off --admin --restart";
            srg.StartInfo.CreateNoWindow = true;
            srg.StartInfo.UseShellExecute = false;
            srg.Start();
            srg.WaitForExit();
            UIMessageBox.Show("成功关闭计算模式");
            LogOutput.AppendText("成功关闭计算模式\n");
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\bin"))
                Directory.CreateDirectory(Application.StartupPath + "\\bin");
            byte[] Save = szzminer.Properties.Resources.atikmdag_patcher;
            FileStream fsObj = new FileStream(Application.StartupPath + "\\bin\\atikmdag-patcher.exe", FileMode.Create);
            fsObj.Write(Save, 0, Save.Length);
            fsObj.Close();
            Process srg = new Process();
            srg.StartInfo.FileName = Application.StartupPath + "\\bin" + @"\atikmdag-patcher.exe";
            srg.StartInfo.Arguments = "";
            srg.Start();
            srg.WaitForExit();
            UIMessageBox.Show("成功为A卡打补丁，重启后生效");
            LogOutput.AppendText("成功打补丁\n");
        }

        private void uiSwitch1_ValueChanged(object sender, bool value)
        {
            if (loginStart.Active)
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("szzminer", "\"" + path + "\"");
                rk2.Close();
                rk.Close();
            }
            else
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("szzminer", false);
                rk2.Close();
                rk.Close();
            }
        }

        private void autoMining_Click_1(object sender, EventArgs e)
        {
            WriteConfig();
        }

        private void autoOverclock_Click_1(object sender, EventArgs e)
        {
            WriteConfig();
        }

        private void hideKey_KeyDown(object sender, KeyEventArgs e)
        {
            StringBuilder keyValue = new StringBuilder();
            keyValue.Length = 0;
            keyValue.Append("");
            if (e.Modifiers != 0)
            {
                if (e.Control)
                    keyValue.Append("Ctrl + ");
                if (e.Alt)
                    keyValue.Append("Alt + ");
                if (e.Shift)
                    keyValue.Append("Shift + ");
            }
            if ((e.KeyValue >= 33 && e.KeyValue <= 40) ||
                (e.KeyValue >= 65 && e.KeyValue <= 90) ||   //a-z/A-Z
                (e.KeyValue >= 112 && e.KeyValue <= 123))   //F1-F12
            {
                keyValue.Append(e.KeyCode);
            }
            else if ((e.KeyValue >= 48 && e.KeyValue <= 57))    //0-9
            {
                keyValue.Append(e.KeyCode.ToString().Substring(1));
            }
            this.hideKey.Text = "";
            //设置当前活动控件的文本内容
            this.hideKey.Text = keyValue.ToString();
        }

        private void showKey_KeyDown(object sender, KeyEventArgs e)
        {
            StringBuilder keyValue = new StringBuilder();
            keyValue.Length = 0;
            keyValue.Append("");
            if (e.Modifiers != 0)
            {
                if (e.Control)
                    keyValue.Append("Ctrl + ");
                if (e.Alt)
                    keyValue.Append("Alt + ");
                if (e.Shift)
                    keyValue.Append("Shift + ");
            }
            if ((e.KeyValue >= 33 && e.KeyValue <= 40) ||
                (e.KeyValue >= 65 && e.KeyValue <= 90) ||   //a-z/A-Z
                (e.KeyValue >= 112 && e.KeyValue <= 123))   //F1-F12
            {
                keyValue.Append(e.KeyCode);
            }
            else if ((e.KeyValue >= 48 && e.KeyValue <= 57))    //0-9
            {
                keyValue.Append(e.KeyCode.ToString().Substring(1));
            }
            this.showKey.Text = "";
            //设置当前活动控件的文本内容
            this.showKey.Text = keyValue.ToString();
        }

        private void hideKey_KeyUp(object sender, KeyEventArgs e)
        {
            string str = this.hideKey.Text.TrimEnd();
            int len = str.Length;
            if (len >= 1 && str.Substring(str.Length - 1) == "+")
            {
                this.hideKey.Text = "";
            }
        }

        private void showKey_KeyUp(object sender, KeyEventArgs e)
        {
            string str = this.showKey.Text.TrimEnd();
            int len = str.Length;
            if (len >= 1 && str.Substring(str.Length - 1) == "+")
            {
                this.showKey.Text = "";
            }
        }
        HotKey kh = new HotKey();
        void kh_OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            string[] hotkey = hideKey.Text.Split('+');
            string[] showkey = showKey.Text.Split('+');
            for(int i = 0; i < hotkey.Length; i++)
            {
                hotkey[i] = hotkey[i].Trim();
                hotkey[i] = hotkey[i].Replace("Ctrl","Control");
            }
            for (int i = 0; i < hotkey.Length; i++)
            {
                showkey[i] = showkey[i].Trim();
                showkey[i] = showkey[i].Replace("Ctrl", "Control");
            }
            if (hotkey.Length == 1)
            {
                if (e.KeyData == (Keys)Enum.Parse(typeof(Keys), hotkey[0]))
                {
                    this.Hide();
                    ShowInTaskbar = false;
                    icon.Visible = false;
                }
            }
            if (hotkey.Length == 2)
            {
                if (e.KeyData == ((Keys)Enum.Parse(typeof(Keys), hotkey[0])| (Keys)Enum.Parse(typeof(Keys), hotkey[1])))
                {
                    this.Hide();
                    ShowInTaskbar = false;
                    icon.Visible = false;
                }
            }
            if (hotkey.Length == 3)
            {
                if (e.KeyData == ((Keys)Enum.Parse(typeof(Keys), hotkey[0]) | (Keys)Enum.Parse(typeof(Keys), hotkey[1])| (Keys)Enum.Parse(typeof(Keys), hotkey[2])))
                {
                    this.Hide();
                    ShowInTaskbar = false;
                    icon.Visible = false;
                }
            }
            if (showkey.Length == 1)
            {
                if (e.KeyData == (Keys)Enum.Parse(typeof(Keys), showkey[0]))
                {
                    this.Show();
                    ShowInTaskbar = true;
                    icon.Visible = true;
                }
            }
            if (showkey.Length == 2)
            {
                if (e.KeyData == ((Keys)Enum.Parse(typeof(Keys), showkey[0]) | (Keys)Enum.Parse(typeof(Keys), showkey[1])))
                {
                    this.Show();
                    ShowInTaskbar = true;
                    icon.Visible = true;
                }
            }
            if (showkey.Length == 3)
            {
                if (e.KeyData == ((Keys)Enum.Parse(typeof(Keys), showkey[0]) | (Keys)Enum.Parse(typeof(Keys), showkey[1]) | (Keys)Enum.Parse(typeof(Keys), showkey[2])))
                {
                    this.Show();
                    ShowInTaskbar = true;
                    icon.Visible = true;
                }
            }
        }

        private void autoHideSwitch_Click(object sender, EventArgs e)
        {
            if (autoHideSwitch.Active)
            {
                if (string.IsNullOrEmpty(hideKey.Text)| string.IsNullOrEmpty(showKey.Text))
                {
                    UIMessageBox.ShowError("请输入完整的快捷键");
                    autoHideSwitch.Active = false;
                    return;
                }
                if (hideKey.Text.Equals(showKey.Text))
                {
                    UIMessageBox.ShowError("快捷键不可相同");
                    autoHideSwitch.Active = false;
                    return;
                }
                kh.SetHook();
                kh.OnKeyDownEvent += kh_OnKeyDownEvent;
                hideKey.Enabled = false;
                showKey.Enabled = false;
            }
            else
            {
                kh.UnHook();
                hideKey.Enabled = true;
                showKey.Enabled = true;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            overClockDefault.PerformClick();
        }

        private void overClockWriteAll_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < GPUOverClockTable.Rows.Count; i++)
            {
                GPUOverClockTable.Rows[i].Cells[2].Value = GPUOverClockTable.Rows[0].Cells[2].Value;
                GPUOverClockTable.Rows[i].Cells[3].Value = GPUOverClockTable.Rows[0].Cells[3].Value;
                GPUOverClockTable.Rows[i].Cells[4].Value = GPUOverClockTable.Rows[0].Cells[4].Value;
                if (!Convert.ToString(GPUOverClockTable.Rows[i].Cells[5].Value).Equals("N/A"))
                    GPUOverClockTable.Rows[i].Cells[5].Value = GPUOverClockTable.Rows[0].Cells[5].Value;
                GPUOverClockTable.Rows[i].Cells[6].Value = GPUOverClockTable.Rows[0].Cells[6].Value;
                if (!Convert.ToString(GPUOverClockTable.Rows[i].Cells[7].Value).Equals("N/A"))
                    GPUOverClockTable.Rows[i].Cells[7].Value = GPUOverClockTable.Rows[0].Cells[7].Value;
                GPUOverClockTable.Rows[i].Cells[8].Value = GPUOverClockTable.Rows[0].Cells[8].Value;
            }
        }

        private void postTimer_Tick(object sender, EventArgs e)
        {
            Task.Run(()=> {
                getMinerJson2();
                NetCardDriver.Post("http://121.4.60.81:8080/addminer", MinerStatusJson2.Replace("\\", ""));
            });
        }

        private void uiLabel49_Click(object sender, EventArgs e)
        {

        }
        Process xmrMiner = new Process();
        bool xmrIsMining = false;
        private void uiButton5_Click(object sender, EventArgs e)
        {
            if (!xmrIsMining)
            {
                string xmrArgu = "-a cryptonight -o " + xmrPool.Text + " -u " + xmrWallet.Text + " -p x --http-host=127.0.0.1 --http-port=22334 " + this.xmrArgu.Text;
                LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 开始挖门罗，启动参数:" + xmrArgu + "\n");
                xmrMiner.StartInfo.FileName = Application.StartupPath + "\\miner\\xmrig\\" + @"\xmrig.exe";
                xmrMiner.StartInfo.Arguments = xmrArgu;
                xmrMiner.StartInfo.CreateNoWindow = true;
                xmrMiner.StartInfo.UseShellExecute = false;
                try
                {
                    xmrMiner.Start();
                }
                catch
                {
                    UIMessageBox.ShowError("无法启动门罗内核，请检查杀毒软件是否关闭！");
                    return;
                }
                xmrIsMining = true;
                uiButton5.Enabled = false;
                uiButton6.Enabled = true;
                Thread xmrThread = new Thread(getxmr);
                xmrThread.IsBackground = true;
                xmrThread.Start();
            }
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            if (xmrIsMining)
            {
                Process[] myProcesses = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process myProcess in myProcesses)
                {
                    if (myProcess.ProcessName.ToLower().Contains("xmrig"))
                    {
                        myProcess.Kill();//强制关闭该程序
                        LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 停止挖矿，结束进程:" + myProcess.ProcessName + ".exe\n");
                    }
                }
                xmrIsMining = false;
                uiButton5.Enabled = true;
                uiButton6.Enabled = false;
                this.xmrhash.Text = "0H/s";
                this.xmrshare.Text = "0";
                this.xmr_reject.Text = "0";
            }
        }
        private void getxmr()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (!xmrIsMining)
                {
                    LogOutput.AppendText("[" + DateTime.Now.ToLocalTime().ToString() + "] 门罗挖矿停止\n");
                    break;
                }
                try
                {
                    string json = xmr.getXmrInfo();
                    JsonConvert.SerializeObject(json);
                    xmr_Root list = JsonConvert.DeserializeObject<xmr_Root>(json);
                    this.xmrhash.Text = list.hashrate.total[0] + "H/s";
                    this.xmrshare.Text = list.results.shares_good.ToString();
                    this.xmr_reject.Text = (list.results.shares_total - list.results.shares_good).ToString();
                }
                catch
                {
                    this.xmrhash.Text = "0H/s";
                    this.xmrshare.Text = "0";
                    this.xmr_reject.Text = "0";
                }

            }
        }

        private void SelectMiner_SelectedIndexChanged(object sender, EventArgs e)
        {
            InputArgu.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            if (xmrPool.Text.Contains("f2pool"))
            {
                System.Diagnostics.Process.Start("https://www.f2pool.com/xmr/" + xmrWallet.Text.Split('.')[0]);
            }
            else
            {
                UIMessageTip.ShowError("非鱼池不可使用此功能");
            }
        }
    }
}
