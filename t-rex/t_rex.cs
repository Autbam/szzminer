using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t_rex
{
    public class GpusItem
    {
        public int hashrate { get; set; }
        public int pci_bus { get; set; }
    }

    public class Stat_by_gpuItem
    {
        public int accepted_count { get; set; }

        public int rejected_count { get; set; }

    }


    public class t_rex
    {
        
        public List<GpusItem> gpus { get; set; }

        public List<Stat_by_gpuItem> stat_by_gpu { get; set; }

    }

}
