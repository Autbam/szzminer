using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szzminerServer.Tools
{
    public class MinerOptions
    {
        public static bool startXmr(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"startXmr\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }
        public static bool stopXmr(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"stopXmr\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }
        public static bool startMiner(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"startMining\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }
        public static bool amdOnCalc(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"amdOnCalc\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }
        public static bool amdOffCalc(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"amdOffCalc\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }
        public static bool amdDriverPatch(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"amdDrivePatch\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }
        public static bool stopMiner(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"stopMining\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }
        public static bool shutdownMiner(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"shutdown\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }
        public static bool rebootMiner(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"reboot\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                    return true;
                }
            }
            return true;
        }

        public static bool updateMiner(UIDataGridView MinerStatusTable)
        {
            if (!checkTableSelected(MinerStatusTable))
            {
                return false;
            }
            var i = 0;
            for (; i < MinerStatusTable.Rows.Count; i++)
            {
                if (MinerStatusTable.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    UDPHelper.Send("{\"function\":\"update\"}", MinerStatusTable.Rows[i].Cells[12].Value.ToString());
                }
            }
            return true;
        }

        private static bool checkTableSelected(UIDataGridView MinerStatusTable)
        {
            int j = 0;
            for (; j < MinerStatusTable.Rows.Count; j++)
            {
                if (MinerStatusTable.Rows[j].Cells[2].Value == null)
                {
                    continue;
                }
                if (MinerStatusTable.Rows[j].Cells[2].Value.ToString() == "True")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
