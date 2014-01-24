using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GuessNumberCustomComponents.DialNumbers
{
    public enum KeyType
    {
        Number,
        Back,
        Ok
    };

    public class DialNumberEventArgs : EventArgs
    {
        public string Number { get; protected set; }

        public KeyType Type { get; protected set; }

        public DialNumberEventArgs(string number)
        {
            Number = number;
            Type = KeyType.Number;
        }

        public DialNumberEventArgs(string number, KeyType type)
            : this(number)
        {
            Type = type;
        }
    }

    public delegate void DialNumberEventHandler(object sender, DialNumberEventArgs e);


    [TemplatePart(Name = Border_Part, Type = typeof(Grid))]
    public class DialNumbers : Control
    {
        #region const proprety
        public const string Border_Part = "border";

        public const int Columns_Count = 3;
        public const double Number_Margin = 10;

        Grid border;

        #endregion

        #region constructor

        public DialNumbers()
        {
            this.DefaultStyleKey = typeof(DialNumbers);
        }

        #endregion

        #region private functions

        public override void OnApplyTemplate()
        {
            border = GetTemplateChild(Border_Part) as Grid;
            stack = new Stack<string>();
            Initialize();

            base.OnApplyTemplate();
        }

        protected void Initialize()
        {
            if (null == border)
                return;

            #region initialize grid

            double columnWidth = Math.Round(Application.Current.Host.Content.ActualWidth * 0.9 / Columns_Count);

            border.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            border.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(columnWidth) });
            border.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(columnWidth) });
            border.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(columnWidth) });
            border.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            border.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(columnWidth) });
            border.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(columnWidth) });
            border.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(columnWidth) });

            #endregion


            #region numbers
            for (int i = 1; i < 10; i++)
            {
                switch (i % Columns_Count)
                {
                    case 1:
                    case 2:
                        {
                            int row = i % 3 != 0 ? i / 3 + 1 : i / 3;
                            int column = (i % Columns_Count) - 1;

                            NumberTextBlock.NumberTextBlock ntk = new NumberTextBlock.NumberTextBlock();
                            ntk.Click += NumberClick;
                            ntk.Margin = new Thickness(Number_Margin);
                            ntk.SetValue(Grid.RowProperty, row);
                            ntk.SetValue(Grid.ColumnProperty, column);
                            ntk.Text = i.ToString();

                            border.Children.Add(ntk);
                        }
                        break;
                    case 0:
                        {
                            int row = i % 3 != 0 ? i / 3 + 1 : i / 3;
                            int column = Columns_Count - 1;
                            NumberTextBlock.NumberTextBlock ntk = new NumberTextBlock.NumberTextBlock();
                            ntk.Click += NumberClick;
                            ntk.Margin = new Thickness(Number_Margin);
                            ntk.SetValue(Grid.RowProperty, row);
                            ntk.SetValue(Grid.ColumnProperty, column);
                            ntk.Text = i.ToString();

                            border.Children.Add(ntk);
                        }
                        break;
                }
            }
            #endregion

            #region ok,back

            NumberTextBlock.ButtonTextBlock back = new NumberTextBlock.ButtonTextBlock();
            back.Click += BackClick;
            back.Margin = new Thickness(Number_Margin);
            back.SetValue(Grid.ColumnSpanProperty, Columns_Count);
            back.Text = "Back";
            back.Height = 60;
            border.Children.Add(back);


            NumberTextBlock.ButtonTextBlock Ok = new NumberTextBlock.ButtonTextBlock();
            Ok.Click += OkClick;
            Ok.Margin = new Thickness(Number_Margin);
            Ok.SetValue(Grid.ColumnSpanProperty, Columns_Count);
            Ok.SetValue(Grid.RowProperty, border.RowDefinitions.Count - 1);
            Ok.Text = "OK";
            Ok.Height = 60;
            border.Children.Add(Ok);

            #endregion
        }

        public event DialNumberEventHandler Dial;

        Stack<string> stack;

        protected void NumberClick(object sender, EventArgs e)
        {
            var ntk = sender as NumberTextBlock.NumberTextBlock;
            if (null != ntk)
            {
                if (null != Dial && ntk.IsEnabled &&
                    (DialTimes > 0 && stack.Count < DialTimes))
                {
                    Dial(this, new DialNumberEventArgs(ntk.Text));
                    stack.Push(ntk.Text);
                    ntk.IsEnabled = false;
                }
            }
        }

        protected void BackClick(object sender, EventArgs e)
        {
            if (stack.Count > 0)
            {
                string number = stack.Pop();
                ResetNumber(number);

                if (null != Dial)
                {
                    Dial(this, new DialNumberEventArgs(number, KeyType.Back));
                }
            }
        }

        protected void OkClick(object sender, EventArgs e)
        {
            if (null != Dial)
            {
                Dial(this, new DialNumberEventArgs(GetStackString(), KeyType.Ok));
            }
        }

        protected string GetStackString()
        {
            int result = 0;
            int i = 0;
            while (i < stack.Count)
            {
                result += ((int)Math.Pow(10, i)) * int.Parse(stack.ElementAt(i));
                i++;
            }
            return result.ToString();
        }

        #endregion

        #region public interface

        public void Reset()
        {
            stack = new Stack<string>();

            if (null != border)
            {
                var disableButtons = from btn in border.Children where !(btn as Control).IsEnabled select btn;
                foreach (Control btn in disableButtons)
                {
                    btn.IsEnabled = true;
                }
            }
        }

        public void ResetNumber(string number)
        {
            var btns = from btn in border.Children
                       where (btn as NumberTextBlock.NumberTextBlock).Text.Equals(number)
                       select btn;

            foreach (Control btn in btns)
            {
                btn.IsEnabled = true;
            }
        }

        #endregion

        #region DialTimes

        public static readonly DependencyProperty DialTimesProperty = DependencyProperty.Register("DialTimes", typeof(int),
                                                                                                    typeof(DialNumbers),
                                                                                                    new PropertyMetadata(4));
        public int DialTimes
        {
            get
            {
                return (int)GetValue(DialTimesProperty);
            }
            set
            {
                if (value >= 0)
                {
                    SetValue(DialTimesProperty, value);
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion
    }
}
