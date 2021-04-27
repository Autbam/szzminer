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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace szzminer_update
{
    public partial class Form1 : UIForm
    {
        string url= "https://szzkg.bj.bcebos.com/szzminer.exe";
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Text = UILocalize.InfoTitle;
            Description = UILocalize.SystemProcessing;
        }

        public Form1(int max, string desc)
        {
            InitializeComponent();

            Maximum = max;
            Description = desc;
            Value = 0;
        }

        [DefaultValue(100)]
        public int Maximum
        {
            get => processBar.Maximum;
            set => processBar.Maximum = value;
        }

        [DefaultValue(0)]
        public int Value
        {
            get => processBar.Value;
            set => processBar.Value = value;
        }

        [DefaultValue(1)]
        public int Step
        {
            get => processBar.Step;
            set => processBar.Step = value;
        }

        [DefaultValue(true)]
        public bool ShowValue
        {
            get => processBar.ShowValue;
            set => processBar.ShowValue = value;
        }

        /// <summary>
        /// 进度到达最大值时自动隐藏
        /// </summary>
        [DefaultValue(true)]
        public bool MaxAutoHide { get; set; } = true;

        private void processBar_ValueChanged(object sender, int value)
        {
            if (MaxAutoHide && value == Maximum)
            {
                Hide();
            }
        }

        public void Show(string title, string desc, int max = 100, int value = 0)
        {
            Text = title;
            labelDescription.Text = desc;
            processBar.Maximum = max;
            processBar.Value = value;
            Show();
        }

        public string Description
        {
            get => labelDescription.Text;
            set => labelDescription.Text = value;
        }

        private delegate void SetTextHandler(string text);

        public void SetDescription(string text)
        {
            if (labelDescription.InvokeRequired)
            {
                Invoke(new SetTextHandler(SetDescription), text);
            }
            else
            {
                labelDescription.Text = text;
                labelDescription.Invalidate();
            }
        }

        private delegate void StepItHandler(int step);

        public void StepIt(int step = 1)
        {
            if (processBar.InvokeRequired)
            {
                Invoke(new StepItHandler(StepIt), step);
            }
            else
            {
                processBar.Step = step;
                processBar.StepIt();
            }
        }
        private static long GetHttpLength(string url)
        {
            long length = 0;

            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);// 打开网络连接
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

                if (rsp.StatusCode == HttpStatusCode.OK)
                {
                    length = rsp.ContentLength;// 从文件头得到远程文件的长度
                }

                rsp.Close();
                return length;
            }
            catch (Exception e)
            {
                return length;
            }

        }
        public void Download()
        {
            string localfile = Application.StartupPath+"\\szzminer.exe";
            Thread.Sleep(1000);
            long startPosition = 0; // 上次下载的文件起始位置
            FileStream writeStream; // 写入本地文件流对象
            long remoteFileLength = GetHttpLength(url);// 取得远程文件长度
            //System.Console.WriteLine("remoteFileLength=" + remoteFileLength);
            if (remoteFileLength == 745)
            {
                MessageBox.Show("远程文件不存在.");
            }

            // 判断要下载的文件夹是否存在
            if (File.Exists(localfile))
            {

                writeStream = File.OpenWrite(localfile);             // 存在则打开要下载的文件
                startPosition = writeStream.Length;                  // 获取已经下载的长度

                if (startPosition >= remoteFileLength)
                {
                    MessageBox.Show("本地文件长度" + startPosition + "已经大于等于远程文件长度" + remoteFileLength);
                    writeStream.Close();
                }
                else
                {
                    writeStream.Seek(startPosition, SeekOrigin.Current); // 本地文件写入位置定位
                }
            }
            else
            {
                writeStream = new FileStream(localfile, FileMode.Create);// 文件不保存创建一个文件
                startPosition = 0;
            }
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);// 打开网络连接
                if (startPosition > 0)
                {
                    myRequest.AddRange((int)startPosition);// 设置Range值,与上面的writeStream.Seek用意相同,是为了定义远程文件读取位置
                }
                Stream readStream = myRequest.GetResponse().GetResponseStream();// 向服务器请求,获得服务器的回应数据流
                byte[] btArray = new byte[512];// 定义一个字节数据,用来向readStream读取内容和向writeStream写入内容
                int contentSize = readStream.Read(btArray, 0, btArray.Length);// 向远程文件读第一次
                long currPostion = startPosition;
                while (contentSize > 0)// 如果读取长度大于零则继续读
                {
                    currPostion += contentSize;
                    int percent = (int)(currPostion * 100 / remoteFileLength);
                    this.processBar.Value = percent;
                    writeStream.Write(btArray, 0, contentSize);// 写入本地文件
                    contentSize = readStream.Read(btArray, 0, btArray.Length);// 继续向远程文件读取
                }
                //关闭流
                writeStream.Close();
                readStream.Close();
            }
            catch (Exception)
            {
                writeStream.Close();
            }
            finally
            {
                this.Close();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Task.Run(() => {
                this.Text = "松之宅矿工自动更新";
                this.labelDescription.Text = "正在等待挖矿进程结束......";
                //等待szzminer进程结束
                bool continueFlag;
                while (true)
                {
                    continueFlag = true;
                    Process[] myProcesses = System.Diagnostics.Process.GetProcesses();
                    foreach (System.Diagnostics.Process myProcess in myProcesses)
                    {
                        if (myProcess.ProcessName.ToLower().Equals("szzminer"))
                        {
                            continueFlag = false;
                            break;
                        }
                    }
                    if(continueFlag)
                    {
                        break;
                    }
                    Thread.Sleep(500);
                }

                this.labelDescription.Text = "正在更新中，请稍候......";
                //删除源文件
                if (File.Exists(Application.StartupPath + "\\szzminer.exe"))
                {
                    File.Delete(Application.StartupPath + "\\szzminer.exe");
                }
                //下载
                Download();
                //打开挖矿程序
                string path = Application.StartupPath + "\\szzminer.exe";
                Process p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                p.Start();
                //关闭更新程序
                Application.Exit();
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
