using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace DisplayDll
{
    class Display
    {
        public List<string> BUSID { get; set; }
        public List<string> Hashrate { get; set; }
        public List<string> Accepted { get; set; }
        public List<string> Rejected { get; set; }
        public static string getHtml(string html)//传入网址
        {
            string pageHtml = "";
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = MyWebClient.DownloadData(html); //从指定网站下载数据
            MemoryStream ms = new MemoryStream(pageData);
            using (StreamReader sr = new StreamReader(ms, Encoding.GetEncoding("UTF-8")))
            {
                pageHtml = sr.ReadLine();
            }
            return pageHtml;
        }

        public void getMinerInfo()
        {
            BUSID = new List<string>();
            Hashrate = new List<string>();
            Accepted = new List<string>();
            Rejected = new List<string>();
            string json = getHtml("http://127.0.0.1:22333/summary");
            t_rex.t_rex trexInfo = JsonConvert.DeserializeObject<t_rex.t_rex>(json);
            for (int gpuCount = 0; gpuCount < trexInfo.gpus.Count; gpuCount++)
            {
                BUSID.Add(trexInfo.gpus[gpuCount].pci_bus.ToString());
                Hashrate.Add(((double)(trexInfo.gpus[gpuCount].hashrate/1000000.0)).ToString("#0.00"));
                Accepted.Add(trexInfo.stat_by_gpu[gpuCount].accepted_count.ToString());
                Rejected.Add(trexInfo.stat_by_gpu[gpuCount].rejected_count.ToString());
            }
        }
        public List<string> getBUSID()
        {
            return BUSID;
        }
        public List<string> getHashrate()
        {
            return Hashrate;
        }
        public List<string> getAccepted()
        {
            return Accepted;
        }
        public List<string> getRejected()
        {
            return Rejected;
        }
    }
}
