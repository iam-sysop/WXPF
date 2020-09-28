using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Win32;

namespace WinXPresentationFoundation.NTInterop
{
    /// <summary>
    /// WinXPresentationFoundation WndProc Handler/Adapter
    /// </summary>
    public static partial class Win32
    {

        [DllImport("user32", CharSet = CharSet.Auto, EntryPoint = "PostMessage", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern bool _PostWindowMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32", CharSet = CharSet.Auto, EntryPoint = "RegisterWindowMessage", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern int _RegisterWindowMessage(string message);


        /// <summary>
        /// RegisterWindowMessage - Registers a native WindowMessage STRING for native Win32 inter-window signalling
        /// </summary>
        /// <param name="Message"></param>
        /// <returns>Integer assigned to newly registered WindowMessage</returns>
        public static int RegisterWindowMessage(string Message)
        {
            return _RegisterWindowMessage(Message);
        }

        /// <summary>
        /// PostWindowMessage - Post a Native Win32 MindowMessage call (broadcast)
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static bool PostWindowMessage(int Message)
        {
            return PostWindowMessage((IntPtr)Enums.WINDOW_MESSAGE_TYPE.HWND_BROADCAST, Message);
        }

        /// <summary>
        /// PostWindowMessage - Post a Native Win32 WindowMessage call against a window handle.
        /// </summary>
        /// <param name="hWindow">Handle to Window</param>
        /// <param name="Message">Message to send (cast to integer equivalent)</param>
        /// <returns></returns>
        public static bool PostWindowMessage(IntPtr hWindow, int Message)
        {
            return PostWindowMessage(hWindow, Message, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// PostWindowMessage - Post a Native Win32 WindowMessage call against a window handle.
        /// </summary>
        /// <param name="hWindow">Handle to Window</param>
        /// <param name="Message">Message to send (cast to integer equivalent)</param>
        /// <param name="wParam">Pointer to unmanaged data in memory for message parameters.</param>
        /// <param name="lParam">Pointer to unmanaged data in memory for message parameters.</param>
        /// <returns></returns>
        public static bool PostWindowMessage(IntPtr hWindow, int Message, IntPtr wParam, IntPtr lParam)
        {
            return _PostWindowMessage(hWindow, Message, wParam, lParam);
        }

       





    }



}
