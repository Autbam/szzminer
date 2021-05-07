using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using szzminerServer.Class;
using szzminerServer.Tools;

namespace szzminerServer.Views
{
    public partial class changeCoin : UIForm
    {
        static UIDataGridView MinerStatusTable;
        public changeCoin(UIDataGridView Table)
        {
            InitializeComponent();
            MinerStatusTable = Table;
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
        private void uiButton1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(InputMiningPool.Text))
            {
                UIMessageBox.ShowError("请输入矿池地址");
                return;
            }
            if (string.IsNullOrEmpty(InputWallet.Text))
            {
                UIMessageBox.ShowError("请输入钱包地址");
                return;
            }
            changeCoinClass changeCoinClass = new changeCoinClass();
            changeCoinClass.coin = SelectCoin.Text;
            changeCoinClass.core = SelectMiner.Text;
            changeCoinClass.miningpool = SelectMiningPool.Text;
            changeCoinClass.miningpoolurl = InputMiningPool.Text;
            changeCoinClass.wallet = InputWallet.Text;
            changeCoinClass.function = "changeCoin";
            changeCoinClass.argu = InputArgu.Text;
            string msg = JsonConvert.SerializeObject(changeCoinClass);
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
            this.Close();
        }

        private void changeCoin_Load(object sender, EventArgs e)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                UIMessageBox.ShowError("请选择矿机");
                this.Close();
            }
            getMiningInfo();
            loadCoinIni(ref SelectCoin);
            SelectCoin.SelectedIndex = 0;
            SelectMiner.SelectedIndex = 0;
            SelectMiningPool.SelectedIndex = 0;
        }
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
        public static void loadCoinIni(ref UIComboBox coin)
        {
            IniHelper.setPath(Application.StartupPath + "\\config\\" + "\\miner.ini");
            List<string> coins = IniHelper.ReadSections();
            foreach (string c in coins)
            {
                coin.Items.Add(c);
            }
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
            }
            else
            {
                InputMiningPool.Enabled = false;
            }
            InputMiningPool.Text = IniHelper.GetValue(SelectCoin.Text, SelectMiningPool.Text, "", Application.StartupPath + "\\config" + "\\miningpool.ini");
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uiLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
