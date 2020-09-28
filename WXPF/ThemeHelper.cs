using System;
using System.Windows;
using System.Windows.Media;

namespace WinXPresentationFoundation
{

    public class ThemeHelper
    {

        public bool DarkModeAvailable { get; private set; }
        public bool DarkModeStatus { get; private set; }
        public bool DarkMode { get; set; }

        public string CurrentTheme { get; private set; }

        
        public ImmersiveColors.ColorPalette ThemePalette;

        public event EventHandler<EventArgs> ThemeLoaded;
        public event EventHandler<EventArgs> ThemeApplied;

        protected virtual void OnThemeLoaded(EventArgs e)
        {
            ThemeLoaded?.Invoke(this, e);
        }

        protected virtual void OnThemeApplied(EventArgs e)
        {
            ThemeApplied?.Invoke(this, e);
        }

        /// <summary>
        /// ThemeHelper - Overrides/Manages the SystemColors Palette in WPF
        /// </summary>
        /// <param name="_DarkMode">Attempt to enable DarkMode features - false will default to "light-mode theme".</param>
        /// <param name="_AutoDarkMode">If DarkMode available, Auto-Init into DarkThemeFeatures</param>

        public ThemeHelper(bool _DarkMode = false, bool _AutoDarkMode = false)
        {
            if (_DarkMode)
            {
                DarkModeAvailable = NTInterop.DarkMode.CanEnableDarkMode();

                if (DarkModeAvailable)
                {
                    DarkModeStatus = NTInterop.DarkMode.GetSystemDarkModeStatus();
                    DarkMode = (_AutoDarkMode && DarkModeStatus) ? (_AutoDarkMode && DarkModeStatus) : false; 
                    NTInterop.DarkMode.SetAppDarkMode(DarkModeAvailable);
                }
                else
                {
                    DarkMode = false;
                    DarkModeStatus = false;
                    NTInterop.DarkMode.SetAppDarkMode(false);
                    
                }
            }
            else
            {
                DarkMode = false;
                DarkModeAvailable = false;
                DarkModeStatus = false;
                
            }                             

            ThemePalette = new ImmersiveColors.ColorPalette(DarkMode);
            CurrentTheme = "";

        }


        /// <summary>
        /// Reload Immersive ColorPalette to ThemePalette using currently set DarkMode property
        /// </summary>        
        public void ReloadTheme()
        {
            ThemePalette = new ImmersiveColors.ColorPalette(DarkMode);
            ApplyTheme();
        }


        /// <summary>
        /// Reload Immersive ColorPalette to ThemePalette using currently set DarkMode property
        /// </summary>        
        /// <param name="_DarkMode">Reset DarkMode property prior to loading Theme</param>
        public void ReloadTheme(bool _DarkMode)
        {
            if (DarkModeAvailable)
            {
                DarkModeStatus = NTInterop.DarkMode.GetSystemDarkModeStatus();
                DarkMode = DarkModeStatus ? _DarkMode : false;
            }
            else
            { 
                DarkModeStatus = false;
                DarkMode = false;
            }
            ReloadTheme();
        }


        /// <summary>
        /// Apply currently loaded Theme to Application Resources - allowing propogation to x:static DynamicResource keys
        /// </summary>    
        public void ApplyTheme()
        {

            #region Active Window Caption

            if (Application.Current.Resources[SystemColors.ActiveBorderBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ActiveBorderBrushKey, GetBrush(ThemePalette.ActiveBorderColor));
            else
                Application.Current.Resources[SystemColors.ActiveBorderBrushKey] = GetBrush(ThemePalette.ActiveBorderColor);

            if (Application.Current.Resources[SystemColors.ActiveBorderColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ActiveBorderColorKey, ThemePalette.ActiveBorderColor);
            else
                Application.Current.Resources[SystemColors.ActiveBorderColorKey] = ThemePalette.ActiveBorderColor;


            if (Application.Current.Resources[SystemColors.ActiveCaptionBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ActiveCaptionBrushKey, GetBrush(ThemePalette.ActiveCaptionColor));
            else
                Application.Current.Resources[SystemColors.ActiveCaptionBrushKey] = GetBrush(ThemePalette.ActiveCaptionColor);

            if (Application.Current.Resources[SystemColors.ActiveCaptionColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ActiveCaptionColorKey, ThemePalette.ActiveCaptionColor);
            else
                Application.Current.Resources[SystemColors.ActiveCaptionColorKey] = ThemePalette.ActiveCaptionColor;


            if (Application.Current.Resources[SystemColors.ActiveCaptionTextBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ActiveCaptionTextBrushKey, GetBrush(ThemePalette.ActiveCaptionTextColor));
            else
                Application.Current.Resources[SystemColors.ActiveCaptionTextBrushKey] = GetBrush(ThemePalette.ActiveCaptionTextColor);

            if (Application.Current.Resources[SystemColors.ActiveCaptionTextColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ActiveCaptionTextColorKey,ThemePalette.ActiveCaptionTextColor);
            else
                Application.Current.Resources[SystemColors.ActiveCaptionTextColorKey] = ThemePalette.ActiveCaptionTextColor;

            #endregion

            #region Inactive Window Caption

            if (Application.Current.Resources[SystemColors.InactiveBorderBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.InactiveBorderBrushKey, GetBrush(ThemePalette.InactiveBorderColor));
            else
                Application.Current.Resources[SystemColors.InactiveBorderBrushKey] = GetBrush(ThemePalette.InactiveBorderColor);

            if (Application.Current.Resources[SystemColors.InactiveBorderColorKey] == null)
                Application.Current.Resources.Add(SystemColors.InactiveBorderColorKey, ThemePalette.InactiveBorderColor);
            else
                Application.Current.Resources[SystemColors.InactiveBorderColorKey] = ThemePalette.InactiveBorderColor;


            if (Application.Current.Resources[SystemColors.InactiveCaptionBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.InactiveCaptionBrushKey, GetBrush(ThemePalette.InactiveCaptionColor));
            else
                Application.Current.Resources[SystemColors.InactiveCaptionBrushKey] = GetBrush(ThemePalette.InactiveCaptionColor);

            if (Application.Current.Resources[SystemColors.InactiveCaptionColorKey] == null)
                Application.Current.Resources.Add(SystemColors.InactiveCaptionColorKey, ThemePalette.InactiveCaptionColor);
            else
                Application.Current.Resources[SystemColors.InactiveCaptionColorKey] = ThemePalette.InactiveCaptionColor;


            if (Application.Current.Resources[SystemColors.InactiveCaptionTextBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.InactiveCaptionTextBrushKey, GetBrush(ThemePalette.InactiveCaptionTextColor));
            else
                Application.Current.Resources[SystemColors.InactiveCaptionTextBrushKey] = GetBrush(ThemePalette.InactiveCaptionTextColor);

            if (Application.Current.Resources[SystemColors.InactiveCaptionTextColorKey] == null)
                Application.Current.Resources.Add(SystemColors.InactiveCaptionTextColorKey, ThemePalette.InactiveCaptionTextColor);
            else
                Application.Current.Resources[SystemColors.InactiveCaptionTextColorKey] = ThemePalette.InactiveCaptionTextColor;

            #endregion

            #region Window Caption Gradient

            if (Application.Current.Resources[SystemColors.GradientActiveCaptionBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.GradientActiveCaptionBrushKey, GetBrush(ThemePalette.GradientActiveCaptionColor));
            else
                Application.Current.Resources[SystemColors.GradientActiveCaptionBrushKey] = GetBrush(ThemePalette.GradientActiveCaptionColor);

            if (Application.Current.Resources[SystemColors.GradientActiveCaptionColorKey] == null)
                Application.Current.Resources.Add(SystemColors.GradientActiveCaptionColorKey, ThemePalette.GradientActiveCaptionColor);
            else
                Application.Current.Resources[SystemColors.GradientActiveCaptionColorKey] = ThemePalette.GradientActiveCaptionColor;


            if (Application.Current.Resources[SystemColors.GradientInactiveCaptionBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.GradientInactiveCaptionBrushKey, GetBrush(ThemePalette.GradientInactiveCaptionColor));
            else
                Application.Current.Resources[SystemColors.GradientInactiveCaptionBrushKey] = GetBrush(ThemePalette.GradientInactiveCaptionColor);

            if (Application.Current.Resources[SystemColors.GradientInactiveCaptionColorKey] == null)
                Application.Current.Resources.Add(SystemColors.GradientInactiveCaptionColorKey, ThemePalette.GradientInactiveCaptionColor);
            else
                Application.Current.Resources[SystemColors.GradientInactiveCaptionColorKey] = ThemePalette.GradientInactiveCaptionColor;

            #endregion

            #region AppWorkspace / Desktop

            if (Application.Current.Resources[SystemColors.AppWorkspaceBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.AppWorkspaceBrushKey, GetBrush(ThemePalette.AppWorkspaceColor));
            else
                Application.Current.Resources[SystemColors.AppWorkspaceBrushKey] = GetBrush(ThemePalette.AppWorkspaceColor);
            
            if (Application.Current.Resources[SystemColors.AppWorkspaceColorKey] == null)
                Application.Current.Resources.Add(SystemColors.AppWorkspaceColorKey, ThemePalette.AppWorkspaceColor);
            else
                Application.Current.Resources[SystemColors.AppWorkspaceColorKey] = ThemePalette.AppWorkspaceColor;


            if (Application.Current.Resources[SystemColors.DesktopBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.DesktopBrushKey, GetBrush(ThemePalette.DesktopColor));
            else
                Application.Current.Resources[SystemColors.DesktopBrushKey] = GetBrush(ThemePalette.DesktopColor);

            if (Application.Current.Resources[SystemColors.DesktopColorKey] == null)
                Application.Current.Resources.Add(SystemColors.DesktopColorKey, ThemePalette.DesktopColor);
            else
                Application.Current.Resources[SystemColors.DesktopColorKey] = ThemePalette.DesktopColor;

            #endregion

            #region ControlBase

            if (Application.Current.Resources[SystemColors.ControlBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlBrushKey, GetBrush(ThemePalette.ControlColor));
            else
                Application.Current.Resources[SystemColors.ControlBrushKey] = GetBrush(ThemePalette.ControlColor);

            if (Application.Current.Resources[SystemColors.ControlColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlColorKey, ThemePalette.ControlColor);
            else
                Application.Current.Resources[SystemColors.ControlColorKey] = ThemePalette.ControlColor;


            if (Application.Current.Resources[SystemColors.ControlDarkBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlDarkBrushKey, GetBrush(ThemePalette.ControlDarkColor));
            else
                Application.Current.Resources[SystemColors.ControlDarkBrushKey] = GetBrush(ThemePalette.ControlDarkColor);

            if (Application.Current.Resources[SystemColors.ControlDarkColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlDarkColorKey, ThemePalette.ControlDarkColor);
            else
                Application.Current.Resources[SystemColors.ControlDarkColorKey] = ThemePalette.ControlDarkColor;


            if (Application.Current.Resources[SystemColors.ControlDarkDarkBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlDarkDarkBrushKey, GetBrush(ThemePalette.ControlDarkDarkColor));
            else
                Application.Current.Resources[SystemColors.ControlDarkDarkBrushKey] = GetBrush(ThemePalette.ControlDarkDarkColor);

            if (Application.Current.Resources[SystemColors.ControlDarkDarkColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlDarkDarkColorKey, ThemePalette.ControlDarkDarkColor);
            else
                Application.Current.Resources[SystemColors.ControlDarkDarkColorKey] = ThemePalette.ControlDarkDarkColor;


            if (Application.Current.Resources[SystemColors.ControlLightBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlLightBrushKey, GetBrush(ThemePalette.ControlLightColor));
            else
                Application.Current.Resources[SystemColors.ControlLightBrushKey] = GetBrush(ThemePalette.ControlLightColor);

            if (Application.Current.Resources[SystemColors.ControlLightColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlLightColorKey, ThemePalette.ControlLightColor);
            else
                Application.Current.Resources[SystemColors.ControlLightColorKey] = ThemePalette.ControlLightColor;


            if (Application.Current.Resources[SystemColors.ControlLightLightBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlLightLightBrushKey, GetBrush(ThemePalette.ControlLightLightColor));
            else
                Application.Current.Resources[SystemColors.ControlLightLightBrushKey] = GetBrush(ThemePalette.ControlLightLightColor);

            if (Application.Current.Resources[SystemColors.ControlLightLightColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlLightLightColorKey, ThemePalette.ControlLightLightColor);
            else
                Application.Current.Resources[SystemColors.ControlLightLightColorKey] = ThemePalette.ControlLightLightColor;


            if (Application.Current.Resources[SystemColors.ControlTextBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlTextBrushKey, GetBrush(ThemePalette.ControlTextColor));
            else
                Application.Current.Resources[SystemColors.ControlTextBrushKey] = GetBrush(ThemePalette.ControlTextColor);

            if (Application.Current.Resources[SystemColors.ControlTextColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ControlTextColorKey, ThemePalette.ControlTextColor);
            else
                Application.Current.Resources[SystemColors.ControlTextColorKey] = ThemePalette.ControlTextColor;

            #endregion

            #region Miscellaneous

            if (Application.Current.Resources[SystemColors.GrayTextBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.GrayTextBrushKey, GetBrush(ThemePalette.GrayTextColor));
            else
                Application.Current.Resources[SystemColors.GrayTextBrushKey] = GetBrush(ThemePalette.GrayTextColor);

            if (Application.Current.Resources[SystemColors.GrayTextColorKey] == null)
                Application.Current.Resources.Add(SystemColors.GrayTextColorKey, ThemePalette.GrayTextColor);
            else
                Application.Current.Resources[SystemColors.GrayTextColorKey] = ThemePalette.GrayTextColor;


            if (Application.Current.Resources[SystemColors.HighlightBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.HighlightBrushKey, GetBrush(ThemePalette.HighlightColor));
            else
                Application.Current.Resources[SystemColors.HighlightBrushKey] = GetBrush(ThemePalette.HighlightColor);

            if (Application.Current.Resources[SystemColors.HighlightColorKey] == null)
                Application.Current.Resources.Add(SystemColors.HighlightColorKey, ThemePalette.HighlightColor);
            else
                Application.Current.Resources[SystemColors.HighlightColorKey] = ThemePalette.HighlightColor;


            if (Application.Current.Resources[SystemColors.HighlightTextBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.HighlightTextBrushKey, GetBrush(ThemePalette.HighlightTextColor));
            else
                Application.Current.Resources[SystemColors.HighlightTextBrushKey] = GetBrush(ThemePalette.HighlightTextColor);

            if (Application.Current.Resources[SystemColors.HighlightTextColorKey] == null)
                Application.Current.Resources.Add(SystemColors.HighlightTextColorKey, ThemePalette.HighlightTextColor);
            else
                Application.Current.Resources[SystemColors.HighlightTextColorKey] = ThemePalette.HighlightTextColor;


            if (Application.Current.Resources[SystemColors.InfoBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.InfoBrushKey, GetBrush(ThemePalette.InfoColor));
            else
                Application.Current.Resources[SystemColors.InfoBrushKey] = GetBrush(ThemePalette.InfoColor);

            if (Application.Current.Resources[SystemColors.InfoColorKey] == null)
                Application.Current.Resources.Add(SystemColors.InfoColorKey, ThemePalette.InfoColor);
            else
                Application.Current.Resources[SystemColors.InfoColorKey] = ThemePalette.InfoColor;


            if (Application.Current.Resources[SystemColors.InfoTextBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.InfoTextBrushKey, GetBrush(ThemePalette.InfoTextColor));
            else
                Application.Current.Resources[SystemColors.InfoTextBrushKey] = GetBrush(ThemePalette.InfoTextColor);

            if (Application.Current.Resources[SystemColors.InfoTextColorKey] == null)
                Application.Current.Resources.Add(SystemColors.InfoTextColorKey, ThemePalette.InfoTextColor);
            else
                Application.Current.Resources[SystemColors.InfoTextColorKey] = ThemePalette.InfoTextColor;


            if (Application.Current.Resources[SystemColors.HotTrackBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.HotTrackBrushKey, GetBrush(ThemePalette.HotTrackColor));
            else
                Application.Current.Resources[SystemColors.HotTrackBrushKey] = GetBrush(ThemePalette.HotTrackColor);

            if (Application.Current.Resources[SystemColors.HotTrackColorKey] == null)
                Application.Current.Resources.Add(SystemColors.HotTrackColorKey, ThemePalette.HotTrackColor);
            else
                Application.Current.Resources[SystemColors.HotTrackColorKey] = ThemePalette.HotTrackColor;


            if (Application.Current.Resources[SystemColors.ScrollBarBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.ScrollBarBrushKey, GetBrush(ThemePalette.ScrollBarColor));
            else
                Application.Current.Resources[SystemColors.ScrollBarBrushKey] = GetBrush(ThemePalette.ScrollBarColor);

            if (Application.Current.Resources[SystemColors.ScrollBarColorKey] == null)
                Application.Current.Resources.Add(SystemColors.ScrollBarColorKey, ThemePalette.ScrollBarColor);
            else
                Application.Current.Resources[SystemColors.ScrollBarColorKey] = ThemePalette.ScrollBarColor;

            #endregion

            #region Menu and MenuBar

            if (Application.Current.Resources[SystemColors.MenuBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.MenuBrushKey, GetBrush(ThemePalette.MenuColor));
            else
                Application.Current.Resources[SystemColors.MenuBrushKey] = GetBrush(ThemePalette.MenuColor);

            if (Application.Current.Resources[SystemColors.MenuColorKey] == null)
                Application.Current.Resources.Add(SystemColors.MenuColorKey, ThemePalette.MenuColor);
            else
                Application.Current.Resources[SystemColors.MenuColorKey] = ThemePalette.MenuColor;


            if (Application.Current.Resources[SystemColors.MenuBarBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.MenuBarBrushKey, GetBrush(ThemePalette.MenuBarColor));
            else
                Application.Current.Resources[SystemColors.MenuBarBrushKey] = GetBrush(ThemePalette.MenuBarColor);

            if (Application.Current.Resources[SystemColors.MenuBarColorKey] == null)
                Application.Current.Resources.Add(SystemColors.MenuBarColorKey, ThemePalette.MenuBarColor);
            else
                Application.Current.Resources[SystemColors.MenuBarColorKey] = ThemePalette.MenuBarColor;


            if (Application.Current.Resources[SystemColors.MenuHighlightBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.MenuHighlightBrushKey, GetBrush(ThemePalette.MenuHighlightColor));
            else
                Application.Current.Resources[SystemColors.MenuHighlightBrushKey] = GetBrush(ThemePalette.MenuHighlightColor);

            if (Application.Current.Resources[SystemColors.MenuHighlightColorKey] == null)
                Application.Current.Resources.Add(SystemColors.MenuHighlightColorKey, ThemePalette.MenuHighlightColor);
            else
                Application.Current.Resources[SystemColors.MenuHighlightColorKey] = ThemePalette.MenuHighlightColor;


            if (Application.Current.Resources[SystemColors.MenuTextBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.MenuTextBrushKey, GetBrush(ThemePalette.MenuTextColor));
            else
                Application.Current.Resources[SystemColors.MenuTextBrushKey] = GetBrush(ThemePalette.MenuTextColor);

            if (Application.Current.Resources[SystemColors.MenuTextColorKey] == null)
                Application.Current.Resources.Add(SystemColors.MenuTextColorKey, ThemePalette.MenuTextColor);
            else
                Application.Current.Resources[SystemColors.MenuTextColorKey] = ThemePalette.MenuTextColor;

            #endregion

            #region Window Background/Border/Text

            if (Application.Current.Resources[SystemColors.WindowBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.WindowBrushKey, GetBrush(ThemePalette.WindowColor));
            else
                Application.Current.Resources[SystemColors.WindowBrushKey] = GetBrush(ThemePalette.WindowColor);

            if (Application.Current.Resources[SystemColors.WindowColorKey] == null)
                Application.Current.Resources.Add(SystemColors.WindowColorKey, ThemePalette.WindowColor);
            else
                Application.Current.Resources[SystemColors.WindowColorKey] = ThemePalette.WindowColor;

            if (Application.Current.Resources[SystemColors.WindowFrameBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.WindowFrameBrushKey, GetBrush(ThemePalette.WindowFrameColor));
            else
                Application.Current.Resources[SystemColors.WindowFrameBrushKey] = GetBrush(ThemePalette.WindowFrameColor);

            if (Application.Current.Resources[SystemColors.WindowFrameColorKey] == null)
                Application.Current.Resources.Add(SystemColors.WindowFrameColorKey, ThemePalette.WindowFrameColor);
            else
                Application.Current.Resources[SystemColors.WindowFrameColorKey] = ThemePalette.WindowFrameColor;

            if (Application.Current.Resources[SystemColors.WindowTextBrushKey] == null)
                Application.Current.Resources.Add(SystemColors.WindowTextBrushKey, GetBrush(ThemePalette.WindowTextColor));
            else
                Application.Current.Resources[SystemColors.WindowTextBrushKey] = GetBrush(ThemePalette.WindowTextColor);

            if (Application.Current.Resources[SystemColors.WindowTextColorKey] == null)
                Application.Current.Resources.Add(SystemColors.WindowTextColorKey, ThemePalette.WindowTextColor);
            else
                Application.Current.Resources[SystemColors.WindowTextColorKey] = ThemePalette.WindowTextColor;
            
            #endregion

        }

        /// <summary>
        /// HandleThemeChange - stock routine to detect current OS theme mode and apply to WXPF.
        /// 
        /// ** NEEDS IMPROVEMENT **
        /// </summary>
        /// <param name="hWnd">Window Handle to "refresh" with new theme.</param>
        public void HandleThemeChange(IntPtr hWnd)
        {
            if (DarkModeAvailable)
            {
                DarkModeStatus = NTInterop.DarkMode.GetSystemDarkModeStatus();                
            } 
            else
            {
                DarkModeStatus = false;
                
            }
            
            if ((DarkModeStatus && CurrentTheme != "dark") || (!DarkModeStatus && CurrentTheme != "light"))
            {
                NTInterop.DarkMode.SetWindowDarkMode(hWnd, DarkModeStatus);
                                
                this.ReloadTheme(DarkModeStatus);
                
            }
        }

        private SolidColorBrush GetBrush(Color _color)
        {
            if (_color != null) return new SolidColorBrush(_color); else throw new ArgumentNullException();
        }


    }


    /// <summary>
    /// ColorHelper - with code for HSL/RGB interop adapted from https://stackoverflow.com/a/55135336
    /// </summary>
    public static class ColorHelper
    {


        private static double _tolerance => 0.000000000000001;


        /// <summary>
        /// Defines brightness levels.
        /// </summary>
        public enum Brightness : byte
        {
            High = 255,
            MediumHigh = 210,
            Medium = 142,
            MediumLow = 98,
            Low = 50
        }


        /// <summary>
        /// Defines alpha levels.
        /// </summary>
        public enum Alpha : byte
        {
            Max = 255,
            MediumHigh = 230,
            Medium = 175,
            Half = 142,
            MediumLow = 109,
            Low = 45,
            Transparent = 0
        }


        /// <summary>
        /// Defines hint alpha levels.
        /// </summary>
        public enum HintAlpha : byte
        {
            L0 = 64,
            L1 = 48,
            L2 = 32,
            L3 = 16
        }


        /// <summary>
        /// Specifies a mode for argb transformations.
        /// </summary>
        public enum ColorTransformMode : byte
        {
            HSL,
            HSB
        }


        /// <summary>
        /// Converts RGB to HSL. Alpha is ignored.
        /// Output is: { H: [0, 360], S: [0, 1], L: [0, 1] }.
        /// </summary>
        /// <param name="color">The System.Windows.Media.Color to convert.</param>
        public static double[] RgbToHsl(Color color)
        {
            double _hue = 0D;
            double _sat = 0D;
            double _lum;

            // normalize red, green, blue values
            double _red = color.R / 255D;
            double _grn = color.G / 255D;
            double _blu = color.B / 255D;

            double max = Math.Max(_red, Math.Max(_grn, _blu));
            double min = Math.Min(_red, Math.Min(_grn, _blu));

            // hue
            if (Math.Abs(max - min) < _tolerance) _hue = 0D;
            else if ((Math.Abs(max - _red) < _tolerance) && (_grn >= _blu)) _hue = (60D * (_grn - _blu)) / (max - min);
            else if ((Math.Abs(max - _red) < _tolerance) && (_grn < _blu)) _hue = ((60D * (_grn - _blu)) / (max - min)) + 360D;
            else if (Math.Abs(max - _grn) < _tolerance) _hue = ((60D * (_blu - _red)) / (max - min)) + 120D;
            else if (Math.Abs(max - _blu) < _tolerance) _hue = ((60D * (_red - _grn)) / (max - min)) + 240D;

            // luminance
            _lum = (max + min) / 2D;

            // saturation
            if ((Math.Abs(_lum) < _tolerance) || (Math.Abs(max - min) < _tolerance)) _sat = 0D;
            else if ((0D < _lum) && (_lum <= .5D)) _sat = (max - min) / (max + min);
            else if (_lum > .5D) _sat = (max - min) / (2D - (max + min));

            return new[]
            {
                Math.Max(0D, Math.Min(360D, double.Parse($"{_hue:0.##}"))),
                Math.Max(0D, Math.Min(1D, double.Parse($"{_sat:0.##}"))),
                Math.Max(0D, Math.Min(1D, double.Parse($"{_lum:0.##}")))
            };
        }


        /// <summary>
        /// Converts HSL to RGB, with a specified output Alpha.
        /// Arguments are limited to the defined range:
        /// does not raise exceptions.
        /// </summary>
        /// <param name="_hue">Hue, must be in [0, 360].</param>
        /// <param name="_sat">Saturation, must be in [0, 1].</param>
        /// <param name="_lum">Luminance, must be in [0, 1].</param>
        /// <param name="_alpha">Output Alpha, must be in [0, 255].</param>
        public static Color HslToRgb(double _hue, double _sat, double _lum, int _alpha = 255)
        {
            _hue = Math.Max(0D, Math.Min(360D, _hue));
            _sat = Math.Max(0D, Math.Min(1D, _sat));
            _lum = Math.Max(0D, Math.Min(1D, _lum));
            _alpha = Math.Max(0, Math.Min(255, _alpha));

            // achromatic argb (gray scale)
            if (Math.Abs(_sat) < _tolerance)
            {
                return
                    Color.FromArgb(
                        (byte)_alpha,
                        (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{_lum * 255D:0.00}")))),
                        (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{_lum * 255D:0.00}")))),
                        (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{_lum * 255D:0.00}"))))
                    );
            }

            double q = _lum < .5D ? _lum * (1D + _sat) : (_lum + _sat) - (_lum * _sat);
            double p = (2D * _lum) - q;

            double hk = _hue / 360D;
            double[] T = new double[3];
            T[0] = hk + (1D / 3D); // Tr
            T[1] = hk; // Tb
            T[2] = hk - (1D / 3D); // Tg

            for (int i = 0; i < 3; i++)
            {
                if (T[i] < 0D) T[i] += 1D;
                if (T[i] > 1D) T[i] -= 1D;

                if ((T[i] * 6D) < 1D) T[i] = p + ((q - p) * 6D * T[i]);
                else if ((T[i] * 2D) < 1) T[i] = q;
                else if ((T[i] * 3D) < 2) T[i] = p + ((q - p) * ((2D / 3D) - T[i]) * 6D);
                else T[i] = p;
            }

            return
                Color.FromArgb(
                    (byte)_alpha,
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{T[0] * 255D:0.00}")))),
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{T[1] * 255D:0.00}")))),
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{T[2] * 255D:0.00}"))))
                );
        }


        /// <summary>
        /// Converts RGB to HSB. Alpha is ignored.
        /// Output is: { H: [0, 360], S: [0, 1], B: [0, 1] }.
        /// </summary>
        /// <param name="color">The System.Windows.Media.Color to convert.</param>
        public static double[] RgbToHsb(Color color)
        {
            // normalize red, green and blue values
            double _red = color.R / 255D;
            double _grn = color.G / 255D;
            double _blu = color.B / 255D;

            // conversion start
            double max = Math.Max(_red, Math.Max(_grn, _blu));
            double min = Math.Min(_red, Math.Min(_grn, _blu));

            double _hue = 0D;
            if ((Math.Abs(max - _red) < _tolerance) && (_grn >= _blu)) _hue = (60D * (_grn - _blu)) / (max - min);
            else if ((Math.Abs(max - _red) < _tolerance) && (_grn < _blu)) _hue = ((60D * (_grn - _blu)) / (max - min)) + 360D;
            else if (Math.Abs(max - _grn) < _tolerance) _hue = ((60D * (_blu - _red)) / (max - min)) + 120D;
            else if (Math.Abs(max - _blu) < _tolerance) _hue = ((60D * (_red - _grn)) / (max - min)) + 240D;

            double _sat = Math.Abs(max) < _tolerance ? 0D : 1D - (min / max);

            return new[]
            {
                Math.Max(0D, Math.Min(360D, _hue)),
                Math.Max(0D, Math.Min(1D, _sat)),
                Math.Max(0D, Math.Min(1D, max))
            };
        }


        /// <summary>
        /// Converts HSB to RGB, with a specified output Alpha.
        /// Arguments are limited to the defined range:
        /// does not raise exceptions.
        /// </summary>
        /// <param name="_hue">Hue, must be in [0, 360].</param>
        /// <param name="_sat">Saturation, must be in [0, 1].</param>
        /// <param name="_brt">Brightness, must be in [0, 1].</param>
        /// <param name="_alpha">Output Alpha, must be in [0, 255].</param>
        public static Color HsbToRgb(double _hue, double _sat, double _brt, int _alpha = 255)
        {
            _hue = Math.Max(0D, Math.Min(360D, _hue));
            _sat = Math.Max(0D, Math.Min(1D, _sat));
            _brt = Math.Max(0D, Math.Min(1D, _brt));
            _alpha = Math.Max(0, Math.Min(255, _alpha));

            double _red = 0D;
            double _grn = 0D;
            double _blu = 0D;

            if (Math.Abs(_sat) < _tolerance) _red = _grn = _blu = _brt;
            else
            {
                // the argb wheel consists of 6 sectors. Figure out which sector
                // you're in.
                double sectorPos = _hue / 60D;
                int sectorNumber = (int)Math.Floor(sectorPos);
                // get the fractional part of the sector
                double fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the argb.
                double p = _brt * (1D - _sat);
                double q = _brt * (1D - (_sat * fractionalSector));
                double t = _brt * (1D - (_sat * (1D - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector
                // the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        _red = _brt;
                        _grn = t;
                        _blu = p;
                        break;
                    case 1:
                        _red = q;
                        _grn = _brt;
                        _blu = p;
                        break;
                    case 2:
                        _red = p;
                        _grn = _brt;
                        _blu = t;
                        break;
                    case 3:
                        _red = p;
                        _grn = q;
                        _blu = _brt;
                        break;
                    case 4:
                        _red = t;
                        _grn = p;
                        _blu = _brt;
                        break;
                    case 5:
                        _red = _brt;
                        _grn = p;
                        _blu = q;
                        break;
                }
            }

            return
                Color.FromArgb(
                    (byte)_alpha,
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{_red * 255D:0.00}")))),
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{_grn * 255D:0.00}")))),
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{_blu * 250D:0.00}"))))
                );
        }


        /// <summary>
        /// Multiplies the Color's Luminance or Brightness by the argument;
        /// and optionally specifies the output Alpha.
        /// </summary>
        /// <param name="color">The color to transform.</param>
        /// <param name="colorTransformMode">Transform mode.</param>
        /// <param name="brightnessTransform">The transformation multiplier.</param>
        /// <param name="outputAlpha">Can optionally specify the Alpha to directly
        /// set on the output. If null, then the input <paramref name="color"/>
        /// Alpha is used.</param>
        public static Color TransformBrightness(Color color, ColorTransformMode colorTransformMode, double brightnessTransform, byte? outputAlpha = null)
        {
            double[] hsl = colorTransformMode == ColorTransformMode.HSL ? RgbToHsl(color) : RgbToHsb(color);

            if ((Math.Abs(hsl[2]) < _tolerance) && (brightnessTransform > 1D)) hsl[2] = brightnessTransform - 1D;
            else hsl[2] *= brightnessTransform;

            return
                colorTransformMode == ColorTransformMode.HSL ?
                HslToRgb(hsl[0], hsl[1], hsl[2], outputAlpha ?? color.A) :
                HsbToRgb(hsl[0], hsl[1], hsl[2], outputAlpha ?? color.A);
        }


        /// <summary>
        /// Multiplies the Color's Saturation, and Luminance or Brightness by the argument;
        /// and optionally specifies the output Alpha.
        /// </summary>
        /// <param name="color">The color to transform.</param>
        /// <param name="colorTransformMode">Transform mode.</param>
        /// <param name="saturationTransform">The transformation multiplier.</param>
        /// <param name="brightnessTransform">The transformation multiplier.</param>
        /// <param name="outputAlpha">Can optionally specify the Alpha to directly
        /// set on the output. If null, then the input <paramref name="color"/>
        /// Alpha is used.</param>
        public static Color TransformSaturationAndBrightness(Color color, ColorTransformMode colorTransformMode, double saturationTransform, double brightnessTransform, byte? outputAlpha = null)
        {
            double[] hsl = colorTransformMode == ColorTransformMode.HSL ? RgbToHsl(color) : RgbToHsb(color);

            if ((Math.Abs(hsl[1]) < _tolerance) && (saturationTransform > 1D)) hsl[1] = saturationTransform - 1D;
            else hsl[1] *= saturationTransform;

            if ((Math.Abs(hsl[2]) < _tolerance) && (brightnessTransform > 1D)) hsl[2] = brightnessTransform - 1D;
            else hsl[2] *= brightnessTransform;

            return
                colorTransformMode == ColorTransformMode.HSL ?
                HslToRgb(hsl[0], hsl[1], hsl[2], outputAlpha ?? color.A) :
                HsbToRgb(hsl[0], hsl[1], hsl[2], outputAlpha ?? color.A);

        }


        /// <summary>
        /// Creates a new Color by combining R, G, and B from each Color, scaled by the Color's Alpha.
        /// The R, G, B of each Color is scaled by the Color's Alpha. The R, G, B of both results is
        /// then added together and divided by 2. The valuea are limited to [0, 255].
        /// The Alpha of the output Color is specified; and is also limited to [0, 255]
        /// (does not raise exceptions).
        /// </summary>
        /// <param name="color1">Combined by scaling RGB by the A.</param>
        /// <param name="color2">Combined by scaling RGB by the A.</param>
        /// <param name="outputAlpha">The Alpha of the output Color.</param>
        public static Color AlphaCombine(Color color1, Color color2, byte outputAlpha)
        {
            double a1 = color1.A / 255D;
            double a2 = color2.A / 255D;

            return
                Color.FromArgb(
                    outputAlpha,
                    (byte)Math.Max(0D, Math.Min(255D, ((color1.R * a1) + (color2.R * a2)) * .5D)),
                    (byte)Math.Max(0D, Math.Min(255D, ((color1.G * a1) + (color2.G * a2)) * .5D)),
                    (byte)Math.Max(0D, Math.Min(255D, ((color1.B * a1) + (color2.B * a2)) * .5D))
                );
        }
    
    }
}
