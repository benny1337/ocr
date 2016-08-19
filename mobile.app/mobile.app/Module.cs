using Autofac;
using mobile.app.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mobile.app
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RootView>().As<MasterDetailPage>().SingleInstance();
            builder.RegisterType<ApplicationContext>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<StartView>();
        }
    }
}