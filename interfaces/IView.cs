using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace interfaces
{
    public interface IViewModel : INotifyPropertyChanged
    {
        void OnPropertyChanged([CallerMemberName] string propertyName = null);

    }
}
