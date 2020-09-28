using System.Windows.Media;
using Microsoft.Win32;

using static WinXPresentationFoundation.NTInterop.Enums;

namespace WinXPresentationFoundation.NTInterop
{
    public static partial class Win32
    {
 
        /// <summary>
        /// Returns a System.Windows.Media Color Struct from a Win32 DWORD value
        /// </summary>
        /// <param name="W32Color">int W32Color</param>
        /// <returns>System.Windows.Media.Color structure.</returns>
        public static Color FromWin32Color(int W32Color = 0x00000000)
        {

            int Win32RedShift = 0;
            int Win32GreenShift = 8;
            int Win32BlueShift = 16;

            Color color = Color.FromArgb(
                255,
                (byte)((W32Color >> Win32RedShift) & 0xFF),
                (byte)((W32Color >> Win32GreenShift) & 0xFF),
                (byte)((W32Color >> Win32BlueShift) & 0xFF)
            );           

	        return color;
        }

        public static bool IsHighContrastMode
        {
            get { return System.Windows.SystemParameters.HighContrast; }
        }

        public static WINDOWSX_BUILD WindowsBuild()
        {

            if (!HasRegValue("HKLM", "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CurrentBuildNumber", out object _regValue)) return WINDOWSX_BUILD.BuildUnknown;


            int.TryParse(_regValue.ToString(), out int _buildValue);

            try
            {
                return (WINDOWSX_BUILD)_buildValue;
            }
            catch
            {
                return WINDOWSX_BUILD.BuildUnknown;
            }

        }

        public static WINDOWS_VERSION GetWindowsVersion()
        {
            /*
             *  start with "CurrentVersion" Key
             *  value of 6.1 = Windows 7
             *  value of 6.2 = Windows 8
             *  value of 6.3 = Windows 8.1 (after trigger check for windows 10 "superKeys")
             */

            //object _cvKeyValue, _bmajKeyValue, _bminKeyValue;

            if (HasRegValue("HKLM", "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CurrentVersion", out object _cvKeyValue))
            {
                switch (_cvKeyValue.ToString())
                {
                    case "6.0":
                        return WINDOWS_VERSION.WindowsVista;
                    case "6.1":
                        return WINDOWS_VERSION.Windows7;
                    case "6.2":
                        return WINDOWS_VERSION.Windows8;
                    case "6.3":
                        // have to test here for win10 versions because of NEW registry keys
                        if (HasRegValue("HKLM", "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CurrentMajorVersionNumber", out object _bmajKeyValue) && HasRegValue("HKLM", "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CurrentMinorVersionNumber", out object _bminKeyValue))
                        {
                            if ((int)_bmajKeyValue == 10)
                            {
                                return WINDOWS_VERSION.Windows10;
                            }
                            else
                            {
                                return WINDOWS_VERSION.WindowsUnknown;
                            }
                        }
                        else
                        {
                            return WINDOWS_VERSION.Windows81;
                        }

                    default:
                        return WINDOWS_VERSION.WindowsUnknown;

                }
            }
            else
            {
                return WINDOWS_VERSION.WindowsUnknown;
            }



        }

        internal static bool HasRegValue(string _hive, string _key, string _valueName, out dynamic _value)
        {
            _value = null;
            bool _result = false;

            RegistryKey _registry;

            switch (_hive)
            {
                case "HKLM":
                    _registry = Registry.LocalMachine;
                    break;
                case "HKCU":
                    _registry = Registry.CurrentUser;
                    break;
                case "HKU":
                    _registry = Registry.Users;
                    break;
                case "HKCC":
                    _registry = Registry.CurrentConfig;
                    break;
                case "HKCR":
                    _registry = Registry.ClassesRoot;
                    break;
                default:
                    _registry = Registry.CurrentUser;
                    break;
            }


            using (RegistryKey key = _registry.OpenSubKey(_key))

            {
                if (key != null)
                {
                    _value = key.GetValue(_valueName);
                    if (_value != null)
                    {
                        RegistryValueKind _valueType = key.GetValueKind(_valueName);
                        switch (_valueType)
                        {
                            case RegistryValueKind.DWord:
                                _value = (int)_value;
                                break;

                            case RegistryValueKind.String:
                                _value = (string)_value;
                                break;                                
                        }
                        _result = true;
                    }
                }
            }

            _registry.Close();
            _registry.Dispose();

            return _result;

        }


    }
}
