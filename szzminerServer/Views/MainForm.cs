using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using szzminerServer.Class;
using szzminerServer.Tools;
using szzminerServer.Views;

namespace szzminerServer
{
    public partial class MainForm : UIForm
    {
        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        Thread autoFlushThread;
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
                    localIpep = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 22429); // 本机IP和监听端口号
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
                        case "minerStatus":
                            MinerStatusLoad.remoteMinerStatus(JsonConvert.DeserializeObject<RemoteMinerStatus>(reData));
                            break;
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
        
        private void Flesh()
        {
            int totalPowerC = 0;
            for (int j = 0; j < MinerStatusLoad.remoteMinerStatusList.Count; j++)
            {
                int i;
                for (i = 0; i < MinerStatusTable.Rows.Count; i++)
                {
                    if (MinerStatusLoad.remoteMinerStatusList[j].MAC == Convert.ToString(MinerStatusTable.Rows[i].Cells[13].Value))
                    {
                        MinerStatusTable.Rows[i].Cells[0].Value = MinerStatusLoad.remoteMinerStatusList[j].Worker;
                        MinerStatusTable.Rows[i].Cells[1].Value = MinerStatusLoad.remoteMinerStatusList[j].if_mining == true ? "挖矿中" : "已停止";
                        MinerStatusTable.Rows[i].Cells[3].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        MinerStatusTable.Rows[i].Cells[4].Value = MinerStatusLoad.remoteMinerStatusList[j].Coin;
                        MinerStatusTable.Rows[i].Cells[5].Value = MinerStatusLoad.remoteMinerStatusList[j].MinerCore;
                        MinerStatusTable.Rows[i].Cells[6].Value = MinerStatusLoad.remoteMinerStatusList[j].Pool;
                        MinerStatusTable.Rows[i].Cells[7].Value = MinerStatusLoad.remoteMinerStatusList[j].Wallet;
                        MinerStatusTable.Rows[i].Cells[8].Value = MinerStatusLoad.remoteMinerStatusList[j].Accepted;
                        MinerStatusTable.Rows[i].Cells[9].Value = MinerStatusLoad.remoteMinerStatusList[j].Rejected;
                        MinerStatusTable.Rows[i].Cells[10].Value = MinerStatusLoad.remoteMinerStatusList[j].Power;
                        totalPowerC += Convert.ToInt32(MinerStatusTable.Rows[i].Cells[10].Value);
                        MinerStatusTable.Rows[i].Cells[11].Value = MinerStatusLoad.remoteMinerStatusList[j].Hashrate;
                        MinerStatusTable.Rows[i].Cells[12].Value = MinerStatusLoad.remoteMinerStatusList[j].IP;
                        MinerStatusTable.Rows[i].Cells[13].Value = MinerStatusLoad.remoteMinerStatusList[j].MAC;
                        MinerStatusTable.Rows[i].Cells[14].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrPool;
                        MinerStatusTable.Rows[i].Cells[15].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrWallet;
                        MinerStatusTable.Rows[i].Cells[16].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrHashrate;
                        MinerStatusTable.Rows[i].Cells[17].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrAccept;
                        MinerStatusTable.Rows[i].Cells[18].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrReject;
                        break;
                    }
                }
                if (i == MinerStatusTable.Rows.Count)
                {
                    MinerStatusTable.AddRow();
                    MinerStatusTable.Rows[i].Cells[0].Value = MinerStatusLoad.remoteMinerStatusList[j].Worker;
                    MinerStatusTable.Rows[i].Cells[1].Value = MinerStatusLoad.remoteMinerStatusList[j].if_mining == true ? "挖矿中" : "已停止";
                    MinerStatusTable.Rows[i].Cells[3].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    MinerStatusTable.Rows[i].Cells[4].Value = MinerStatusLoad.remoteMinerStatusList[j].Coin;
                    MinerStatusTable.Rows[i].Cells[5].Value = MinerStatusLoad.remoteMinerStatusList[j].MinerCore;
                    MinerStatusTable.Rows[i].Cells[6].Value = MinerStatusLoad.remoteMinerStatusList[j].Pool;
                    MinerStatusTable.Rows[i].Cells[7].Value = MinerStatusLoad.remoteMinerStatusList[j].Wallet;
                    MinerStatusTable.Rows[i].Cells[8].Value = MinerStatusLoad.remoteMinerStatusList[j].Accepted;
                    MinerStatusTable.Rows[i].Cells[9].Value = MinerStatusLoad.remoteMinerStatusList[j].Rejected;
                    MinerStatusTable.Rows[i].Cells[10].Value = MinerStatusLoad.remoteMinerStatusList[j].Power;
                    totalPowerC += Convert.ToInt32(MinerStatusTable.Rows[i].Cells[10].Value);
                    MinerStatusTable.Rows[i].Cells[11].Value = MinerStatusLoad.remoteMinerStatusList[j].Hashrate;
                    MinerStatusTable.Rows[i].Cells[12].Value = MinerStatusLoad.remoteMinerStatusList[j].IP;
                    MinerStatusTable.Rows[i].Cells[13].Value = MinerStatusLoad.remoteMinerStatusList[j].MAC;
                    MinerStatusTable.Rows[i].Cells[14].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrPool;
                    MinerStatusTable.Rows[i].Cells[15].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrWallet;
                    MinerStatusTable.Rows[i].Cells[16].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrHashrate;
                    MinerStatusTable.Rows[i].Cells[17].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrAccept;
                    MinerStatusTable.Rows[i].Cells[18].Value = MinerStatusLoad.remoteMinerStatusList[j].xmrReject;
                }
            }
            this.totalPower.Text = totalPowerC.ToString() + " W";
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            Flesh();
        }

        private void autoFlush()
        {
            while (true)
            {
                Thread.Sleep(30 * 1000);
                Flesh();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StartReceive();
        }

        private void MinerStatusTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.GPUStatusTable.Rows.Clear();
                string ip = MinerStatusTable.Rows[e.RowIndex].Cells[12].Value.ToString();
                for (int i = 0; i < MinerStatusLoad.remoteMinerStatusList.Count; i++)
                {
                    if (MinerStatusLoad.remoteMinerStatusList[i].IP == ip)
                    {
                        if (MinerStatusLoad.remoteMinerStatusList[i].Devices == null)
                        {
                            break;
                        }
                        else
                        {
                            for (int j = 0; j < MinerStatusLoad.remoteMinerStatusList[i].Devices.Count; j++)
                            {
                                GPUStatusTable.Rows.Add();
                                GPUStatusTable.Rows[j].Cells[0].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].idbus;
                                GPUStatusTable.Rows[j].Cells[1].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].name;
                                GPUStatusTable.Rows[j].Cells[2].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].Hashrate;
                                GPUStatusTable.Rows[j].Cells[3].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].accept;
                                GPUStatusTable.Rows[j].Cells[4].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].reject;
                                GPUStatusTable.Rows[j].Cells[5].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].power;
                                GPUStatusTable.Rows[j].Cells[6].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].temp;
                                GPUStatusTable.Rows[j].Cells[7].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].fan;
                                GPUStatusTable.Rows[j].Cells[8].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].coreclock;
                                GPUStatusTable.Rows[j].Cells[9].Value = MinerStatusLoad.remoteMinerStatusList[i].Devices[j].memoryclock;
                            }
                        }

                    }
                }
            }
        }

        private void SelectAll_ValueChanged(object sender, bool value)
        {
            if (SelectAll.Checked)
            {
                for (var i = 0; i < MinerStatusTable.Rows.Count; i++)
                {
                    MinerStatusTable.Rows[i].Cells[2].Value = true;
                }
            }
            else
            {
                for (var i = 0; i < MinerStatusTable.Rows.Count; i++)
                {
                    MinerStatusTable.Rows[i].Cells[2].Value = false;
                }
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (!MinerOptions.startMiner(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
            }
            else
            {
                UIMessageBox.Show("设置完成");
            }
        }

        private void MinerStatusTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex != -1)
            {
                if ((bool)MinerStatusTable.Rows[e.RowIndex].Cells[2].EditedFormattedValue == true)
                {
                    MinerStatusTable.Rows[e.RowIndex].Cells[2].Value = false;
                }
                else
                {
                    MinerStatusTable.Rows[e.RowIndex].Cells[2].Value = true;
                }
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (!MinerOptions.stopMiner(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
            }
            else
            {
                UIMessageBox.Show("设置完成");
            }
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            changeCoin changeCoin = new changeCoin(MinerStatusTable);
            changeCoin.ShowDialog();
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            if (UIMessageBox.ShowAsk("你真的要关机吗"))
            {
                if (!MinerOptions.shutdownMiner(MinerStatusTable))
                {
                    UIMessageBox.ShowError("请选择矿机");
                }
                else
                {
                    UIMessageBox.Show("关机完成");
                }
            }
        }

        private void uiCheckBox1_ValueChanged(object sender, bool value)
        {
            if (uiCheckBox1.Checked)
            {
                autoFlushThread = new Thread(autoFlush);
                autoFlushThread.IsBackground = true;
                autoFlushThread.Start();
                this.flush.Enabled = false;
            }
            else
            {
                if (autoFlushThread != null)
                    autoFlushThread.Abort();
                this.flush.Enabled = true;
            }
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            if (UIMessageBox.ShowAsk("你真的要重启吗"))
            {
                if (!MinerOptions.rebootMiner(MinerStatusTable))
                {
                    UIMessageBox.ShowError("请选择矿机");
                }
                else
                {
                    UIMessageBox.Show("重启完成");
                }
            }
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            if (!MinerOptions.updateMiner(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
            }
            else
            {
                UIMessageBox.Show("更新完成");
            }
        }

        private void uiButton2_Click_1(object sender, EventArgs e)
        {
            List<RemoteMinerStatus> selectedMiner=new List<RemoteMinerStatus>();
            for (int i = 0; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    for(int j=0;j<MinerStatusLoad.remoteMinerStatusList.Count;j++)
                    {
                        if(MinerStatusLoad.remoteMinerStatusList[i].IP==MinerStatusTable.Rows[i].Cells[12].Value.ToString())
                        {
                            selectedMiner.Add(MinerStatusLoad.remoteMinerStatusList[i]);
                        }
                    }
                }
            }
            if (selectedMiner.Count <= 0)
            {
                UIMessageBox.ShowError("请选择矿机");
                return;
            }
            overClockForm overClockForm = new overClockForm(selectedMiner);
            overClockForm.ShowDialog();
        }
        private static bool checkTableSelected(UIDataGridView MinerStatusTable)
        {
            int j = 0;
            for (; j < MinerStatusTable.Rows.Count; j++)
            {
                if (MinerStatusTable.Rows[j].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[j].Cells[2].Value.ToString() == "True")
                {
                    return true;
                }
            }
            return false;
        }
        private void uiButton8_Click(object sender, EventArgs e)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
                return;
            }
            RemoteReboot remoteReboot = new RemoteReboot();
            remoteReboot.function = "setreboot";
            remoteReboot.hourReboot = timeRestart.Text;
            remoteReboot.hashrateReboot = lowHashrateRestart.Text;
            string msg = JsonConvert.SerializeObject(remoteReboot);
            
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send(msg, MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    UIMessageBox.Show("设置完成", "提示");
                    return;
                }
            }
        }

        private void uiButton9_Click(object sender, EventArgs e)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
                return;
            }
            RemoteOtherOptions remoteOtherOptions = new RemoteOtherOptions();
            remoteOtherOptions.function = "otherOption";
            remoteOtherOptions.autoLogin = loginStart.Checked;
            remoteOtherOptions.autoMining = autoMining.Checked;
            remoteOtherOptions.autoMiningTime = autoMiningTime.Text;
            remoteOtherOptions.autoOv = autoOverclock.Checked;
            string msg = JsonConvert.SerializeObject(remoteOtherOptions);

            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send(msg, MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    UIMessageBox.Show("设置完成","提示");
                    return;
                }
            }
        }

        private void uiButton12_Click(object sender, EventArgs e)
        {
            if (!MinerOptions.amdOnCalc(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
            }
            else
            {
                UIMessageBox.Show("设置完成");
            }
        }

        private void uiButton10_Click(object sender, EventArgs e)
        {
            if (!MinerOptions.amdDriverPatch(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
            }
            else
            {
                UIMessageBox.Show("设置完成");
            }
        }

        private void uiButton11_Click(object sender, EventArgs e)
        {
            if (!MinerOptions.amdOffCalc(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
            }
            else
            {
                UIMessageBox.Show("设置完成");
            }
        }

        private void uiButton13_Click(object sender, EventArgs e)
        {
            if (!MinerOptions.startXmr(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
            }
            else
            {
                UIMessageBox.Show("设置完成");
            }
        }

        private void uiButton14_Click(object sender, EventArgs e)
        {
            if (!MinerOptions.stopXmr(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
            }
            else
            {
                UIMessageBox.Show("设置完成");
            }
        }

        private void uiButton15_Click(object sender, EventArgs e)
        {
            changeXmr changeXmr = new changeXmr();
            changeXmr.pool = xmrPool.Text;
            changeXmr.wallet = xmrWallet.Text;
            changeXmr.function = "changeXmr";
            changeXmr.argu = xmrArgu.Text;
            string msg = JsonConvert.SerializeObject(changeXmr);
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send(msg, MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                }
            }
            UIMessageBox.Show("设置完成");
        }
    }
}
