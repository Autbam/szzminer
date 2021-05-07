
namespace szzminerServer.Views
{
    partial class changeCoin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputWallet = new Sunny.UI.UITextBox();
            this.InputMiningPool = new Sunny.UI.UITextBox();
            this.SelectMiningPool = new Sunny.UI.UIComboBox();
            this.SelectMiner = new Sunny.UI.UIComboBox();
            this.SelectCoin = new Sunny.UI.UIComboBox();
            this.uiLabel19 = new Sunny.UI.UILabel();
            this.uiLabel18 = new Sunny.UI.UILabel();
            this.uiLabel17 = new Sunny.UI.UILabel();
            this.uiLabel16 = new Sunny.UI.UILabel();
            this.uiLabel15 = new Sunny.UI.UILabel();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.InputArgu = new Sunny.UI.UITextBox();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputWallet
            // 
            this.InputWallet.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.InputWallet.FillColor = System.Drawing.Color.White;
            this.InputWallet.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.InputWallet.Location = new System.Drawing.Point(122, 214);
            this.InputWallet.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InputWallet.Maximum = 2147483647D;
            this.InputWallet.Minimum = -2147483648D;
            this.InputWallet.MinimumSize = new System.Drawing.Size(1, 1);
            this.InputWallet.Name = "InputWallet";
            this.InputWallet.Padding = new System.Windows.Forms.Padding(5);
            this.InputWallet.Size = new System.Drawing.Size(220, 22);
            this.InputWallet.TabIndex = 14;
            // 
            // InputMiningPool
            // 
            this.InputMiningPool.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.InputMiningPool.Enabled = false;
            this.InputMiningPool.FillColor = System.Drawing.Color.White;
            this.InputMiningPool.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.InputMiningPool.Location = new System.Drawing.Point(122, 176);
            this.InputMiningPool.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InputMiningPool.Maximum = 2147483647D;
            this.InputMiningPool.Minimum = -2147483648D;
            this.InputMiningPool.MinimumSize = new System.Drawing.Size(1, 1);
            this.InputMiningPool.Name = "InputMiningPool";
            this.InputMiningPool.Padding = new System.Windows.Forms.Padding(5);
            this.InputMiningPool.Size = new System.Drawing.Size(220, 22);
            this.InputMiningPool.TabIndex = 13;
            // 
            // SelectMiningPool
            // 
            this.SelectMiningPool.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.SelectMiningPool.FillColor = System.Drawing.Color.White;
            this.SelectMiningPool.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.SelectMiningPool.Location = new System.Drawing.Point(122, 138);
            this.SelectMiningPool.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SelectMiningPool.MinimumSize = new System.Drawing.Size(63, 0);
            this.SelectMiningPool.Name = "SelectMiningPool";
            this.SelectMiningPool.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.SelectMiningPool.Size = new System.Drawing.Size(150, 22);
            this.SelectMiningPool.TabIndex = 12;
            this.SelectMiningPool.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.SelectMiningPool.SelectedIndexChanged += new System.EventHandler(this.SelectMiningPool_SelectedIndexChanged);
            // 
            // SelectMiner
            // 
            this.SelectMiner.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.SelectMiner.FillColor = System.Drawing.Color.White;
            this.SelectMiner.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.SelectMiner.Location = new System.Drawing.Point(122, 100);
            this.SelectMiner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SelectMiner.MinimumSize = new System.Drawing.Size(63, 0);
            this.SelectMiner.Name = "SelectMiner";
            this.SelectMiner.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.SelectMiner.Size = new System.Drawing.Size(150, 22);
            this.SelectMiner.TabIndex = 6;
            this.SelectMiner.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SelectCoin
            // 
            this.SelectCoin.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.SelectCoin.FillColor = System.Drawing.Color.White;
            this.SelectCoin.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.SelectCoin.Location = new System.Drawing.Point(122, 62);
            this.SelectCoin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SelectCoin.MinimumSize = new System.Drawing.Size(63, 0);
            this.SelectCoin.Name = "SelectCoin";
            this.SelectCoin.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.SelectCoin.Size = new System.Drawing.Size(150, 22);
            this.SelectCoin.TabIndex = 5;
            this.SelectCoin.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.SelectCoin.SelectedIndexChanged += new System.EventHandler(this.SelectCoin_SelectedIndexChanged);
            // 
            // uiLabel19
            // 
            this.uiLabel19.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiLabel19.Location = new System.Drawing.Point(45, 213);
            this.uiLabel19.Name = "uiLabel19";
            this.uiLabel19.Size = new System.Drawing.Size(70, 23);
            this.uiLabel19.TabIndex = 7;
            this.uiLabel19.Text = "钱包地址:";
            this.uiLabel19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel18
            // 
            this.uiLabel18.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiLabel18.Location = new System.Drawing.Point(45, 175);
            this.uiLabel18.Name = "uiLabel18";
            this.uiLabel18.Size = new System.Drawing.Size(70, 23);
            this.uiLabel18.TabIndex = 8;
            this.uiLabel18.Text = "矿池地址:";
            this.uiLabel18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel17
            // 
            this.uiLabel17.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiLabel17.Location = new System.Drawing.Point(45, 137);
            this.uiLabel17.Name = "uiLabel17";
            this.uiLabel17.Size = new System.Drawing.Size(55, 23);
            this.uiLabel17.TabIndex = 9;
            this.uiLabel17.Text = "矿池:";
            this.uiLabel17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel16
            // 
            this.uiLabel16.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiLabel16.Location = new System.Drawing.Point(45, 99);
            this.uiLabel16.Name = "uiLabel16";
            this.uiLabel16.Size = new System.Drawing.Size(55, 23);
            this.uiLabel16.TabIndex = 10;
            this.uiLabel16.Text = "内核:";
            this.uiLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel15
            // 
            this.uiLabel15.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiLabel15.Location = new System.Drawing.Point(45, 61);
            this.uiLabel15.Name = "uiLabel15";
            this.uiLabel15.Size = new System.Drawing.Size(55, 23);
            this.uiLabel15.TabIndex = 11;
            this.uiLabel15.Text = "币种:";
            this.uiLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiButton1.Location = new System.Drawing.Point(71, 11);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(100, 35);
            this.uiButton1.TabIndex = 15;
            this.uiButton1.Text = "切换";
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiButton2.Location = new System.Drawing.Point(239, 11);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(100, 35);
            this.uiButton2.TabIndex = 16;
            this.uiButton2.Text = "取消";
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiButton2);
            this.uiPanel1.Controls.Add(this.uiButton1);
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiPanel1.Location = new System.Drawing.Point(0, 293);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(410, 56);
            this.uiPanel1.TabIndex = 17;
            this.uiPanel1.Text = null;
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiLabel1.Location = new System.Drawing.Point(45, 251);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(70, 23);
            this.uiLabel1.TabIndex = 7;
            this.uiLabel1.Text = "附加参数:";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel1.Click += new System.EventHandler(this.uiLabel1_Click);
            // 
            // InputArgu
            // 
            this.InputArgu.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.InputArgu.FillColor = System.Drawing.Color.White;
            this.InputArgu.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.InputArgu.Location = new System.Drawing.Point(122, 252);
            this.InputArgu.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InputArgu.Maximum = 2147483647D;
            this.InputArgu.Minimum = -2147483648D;
            this.InputArgu.MinimumSize = new System.Drawing.Size(1, 1);
            this.InputArgu.Name = "InputArgu";
            this.InputArgu.Padding = new System.Windows.Forms.Padding(5);
            this.InputArgu.Size = new System.Drawing.Size(220, 22);
            this.InputArgu.TabIndex = 14;
            // 
            // changeCoin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 349);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.InputArgu);
            this.Controls.Add(this.InputWallet);
            this.Controls.Add(this.InputMiningPool);
            this.Controls.Add(this.SelectMiningPool);
            this.Controls.Add(this.SelectMiner);
            this.Controls.Add(this.SelectCoin);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.uiLabel19);
            this.Controls.Add(this.uiLabel18);
            this.Controls.Add(this.uiLabel17);
            this.Controls.Add(this.uiLabel16);
            this.Controls.Add(this.uiLabel15);
            this.Name = "changeCoin";
            this.Text = "切换币种";
            this.Load += new System.EventHandler(this.changeCoin_Load);
            this.uiPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITextBox InputWallet;
        private Sunny.UI.UITextBox InputMiningPool;
        private Sunny.UI.UIComboBox SelectMiningPool;
        private Sunny.UI.UIComboBox SelectMiner;
        private Sunny.UI.UIComboBox SelectCoin;
        private Sunny.UI.UILabel uiLabel19;
        private Sunny.UI.UILabel uiLabel18;
        private Sunny.UI.UILabel uiLabel17;
        private Sunny.UI.UILabel uiLabel16;
        private Sunny.UI.UILabel uiLabel15;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UITextBox InputArgu;
    }
}