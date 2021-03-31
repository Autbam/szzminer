using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t_rex
{
    public class Active_pool
    {
        /// <summary>
        /// 
        /// </summary>
        public string difficulty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ping { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int retries { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string worker { get; set; }
    }

    public class GpusItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int dag_build_mode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int device_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string efficiency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int fan_speed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int gpu_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int gpu_user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashrate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashrate_day { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashrate_hour { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ulong hashrate_instant { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashrate_minute { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double intensity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string low_load { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int mtweak { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pci_bus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pci_domain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pci_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string potentially_unstable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int power { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int power_avr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int temperature { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vendor { get; set; }
    }

    public class Stat_by_gpuItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int accepted_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int invalid_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int rejected_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int solved_count { get; set; }
    }


    public class t_rex
    {
        /// <summary>
        /// 
        /// </summary>
        public int accepted_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Active_pool active_pool { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string algorithm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string api { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string build_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string coin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cuda { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double difficulty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string driver { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int gpu_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<GpusItem> gpus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashrate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashrate_day { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashrate_hour { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hashrate_minute { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int invalid_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string os { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int rejected_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string revision { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double sharerate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double sharerate_average { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int solved_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Stat_by_gpuItem> stat_by_gpu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int uptime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string validate_shares { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
    }

}
