using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gminer
{
    public class DevicesItem
    {
        public string bus_id { get; set; }
        public int speed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int accepted_shares { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int rejected_shares { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int stale_shares { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int invalid_shares { get; set; }
    }

    public class gminer
    {

        public List<DevicesItem> devices { get; set; }
    }


}
