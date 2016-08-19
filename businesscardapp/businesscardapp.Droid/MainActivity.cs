using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using FFImageLoading.Forms.Droid;
using FFImageLoading;
using Autofac;
using businesscardapp.Droid.PlatformSpecifics;
using interfaces;

namespace businesscardapp.Droid
{
    [Activity(Label = "businesscardapp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity :  FormsAppCompatActivity/*global::Xamarin.Forms.Platform.Android.FormsApplicationActivity*/
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            App.Init();

            //FFImaging initialization
            CachedImageRenderer.Init();
            ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration
            {
                LoadWithTransparencyChannel = false,
                MaxMemoryCacheSize = 2048,                
                FadeAnimationEnabled = false,
                FadeAnimationForCachedImages = false
            });

            var newbuilder = new ContainerBuilder();
            newbuilder.RegisterType<DialogService>().As<IUserDialogService>();

            newbuilder.Update(App.Container);
            App.Builder = newbuilder;

            LoadApplication(new App());
        }
    }
}

