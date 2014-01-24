using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GuessNumberCustomComponents.NumberTextBlock
{
    [TemplatePart(Name = Background_Part, Type = typeof(Border))]
    public class ButtonTextBlock : NumberTextBlock
    {
        Stack<string> stack;

        Border border;

        public ButtonTextBlock()
        {
            this.DefaultStyleKey = typeof(ButtonTextBlock);
            stack = new Stack<string>();
        }

        public override void OnApplyTemplate()
        {
            border = GetTemplateChild(Background_Part) as Border;
            if (null != border)
            {
                border.MouseLeftButtonDown -= ControlMouseLeftButtonDown;
                border.MouseLeftButtonDown += ControlMouseLeftButtonDown;

                border.MouseLeftButtonUp -= ControlMouseLeftButtonUp;
                border.MouseLeftButtonUp += ControlMouseLeftButtonUp;

                border.MouseLeave -= ControlMouseLeave;
                border.MouseLeave += ControlMouseLeave;
            }

            BorderThickness = new System.Windows.Thickness(2);
            base.OnApplyTemplate();
        }

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
                                                                                                       typeof(ButtonTextBlock),
                                                                                                       new PropertyMetadata(new CornerRadius(15)));

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        #endregion
    }
}
