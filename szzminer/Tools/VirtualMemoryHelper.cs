using Microsoft.Win32;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static szzminer.Tools.VirtualMemory;

namespace szzminer.Tools
{
    class VirtualMemoryHelper
    {
        public static DriveSetImpl driveSetImpl = new DriveSetImpl();
        public static void getVirtualMemoryInfo(ref UIComboBox disk)
        {
            for (int i = 0; i < driveSetImpl._drives.Count; i++)
            {
                disk.Items.Add(driveSetImpl._drives[i].Name.Replace("\\", "盘").Replace(":", ""));
            }
        }
        public static void getVirtualMemoryUsage(int index,ref UILabel display)
        {
            driveSetImpl = new DriveSetImpl();
            display.Text = "本盘已使用虚拟内存" + (driveSetImpl._drives[index].VirtualMemoryMaxSizeMb / 1024).ToString() + "GB，" + "剩余空间" + (driveSetImpl._drives[index].AvailableFreeSpace / 1024 / 1024 / 1024).ToString() + "GB";
        }
        public static void setVirtualMemory(UIComboBox disk,int size,ref UILabel display)
        {
            driveSetImpl = new DriveSetImpl();
            if (size < 0)
            {
                MessageBox.Show("输入错误！", "提示");
                return;
            }
            //if()
            if ((driveSetImpl._drives[disk.SelectedIndex].AvailableFreeSpace / 1024 / 1024 / 1024)+ (driveSetImpl._drives[disk.SelectedIndex].AvailableFreeSpace / 1024 / 1024 / 1024) < size)
            {
                MessageBox.Show("虚拟内存不可大于磁盘剩余空间！", "提示");
                return;
            }
            List<string> list = new List<string>();
            for (int i = 0; i < driveSetImpl._drives.Count; i++)
            {
                if (i == disk.SelectedIndex)
                {
                    if (size != 0)
                    {
                        list.Add(DriveSetImpl.VirtualMemoryFormatString(driveSetImpl._drives[i].Name, size * 1024));
                    }
                }
                else
                {
                    if (driveSetImpl._drives[i].VirtualMemoryMaxSizeMb != 0)
                    {
                        list.Add(DriveSetImpl.VirtualMemoryFormatString(driveSetImpl._drives[i].Name, driveSetImpl._drives[i].VirtualMemoryMaxSizeMb));
                    }
                }
            }
            DriveSetImpl.SetValue(Registry.LocalMachine, @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", "PagingFiles", list.ToArray());
            UIMessageBox.ShowSuccess("虚拟内存设置成功，重启后生效");
            driveSetImpl = new DriveSetImpl();
            display.Text = "本盘已使用虚拟内存" + (driveSetImpl._drives[disk.SelectedIndex].VirtualMemoryMaxSizeMb / 1024).ToString() + "GB，" + "剩余空间" + (driveSetImpl._drives[disk.SelectedIndex].AvailableFreeSpace / 1024 / 1024 / 1024).ToString() + "GB";
        }
    }
    public static partial class VirtualMemory
    {
        private static readonly object _locker = new object();
        public const double DoubleK = 1024;
        public const int IntK = 1024;
        public const double DoubleM = 1024 * 1024;
        public const int IntM = 1024 * 1024;
        public const double DoubleG = 1024 * 1024 * 1024;
        public const ulong ULongG = 1024 * 1024 * 1024;
        public const long LongG = 1024 * 1024 * 1024;
        public interface IDriveSet
        {
            int OSVirtualMemoryMb { get; }
            string ToDiskSpaceString();
            IEnumerable<DriveDto> AsEnumerable();
            void SetVirtualMemory(Dictionary<string, int> virtualMemories);
        }

        private static IDriveSet _driveSet = null;
        public static IDriveSet DriveSet
        {
            get
            {
                if (_driveSet == null)
                {
                    lock (_locker)
                    {
                        if (_driveSet == null)
                        {
                            _driveSet = new DriveSetImpl();
                        }
                    }
                }
                return _driveSet;
            }
        }
    }
    public class DriveSetImpl : IDriveSet
    {
        private const string MemoryManagementSubKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management";
        public readonly List<DriveDto> _drives = new List<DriveDto>();
        private void WriteLog(string msg)//写日志
        {
            try
            {
                string path = Path.Combine("./log");
                if (!Directory.Exists(path))//判断是否有该文件 
                    Directory.CreateDirectory(path);
                string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//生成日志文件 
                if (!File.Exists(logFileName))//判断日志文件是否为当天
                {
                    FileStream fs;
                    fs = File.Create(logFileName);//创建文件
                    fs.Close();
                }
                StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流
                writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + msg + "\n");
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                string path = Path.Combine("./log");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                if (!File.Exists(logFileName))//判断日志文件是否为当天
                {
                    FileStream fs;
                    fs = File.Create(logFileName);//创建文件
                    fs.Close();
                }
                StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流
                writer.WriteLine(DateTime.Now.ToString("日志记录错误HH:mm:ss") + "\r\n " + e.Message + " " + msg);
                writer.WriteLine("--------------------------------分割线--------------------------------");
                writer.Flush();
                writer.Close();
            }

        }
        public DriveSetImpl()
        {
            InitOnece();
        }

        // 因为虚拟内存修改后重启电脑才会生效所以不需要刷新内存中的数据
        private DateTime _diskSpaceOn = DateTime.MinValue;
        private readonly object _locker = new object();
        private void InitOnece()
        {
            var now = DateTime.Now;
            if (_diskSpaceOn.AddMinutes(20) < now)
            {
                lock (_locker)
                {
                    if (_diskSpaceOn.AddMinutes(20) < now)
                    {
                        _diskSpaceOn = now;
                        _drives.Clear();
                        var virtualMemoryDicByDriveName = GetVirtualMemoryDic();
                        foreach (var item in DriveInfo.GetDrives().Where(a => a.DriveType == DriveType.Fixed))
                        {
                            try
                            {
                                int virtualMemoryMaxSizeMb = 0;
                                if (virtualMemoryDicByDriveName.TryGetValue(item.Name, out int value))
                                {
                                    virtualMemoryMaxSizeMb = value;
                                }
                                _drives.Add(new DriveDto(item, virtualMemoryMaxSizeMb));
                            }
                            catch (Exception e)
                            {
                                WriteLog(e.ToString());
                            }
                        }
                    }
                }
            }
        }

        public int OSVirtualMemoryMb
        {
            get
            {
                InitOnece();
                return _drives.Sum(a => a.VirtualMemoryMaxSizeMb);
            }
        }

        public IEnumerable<DriveDto> AsEnumerable()
        {
            InitOnece();
            return _drives;
        }
        public static object GetValue(RegistryKey root, string subkey, string valueName)
        {
            object registData = "";
            try
            {
                using (RegistryKey registryKey = root.OpenSubKey(subkey, true))
                {
                    if (registryKey != null)
                    {
                        registData = registryKey.GetValue(valueName);
                        registryKey.Close();
                    }
                    return registData;
                }
            }
            catch (System.Exception e)
            {
                return registData;
            }
        }
        public string ToDiskSpaceString()
        {
            InitOnece();
            StringBuilder sb = new StringBuilder();
            int len = sb.Length;
            foreach (var item in _drives)
            {
                if (len != sb.Length)
                {
                    sb.Append(";");
                }
                // item.Name like C:\
                sb.Append(item.Name).Append((item.AvailableFreeSpace / DoubleG).ToString("f1")).Append(" Gb");
            }
            return sb.ToString();
        }
        public static void SetValue(RegistryKey root, string subkey, string valueName, object value)
        {
            try
            {
                using (RegistryKey registryKey = root.CreateSubKey(subkey))
                {
                    registryKey.SetValue(valueName, value);
                    registryKey.Close();
                }
            }
            catch (System.Exception e)
            {

            }
        }
        public void SetVirtualMemory(Dictionary<string, int> virtualMemories)
        {
            if (virtualMemories == null || virtualMemories.Count == 0)
            {
                return;
            }
            if (virtualMemories.TryGetValue("Auto", out int virtualMemoryMb))
            {
                long virtualMemoryB = virtualMemoryMb * IntM;
                // 系统盘留出1Gb
                long systemReserveB = LongG;
                // 非系统盘留出100Mb
                long reserveB = 100 * IntM;
                if (_drives.Sum(a => a.AvailableFreeSpace) - systemReserveB - (_drives.Count - 1) * reserveB < virtualMemoryMb)
                {
                    return;
                }
                var systemDrive = _drives.FirstOrDefault(a => a.IsSystemDisk);
                // 不可能没有系统盘
                if (systemDrive == null)
                {
                    return;
                }
                int setedMb = 0;
                List<string> list = new List<string>();
                // 如果系统盘够大设置在系统盘
                if (systemDrive.AvailableFreeSpace - systemReserveB > virtualMemoryB)
                {
                    list.Add(VirtualMemoryFormatString(systemDrive.Name, virtualMemoryMb));
                    setedMb += virtualMemoryMb;
                }
                else
                {
                    // 设置在系统盘mb
                    int mb = Convert.ToInt32((systemDrive.AvailableFreeSpace - systemReserveB) / IntM);
                    list.Add(VirtualMemoryFormatString(systemDrive.Name, mb));
                    setedMb += mb;
                    var bigDrive = _drives.Where(a => !a.IsSystemDisk).OrderByDescending(a => a.AvailableFreeSpace).FirstOrDefault();
                    // 还需设置mb
                    mb = virtualMemoryMb - setedMb;
                    // 如果最大的盘可以装下剩余的虚拟内存就把剩余的都设置在这个盘
                    if (bigDrive != null && bigDrive.AvailableFreeSpace - reserveB > mb * IntM)
                    {
                        list.Add(VirtualMemoryFormatString(bigDrive.Name, mb));
                        setedMb += mb;
                    }
                    else
                    {
                        foreach (var drive in _drives)
                        {
                            if (drive.IsSystemDisk)
                            {
                                continue;
                            }
                            mb = Convert.ToInt32((drive.AvailableFreeSpace - reserveB) / IntM);
                            if (mb <= 0)
                            {
                                continue;
                            }
                            list.Add(VirtualMemoryFormatString(drive.Name, mb));
                            setedMb += mb;
                            if (setedMb >= virtualMemoryMb)
                            {
                                break;
                            }
                        }
                    }
                }
                if (setedMb >= virtualMemoryMb)
                {
                    SetValue(Registry.LocalMachine, MemoryManagementSubKey, "PagingFiles", list.ToArray());
                }
            }
            else
            {
                List<string> list = new List<string>();
                foreach (var drive in _drives)
                {
                    if (virtualMemories.TryGetValue(drive.Name, out int value) && value > 0)
                    {
                        list.Add(VirtualMemoryFormatString(drive.Name, value));
                    }
                }

                SetValue(Registry.LocalMachine, MemoryManagementSubKey, "PagingFiles", list.ToArray());
            }
        }

        #region 静态私有方法
        public static string VirtualMemoryFormatString(string name, int value)
        {
            return $"{name}pagefile.sys  {value.ToString()} {value.ToString()}";
        }

        private static Dictionary<string, int> GetVirtualMemoryDic()
        {
            object value = GetValue(Registry.LocalMachine, MemoryManagementSubKey, "PagingFiles");
            // REG_SZ or REG_MULTI_SZ
            Dictionary<string, int> dicByDriveName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (value is string[] vmReg)
            {
                foreach (string item in vmReg)
                {
                    if (TryParseVirtualMemory(item, out KeyValuePair<string, int> kv))
                    {
                        dicByDriveName.Add(kv.Key, kv.Value);
                    }
                }
            }
            return dicByDriveName;
        }

        private static bool TryParseVirtualMemory(string vmReg, out KeyValuePair<string, int> kv)
        {
            string driveName;
            try
            {
                string[] strarr = vmReg.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (strarr.Length == 3)
                {
                    driveName = strarr[0].Substring(0, 3);
                    int minsize = Convert.ToInt32(strarr[1]);
                    int maxsize = Convert.ToInt32(strarr[2]);
                    kv = new KeyValuePair<string, int>(driveName, maxsize);
                    return true;
                }
                kv = default;
                return false;
            }
            catch (Exception e)
            {
                kv = default;
                return false;
            }
        }
        #endregion
    }
}
