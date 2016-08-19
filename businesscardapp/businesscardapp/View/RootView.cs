using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace businesscardapp.View
{
    public class RootView: MasterDetailPage
    {
        public RootView()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                var view = scope.Resolve<StartView>();
                Master = new Page()
                {
                    Title = "businesscardapp"
                };
                Detail = new NavigationPage(view);
                App.Context.Navigation = Detail.Navigation;
            }
        }
    }
}
