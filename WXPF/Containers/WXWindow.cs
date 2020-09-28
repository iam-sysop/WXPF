using System;
using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using WinXPresentationFoundation.NTInterop;

namespace WinXPresentationFoundation.Containers
{

    public class WXWindow : Window, IRegisterMetadata
    {

        static WXWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WXWindow), new FrameworkPropertyMetadata(typeof(WXWindow), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentArrange));
        }

        #region AttributeTable

        public void Register()
        {
            AttributeTableBuilder builder = new WXWindow_AttributeTableBuilder();
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }

        internal class WXWindow_AttributeTableBuilder : AttributeTableBuilder
        {
            internal WXWindow_AttributeTableBuilder()
            {
                AddToolboxBrowsableAttributes();
            }


            private void AddToolboxBrowsableAttributes()
            {
                var builder = new AttributeTableBuilder();
                builder.AddCustomAttributes(typeof(WXWindow), BrowsableAttribute.Yes);
                MetadataStore.AddAttributeTable(builder.CreateTable());
            }


        }

        #endregion

        public WXWindow()
        {
           

            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));

            this.WindowHandle = new WindowInteropHelper(this).EnsureHandle();

        }

        public delegate bool WindowMessageHandler(object sender, Enums.WindowMessageArgs e);

        public event WindowMessageHandler WndProc;

        public IntPtr OnReceiveMessage(IntPtr hWindow, int Message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (WndProc != null)
            {
                
                Enums.WindowMessageArgs windowMessage = new Enums.WindowMessageArgs(hWindow, Message, wParam, lParam);
                WndProc(this, windowMessage);
                if (windowMessage.Handled)
                {
                    handled = true;
                    return IntPtr.Zero;
                }
            }

            return IntPtr.Zero;
        }

        public IntPtr WindowHandle { get; private set; }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }


        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        



    }
}
