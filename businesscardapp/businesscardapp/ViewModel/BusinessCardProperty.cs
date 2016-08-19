using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace businesscardapp.ViewModel
{
    public class BusinessCardProperty:ViewModelBase
    {
        public string Label { get; set; }
        public string Name { get; set; }

        private string _data;
        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("IsClearVisible");
                OnPropertyChanged();
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged();
                OnPropertyChanged("ButtonSource");
                OnPropertyChanged("ButtonOpacity");
            }
        }

        public string ButtonSource
        {
            get
            {
                return "swap.png";
            }
        }

        public double ButtonOpacity
        {
            get
            {
                return IsActive ? 0.54 : 0.26;
            }
        }

        private Command _clear;    
        public Command Clear
        {
            get
            {
                return _clear ?? (_clear = new Command(() => 
                {
                    Data = "";
                }));
            }
        }

        public bool IsClearVisible
        {
            get
            {
                return !string.IsNullOrEmpty(Data);
            }
        }
    }
}
