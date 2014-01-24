using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace GuessNumberCustomComponents.Popups
{
    public class GuessResultPopup : Control
    {
        Popup popup;
        Grid container;

        public GuessResultPopup()
        {
            this.DefaultStyleKey = typeof(GuessResultPopup);
            popup = new Popup();
            container = new Grid();
            InitContainer();

            popup.Child = container;
        }

        private void InitContainer()
        {
            container.Width = Application.Current.Host.Content.ActualWidth;
            container.Height = Application.Current.Host.Content.ActualHeight;

            container.Background = new SolidColorBrush(Colors.Black);
            container.Opacity = 0.8;

            Grid content = new Grid();
            content.Background = new SolidColorBrush(Colors.Orange);
            content.Height = 200;
            content.Width = 300;
            content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            container.Children.Add(content);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public void Show()
        {
            if (null != popup)
            {
                popup.IsOpen = true;
            }
        }

        public void Close()
        {
            if (null != popup && !popup.IsOpen)
            {
                popup.IsOpen = false;
            }
        }
    }
}
