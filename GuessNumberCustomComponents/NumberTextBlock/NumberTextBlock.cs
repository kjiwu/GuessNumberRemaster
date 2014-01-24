using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GuessNumberCustomComponents.NumberTextBlock
{
    [TemplateVisualState(Name = NormalState, GroupName = GroupNameState)]
    [TemplateVisualState(Name = ClickState, GroupName = GroupNameState)]
    [TemplateVisualState(Name = DisableState, GroupName = GroupNameState)]
    [TemplatePart(Name = Background_Part, Type = typeof(Ellipse))]
    [TemplatePart(Name = Content_Part, Type = typeof(TextBlock))]
    public class NumberTextBlock : Control
    {
        public const string GroupNameState  = "Base";
        public const string NormalState = "Normal";
        public const string ClickState = "Click";
        public const string DisableState = "Disable";

        public const string Background_Part = "border";
        public const string Content_Part = "tbkContent";

        Ellipse border;
        TextBlock content;

        public NumberTextBlock()
        {
            this.DefaultStyleKey = typeof(NumberTextBlock);

            this.IsEnabledChanged += NumberTextBlock_IsEnabledChanged;
        }

        void NumberTextBlock_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsEnabled)
            {
                VisualStateManager.GoToState(this, NormalState, true);
            }
            else
            {
                VisualStateManager.GoToState(this, DisableState, true);
            }
        }

        public override void OnApplyTemplate()
        {
            this.FontSize = 24;

            border = GetTemplateChild(Background_Part) as Ellipse;
            if (null != border)
            {
                border.MouseLeftButtonDown -= ControlMouseLeftButtonDown;
                border.MouseLeftButtonDown += ControlMouseLeftButtonDown;

                border.MouseLeftButtonUp -= ControlMouseLeftButtonUp;
                border.MouseLeftButtonUp += ControlMouseLeftButtonUp;

                border.MouseLeave -= ControlMouseLeave;
                border.MouseLeave += ControlMouseLeave;
            }

            content = GetTemplateChild(Content_Part) as TextBlock;
            if (null != content)
            {
                content.MouseLeftButtonDown -= ControlMouseLeftButtonDown;
                content.MouseLeftButtonDown += ControlMouseLeftButtonDown;

                content.MouseLeftButtonUp -= ControlMouseLeftButtonUp;
                content.MouseLeftButtonUp += ControlMouseLeftButtonUp;

                content.MouseLeave -= ControlMouseLeave;
                content.MouseLeave += ControlMouseLeave;
            }

            base.OnApplyTemplate();
        }

        protected void ControlMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            VisualStateManager.GoToState(this, ClickState, true);
            if (null != Click)
            {
                Click(this, new EventArgs());
            }
        }

        protected void ControlMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            VisualStateManager.GoToState(this, NormalState, true);
        }

        protected void ControlMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, NormalState, true);
        }

        public event EventHandler Click;

        #region StrokeThickness

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double),
                                                                                                          typeof(NumberTextBlock), new PropertyMetadata(2D));

        public double StrokeThickness
        {
            get
            {
                return (double)GetValue(StrokeThicknessProperty);
            }
            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }

        #endregion

        #region Stroke

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush),
                                                                                                 typeof(NumberTextBlock),
                                                                                                 new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush Stroke
        {
            get
            {
                return (Brush)GetValue(StrokeProperty);
            }
            set
            {
                SetValue(StrokeProperty, value);
            }
        }

        #endregion

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string),
                                                                                               typeof(NumberTextBlock),
                                                                                               new PropertyMetadata("1"));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        #endregion
    }
}
