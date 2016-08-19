using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using businesscardapp.View;
using businesscardapp.ViewModel;
using businesscardapp.Communicator;

namespace businesscardapp
{
    class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RootView>().As<MasterDetailPage>().SingleInstance();
            builder.RegisterType<ApplicationContext>().SingleInstance().PropertiesAutowired();

            builder.RegisterType<CardCommunicator>();

            //views
            builder.RegisterType<StartView>();

            //viewmodels
            builder.RegisterType<StartViewModel>();
        }
    }
}
