using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfaces
{
    public interface IUserDialogService
    {
        void HideDialog();
        void ShowLoading(Action onClick = null, string title = "Loading");
        void Toast(string message, Action onClick = null, double timeoutSeconds = 3);
    }
}
