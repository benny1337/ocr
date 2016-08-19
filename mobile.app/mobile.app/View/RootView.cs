using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mobile.app.View
{
    public class RootView: MasterDetailPage
    {
        public RootView()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                var startview = scope.Resolve<StartView>();
                Detail = new NavigationPage(startview);
                Master = new Page();

                App.Context.Navigation = Detail.Navigation;
            }
        }
    }
}
