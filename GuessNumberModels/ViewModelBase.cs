using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GuessNumberModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RaisePropertyChanged<T>(Expression<Func<T>> expressionName)
        {
            if (null != expressionName)
            {
                MemberExpression me = expressionName.Body as MemberExpression;
                if (null != me)
                {
                    string name = me.Member.Name;
                    RaisePropertyChanged(name);
                }
            }
        }
    }
}
