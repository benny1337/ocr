using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using FFImageLoading.Forms.Droid;
using Autofac;
using mobile.app.Droid.PlatformSpecifics;
using interfaces;

namespace mobile.app.Droid
{
    [Activity(Label = "mobile.app", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            //FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            App.Init();

            CachedImageRenderer.Init();
            FFImageLoading.ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration
            {
                LoadWithTransparencyChannel = false,
                FadeAnimationEnabled = false,
                FadeAnimationForCachedImages = false,
                MaxMemoryCacheSize = 2048
            });

            var newbuilder = new ContainerBuilder();
            newbuilder.RegisterType<UserDialogServiceAndroid>().As<IUserDialogService>();
            //newbuilder.RegisterType<NetworkConnectivity>().As<INetworkConnectivity>().SingleInstance();            
            //newbuilder.RegisterType<CameraProvider>().As<ICameraProvider>();

            newbuilder.Update(App.Container);
            App.Builder = newbuilder;

            LoadApplication(new App());
        }
    }
}

