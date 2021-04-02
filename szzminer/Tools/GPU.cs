using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szzminer_overclock.AMD;

namespace szzminer.Tools
{
    class GPU
    {
        public static void getGPU(ref UIDataGridView GPUStatusTable,ref int totalPower)
        {
            int GPUCount = 0;
            try
            {
                szzminer_overclock.AMD.NvmlHelper nvmlHelper = new szzminer_overclock.AMD.NvmlHelper();
                szzminer_overclock.AMD.NvapiHelper nvapiHelper = new szzminer_overclock.AMD.NvapiHelper();
                List<szzminer_overclock.AMD.NvmlHelper.NvGpu> gpus = nvmlHelper.GetGpus();
                uint coretemp = 0;
                uint memtemp = 0;
                for (int i = 0; i < gpus.Count; i++)
                {
                    szzminer_overclock.AMD.OverClockRange overClockRange = nvapiHelper.GetClockRange(gpus[i].BusId);
                    GPUStatusTable.Rows[GPUCount].Cells[0].Value = gpus[i].BusId;
                    GPUStatusTable.Rows[GPUCount].Cells[1].Value = "NVIDIA " + gpus[i].Name + " " + gpus[i].TotalMemory / 1024 / 1000 / 1000 + "GB";
                    GPUStatusTable.Rows[GPUCount].Cells[5].Value = nvmlHelper.GetPowerUsage(i);
                    nvapiHelper.GetTemperature(gpus[i].BusId, out coretemp,out memtemp);
                    GPUStatusTable.Rows[GPUCount].Cells[6].Value = coretemp.ToString()+"/"+memtemp.ToString();
                    GPUStatusTable.Rows[GPUCount].Cells[7].Value = nvmlHelper.GetFanSpeed(i);
                    GPUStatusTable.Rows[GPUCount].Cells[8].Value = nvmlHelper.GetCoreClock(i) + "Mhz";
                    GPUStatusTable.Rows[GPUCount].Cells[9].Value = nvmlHelper.GetMemoryClock(i)+"Mhz";
                    totalPower += Convert.ToInt32(GPUStatusTable.Rows[GPUCount].Cells[5].Value);
                    GPUCount++;
                }
            }
            catch
            {
                //WriteLog("没有发现N卡");
            }
            try
            {
                AdlHelper adl = new AdlHelper();
                const ulong a = 1024 * 1024 * 1024;
                uint power = 0, fan = 0;
                int temp = 0;
                int memtemp = 0;
                int coreClock = 0, memoryClock = 0;
                for (int i = 0; i < adl.ATIGpus.Count; i++)
                {
                    adl.GetPowerFanTemp(adl.ATIGpus[i].BusNumber,out power,out fan,out temp,out memtemp);
                    adl.GetClockRange(adl.ATIGpus[i].BusNumber,out coreClock,out memoryClock);
                    GPUStatusTable.Rows[GPUCount].Cells[0].Value = adl.ATIGpus[i].BusNumber;
                    GPUStatusTable.Rows[GPUCount].Cells[1].Value = adl.ATIGpus[i].AdapterName + " " + ((adl.GetTotalMemory(adl.ATIGpus[i].AdapterIndex) + a - 1) / a).ToString() + "GB"; ;
                    GPUStatusTable.Rows[GPUCount].Cells[5].Value = power;
                    GPUStatusTable.Rows[GPUCount].Cells[6].Value = temp;
                    GPUStatusTable.Rows[GPUCount].Cells[7].Value = fan;
                    GPUStatusTable.Rows[GPUCount].Cells[8].Value = coreClock/1000 + "Mhz";
                    GPUStatusTable.Rows[GPUCount].Cells[9].Value = memoryClock/1000 + "Mhz";
                    totalPower += Convert.ToInt32(GPUStatusTable.Rows[GPUCount].Cells[5].Value);
                    GPUCount++;
                }
            }
            catch
            {
                

            }
        }

        public static void getOverclockGPU(ref UIDataGridView GPUOverClockTable)
        {
            int GPUCount = 0;
            try
            {
                szzminer_overclock.AMD.NvmlHelper nvmlHelper = new szzminer_overclock.AMD.NvmlHelper();
                szzminer_overclock.AMD.NvapiHelper nvapiHelper = new szzminer_overclock.AMD.NvapiHelper();
                List<szzminer_overclock.AMD.NvmlHelper.NvGpu> gpus = nvmlHelper.GetGpus();
                for (int i = 0; i < gpus.Count; i++)
                {
                    szzminer_overclock.AMD.OverClockRange overClockRange = nvapiHelper.GetClockRange(gpus[i].BusId);
                    GPUOverClockTable.Rows[GPUCount].Cells[0].Value = gpus[i].BusId;
                    GPUOverClockTable.Rows[GPUCount].Cells[1].Value = "NVIDIA " + gpus[i].Name + " " + gpus[i].TotalMemory / 1024 / 1000 / 1000 + "GB";
                    GPUOverClockTable.Rows[GPUCount].Cells[5].Value = "N/A";
                    GPUOverClockTable.Rows[GPUCount].Cells[7].Value = "N/A";
                    GPUCount++;
                }
            }
            catch
            {
                //WriteLog("没有发现N卡");
            }
            try
            {
                AdlHelper adl = new AdlHelper();
                const ulong a= 1024 * 1024 * 1024;
                for (int i = 0; i < adl.ATIGpus.Count; i++)
                {
                    uint power, fan;
                    int temp;
                    int coreClock, memoryClock;
                    adl.GetPowerFanTemp(adl.ATIGpus[i].BusNumber, out power, out fan, out temp);
                    adl.GetClockRange(adl.ATIGpus[i].BusNumber, out coreClock, out memoryClock);
                    GPUOverClockTable.Rows[GPUCount].Cells[0].Value = adl.ATIGpus[i].BusNumber;
                    GPUOverClockTable.Rows[GPUCount].Cells[1].Value = adl.ATIGpus[i].AdapterName + " " + ((adl.GetTotalMemory(adl.ATIGpus[i].AdapterIndex) + a - 1) / a).ToString() + "GB"; ;
                    GPUOverClockTable.Rows[GPUCount].Cells[2].Value = adl.ATIGpus[i].PowerDefault.ToString();
                    GPUOverClockTable.Rows[GPUCount].Cells[3].Value = adl.ATIGpus[i].TempLimitDefault.ToString();
                    GPUOverClockTable.Rows[GPUCount].Cells[4].Value = (adl.ATIGpus[i].coreClockSelf).ToString();
                    GPUOverClockTable.Rows[GPUCount].Cells[5].Value = adl.ATIGpus[i].CoreVolt;
                    GPUOverClockTable.Rows[GPUCount].Cells[6].Value = (adl.ATIGpus[i].memoryClockSelf / 1000).ToString();
                    GPUOverClockTable.Rows[GPUCount].Cells[7].Value = adl.ATIGpus[i].MemoryVolt;
                    GPUCount++;
                }
            }
            catch
            {


            }
        }

        public static void addRow(ref UIDataGridView GPUStatusTable,ref UIDataGridView GPUOverClockTable)
        {
            try
            {
                szzminer_overclock.AMD.NvmlHelper nvmlHelper = new szzminer_overclock.AMD.NvmlHelper();
                List<szzminer_overclock.AMD.NvmlHelper.NvGpu> gpus = nvmlHelper.GetGpus();
                for (int i = 0; i < gpus.Count; i++)
                {
                    GPUStatusTable.Rows.Add();
                    GPUOverClockTable.Rows.Add();
                    GPUStatusTable.Rows[i].Cells[2].Value = "0";
                    GPUStatusTable.Rows[i].Cells[3].Value = "0";
                    GPUStatusTable.Rows[i].Cells[4].Value = "0";
                }
            }
            catch
            {

            }
            try
            {
                AdlHelper adl = new AdlHelper();
                for (int i = 0; i < adl.ATIGpus.Count; i++)
                {
                    GPUStatusTable.Rows.Add();
                    GPUOverClockTable.Rows.Add();
                }
            }
            catch
            {

            }
        }
    }
}
