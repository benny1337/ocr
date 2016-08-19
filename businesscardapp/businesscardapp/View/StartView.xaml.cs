using businesscardapp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace businesscardapp.View
{
    public partial class StartView : ContentPage
    {
        public StartView(StartViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
