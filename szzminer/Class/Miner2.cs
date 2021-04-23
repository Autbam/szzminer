using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szzminer.Class
{
    public class MinerItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Busid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Hashrate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Accept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Reject { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Power { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Coretemp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Memtemp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Coreclock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Memclock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Fan { get; set; }
    }

    public class OverclockItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Busid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Power { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Templimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Core { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Memory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CoreV { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MemV { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Fan { get; set; }
    }

    public class Miner2
    {
        /// <summary>
        /// 
        /// </summary>
        public string Miningpool { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Coin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Core { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Wallet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Worker { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Argu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Delay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MinerItem> Miner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OverclockItem> Overclock { get; set; }
    }

}
