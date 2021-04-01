using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayDll
{
    class nbminer
    {
        public Miner miner { get; set; }
    }
    public class DevicesItem
    {
        public int accepted_shares { get; set; }
        public string hashrate { get; set; }
        public int pci_bus_id { get; set; }
        public int rejected_shares { get; set; }

    }
    public class Miner
    {
        public List<DevicesItem> devices { get; set; }
    }
}
