using MyUtil.Memory.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MyUtil.Memory
{
    public class CheatTools
    {

        #region 进程相关方法

        //根据进程名获取PID
        public static int GetPidByProcessName(string processName)
        {
            Process[] arrayProcess = Process.GetProcessesByName(processName);
            foreach (Process p in arrayProcess)
            {
                return p.Id;
            }
            return 0;
        }

        //打开进程handle  0x1F0FFF 最高权限
        public static IntPtr GetProcessHandle(int Pid)
        {
            return WinAPI.OpenProcess(0x1F0FFF, false, Pid);
        }

        #endregion

        #region 内存相关方法

        //读内存模块  
        public static IntPtr ReadModule(string ModuleName)
        {
            return WinAPI.GetModuleHandle(ModuleName);
        }

        //读取内存中的值
        public static int ReadMemoryValue(int baseAddress, IntPtr hProcess)
        {
            try
            {
                byte[] buffer = new byte[4];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //将制定内存中的值读入缓冲区
                WinAPI.ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero);
                ////关闭操作
                //WinAPI.CloseHandle(hProcess);
                //从非托管内存中读取一个 32 位带符号整数。
                return Marshal.ReadInt32(byteAddress);
            }
            catch
            {
                return 0;
            }
        }

        //读浮点数
        public static float ReadMemoryFloat(int baseAddress, IntPtr hProcess)
        {
            try
            {
                byte[] buffer = new byte[4];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //将制定内存中的值读入缓冲区
                WinAPI.ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero);
                ////关闭操作
                //WinAPI.CloseHandle(hProcess);

                byte[] res = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    res[i] = Marshal.ReadByte(byteAddress + i);
                }

                return ByteToFloat(res);
            }
            catch
            {
                return 0;
            }
        }

        //读长整数型
        public static long ReadMemoryValue(long baseAddress, IntPtr hProcess)
        {
            try
            {
                string temp = ((IntPtr)baseAddress).ToString("x");
                byte[] buffer = new byte[4];
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                WinAPI.ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero);
                //WinAPI.CloseHandle(hProcess);
                string ss = ((IntPtr)baseAddress).ToString("x");
                return Marshal.ReadInt32(byteAddress);
            }
            catch
            {
                return 0;
            }
        }

        //写内存整数型
        public static void WriteMemoryInt(long baseAddress, IntPtr hProcess, int value)
        {
            bool flag;
            int[] Data = new int[] { value };
            flag = WinAPI.WriteProcessMemory(hProcess, (IntPtr)baseAddress, Data, 4, IntPtr.Zero);
            //WinAPI.CloseHandle(hProcess);
        }
        //写内存整数型
        public static void WriteMemoryInt(int baseAddress, IntPtr hProcess, int value)
        {
            bool flag;
            int[] Data = new int[] { value };
            flag = WinAPI.WriteProcessMemory(hProcess, (IntPtr)baseAddress, Data, 4, IntPtr.Zero);
            //WinAPI.CloseHandle(hProcess);
        }

        //写内存字节型  
        public static void WriteMemoryByte(int baseAddress, IntPtr hProcess, byte[] value)
        {
            bool flag;
            flag = WinAPI.WriteProcessMemory(hProcess, (IntPtr)baseAddress, value, value.Length, IntPtr.Zero);
            //WinAPI.CloseHandle(hProcess);
        }

        //写内存浮点型 
        public static void WriteMemoryFloat(int baseAddress, IntPtr hProcess, float value_f)
        {
            byte[] value = FloatToByte(value_f);
            bool flag;
            flag = WinAPI.WriteProcessMemory(hProcess, (IntPtr)baseAddress, value, value.Length, IntPtr.Zero);
            //WinAPI.CloseHandle(hProcess);
        }
        //关闭句柄
        public static void CloseHandle(IntPtr hProcess)
        {
            WinAPI.CloseHandle(hProcess);
        }

        //寻找地址
        public static List<uint> FindData(IntPtr hProcess, uint beginAddr, uint endAddr, String data)
        {
            List<uint> result = new List<uint>();
            data = data.ToUpper();
            data = data.Replace(" ", "");
            data = data.Replace("??", @"\S{2}");
            Console.WriteLine(data);
            uint len = (endAddr - beginAddr) / 2;
            int pageSize = 0x8000;
            int dlen = data.Length;
            int count = (int)(len / pageSize + 1);
            uint slen = (uint)(pageSize + dlen);
            try
            {
                for (int i = 0; i < count; i++)
                {
                    byte[] buffer = new byte[pageSize + dlen];
                    IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                    uint b = (uint)(beginAddr + i * pageSize);
                    WinAPI.ReadProcessMemory(hProcess, (IntPtr)b, byteAddress, slen, IntPtr.Zero);
                    String hex = BitConverter.ToString(buffer, 0).Replace("-", "").ToUpper();
                    Regex regex = new Regex(data, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection matchs = regex.Matches(hex);
                    foreach (Match m in matchs)
                    {
                        uint r = (uint)(m.Index / 2 + m.Index % 2 + b);
                        //Console.WriteLine(r.ToString("X8"));
                        result.Add(r);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return result;
            }
            finally
            {
                //WinAPI.CloseHandle(hProcess);
            }
        }

        //字节数组变为浮点数值
        private static float ByteToFloat(byte[] data)
        {
            return BitConverter.ToSingle(data, 0);
        }
        //浮点数值变为字节数组
        private static byte[] FloatToByte(float data)
        {
            return BitConverter.GetBytes(data);
        }

        //获得目标进程的模块地址
        public static uint GetProcessModuleHandle(uint pid, string moduleName)
        {
            uint address = WinAPI.GetProcessModuleHandle(pid, moduleName);
            int count = 0;
            while (address < 5 && count < 1000)
            {
                address = WinAPI.GetProcessModuleHandle(pid, moduleName);
                count++;
            }
            return address;
        }

        #endregion

        #region Windows窗口相关方法

        /// 设置窗体具有鼠标穿透效果
        public static void SetPenetrate(IntPtr Handle)
        {
            WinAPI.GetWindowLong(Handle, WinAPI.GWL_EXSTYLE);
            WinAPI.SetWindowLong(Handle, WinAPI.GWL_EXSTYLE, WinAPI.WS_EX_TRANSPARENT | WinAPI.WS_EX_LAYERED);
            WinAPI.SetLayeredWindowAttributes(Handle, 0, 100, WinAPI.LWA_COLORKEY);
        }

        #endregion

        #region 调用相关程序方法
        public static void StartOtherApp(string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.Start();

        }
        #endregion

    }
}
