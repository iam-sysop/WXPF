using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Activities.Presentation.Metadata;



namespace WinXPresentationFoundation.Controls
{


    [TemplatePart(Name = "PART_ValueBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public class NumericUpDown : Control, IRegisterMetadata
    {
        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentArrange));
        }

        public NumericUpDown()
        {

            Culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            Culture.NumberFormat.NumberDecimalDigits = DecimalPlaces;
            Loaded += OnLoaded;

        }

        #region AttributeTable

        public void Register()
        {
            AttributeTableBuilder builder = new NumericUpDown_AttributeTableBuilder();
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }

        internal class NumericUpDown_AttributeTableBuilder : AttributeTableBuilder
        {
            internal NumericUpDown_AttributeTableBuilder()
            {
                AddToolboxBrowsableAttributes();
            }


            private void AddToolboxBrowsableAttributes()
            {
                var builder = new AttributeTableBuilder();
                builder.AddCustomAttributes(typeof(NumericUpDown), BrowsableAttribute.Yes);
                MetadataStore.AddAttributeTable(builder.CreateTable());
            }


        }

        #endregion

        #region Defaults

        #region TextBox
        private TextBox DefaultTextBox = new TextBox()
        {
            Name = "PART_ValueBox",
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Right,
            SnapsToDevicePixels = true
        };
        #endregion

        #region ButtonGlyphs

        private readonly static Path DefaultGlyphIncrease = new Path()
        {
            Margin = new Thickness(1),
            Data = Geometry.Parse("M 0 20 L 35 -20 L 70 20 Z"),
            Fill = new SolidColorBrush() { Color = Color.FromArgb(255, 32, 32, 32) },
            Stretch = Stretch.Uniform,
            SnapsToDevicePixels = true

        };

        private readonly static Path DefaultGlyphDecrease = new Path()
        {
            Margin = new Thickness(1),
            Data = Geometry.Parse("M 0 0 L 35 40 L 70 0 Z"),
            Fill = new SolidColorBrush() { Color = Color.FromArgb(255, 32, 32, 32) },
            Stretch = Stretch.Uniform,
            SnapsToDevicePixels = true
        };

        #endregion

        #region ButtonUpDown

        private static RepeatButton DefaultButtonIncrease = new RepeatButton()
        {
            Name = "PART_IncreaseButton",
            Width = 20,
            Margin = new Thickness(1, 1, 0, 0),
            Content = DefaultGlyphIncrease,
            SnapsToDevicePixels = true
        };

        private static RepeatButton DefaultButtonDecrease = new RepeatButton()
        {
            Name = "PART_DecreaseButton",
            Width = 20,
            Margin = new Thickness(1, 0, 0, 1),
            Content = DefaultGlyphDecrease,
            SnapsToDevicePixels = true
        };

        #endregion

        #region DefaultProperties

        private static readonly decimal DefaultValue = 1;
        private static readonly int DefaultDecimalPlaces = 0;
        private static readonly decimal DefaultMinValue = 0;
        private static readonly decimal DefaultMaxValue = 10;
        private static readonly decimal DefaultIncrement = 1;

        #endregion

        #endregion

        #region Fields

        protected TextBox ValueBox;
        protected RepeatButton IncreaseBtn;
        protected RepeatButton DecreaseBtn;
        protected readonly CultureInfo Culture;

        #endregion


        #region Properties

        #region DecimalPlaces
        [EditorBrowsable(EditorBrowsableState.Always)]

        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }
        private static void OnDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)element;
            var decimalPlaces = (int)e.NewValue;

            control.Culture.NumberFormat.NumberDecimalDigits = decimalPlaces;
            control.InvalidateProperty(ValueProperty);
        }

        private static object CoerceDecimalPlaces(DependencyObject element, Object baseValue)
        {
            var decimalPlaces = (int)baseValue;

            if (decimalPlaces < 0)
            {
                decimalPlaces = 0;
            }
            else if (decimalPlaces > 28)
            {
                decimalPlaces = 28;
            }

            return decimalPlaces;
        }


        #endregion

        #region Increment

        public decimal Increment
        {
            get { return (decimal)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }
        #endregion

        #region Value      

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown control = (NumericUpDown)element;
            if (control != null && control.ValueBox != null)
            {
                control.ValueBox.UndoLimit = 0;
                control.ValueBox.UndoLimit = 1;
            }

        }

        private static object CoerceValue(DependencyObject element, object baseValue)
        {
            var control = (NumericUpDown)element;
            var value = (decimal)baseValue;

            control.CoerceValueToBounds(ref value);

            var valueString = value.ToString(control.Culture);


            var decimalPlaces = control.GetDecimalPlacesCount(valueString);
            if (decimalPlaces > control.DecimalPlaces)
            {
                value = control.TruncateDecimalValue(valueString, decimalPlaces);
            }

            if (control.ValueBox != null)
            {
                if (control.IsThousandSeparatorVisible)
                {
                    control.ValueBox.Text = value.ToString("N", control.Culture);
                }
                else
                {
                    control.ValueBox.Text = value.ToString("F", control.Culture);
                }
            }
            return value;
        }

        #endregion

        #region MaxValue

        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        private static void OnMaxValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)element;
            var maxValue = (decimal)e.NewValue;

            // If maxValue steps over MinValue, shift it
            if (maxValue < control.MinValue)
            {
                control.MinValue = maxValue;
            }
        }

        private static object CoerceMaxValue(DependencyObject element, Object baseValue)
        {
            var maxValue = (decimal)baseValue;
            return maxValue;
        }

        #endregion

        #region MinValue
        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        private static void OnMinValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)element;
            var minValue = (decimal)e.NewValue;

            // If minValue steps over MaxValue, shift it
            if (minValue > control.MaxValue)
            {
                control.MaxValue = minValue;
            }

        }

        private static object CoerceMinValue(DependencyObject element, Object baseValue)
        {
            var minValue = (decimal)baseValue;
            return minValue;
        }
        #endregion

        #region ThousandsSeperator


        public bool IsThousandSeparatorVisible
        {
            get { return (bool)GetValue(IsThousandSeparatorVisibleProperty); }
            set { SetValue(IsThousandSeparatorVisibleProperty, value); }
        }

        private static void OnIsThousandSeparatorVisibleChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)element;

            control.InvalidateProperty(ValueProperty);
        }

        #endregion

        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(DefaultIncrement));
        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(NumericUpDown), new PropertyMetadata(DefaultDecimalPlaces, OnDecimalPlacesChanged, CoerceDecimalPlaces));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(DefaultValue, OnValueChanged, CoerceValue));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(DefaultMaxValue, OnMaxValueChanged, CoerceMaxValue));
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(DefaultMinValue, OnMinValueChanged, CoerceMinValue));

        public static readonly DependencyProperty IsThousandSeparatorVisibleProperty = DependencyProperty.Register("IsThousandSeparatorVisible", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false, OnIsThousandSeparatorVisibleChanged));

        #endregion


        #region RoutingAndCommands

        private readonly RoutedUICommand _cmdIncreaseValue = new RoutedUICommand("IncreaseValue", "IncreaseValue", typeof(NumericUpDown));
        private readonly RoutedUICommand _cmdDecreaseValue = new RoutedUICommand("DecreaseValue", "DecreaseValue", typeof(NumericUpDown));
        private readonly RoutedUICommand _cmdUpdateValueString = new RoutedUICommand("UpdateValueString", "UpdateValueString", typeof(NumericUpDown));

        private readonly RoutedUICommand _cmdCancelChanges = new RoutedUICommand("CancelChanges", "CancelChanges", typeof(NumericUpDown));

        private void AttachCommands()
        {
            IncreaseBtn.CommandBindings.Add(new CommandBinding(_cmdIncreaseValue, (a, b) => IncreaseValue()));
            DecreaseBtn.CommandBindings.Add(new CommandBinding(_cmdDecreaseValue, (a, b) => DecreaseValue()));
            ValueBox.CommandBindings.Add(new CommandBinding(_cmdUpdateValueString, (a, b) => { Value = ToDecimal(ValueBox.Text); }));
            ValueBox.CommandBindings.Add(new CommandBinding(_cmdCancelChanges, (a, b) => CancelChanges()));

            ValueBox.InputBindings.Add(new KeyBinding(_cmdIncreaseValue, new KeyGesture(Key.Up)));
            ValueBox.InputBindings.Add(new KeyBinding(_cmdDecreaseValue, new KeyGesture(Key.Down)));
            ValueBox.InputBindings.Add(new KeyBinding(_cmdUpdateValueString, new KeyGesture(Key.Enter)));
            ValueBox.InputBindings.Add(new KeyBinding(_cmdCancelChanges, new KeyGesture(Key.Escape)));
        }
        #endregion


        #region Overrides

        public override void OnApplyTemplate()
        {
            
            
            AttachValueBox();
            AttachIncreaseBtn();
            AttachDecreaseBtn();
            AttachCommands();

            base.OnApplyTemplate();

        }

        #endregion

        #region ValueMethods

        private void IncreaseValue()
        {
            decimal _value = ToDecimal(ValueBox.Text);
            CoerceValueToBounds(ref _value);

            if (_value <= MaxValue && (_value + Increment) <= MaxValue)
            {
                Value = _value + Increment;
            }
            else
            {
                Value = MaxValue;
            }
        }

        private void DecreaseValue()
        {
            decimal _value = ToDecimal(ValueBox.Text);
            CoerceValueToBounds(ref _value);

            if (_value >= MinValue && (_value - Increment) >= MinValue)
            {
                Value = _value - Increment;
            }
            else
            {
                Value = MinValue;
            }
        }

        private void CoerceValueToBounds(ref decimal value)
        {
            if (value < MinValue)
            {
                value = MinValue;
            }
            else if (value > MaxValue)
            {
                value = MaxValue;
            }
        }

        #endregion


        #region UtilityMethods

        private void AttachValueBox()
        {
            TextBox _test = (TextBox)GetTemplateChild("PART_ValueBox");
            if (_test != null)
            {
                ValueBox = _test;
            }
            else
            {                
                ValueBox = DefaultTextBox;
                
            }

            ValueBox.LostFocus += ValueBoxOnLostFocus;
            ValueBox.UndoLimit = 1;
            ValueBox.IsUndoEnabled = true;
        }

        private void AttachIncreaseBtn()
        {
            RepeatButton _test = (RepeatButton)GetTemplateChild("PART_IncreaseButton");

            if (_test != null)
            {
                IncreaseBtn = _test;
            }
            else
            {
                IncreaseBtn = DefaultButtonIncrease;
            }
            IncreaseBtn.Focusable = false;
            IncreaseBtn.Command = _cmdIncreaseValue;
            IncreaseBtn.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();

        }

        private void AttachDecreaseBtn()
        {
            RepeatButton _test = (RepeatButton)GetTemplateChild("PART_DecreaseButton");

            if (_test != null)
            {
                DecreaseBtn = _test;

            }
            else
            {
                DecreaseBtn = DefaultButtonDecrease;
            }
            DecreaseBtn.Focusable = false;
            DecreaseBtn.Command = _cmdDecreaseValue;
            DecreaseBtn.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
        }

        private void ValueBoxOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            Value = ToDecimal(ValueBox.Text);
        }

        private void CancelChanges()
        {
            ValueBox.Undo();
        }

        private void RemoveFocus()
        {
            Focusable = true;
            Focus();
            Focusable = false;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {

            InvalidateProperty(ValueProperty);
        }

        private decimal ToDecimal(object o)
        {
            Decimal.TryParse(o.ToString(), out decimal _value);
            return _value;
        }

        private int GetDecimalPlacesCount(string valueString)
        {
            return valueString.SkipWhile(c => c.ToString(Culture) != Culture.NumberFormat.NumberDecimalSeparator).Skip(1).Count();
        }

        private decimal TruncateDecimalValue(string valueString, int decimalPlaces)
        {
            var endPoint = valueString.Length - (decimalPlaces - DecimalPlaces);
            var tempValueString = valueString.Substring(0, endPoint);

            return decimal.Parse(tempValueString, Culture);
        }

        #endregion




    }


}
