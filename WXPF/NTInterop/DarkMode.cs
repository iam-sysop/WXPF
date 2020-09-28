using System;
using System.Runtime.InteropServices;

using static WinXPresentationFoundation.NTInterop.Enums;


namespace WinXPresentationFoundation.NTInterop
{

    public static class DarkMode
    {

        public static bool CanEnableDarkMode()
        {
            if (DarkModeVersion() > 0) return true;
            
            return false;

        }

        public static bool GetSystemDarkModeStatus()
        {
            switch (DarkModeVersion())
            {

                case 1:
                    // rely on 'v1' darkmode detection
                    // AppsUseLightTheme >= 1803
                    //Console.WriteLine("Checking v1 DarkMode Support");
                    return GetSystemDarkMode(1);

                case 2:
                    // rely on 'v2' darkmode detection
                    // SystemUsesLightTheme  then AppsUseLightTheme 
                    //Console.WriteLine("Checking v2 DarkMode Support");
                    return GetSystemDarkMode(2);

                default:
                    return false;

            }

            
        }

        // before ANY window creation/instantiation
        public static void SetAppDarkMode(bool useDark = false)
        {

            switch (DarkModeVersion())
            {
                case 1:
                    Win32.AllowDarkModeForApp(useDark);
                    break;

                case 2:
                    if (useDark) Win32.SetPreferredAppMode(PREFERRED_APP_MODE.AllowDark);
                    else Win32.SetPreferredAppMode(PREFERRED_APP_MODE.Default);
                    break;
            }
        }

        // after window creation - also attempts to apply "DarkMode_Explorer" window class for native Win32 windows (menus, titlebar, scrollbars)
        public static void SetWindowDarkMode(IntPtr hWindow, bool useDark = false)
        {
            int _dwmUseDark = useDark ? 1 : 0;

            switch (DarkModeVersion())
            {
                case 1:
                    //_AllowDarkModeForApp(useDark);
                    Win32.AllowDarkModeForWindow(hWindow, useDark);
                    Win32.SetWindowCompositionAttribute(hWindow, WINDOW_COMPOSITION_ATTRIBUTE.WCA_USEDARKMODECOLORS, useDark);
                    Win32.ResetDwmDarkMode(hWindow, _dwmUseDark);
                    break;

                case 2:
                    //if (useDark) _SetPreferredAppMode((int)PREFERRED_APP_MODE.AllowDark);
                    //else _SetPreferredAppMode((int)PREFERRED_APP_MODE.Default);
                    Win32.AllowDarkModeForWindow(hWindow, useDark);
                    Win32.SetWindowCompositionAttribute(hWindow, WINDOW_COMPOSITION_ATTRIBUTE.WCA_USEDARKMODECOLORS, useDark);
                    Win32.ResetDwmDarkMode(hWindow, _dwmUseDark);
                    break;

                default:
                    break;
            }
        }

        private static int DarkModeVersion()
        {

            WINDOWSX_BUILD _build = Win32.WindowsBuild();

            if (_build == WINDOWSX_BUILD.Build1803 || _build == WINDOWSX_BUILD.Build1809)
            {
                return 1;
            }
            else if (_build >= WINDOWSX_BUILD.Build1903)
            {
                return 2;
            }
            return 0;
        }

        private static bool GetSystemDarkMode(int _checkType)
        {

            if (_checkType == 1)
            {
                // v1 check

                if (Win32.HasRegValue("HKLM", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", out object _testValue))
                {
                    if (int.TryParse(_testValue.ToString(), out int _parsed) && _parsed == 0)
                    {
                        return true;
                    }
                }
            }
            else if (_checkType == 2)
            {

                Win32.HasRegValue("HKCU", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "SystemUsesLightTheme", out object _testSysValue);
                Win32.HasRegValue("HKCU", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", out object _testAppValue);

                if ((int.TryParse(_testSysValue.ToString(), out int _sysTheme) && _sysTheme == 0) && (int.TryParse(_testAppValue.ToString(), out int _appTheme) && _appTheme == 0))
                {
                    return true;
                }


            }
            return false;
        }

    }


    public static partial class Win32
    {

        // Native Calls

        [DllImport("uxtheme.dll", CharSet = CharSet.Auto, EntryPoint = "#135", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern int _SetPreferredAppMode(int iPreferredAppMode);

        [DllImport("uxtheme.dll", CharSet = CharSet.Auto, EntryPoint = "#135", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern int _AllowDarkModeForApp(bool bUseDarkMode);

        [DllImport("uxtheme.dll", CharSet = CharSet.Auto, EntryPoint = "#133", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern int _AllowDarkModeForWindow(IntPtr hWnd, bool bUseDarkMode);

        [DllImport("uxtheme.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowTheme", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern int _SetWindowTheme(IntPtr hWindow, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowCompositionAttribute", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern int _SetWindowCompositionAttribute(IntPtr hWindow, ref WINDOW_COMPOSITION_ATTRIBUTE_DATA WindowCompositionAttributeData);

        [DllImport("dwmapi.dll", CharSet = CharSet.Auto, EntryPoint = "DwmSetWindowAttribute", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern int _DwmSetWindowAttribute(IntPtr hWindow, uint DwmAttribute, ref dynamic DwmValue, int DwmValueSize);

        [DllImport("uxtheme.dll", CharSet = CharSet.Auto, EntryPoint = "#104", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal Function Declaration")]
        private static extern int _RefreshImmersiveColorPolicyState();


        // Managed Calls

        internal static int DwmSetWindowAttribute(IntPtr _hWnd, uint _DwmAttribute, ref dynamic _DwmValue)
        {
            int _result = _DwmSetWindowAttribute(_hWnd, _DwmAttribute, ref _DwmValue, Marshal.SizeOf(_DwmValue));
            return _result;
        }

        internal static void SetWindowCompositionAttribute(IntPtr hWindow, WINDOW_COMPOSITION_ATTRIBUTE wcAttribute, dynamic Value)
        {
            object _valueObject = (object)Value;


            switch (wcAttribute)
            {
                case WINDOW_COMPOSITION_ATTRIBUTE.WCA_USEDARKMODECOLORS:

                    bool _result;
                    bool.TryParse(_valueObject.ToString(), out _result);

                    int _sizeOfValue = Marshal.SizeOf(_result);

                    WINDOW_COMPOSITION_ATTRIBUTE_DATA WCA_DATA = new WINDOW_COMPOSITION_ATTRIBUTE_DATA
                    {
                        Attrib = (uint)WINDOW_COMPOSITION_ATTRIBUTE.WCA_USEDARKMODECOLORS,
                        pvData = _result,
                        pvDataSize = _sizeOfValue
                    };

                    _SetWindowCompositionAttribute(hWindow, ref WCA_DATA);
                    // RefreshImmersiveColorPolicyState();

                    break;

            }

        }

        internal static void RefreshImmersiveColorPolicyState()
        {
            _RefreshImmersiveColorPolicyState();
        }

        internal static void SetPreferredAppMode(PREFERRED_APP_MODE PreferredAppMode)
        {
            _SetPreferredAppMode((int)PreferredAppMode);
        }

        internal static void AllowDarkModeForApp(bool UseDarkMode)
        {
            _AllowDarkModeForApp(UseDarkMode);
        }

        internal static void AllowDarkModeForWindow(IntPtr WindowHandle, bool UseDarkMode)
        {
            _AllowDarkModeForWindow(WindowHandle, UseDarkMode);
        }

        internal static void SetWindowTheme(IntPtr WindowHandle, string ThemeAppName, string ThemeSubClassList = "")
        {
            _SetWindowTheme(WindowHandle, ThemeAppName, ThemeSubClassList);
        }

        internal static void ResetDwmDarkMode(IntPtr hWnd, dynamic value)
        {

            if (DwmSetWindowAttribute(hWnd, 20, ref value) != 0) DwmSetWindowAttribute(hWnd, 19, ref value);

            if (value == 1)
            {
                SetWindowTheme(hWnd, "DarkMode_Explorer", null);
            }
            else
            {
                SetWindowTheme(hWnd, "Explorer", null);
            }

            // RefreshImmersiveColorPolicyState();
        }


    }
}
