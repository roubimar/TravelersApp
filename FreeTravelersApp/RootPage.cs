using FreeTravelersApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace FreeTravelersApp
{
    public class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            Detail = new NavigationPage(new WelcomePage());
            Master = new MenuPage();
            ShowLoginDialog();
        }

        private async void ShowLoginDialog()
        {
            await Navigation.PushModalAsync(new LoginPage());
        }        

    }
}
