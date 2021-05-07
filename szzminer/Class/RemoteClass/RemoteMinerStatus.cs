using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szzminer.Class.RemoteClass
{
    public class DevicesItem
    {
        /// <summary>
        /// 
        /// </summary>
        ///
        public string name { get; set; }
        public string idbus { get; set; }
        public string power { get; set; }
        public string Hashrate { get; set; }
        public string temp { get; set; }
        public string fan { get; set; }
        public string accept { get; set; }
        public string reject { get; set; }
        public string coreclock { get; set; }
        public string memoryclock { get; set; }
    }
    public class RemoteMinerStatus
    {
        public string function { get; set; }
        public bool if_mining { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        public string MAC { get; set; }
        public string Coin { get; set; }
        public string MinerCore { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Worker { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Pool { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Wallet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Accepted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Rejected { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Power { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Hashrate { get; set; }

        public List<DevicesItem> Devices { get; set; }

        public string xmrPool { get; set; }
        public string xmrWallet { get; set; }
        public string xmrAccept { get; set; }
        public string xmrReject { get; set; }
        public string xmrHashrate { get; set; }
    }
}
