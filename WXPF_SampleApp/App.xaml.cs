using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

using WinXPresentationFoundation;
using WinXPresentationFoundation.NTInterop;

namespace WXPF_SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public ThemeHelper AppThemer;
        public MainWindow AppWindow;

        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Init the ThemeHelper FIRST - this tests and sets variables whether or not the OS supports DarkMode PRIOR TO FIRST WINDOW CREATION
            AppThemer = new ThemeHelper(true, true);

            // Init a WinXPresentation.WXWindow
            AppWindow = new MainWindow();

            /* Detect and Initialize DarkMode Features - tie in Windows MessageLoop to detect OS theme change
             * 
             * -- this will change with an WXApp Container in future update
             */            
            
            AppThemer.HandleThemeChange(AppWindow.WindowHandle);
            HwndSource.FromHwnd(AppWindow.WindowHandle).AddHook(AppWindow.OnReceiveMessage);
            AppWindow.WndProc += ThemeMessageHandler;

            // Set the Application MainWindow to the WXWindow we control and have pre-inited with DarkMode support.
            this.MainWindow = AppWindow;
            
            // Make sure some basic window options are set
            this.MainWindow.WindowState = WindowState.Normal;
            this.MainWindow.ShowInTaskbar = true;

            // Show our DarkMode capable MainWindow
            this.MainWindow.Show();            

            base.OnStartup(e);

        }




        private bool ThemeMessageHandler(object sender, Enums.WindowMessageArgs Msg)
        {
            bool _result = false;

            if ((Enums.WINDOW_MESSAGE)Msg.Message == Enums.WINDOW_MESSAGE.WM_WININICHANGE) { 

                string _lParam = (Msg.lParam != IntPtr.Zero) ? Marshal.PtrToStringUni(Msg.lParam) : "";

                if (_lParam == "ImmersiveColorSet")
                {
                    AppThemer.HandleThemeChange(AppWindow.WindowHandle);
                    Msg.Handled = true;
                    _result = true;
                }

            }
            return _result;

        }

    }

}
