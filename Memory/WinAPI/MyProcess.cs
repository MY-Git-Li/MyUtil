using System;
using IO = System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace MyUtil.Memory.WinApi
{
    
    [StructLayout(LayoutKind.Sequential)]
    public struct Module
    {
        [DllImportAttribute("kernel32.dll")]
        static extern int FreeLibrary(int hLibModule);

        [DllImportAttribute("kernel32.dll")]
        static extern void FreeLibraryAndExitThread(int hLibModule, uint dwExitCode);

        public string Name;
        public string Path;
        public string Type;
        public int hModule;
        public string Description;

        public bool FreeModule()
        {
            return FreeLibrary(this.hModule) != 0;
        }
        public void FreeModuleAndThread(uint dwExitCode)
        {
            FreeLibraryAndExitThread(this.hModule, dwExitCode);
        }
    }
    public partial class MyProcess
    {
        [StructLayout(LayoutKind.Sequential)]
        struct LPMODULEENTRY32
        {
            public int size;
            public int mid;
            public int pid;
            public int gusage;
            public int pusage;
            public int bases;
            public int bsize;
            public int hModule;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] mFile;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] mPath;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public int hIcon;
            public int iIcon;
            public int dwAttributes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] szDisplayName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] szTypeName;
        }

        [DllImportAttribute("user32.dll")]
        static extern int GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId);

        [DllImportAttribute("kernel32.dll")]
        static extern int CreateToolhelp32Snapshot(int Falge, int dwProcessId);

        [DllImportAttribute("kernel32.dll")]
        static extern int Module32First(int hSnapshot, ref LPMODULEENTRY32 lpme);

        [DllImportAttribute("kernel32.dll")]
        static extern int Module32Next(int hSnapshot, ref LPMODULEENTRY32 lpme);

        [DllImportAttribute("kernel32.dll")]
        static extern int GetCurrentProcessId();

        [DllImportAttribute("kernel32.dll", EntryPoint = "GetModuleHandleA")]
        static extern int GetModuleHandle(string lpModuleName);

        [DllImportAttribute("ntdll.dll")]
        static extern int RtlAdjustPrivilege(int s, int t, int hprocess, ref int ret);

        [DllImportAttribute("psapi.dll")]
        static extern int GetModuleFileNameEx(int hProcess, int hModule, StringBuilder lpFileName, int nSize);

        [DllImportAttribute("kernel32.dll", EntryPoint = "OpenProcess")]
        static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImportAttribute("kernel32.dll", EntryPoint = "ExitProcess")]
        static extern void ExitProcess_(int uExitCode);

        [DllImportAttribute("kernel32.dll", EntryPoint = "TerminateProcess")]
        static extern int TerminateProcess_(int hProcess, int uExitCode);

        [DllImportAttribute("shell32.dll")]
        static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);

    }
    public partial class MyProcess
    {
        int _Id, _hPrcoess;

        const int PROCESS_VM_READ = 0x10, PROCESS_ALL_ACCESS = 0x1F0FFF, PROCESS_QUERY_INFORMATION = 0x400;

        public int TerminateProcess(int hProcess, int uExitCode)
        {
            return TerminateProcess_(hProcess, uExitCode);
        }

        public void ExitProcess(int uExitCode)
        {
            ExitProcess_(uExitCode);
        }

        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this._Id = value;
            }
        }
        public int hProcess
        {
            get
            {
                return this._hPrcoess;
            }
            set
            {
                this._hPrcoess = value;
            }
        }
        public int hInstance
        {
            get
            {
                return GetModuleHandle(null);
            }
        }
        public bool Open(bool ReadOnly)
        {
            this.hProcess = 0;
            this.hProcess = ReadOnly ? OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, 1, this.Id) : OpenProcess(PROCESS_ALL_ACCESS, 1, this.Id);
            return this.hProcess != 0;
        }
        public int CurrentProcessId
        {
            get { return GetCurrentProcessId(); }
        }
        public int AdjustPrivilege(int Mode)
        {
            var ret = 0;
            RtlAdjustPrivilege(Mode, 0, this.hProcess, ref ret);
            return ret;
        }
        public List<Module> Modules
        {
            get
            {
                var ret = new List<Module>();
                var hSnapshot = CreateToolhelp32Snapshot(8, this.Id);
                if (hSnapshot == 0)
                {
                    return ret;
                }
                var lpMe = new LPMODULEENTRY32();
                lpMe.size = 1024;
                var h = Module32First(hSnapshot, ref lpMe);
                var mod = new Module();
                var sh = new SHFILEINFO();
                var cod = Encoding.Default;
                while (h != 0)
                {
                    mod.Path = cod.GetString(lpMe.mPath, 16, 240); // W2A
                    mod.Name = cod.GetString(lpMe.mFile, 16, 240);
                    SHGetFileInfo(mod.Path, 128, ref sh, 692, 1 | 16 | 256 | 512 | 1024);
                    mod.Type = cod.GetString(sh.szTypeName, 8, 72);
                    mod.Description = FileVersionInfo.GetVersionInfo(mod.Path.Split('\0')[0]).FileDescription;
                    mod.hModule = GetModuleHandle(mod.Name);
                    ret.Add(mod);
                    h = Module32Next(hSnapshot, ref lpMe);
                }
                return ret;
            }
        }
        public string Path
        {
            get
            {
                var sb = new StringBuilder(255);
                GetModuleFileNameEx(this.hProcess, 0, sb, sb.Capacity);
                return sb.ToString();
            }
        }
        public string Name
        {
            get
            {
                return IO.Path.GetFileNameWithoutExtension(this.Path);
            }
        }
    }
    
    
}
