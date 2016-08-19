using interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace businesscardapp.ViewModel
{
    public class ViewModelBase : IViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _isAppLoading;

        public bool IsAppLoading
        {
            get
            {
                return _isAppLoading;
            }
            set
            {
                _isAppLoading = value;
                OnPropertyChanged();
            }
        }

    }
}
