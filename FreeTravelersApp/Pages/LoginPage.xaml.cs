using System;
using FreeTravelersApp.ViewModels;
using Xamarin.Forms;

namespace FreeTravelersApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginViewModel LoginModel { get; set; }

        public LoginPage()
        {
            LoginModel = new LoginViewModel(Navigation);
            InitializeComponent();
            BindingContext = LoginModel;
            ErrorText.SetBinding(Label.TextProperty, LoginViewModel.ErrorPropertyName);
            LoginEntry.SetBinding(Entry.TextProperty, LoginViewModel.UsernamePropertyName);
            PasswordEntry.SetBinding(Entry.TextProperty, LoginViewModel.PasswordPropertyName);
            LoginButton.SetBinding(Button.CommandProperty, LoginViewModel.LoginCommandPropertyName);
        }
        
        
    }
}
