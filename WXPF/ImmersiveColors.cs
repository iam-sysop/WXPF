using System.Windows.Media;

using WinNT = WinXPresentationFoundation.NTInterop.Win32;

namespace WinXPresentationFoundation.ImmersiveColors
{


    public class BasePalette
    {
        public Color BlackSolid => Color.FromArgb(255, 0, 0, 0);
        public Color BlackHigh => Color.FromArgb(204, 0, 0, 0);
        public Color BlackMedium => Color.FromArgb(153, 0, 0, 0);
        public Color BlackLow => Color.FromArgb(102, 0, 0, 0);
        public Color BlackMild => Color.FromArgb(51, 0, 0, 0);

        public Color BlackLight => ColorHelper.TransformBrightness(BlackSolid, ColorHelper.ColorTransformMode.HSB, 1.1, 255);
        public Color BlackLighter => ColorHelper.TransformBrightness(BlackSolid, ColorHelper.ColorTransformMode.HSB, 1.2, 255);
        public Color BlackLightest => ColorHelper.TransformBrightness(BlackSolid, ColorHelper.ColorTransformMode.HSB, 1.4, 255);

        public Color WhiteSolid => Color.FromArgb(255, 255, 255, 255);
        public Color WhiteHigh => Color.FromArgb(204, 255, 255, 255);
        public Color WhiteMedium => Color.FromArgb(153, 255, 255, 255);
        public Color WhiteLow => Color.FromArgb(102, 255, 255, 255);
        public Color WhiteMild => Color.FromArgb(51, 255, 255, 255);

        public Color WhiteDark => ColorHelper.TransformBrightness(WhiteSolid, ColorHelper.ColorTransformMode.HSB, 0.9, 255);
        public Color WhiteDarker => ColorHelper.TransformBrightness(WhiteSolid, ColorHelper.ColorTransformMode.HSB, 0.8, 255);
        public Color WhiteDarkest => ColorHelper.TransformBrightness(WhiteSolid, ColorHelper.ColorTransformMode.HSB, 0.6, 255);

        public Color GraySolid => Color.FromArgb(255, 128, 128, 128);
        public Color GrayHigh => Color.FromArgb(204, 128, 128, 128);
        public Color GrayMedium => Color.FromArgb(153, 128, 128, 128);
        public Color GrayLow => Color.FromArgb(102, 128, 128, 128);
        public Color GrayMild => Color.FromArgb(51, 128, 128, 128);

        public Color GrayLightest => ColorHelper.TransformBrightness(GraySolid, ColorHelper.ColorTransformMode.HSB, 1.2, 255);
        public Color GrayLight => ColorHelper.TransformBrightness(GraySolid, ColorHelper.ColorTransformMode.HSB, 1.1, 255);
        public Color GrayDark => ColorHelper.TransformBrightness(GraySolid, ColorHelper.ColorTransformMode.HSB, 0.9, 255);
        public Color GrayDarkest => ColorHelper.TransformBrightness(GraySolid, ColorHelper.ColorTransformMode.HSB, 0.8, 255);

        public Color AccentSolid => Color.FromArgb(255, _accentColorBase.R, _accentColorBase.G, _accentColorBase.B);
        public Color AccentHigh => Color.FromArgb(204, _accentColorBase.R, _accentColorBase.G, _accentColorBase.B);
        public Color AccentMedium => Color.FromArgb(153, _accentColorBase.R, _accentColorBase.G, _accentColorBase.B);
        public Color AccentLow => Color.FromArgb(102, _accentColorBase.R, _accentColorBase.G, _accentColorBase.B);
        public Color AccentMild => Color.FromArgb(51, _accentColorBase.R, _accentColorBase.G, _accentColorBase.B);

        public Color AccentLightest => ColorHelper.TransformBrightness(AccentSolid, ColorHelper.ColorTransformMode.HSB, 1.4, 255);
        public Color AccentLighter => ColorHelper.TransformBrightness(AccentSolid, ColorHelper.ColorTransformMode.HSB, 1.2, 255);
        public Color AccentLight => ColorHelper.TransformBrightness(AccentSolid, ColorHelper.ColorTransformMode.HSB, 1.1, 255);
        public Color AccentDark => ColorHelper.TransformBrightness(AccentSolid, ColorHelper.ColorTransformMode.HSB, 0.9, 255);
        public Color AccentDarker => ColorHelper.TransformBrightness(AccentSolid, ColorHelper.ColorTransformMode.HSB, 0.8, 255);
        public Color AccentDarkest => ColorHelper.TransformBrightness(AccentSolid, ColorHelper.ColorTransformMode.HSB, 0.6, 255);

        public Color DisabledHigh => Color.FromArgb(255, 176, 176, 176);
        public Color DisabledSolid => Color.FromArgb(255, 128, 128, 128);
        public Color DisabledLow => Color.FromArgb(255, 102, 102, 102);

        public Color ErrorSolid => Color.FromArgb(255, 255, 0, 0);
        public Color ErrorHigh => ColorHelper.TransformBrightness(ErrorSolid, ColorHelper.ColorTransformMode.HSB, 1.1);
        public Color ErrorLow => ColorHelper.TransformBrightness(ErrorSolid, ColorHelper.ColorTransformMode.HSB, 0.9);

        private static readonly Color _accentColorBase = WinNT.FromWin32Color(WinNT.HasRegValue("HKCU", "SOFTWARE\\Microsoft\\Windows\\DWM", "AccentColor", out dynamic _accentColor) ? _accentColor : 0xFFB16300);

    }

        

    public class ColorPalette
    {

        public Color ActiveBorderColor { get; set; }

        public Color ActiveCaptionColor { get; set; }
        public Color ActiveCaptionTextColor { get; set; }

        public Color AppWorkspaceColor { get; set; }

        public Color ControlColor { get; set; }

        public Color ControlDarkColor { get; set; }
        public Color ControlDarkDarkColor { get; set; }
        public Color ControlLightColor { get; set; }
        public Color ControlLightLightColor { get; set; }

        public Color ControlTextColor { get; set; }

        public Color DesktopColor { get; set; }

        public Color GradientActiveCaptionColor { get; set; }
        public Color GradientInactiveCaptionColor { get; set; }

        public Color GrayTextColor { get; set; }
        public Color HighlightColor { get; set; }
        public Color HighlightTextColor { get; set; }
        public Color HotTrackColor { get; set; }

        public Color InactiveBorderColor { get; set; }

        public Color InactiveCaptionColor { get; set; }
        public Color InactiveCaptionTextColor { get; set; }

        public Color InfoColor { get; set; }
        public Color InfoTextColor { get; set; }

        public Color MenuColor { get; set; }
        public Color MenuBarColor { get; set; }
        public Color MenuHighlightColor { get; set; }
        public Color MenuTextColor { get; set; }

        public Color ScrollBarColor { get; set; }

        public Color WindowColor { get; set; }
        public Color WindowFrameColor { get; set; }
        public Color WindowTextColor { get; set; }



        public ColorPalette(bool DarkMode = false)
        {

            BasePalette _basePalette = new BasePalette();

            ActiveBorderColor = DarkMode ? _basePalette.AccentSolid : _basePalette.AccentLightest;

            ActiveCaptionColor = DarkMode ? _basePalette.BlackLighter : _basePalette.AccentLightest;
            ActiveCaptionTextColor = DarkMode ? _basePalette.WhiteSolid : _basePalette.BlackSolid;

            AppWorkspaceColor = DarkMode ? _basePalette.BlackSolid : _basePalette.WhiteSolid;

            ControlColor = DarkMode ? _basePalette.BlackLighter : _basePalette.WhiteDarker;

            ControlDarkColor = DarkMode ? _basePalette.GrayLight : _basePalette.GrayDark;
            ControlDarkDarkColor = DarkMode ? _basePalette.GrayLightest : _basePalette.GrayDarkest;
            ControlLightColor = DarkMode ? _basePalette.GrayDark : _basePalette.GrayLight;
            ControlLightLightColor = DarkMode ? _basePalette.GrayDarkest : _basePalette.GrayLightest;

            ControlTextColor = DarkMode ? _basePalette.WhiteDark : _basePalette.BlackLight;

            DesktopColor = DarkMode ? _basePalette.BlackSolid : _basePalette.AccentDarkest;

            GradientActiveCaptionColor = _basePalette.AccentLighter;
            GradientInactiveCaptionColor = _basePalette.AccentDarkest;

            GrayTextColor = DarkMode ? _basePalette.BlackMedium : _basePalette.WhiteMedium;
            HighlightColor = _basePalette.AccentMedium;
            HighlightTextColor = DarkMode ? _basePalette.WhiteHigh : _basePalette.BlackHigh;
            HotTrackColor = _basePalette.AccentSolid;

            InactiveBorderColor = _basePalette.GrayMedium;

            InactiveCaptionColor = DarkMode ? _basePalette.BlackLight : _basePalette.WhiteDark;
            InactiveCaptionTextColor = _basePalette.DisabledSolid;

            InfoColor = DarkMode ? _basePalette.GrayDarkest : _basePalette.GrayLightest;
            InfoTextColor = DarkMode ? _basePalette.WhiteHigh : _basePalette.BlackHigh;

            MenuColor = DarkMode ? _basePalette.BlackLight : _basePalette.WhiteDark;
            MenuBarColor = DarkMode ? _basePalette.BlackLighter : _basePalette.WhiteDarker;
            MenuHighlightColor = DarkMode ? _basePalette.GrayMild : _basePalette.AccentMedium;
            MenuTextColor = DarkMode ? _basePalette.WhiteHigh : _basePalette.BlackHigh;

            ScrollBarColor = DarkMode ? _basePalette.BlackLighter : _basePalette.WhiteDarker;

            WindowColor = DarkMode ? _basePalette.BlackLight : _basePalette.WhiteDark;
            WindowFrameColor = _basePalette.AccentSolid;
            WindowTextColor = DarkMode ? _basePalette.WhiteDarker : _basePalette.BlackLight;

        }

    }

}

