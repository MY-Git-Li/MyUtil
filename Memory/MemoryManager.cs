using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MyUtil.Memory
{
    class MemoryManager
    {

        public static Process m_Process;
        public static IntPtr m_pWindowHandle;
        public static IntPtr m_pProcessHandle;

        public struct WindowData
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }

        public MemoryManager(string ProcessName)
        {
            m_Process = Process.GetProcessesByName(ProcessName)[0];
            m_pWindowHandle = m_Process.MainWindowHandle;
            m_pProcessHandle = CheatTools.GetProcessHandle(m_Process.Id);
            SetForegroundWindow();
        }

         ~MemoryManager()
        {
            CloseHandle();
        }
            
        public static void CloseHandle()
        {
            CloseHandle(m_pProcessHandle);
        }

        public int GetModule(string moduleName)
        {
            foreach (ProcessModule module in m_Process.Modules)
            {
                if (module.ModuleName == (moduleName))
                {
                    return (int)module.BaseAddress;
                }
            }

            return 0;
        }

        public void SetForegroundWindow()
        {
            SetForegroundWindow(m_pWindowHandle);
        }

        public WindowData GetGameWindowData()
        {
            // 获取指定窗口句柄的窗口矩形数据和客户区矩形数据
            GetWindowRect(m_pWindowHandle, out RECT windowRect);
            GetClientRect(m_pWindowHandle, out RECT clientRect);

            // 计算窗口区的宽和高
            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            // 处理窗口最小化
            if (windowRect.Left < 0)
            {
                return new WindowData()
                {
                    Left = 0,
                    Top = 0,
                    Width = 1,
                    Height = 1
                };
            }

            // 计算客户区的宽和高
            int clientWidth = clientRect.Right - clientRect.Left;
            int clientHeight = clientRect.Bottom - clientRect.Top;

            // 计算边框
            int borderWidth = (windowWidth - clientWidth) / 2;
            int borderHeight = windowHeight - clientHeight - borderWidth;

            return new WindowData()
            {
                Left = windowRect.Left += borderWidth,
                Top = windowRect.Top += borderHeight,
                Width = clientWidth,
                Height = clientHeight
            };
        }


        public T ReadMemory<T>(int address) where T : struct
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            ReadProcessMemory(m_pProcessHandle, address, buffer, buffer.Length, out _);
            return ByteArrayToStructure<T>(buffer);
        }

        public void WriteMemory<T>(int address, object Value) where T : struct
        {
            byte[] buffer = StructureToByteArray(Value);
            WriteProcessMemory(m_pProcessHandle, address, buffer, buffer.Length, out _);
        }

        public float[] ReadMatrix<T>(int address, int MatrixSize) where T : struct
        {
            int ByteSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[ByteSize * MatrixSize];
            ReadProcessMemory(m_pProcessHandle, address, buffer, buffer.Length, out _);
            return ConvertToFloatArray(buffer);
        }

        public string ReadString(int address, int size)
        {
            byte[] buffer = new byte[size];
            ReadProcessMemory(m_pProcessHandle, address, buffer, size, out _);

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                {
                    byte[] _buffer = new byte[i];
                    Buffer.BlockCopy(buffer, 0, _buffer, 0, i);
                    return Encoding.ASCII.GetString(_buffer);
                }
            }

            return Encoding.ASCII.GetString(buffer);
        }

        #region Conversion
        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        private static byte[] StructureToByteArray(object obj)
        {
            int length = Marshal.SizeOf(obj);
            byte[] array = new byte[length];
            IntPtr pointer = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(obj, pointer, true);
            Marshal.Copy(pointer, array, 0, length);
            Marshal.FreeHGlobal(pointer);
            return array;
        }

        private static float[] ConvertToFloatArray(byte[] bytes)
        {
            if (bytes.Length % 4 != 0)
            {
                throw new ArgumentException();
            }

            float[] floats = new float[bytes.Length / 4];
            for (int i = 0; i < floats.Length; i++)
            {
                floats[i] = BitConverter.ToSingle(bytes, i * 4);
            }
            return floats;
        }
        #endregion

        #region Dll导入

        public const int PROCESS_VM_READ = 0x0010;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const int PROCESS_VM_OPERATION = 0x0008;
        public const int PAGE_READWRITE = 0x0004;
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, [In, Out] byte[] lpBuffer, int nsize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, [In, Out] byte[] lpBuffer, int nsize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int vKey);
        #endregion
    }
}
