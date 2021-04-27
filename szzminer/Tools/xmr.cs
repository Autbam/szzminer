using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace szzminer.Tools
{
    public class Results
    {
        /// <summary>
        /// 
        /// </summary>
        public int diff_current { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int shares_good { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int shares_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int avg_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashes_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> best { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> error_log { get; set; }
    }

    public class Connection
    {
        /// <summary>
        /// 
        /// </summary>
        public string pool { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int uptime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ping { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int failures { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tls_fingerprint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> error_log { get; set; }
    }

    public class Cpu
    {
        /// <summary>
        /// 
        /// </summary>
        public string brand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avx2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string x64 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int l2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int l3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int threads { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int packages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int nodes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string backend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string assembly { get; set; }
    }

    public class Hashrate
    {
        /// <summary>
        /// 
        /// </summary>
        public List<double> total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double highest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<List<double>> threads { get; set; }
    }

    public class xmr_Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string worker_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int uptime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string restricted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> features { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Results results { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string algo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Connection connection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ua { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Cpu cpu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int donate_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paused { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> algorithms { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Hashrate hashrate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hugepages { get; set; }
    }

    class xmr
    {
        private static string getHtml(string html)//传入网址
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData(html);
                string pageHtml = Encoding.UTF8.GetString(pageData);
                return pageHtml;
            }
            catch
            {
                return null;
            }
        }

        public static string getXmrInfo()
        {
            try
            {
                string json = getHtml("http://127.0.0.1:22334/1/summary");
                json = json.Replace("null", "0.00");
                return json;
            }
            catch
            {
                return "0";
            }
        }
    }
}
