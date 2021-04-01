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
    public class Display
    {
        public List<string> BUSID { get; set; }
        public List<string> Hashrate { get; set; }
        public List<string> Accepted { get; set; }
        public List<string> Rejected { get; set; }
        public static string getHtml(string html)//传入网址
        {
            try
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
            catch(Exception ex)
            {
                return null;
            }
        }

        public void getMinerInfo()
        {
            BUSID = new List<string>();
            Hashrate = new List<string>();
            Accepted = new List<string>();
            Rejected = new List<string>();
            string json = getHtml("http://127.0.0.1:22333/api/v1/status");
            try
            {
                nbminer nbminerInfo = JsonConvert.DeserializeObject<nbminer>(json);
                for (int gpuCount = 0; gpuCount < nbminerInfo.miner.devices.Count; gpuCount++)
                {
                    BUSID.Add(nbminerInfo.miner.devices[gpuCount].pci_bus_id.ToString());
                    Hashrate.Add(nbminerInfo.miner.devices[gpuCount].hashrate.Trim());
                    Accepted.Add(nbminerInfo.miner.devices[gpuCount].accepted_shares.ToString());
                    Rejected.Add(nbminerInfo.miner.devices[gpuCount].rejected_shares.ToString());
                }
            }
            catch(Exception ex)
            {
                
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
