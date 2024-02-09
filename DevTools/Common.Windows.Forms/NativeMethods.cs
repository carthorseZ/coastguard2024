namespace DevTools.Windows.Forms
{
    using System;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        internal const uint BCM_SETSHIELD = 0x160c;
        internal const uint ECM_FIRST = 0x1500;
        internal const uint EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet=CharSet.Unicode)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, bool lParam);
        [DllImport("user32.dll", CharSet=CharSet.Unicode)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, string lParam);
    }
}

