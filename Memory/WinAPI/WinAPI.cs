﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MyUtil.Memory.WinApi
{
    public class WinAPI
    {
        #region MemoryAPI
        //从指定内存中读取字节集数据
        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, IntPtr lpNumberOfBytesRead);

        //从指定内存中写入字节集数据
        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, int[] lpBuffer, int nSize, IntPtr lpNumberOfBytesWritten);

        //写内存  
        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory
        (
            IntPtr lpProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            IntPtr BytesWrite
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx
        (
            //修改内存的句柄  
            IntPtr hProcess,
            //要修改的起始地址  
            IntPtr lpAddress,
            //页区域大小  
            int dwSize,
            //访问方式  
            int flNewProtect,
            //用于保护改变前的保护属性  
            ref IntPtr lpflOldProtect
        );

        #endregion

        #region ProcessAPI

        #region 进程
        //打开一个已存在的进程对象，并返回进程的句柄
        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        //关闭一个内核对象。其中包括文件、文件映射、进程、线程、安全和同步对象等。
        [DllImport("kernel32.dll")]
        public static extern void CloseHandle(IntPtr hObject);

        //GetModuleHandle是获取应用程序或动态链接库的模块句柄。  
        [DllImport("kernel32")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        //函数通过获取进程信息为指定的进程、进程使用的堆[HEAP]、模块[MODULE]、线程建立一个快照.
        [DllImport("KERNEL32.DLL ")]
        public static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);

        //unsafe  public static IntPtr GetProcessModuleHandle(uint pid, char* moduleName)
        //{
        //    ManageClass.ManageClass manage = new ManageClass.ManageClass();
        //    return (IntPtr)manage.GetProcessModuleHandle(pid, moduleName);
        //}

        [DllImport("MyAPI.dll")]
        public static extern uint GetProcessModuleHandle(uint pid, [MarshalAs(UnmanagedType.LPWStr)] string moduleName);

        #endregion

        #region 钩子

        /// <summary>
        /// 消息类型
        /// 作为SendMessage和PostMessage的参数
        /// </summary>
        public enum MsgType : uint
        {
            WM_KEYFIRST = 0x0100,

            //Msg参数常量值：
            /// <summary>
            /// 按下一个键
            /// </summary>
            WM_KEYDOWN = 0x0100,
            /// <summary>
            /// 释放一个键
            /// </summary>
            WM_KEYUP = 0x0101,
            /// <summary>
            /// 按下某键，并已发出WM_KEYDOWN， WM_KEYUP消息
            /// </summary>
            WM_CHAR = 0x102,
            /// <summary>
            /// 当用translatemessage函数翻译WM_KEYUP消息时发送此消息给拥有焦点的窗口
            /// </summary>
            WM_DEADCHAR = 0x103,
            /// <summary>
            /// 当用户按住ALT键同时按下其它键时提交此消息给拥有焦点的窗口
            /// </summary>
            WM_SYSKEYDOWN = 0x104,
            /// <summary>
            /// 当用户释放一个键同时ALT 键还按着时提交此消息给拥有焦点的窗口
            /// </summary>
            WM_SYSKEYUP = 0x105,
            /// <summary>
            /// 当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后提交此消息给拥有焦点的窗口
            /// </summary>
            WM_SYSCHAR = 0x106,
            /// <summary>
            /// 当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后发送此消息给拥有焦点的窗口
            /// </summary>
            WM_SYSDEADCHAR = 0x107,
            /// <summary>
            /// 在一个对话框程序被显示前发送此消息给它，通常用此消息初始化控件和执行其它任务
            /// </summary>
            WM_INITDIALOG = 0x110,
            /// <summary>
            /// 当用户选择一条菜单命令项或当某个控件发送一条消息给它的父窗口，一个快捷键被翻译
            /// </summary>
            WM_COMMAND = 0x111,
            /// <summary>
            /// 当用户选择窗口菜单的一条命令或//当用户选择最大化或最小化时那个窗口会收到此消息
            /// </summary>
            WM_SYSCOMMAND = 0x112,
            /// <summary>
            /// 发生了定时器事件
            /// </summary>
            WM_TIMER = 0x113,
            /// <summary>
            /// 当一个窗口标准水平滚动条产生一个滚动事件时发送此消息给那个窗口，也发送给拥有它的控件
            /// </summary>
            WM_HSCROLL = 0x114,
            /// <summary>
            /// 当一个窗口标准垂直滚动条产生一个滚动事件时发送此消息给那个窗口也，发送给拥有它的控件
            /// </summary>
            WM_VSCROLL = 0x115,
            /// <summary>
            /// 当一个菜单将要被激活时发送此消息，它发生在用户菜单条中的某项或按下某个菜单键，它允许程序在显示前更改菜单
            /// </summary>
            WM_INITMENU = 0x116,
            /// <summary>
            /// 当一个下拉菜单或子菜单将要被激活时发送此消息，它允许程序在它显示前更改菜单，而不要改变全部
            /// </summary>
            WM_INITMENUPOPUP = 0x117,
            /// <summary>
            /// 当用户选择一条菜单项时发送此消息给菜单的所有者（一般是窗口）
            /// </summary>
            WM_MENUSELECT = 0x11F,
            /// <summary>
            /// 当菜单已被激活用户按下了某个键（不同于加速键），发送此消息给菜单的所有者
            /// </summary>
            WM_MENUCHAR = 0x120,
            /// <summary>
            /// 当一个模态对话框或菜单进入空载状态时发送此消息给它的所有者，一个模态对话框或菜单进入空载状态就是在处理完一条或几条先前的消息后没有消息它的列队中等待
            /// </summary>
            WM_ENTERIDLE = 0x121,
            /// <summary>
            /// 在windows绘制消息框前发送此消息给消息框的所有者窗口，通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置消息框的文本和背景颜色
            /// </summary>
            WM_CTLCOLORMSGBOX = 0x132,
            /// <summary>
            /// 当一个编辑型控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置编辑框的文本和背景颜色
            /// </summary>
            WM_CTLCOLOREDIT = 0x133,

            /// <summary>
            /// 当一个列表框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置列表框的文本和背景颜色
            /// </summary>
            WM_CTLCOLORLISTBOX = 0x134,
            /// <summary>
            /// 当一个按钮控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置按纽的文本和背景颜色
            /// </summary>
            WM_CTLCOLORBTN = 0x135,
            /// <summary>
            /// 当一个对话框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置对话框的文本背景颜色
            /// </summary>
            WM_CTLCOLORDLG = 0x136,
            /// <summary>
            /// 当一个滚动条控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置滚动条的背景颜色
            /// </summary>
            WM_CTLCOLORSCROLLBAR = 0x137,
            /// <summary>
            /// 当一个静态控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以 通过使用给定的相关显示设备的句柄来设置静态控件的文本和背景颜色
            /// </summary>
            WM_CTLCOLORSTATIC = 0x138,
            /// <summary>
            /// 当鼠标轮子转动时发送此消息个当前有焦点的控件
            /// </summary>
            WM_MOUSEWHEEL = 0x20A,
            /// <summary>
            /// 双击鼠标中键
            /// </summary>
            WM_MBUTTONDBLCLK = 0x209,
            /// <summary>
            /// 释放鼠标中键
            /// </summary>
            WM_MBUTTONUP = 0x208,
            /// <summary>
            /// 移动鼠标时发生，同WM_MOUSEFIRST
            /// </summary>
            WM_MOUSEMOVE = 0x200,
            /// <summary>
            /// 按下鼠标左键
            /// </summary>
            WM_LBUTTONDOWN = 0x201,
            /// <summary>
            /// 释放鼠标左键
            /// </summary>
            WM_LBUTTONUP = 0x202,
            /// <summary>
            /// 双击鼠标左键
            /// </summary>
            WM_LBUTTONDBLCLK = 0x203,
            /// <summary>
            /// 按下鼠标右键
            /// </summary>
            WM_RBUTTONDOWN = 0x204,
            /// <summary>
            /// 释放鼠标右键
            /// </summary>
            WM_RBUTTONUP = 0x205,
            /// <summary>
            /// 双击鼠标右键
            /// </summary>
            WM_RBUTTONDBLCLK = 0x206,
            /// <summary>
            /// 按下鼠标中键
            /// </summary>
            WM_MBUTTONDOWN = 0x207,

            //WM_USER = 0x0400;
            //public static int MK_LBUTTON = 0x0001;
            //public static int MK_RBUTTON = 0x0002;
            //public static int MK_SHIFT = 0x0004;
            //public static int MK_CONTROL = 0x0008;
            //public static int MK_MBUTTON = 0x0010;
            //public static int MK_XBUTTON1 = 0x0020;
            //public static int MK_XBUTTON2 = 0x0040;
            /// <summary>
            /// 创建一个窗口
            /// </summary>
            WM_CREATE = 0x01,
            /// <summary>
            /// 当一个窗口被破坏时发送
            /// </summary>
            WM_DESTROY = 0x02,
            /// <summary>
            /// 移动一个窗口
            /// </summary>
            WM_MOVE = 0x03,
            /// <summary>
            /// 改变一个窗口的大小
            /// </summary>
            WM_SIZE = 0x05,
            /// <summary>
            /// 一个窗口被激活或失去激活状态
            /// </summary>
            WM_ACTIVATE = 0x06,
            /// <summary>
            /// 一个窗口获得焦点
            /// </summary>
            WM_SETFOCUS = 0x07,
            /// <summary>
            /// 一个窗口失去焦点
            /// </summary>
            WM_KILLFOCUS = 0x08,
            /// <summary>
            /// 一个窗口改变成Enable状态
            /// </summary>
            WM_ENABLE = 0x0A,
            /// <summary>
            /// 设置窗口是否能重画
            /// </summary>
            WM_SETREDRAW = 0x0B,
            /// <summary>
            /// 应用程序发送此消息来设置一个窗口的文本
            /// </summary>
            WM_SETTEXT = 0x0C,
            /// <summary>
            /// 应用程序发送此消息来复制对应窗口的文本到缓冲区
            /// </summary>
            WM_GETTEXT = 0x0D,
            /// <summary>
            /// 得到与一个窗口有关的文本的长度（不包含空字符）
            /// </summary>
            WM_GETTEXTLENGTH = 0x0E,
            /// <summary>
            /// 要求一个窗口重画自己
            /// </summary>
            WM_PAINT = 0x0F,
            /// <summary>
            /// 当一个窗口或应用程序要关闭时发送一个信号
            /// </summary>
            WM_CLOSE = 0x10,
            /// <summary>
            /// 当用户选择结束对话框或程序自己调用ExitWindows函数
            /// </summary>
            WM_QUERYENDSESSION = 0x11,
            /// <summary>
            /// 用来结束程序运行
            /// </summary>
            WM_QUIT = 0x12,
            /// <summary>
            /// 当用户窗口恢复以前的大小位置时，把此消息发送给某个图标
            /// </summary>
            WM_QUERYOPEN = 0x13,
            /// <summary>
            /// 当窗口背景必须被擦除时（例在窗口改变大小时）
            /// </summary>
            WM_ERASEBKGND = 0x14,
            /// <summary>
            /// 当系统颜色改变时，发送此消息给所有顶级窗口
            /// </summary>
            WM_SYSCOLORCHANGE = 0x15,
            /// <summary>
            /// 当系统进程发出WM_QUERYENDSESSION消息后，此消息发送给应用程序，通知它对话是否结束
            /// </summary>
            WM_ENDSESSION = 0x16,
            /// <summary>
            /// 当隐藏或显示窗口是发送此消息给这个窗口
            /// </summary>
            WM_SHOWWINDOW = 0x18,
            /// <summary>
            /// 发此消息给应用程序哪个窗口是激活的，哪个是非激活的
            /// </summary>
            WM_ACTIVATEAPP = 0x1C,
            /// <summary>
            /// 当系统的字体资源库变化时发送此消息给所有顶级窗口
            /// </summary>
            WM_FONTCHANGE = 0x1D,
            /// <summary>
            /// 当系统的时间变化时发送此消息给所有顶级窗口
            /// </summary>
            WM_TIMECHANGE = 0x1E,
            /// <summary>
            /// 发送此消息来取消某种正在进行的摸态（操作）
            /// </summary>
            WM_CANCELMODE = 0x1F,
            /// <summary>
            /// 如果鼠标引起光标在某个窗口中移动且鼠标输入没有被捕获时，就发消息给某个窗口
            /// </summary>
            WM_SETCURSOR = 0x20,
            /// <summary>
            /// 当光标在某个非激活的窗口中而用户正按着鼠标的某个键发送此消息给//当前窗口
            /// </summary>
            WM_MOUSEACTIVATE = 0x21,
            /// <summary>
            /// 发送此消息给MDI子窗口//当用户点击此窗口的标题栏，或//当窗口被激活，移动，改变大小
            /// </summary>
            WM_CHILDACTIVATE = 0x22,
            /// <summary>
            /// 此消息由基于计算机的训练程序发送，通过WH_JOURNALPALYBACK的hook程序分离出用户输入消息
            /// </summary>
            WM_QUEUESYNC = 0x23,
            /// <summary>
            /// 此消息发送给窗口当它将要改变大小或位置
            /// </summary>
            WM_GETMINMAXINFO = 0x24,
            /// <summary>
            /// 发送给最小化窗口当它图标将要被重画
            /// </summary>
            WM_PAINTICON = 0x26,
            /// <summary>
            /// 此消息发送给某个最小化窗口，仅//当它在画图标前它的背景必须被重画
            /// </summary>
            WM_ICONERASEBKGND = 0x27,
            /// <summary>
            /// 发送此消息给一个对话框程序去更改焦点位置
            /// </summary>
            WM_NEXTDLGCTL = 0x28,
            /// <summary>
            /// 每当打印管理列队增加或减少一条作业时发出此消息 
            /// </summary>
            WM_SPOOLERSTATUS = 0x2A,
            /// <summary>
            /// 当button，combobox，listbox，menu的可视外观改变时发送
            /// </summary>
            WM_DRAWITEM = 0x2B,
            /// <summary>
            /// 当button, combo box, list box, list view control, or menu item 被创建时
            /// </summary>
            WM_MEASUREITEM = 0x2C,
            /// <summary>
            /// 此消息有一个LBS_WANTKEYBOARDINPUT风格的发出给它的所有者来响应WM_KEYDOWN消息 
            /// </summary>
            WM_VKEYTOITEM = 0x2E,
            /// <summary>
            /// 此消息由一个LBS_WANTKEYBOARDINPUT风格的列表框发送给他的所有者来响应WM_CHAR消息 
            /// </summary>
            WM_CHARTOITEM = 0x2F,
            /// <summary>
            /// 当绘制文本时程序发送此消息得到控件要用的颜色
            /// </summary>
            WM_SETFONT = 0x30,
            /// <summary>
            /// 应用程序发送此消息得到当前控件绘制文本的字体
            /// </summary>
            WM_GETFONT = 0x31,
            /// <summary>
            /// 应用程序发送此消息让一个窗口与一个热键相关连 
            /// </summary>
            WM_SETHOTKEY = 0x32,
            /// <summary>
            /// 应用程序发送此消息来判断热键与某个窗口是否有关联
            /// </summary>
            WM_GETHOTKEY = 0x33,
            /// <summary>
            /// 此消息发送给最小化窗口，当此窗口将要被拖放而它的类中没有定义图标，应用程序能返回一个图标或光标的句柄，当用户拖放图标时系统显示这个图标或光标
            /// </summary>
            WM_QUERYDRAGICON = 0x37,
            /// <summary>
            /// 发送此消息来判定combobox或listbox新增加的项的相对位置
            /// </summary>
            WM_COMPAREITEM = 0x39,
            /// <summary>
            /// 显示内存已经很少了
            /// </summary>
            WM_COMPACTING = 0x41,
            /// <summary>
            /// 发送此消息给那个窗口的大小和位置将要被改变时，来调用setwindowpos函数或其它窗口管理函数
            /// </summary>
            WM_WINDOWPOSCHANGING = 0x46,
            /// <summary>
            /// 发送此消息给那个窗口的大小和位置已经被改变时，来调用setwindowpos函数或其它窗口管理函数
            /// </summary>
            WM_WINDOWPOSCHANGED = 0x47,
            /// <summary>
            /// 当系统将要进入暂停状态时发送此消息
            /// </summary>
            WM_POWER = 0x48,
            /// <summary>
            /// 当一个应用程序传递数据给另一个应用程序时发送此消息
            /// </summary>
            WM_COPYDATA = 0x4A,
            /// <summary>
            /// 当某个用户取消程序日志激活状态，提交此消息给程序
            /// </summary>
            WM_CANCELJOURNA = 0x4B,
            /// <summary>
            /// 当某个控件的某个事件已经发生或这个控件需要得到一些信息时，发送此消息给它的父窗口 
            /// </summary>
            WM_NOTIFY = 0x4E,
            /// <summary>
            /// 当用户选择某种输入语言，或输入语言的热键改变
            /// </summary>
            WM_INPUTLANGCHANGEREQUEST = 0x50,
            /// <summary>
            /// 当平台现场已经被改变后发送此消息给受影响的最顶级窗口
            /// </summary>
            WM_INPUTLANGCHANGE = 0x51,
            /// <summary>
            /// 当程序已经初始化windows帮助例程时发送此消息给应用程序
            /// </summary>
            WM_TCARD = 0x52,
            /// <summary>
            /// 此消息显示用户按下了F1，如果某个菜单是激活的，就发送此消息个此窗口关联的菜单，否则就发送给有焦点的窗口，如果//当前都没有焦点，就把此消息发送给//当前激活的窗口
            /// </summary>
            WM_HELP = 0x53,
            /// <summary>
            /// 当用户已经登入或退出后发送此消息给所有的窗口，//当用户登入或退出时系统更新用户的具体设置信息，在用户更新设置时系统马上发送此消息
            /// </summary>
            WM_USERCHANGED = 0x54,
            /// <summary>
            /// 公用控件，自定义控件和他们的父窗口通过此消息来判断控件是使用ANSI还是UNICODE结构
            /// </summary>
            WM_NOTIFYFORMAT = 0x55,
            /// <summary>
            /// 当用户某个窗口中点击了一下右键就发送此消息给这个窗口
            /// </summary>
            WM_CONTEXTMENU = 0x7B,
            /// <summary>
            /// 当调用SETWINDOWLONG函数将要改变一个或多个 窗口的风格时发送此消息给那个窗口
            /// </summary>
            WM_STYLECHANGING = 0x7C,
            /// <summary>
            /// 当调用SETWINDOWLONG函数一个或多个 窗口的风格后发送此消息给那个窗口
            /// </summary>
            WM_STYLECHANGED = 0x7D,
            /// <summary>
            /// 当显示器的分辨率改变后发送此消息给所有的窗口
            /// </summary>
            WM_DISPLAYCHANGE = 0x7E,
            /// <summary>
            /// 此消息发送给某个窗口来返回与某个窗口有关连的大图标或小图标的句柄
            /// </summary>
            WM_GETICON = 0x7F,
            /// <summary>
            /// 程序发送此消息让一个新的大图标或小图标与某个窗口关联
            /// </summary>
            WM_SETICON = 0x80,
            /// <summary>
            /// 当某个窗口第一次被创建时，此消息在WM_CREATE消息发送前发送
            /// </summary>
            WM_NCCREATE = 0x81,
            /// <summary>
            /// 此消息通知某个窗口，非客户区正在销毁 
            /// </summary>
            WM_NCDESTROY = 0x82,
            /// <summary>
            /// 当某个窗口的客户区域必须被核算时发送此消息
            /// </summary>
            WM_NCCALCSIZE = 0x83,
            /// <summary>
            /// 移动鼠标，按住或释放鼠标时发生
            /// </summary>
            WM_NCHITTEST = 0x84,
            /// <summary>
            /// 程序发送此消息给某个窗口当它（窗口）的框架必须被绘制时
            /// </summary>
            WM_NCPAINT = 0x85,
            /// <summary>
            /// 此消息发送给某个窗口仅当它的非客户区需要被改变来显示是激活还是非激活状态
            /// </summary>
            WM_NCACTIVATE = 0x86,
            /// <summary>
            /// 发送此消息给某个与对话框程序关联的控件，widdows控制方位键和TAB键使输入进入此控件通过应
            /// </summary>
            WM_GETDLGCODE = 0x87,
            /// <summary>
            /// 当光标在一个窗口的非客户区内移动时发送此消息给这个窗口 非客户区为：窗体的标题栏及窗 的边框体
            /// </summary>
            WM_NCMOUSEMOVE = 0xA0,
            /// <summary>
            /// 当光标在一个窗口的非客户区同时按下鼠标左键时提交此消息
            /// </summary>
            WM_NCLBUTTONDOWN = 0xA1,
            /// <summary>
            /// 当用户释放鼠标左键同时光标某个窗口在非客户区十发送此消息
            /// </summary>
            WM_NCLBUTTONUP = 0xA2,
            /// <summary>
            /// 当用户双击鼠标左键同时光标某个窗口在非客户区十发送此消息
            /// </summary>
            WM_NCLBUTTONDBLCLK = 0xA3,
            /// <summary>
            /// 当用户按下鼠标右键同时光标又在窗口的非客户区时发送此消息
            /// </summary>
            WM_NCRBUTTONDOWN = 0xA4,
            /// <summary>
            /// 当用户释放鼠标右键同时光标又在窗口的非客户区时发送此消息
            /// </summary>
            WM_NCRBUTTONUP = 0xA5,
            /// <summary>
            /// 当用户双击鼠标右键同时光标某个窗口在非客户区十发送此消息
            /// </summary>
            WM_NCRBUTTONDBLCLK = 0xA6,
            /// <summary>
            /// 当用户按下鼠标中键同时光标又在窗口的非客户区时发送此消息
            /// </summary>
            WM_NCMBUTTONDOWN = 0xA7,
            /// <summary>
            /// 当用户释放鼠标中键同时光标又在窗口的非客户区时发送此消息
            /// </summary>
            WM_NCMBUTTONUP = 0xA8,
            /// <summary>
            /// 当用户双击鼠标中键同时光标又在窗口的非客户区时发送此消息
            /// </summary>
            WM_NCMBUTTONDBLCLK = 0xA9
        }

        /// <summary>
        /// 设置的钩子类型
        /// </summary>
        public enum HookType : int
        {
            /// <summary>
            /// WH_MSGFILTER 和 WH_SYSMSGFILTER Hooks使我们可以监视菜单，滚动 
            ///条，消息框，对话框消息并且发现用户使用ALT+TAB or ALT+ESC 组合键切换窗口。 
            ///WH_MSGFILTER Hook只能监视传递到菜单，滚动条，消息框的消息，以及传递到通 
            ///过安装了Hook子过程的应用程序建立的对话框的消息。WH_SYSMSGFILTER Hook 
            ///监视所有应用程序消息。 
            /// 
            ///WH_MSGFILTER 和 WH_SYSMSGFILTER Hooks使我们可以在模式循环期间 
            ///过滤消息，这等价于在主消息循环中过滤消息。 
            ///    
            ///通过调用CallMsgFilter function可以直接的调用WH_MSGFILTER Hook。通过使用这 
            ///个函数，应用程序能够在模式循环期间使用相同的代码去过滤消息，如同在主消息循 
            ///环里一样
            /// </summary>
            WH_MSGFILTER = -1,
            /// <summary>
            /// WH_JOURNALRECORD Hook用来监视和记录输入事件。典型的，可以使用这 
            ///个Hook记录连续的鼠标和键盘事件，然后通过使用WH_JOURNALPLAYBACK Hook 
            ///来回放。WH_JOURNALRECORD Hook是全局Hook，它不能象线程特定Hook一样 
            ///使用。WH_JOURNALRECORD是system-wide local hooks，它们不会被注射到任何行 
            ///程地址空间
            /// </summary>
            WH_JOURNALRECORD = 0,
            /// <summary>
            /// WH_JOURNALPLAYBACK Hook使应用程序可以插入消息到系统消息队列。可 
            ///以使用这个Hook回放通过使用WH_JOURNALRECORD Hook记录下来的连续的鼠 
            ///标和键盘事件。只要WH_JOURNALPLAYBACK Hook已经安装，正常的鼠标和键盘 
            ///事件就是无效的。WH_JOURNALPLAYBACK Hook是全局Hook，它不能象线程特定 
            ///Hook一样使用。WH_JOURNALPLAYBACK Hook返回超时值，这个值告诉系统在处 
            ///理来自回放Hook当前消息之前需要等待多长时间（毫秒）。这就使Hook可以控制实 
            ///时事件的回放。WH_JOURNALPLAYBACK是system-wide local hooks，它们不会被 
            ///注射到任何行程地址空间
            /// </summary>
            WH_JOURNALPLAYBACK = 1,
            /// <summary>
            /// 在应用程序中，WH_KEYBOARD Hook用来监视WM_KEYDOWN and  
            ///WM_KEYUP消息，这些消息通过GetMessage or PeekMessage function返回。可以使 
            ///用这个Hook来监视输入到消息队列中的键盘消息
            /// </summary>
            WH_KEYBOARD = 2,
            /// <summary>
            /// 应用程序使用WH_GETMESSAGE Hook来监视从GetMessage or PeekMessage函 
            ///数返回的消息。你可以使用WH_GETMESSAGE Hook去监视鼠标和键盘输入，以及 
            ///其它发送到消息队列中的消息
            /// </summary>
            WH_GETMESSAGE = 3,
            /// <summary>
            /// 监视发送到窗口过程的消息，系统在消息发送到接收窗口过程之前调用
            /// </summary>
            WH_CALLWNDPROC = 4,
            /// <summary>
            /// 在以下事件之前，系统都会调用WH_CBT Hook子过程，这些事件包括： 
            ///1. 激活，建立，销毁，最小化，最大化，移动，改变尺寸等窗口事件； 
            ///2. 完成系统指令； 
            ///3. 来自系统消息队列中的移动鼠标，键盘事件； 
            ///4. 设置输入焦点事件； 
            ///5. 同步系统消息队列事件。
            ///Hook子过程的返回值确定系统是否允许或者防止这些操作中的一个
            /// </summary>
            WH_CBT = 5,
            /// <summary>
            /// WH_MSGFILTER 和 WH_SYSMSGFILTER Hooks使我们可以监视菜单，滚动 
            ///条，消息框，对话框消息并且发现用户使用ALT+TAB or ALT+ESC 组合键切换窗口。 
            ///WH_MSGFILTER Hook只能监视传递到菜单，滚动条，消息框的消息，以及传递到通 
            ///过安装了Hook子过程的应用程序建立的对话框的消息。WH_SYSMSGFILTER Hook 
            ///监视所有应用程序消息。 
            /// 
            ///WH_MSGFILTER 和 WH_SYSMSGFILTER Hooks使我们可以在模式循环期间 
            ///过滤消息，这等价于在主消息循环中过滤消息。 
            ///    
            ///通过调用CallMsgFilter function可以直接的调用WH_MSGFILTER Hook。通过使用这 
            ///个函数，应用程序能够在模式循环期间使用相同的代码去过滤消息，如同在主消息循 
            ///环里一样
            /// </summary>
            WH_SYSMSGFILTER = 6,
            /// <summary>
            /// WH_MOUSE Hook监视从GetMessage 或者 PeekMessage 函数返回的鼠标消息。 
            ///使用这个Hook监视输入到消息队列中的鼠标消息
            /// </summary>
            WH_MOUSE = 7,
            /// <summary>
            /// 当调用GetMessage 或 PeekMessage 来从消息队列种查询非鼠标、键盘消息时
            /// </summary>
            WH_HARDWARE = 8,
            /// <summary>
            /// 在系统调用系统中与其它Hook关联的Hook子过程之前，系统会调用 
            ///WH_DEBUG Hook子过程。你可以使用这个Hook来决定是否允许系统调用与其它 
            ///Hook关联的Hook子过程
            /// </summary>
            WH_DEBUG = 9,
            /// <summary>
            /// 外壳应用程序可以使用WH_SHELL Hook去接收重要的通知。当外壳应用程序是 
            ///激活的并且当顶层窗口建立或者销毁时，系统调用WH_SHELL Hook子过程。 
            ///WH_SHELL 共有５钟情况： 
            ///1. 只要有个top-level、unowned 窗口被产生、起作用、或是被摧毁； 
            ///2. 当Taskbar需要重画某个按钮； 
            ///3. 当系统需要显示关于Taskbar的一个程序的最小化形式； 
            ///4. 当目前的键盘布局状态改变； 
            ///5. 当使用者按Ctrl+Esc去执行Task Manager（或相同级别的程序）。 
            ///
            ///按照惯例，外壳应用程序都不接收WH_SHELL消息。所以，在应用程序能够接 
            ///收WH_SHELL消息之前，应用程序必须调用SystemParametersInfo function注册它自 
            ///己
            /// </summary>
            WH_SHELL = 10,
            /// <summary>
            /// 当应用程序的前台线程处于空闲状态时，可以使用WH_FOREGROUNDIDLE  
            ///Hook执行低优先级的任务。当应用程序的前台线程大概要变成空闲状态时，系统就 
            ///会调用WH_FOREGROUNDIDLE Hook子过程
            /// </summary>
            WH_FOREGROUNDIDLE = 11,
            /// <summary>
            /// 监视发送到窗口过程的消息，系统在消息发送到接收窗口过程之后调用
            /// </summary>
            WH_CALLWNDPROCRET = 12,
            /// <summary>
            /// 监视输入到线程消息队列中的键盘消息
            /// </summary>
            WH_KEYBOARD_LL = 13,
            /// <summary>
            /// 监视输入到线程消息队列中的鼠标消息
            /// </summary>
            WH_MOUSE_LL = 14
        }

        /// <summary>
        /// 声明鼠标钩子的封送结构类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class MOUSEHOOKSTRUCT
        {

            /// <summary>
            /// POINT结构对象，保存鼠标在屏幕上的x,y坐标
            /// </summary>
            public Point pt;
            /// <summary>
            /// 接收到鼠标消息的窗口的句柄
            /// </summary>
            public IntPtr hWnd;
            /// <summary>
            /// hit-test值，详细描述参见WM_NCHITTEST消息
            /// </summary>
            public int wHitTestCode;
            /// <summary>
            /// 指定与本消息联系的额外消息
            /// </summary>
            public int dwExtraInfo;
        }
        /// <summary>
        /// 键盘Hook结构函数
        /// 即钩子发挥作用时能够得到的一些参数
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            /// <summary>
            /// 虚拟按键码(1--254)
            /// </summary>
            public int vkCode;
            /// <summary>
            /// 硬件按键扫描码
            /// </summary>
            public int scanCode;
            /// <summary>
            /// 键按下：128 抬起：0
            /// </summary>
            public int flags;
            /// <summary>
            /// 消息时间戳间
            /// </summary>
            public int time;
            /// <summary>
            /// 额外信息
            /// </summary>
            public int dwExtraInfo;
        }

        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(
            HookType idHook,//钩子类型，此处用整形的枚举表示
            HookProc lpfn,//钩子发挥作用时的回调函数
            IntPtr hInstance,//应用程序实例的模块句柄(一般来说是你钩子回调函数所在的应用程序实例模块句柄)
            int threadId //与安装的钩子子程相关联的线程的标识符
            );
        #endregion

        #endregion

        #region WindowsAPI

        #region Windows常量
        public const uint WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);
        public const int LWA_COLORKEY = 1;
        public const int LWA_ALPHA = 2;

        #endregion

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern uint SetWindowLong(
        IntPtr hwnd,
        int nIndex,
        uint dwNewLong
        );

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong(
        IntPtr hwnd,
        int nIndex
        );

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern int SetLayeredWindowAttributes(
        IntPtr hwnd,
        int crKey,
        int bAlpha,
        int dwFlags
        );

        //取窗口句柄 FindWindow 根据窗体标题查找窗口句柄（支持模糊匹配）
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess")]
        public static extern int GetCurrentProcess();

        [DllImport("User32.dll", EntryPoint = "UpdateWindow")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        

        //该函数可以映射一个unicode字符串到一个多字节字符串
        [DllImport("kernel32.dll", EntryPoint = "WideCharToMultiByte")]
        public static extern int WideCharToMultiByte(
            int CodePage, //指定执行转换的代码页
            int dwFlags, //允许你进行额外的控制，它会影响使用了读音符号（比如重音）的字符
            int lpWideCharStr, //指定要转换为宽字节字符串的缓冲区
            int cchWideChar, //指定由参数lpWideCharStr指向的缓冲区的字符个数
            int lpMultiByteStr, //指向接收被转换字符串的缓冲区
            int cchMultiByte, //指定由参数lpMultiByteStr指向的缓冲区最大值
            int lpDefaultChar, //遇到一个不能转换的宽字符，函数便会使用pDefaultChar参数指向的字符
            int pfUsedDefaultChar //至少有一个字符不能转换为其多字节形式，函数就会把这个变量设为TRUE
            );

        //该函数映射一个字符串到一个宽字符（unicode）的字符
        [DllImport("kernel32.dll", EntryPoint = "MultiByteToWideChar")]
        public static extern int MultiByteToWideChar(
            int CodePage,
            int dwFlags,
            int lpMultiByteStr,
            int cchMultiByte,
            int lpWideCharStr,
            int cchWideChar
            );

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        //此处主要用来让窗口置于最前(SetWindowPos(this.Handle,-1,0,0,0,0,0x4000|0x0001|0x0002);)
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd,
        int hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        int uFlags
        );
        #endregion

        //// wchar_t to string
        //void Wchar_tToString(std::string& szDst, wchar_t* wchar)
        //{
        //    wchar_t* wText = wchar;
        //    DWORD dwNum = WideCharToMultiByte(CP_OEMCP, NULL, wText, -1, NULL, 0, NULL, FALSE);//WideCharToMultiByte的运用
        //    char* psText;  // psText为char*的临时数组，作为赋值给std::string的中间变量
        //    psText = new char[dwNum];
        //    WideCharToMultiByte(CP_OEMCP, NULL, wText, -1, psText, dwNum, NULL, FALSE);//WideCharToMultiByte的再次运用
        //    szDst = psText;// std::string赋值
        //    delete[] psText;// psText的清除
        //}

        //// string to wstring
        //void StringToWstring(std::wstring& szDst, std::string str)
        //{
        //    std::string temp = str;
        //    int len = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)temp.c_str(), -1, NULL, 0);
        //    wchar_t* wszUtf8 = new wchar_t[len + 1];
        //    memset(wszUtf8, 0, len * 2 + 2);
        //    MultiByteToWideChar(CP_ACP, 0, (LPCSTR)temp.c_str(), -1, (LPWSTR)wszUtf8, len);
        //    szDst = wszUtf8;
        //    std::wstring r = wszUtf8;
        //    delete[] wszUtf8;
        //}


        //class MODULEENTRY32
        //{
        //    int dwSize;
        //    int th32ModuleID;
        //    int th32ProcessID;
        //    int GlblcntUsage;
        //    int ProccntUsage;
        //    int modBaseAddr;
        //    int modBaseSize;
        //    int hModule;
        //    char[] szModule=new char[256];
        //    char[] szExePath=new char[260];
        //}




        //int GetProcessModuleHandle(int pid,  TCHAR* moduleName)
        //{
        //    MODULEENTRY32 moduleEntry = new MODULEENTRY32();
        //    IntPtr handle;
        //    handle = CreateToolhelp32Snapshot(0x00000008, pid); //  获取进程快照中包含在th32ProcessID中指定的进程的所有的模块。
        //    if (handle==(IntPtr)0)
        //    {
        //        CloseHandle(handle);
        //        return 0;
        //    }
        //   // ZeroMemory(&moduleEntry, sizeof(MODULEENTRY32));
        //    moduleEntry.dwSize = sizeof(MODULEENTRY32);
        //    if (!Module32First(handle, &moduleEntry))
        //    {
        //        CloseHandle(handle);
        //        return NULL;
        //    }
        //    do
        //    {
        //        if (lstrcmpi(moduleEntry.szModule, moduleName) == 0)
        //        {
        //            return (DWORD)moduleEntry.hModule;
        //        }
        //    } while (Module32Next(handle, &moduleEntry));
        //    CloseHandle(handle);
        //    return 0;
        //}
    }
}
