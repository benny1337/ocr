using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using interfaces;
using AndroidHUD;

namespace businesscardapp.Droid.PlatformSpecifics
{
    public class DialogService : IUserDialogService
    {
        public void HideDialog()
        {
            AndHUD.Shared.Dismiss(Xamarin.Forms.Forms.Context);
        }

        public void ShowLoading(Action onClick = null, string title = "Loading")
        {
            AndHUD.Shared.Show(Xamarin.Forms.Forms.Context, title, -1, MaskType.Clear, new TimeSpan(0,3,0), onClick);
        }

        public void Toast(string message, Action onClick = null, double timeoutSeconds = 3)
        {
            if (onClick == null)
                onClick = new Action(() => { HideDialog(); });

            AndHUD.Shared.ShowToast(Xamarin.Forms.Forms.Context, message, clickCallback: onClick);
        }
    }
}