using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace mobile.app
{
    public class App : Application
    {
        public static IContainer Container { get; set; }
        public static ContainerBuilder Builder { get; set; }
        public static ApplicationContext Context { get; set; }
        public static void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<Module>();
            Container = builder.Build();
            Builder = builder;
        }

        public App()
        {
            Context = Container.Resolve<ApplicationContext>();
            MainPage = Container.Resolve<MasterDetailPage>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
