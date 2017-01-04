using FreeTravelersApp.DataClass;
using FreeTravelersApp.Pages;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FreeTravelersApp.Pages
{
    public partial class WelcomePage : ContentPage
    {
        public int rowCount = 3;
        public int columnCount = 2;

        public WelcomePage()
        {
            InitializeComponent();
            var commands = InitializeCommands();

            var grid = new Grid();
            
            for (int i = 0; i < commands.Count; i++)
            {
                var imageButton = new XLabs.Forms.Controls.ImageButton()
                {
                    Orientation = XLabs.Enums.ImageOrientation.ImageOnTop,
                    BackgroundColor = Color.White,
                    Source = commands[i].Icon,
                    Text = commands[i].Capture,
                    ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Bottom, 100),
                    //Command = new XLabs.RelayCommand(() => DisplayAlert("Hello", "World", "Click"))
                    Command = new XLabs.RelayCommand(commands[i].Command)
                };
                var r = i % rowCount;
                var c = (i - r) % columnCount;
                grid.Children.Add(imageButton, c, r);
            }

            grid.Padding = new Thickness(20);
            Content = grid;
        }

        private List<CommandButton> InitializeCommands()
        {
            var icons = new[] { "about.png", "guides.png", "help.png", "message.png", "profile.png", "search.png" };
            var captures = new[] { "Podmínky", "Průvodci", "Nápověda", "Zprávy", "Profil", "Hledat" };

            var commands = new List<CommandButton>()
            {
                new CommandButton() { Icon = "about.png", Capture = "Podmínky", Command = () => Navigation.PushAsync(new ConditionsPage()) },
                new CommandButton() { Icon = "guides.png", Capture = "Průvodci", Command = () => Navigation.PushAsync(new ConditionsPage()) },
                new CommandButton() { Icon = "help.png", Capture = "Nápověda", Command = () => Navigation.PushAsync(new ConditionsPage()) },
                new CommandButton() { Icon = "message.png", Capture = "Zprávy", Command = () => Navigation.PushAsync(new ConditionsPage()) },
                new CommandButton() { Icon = "profile.png", Capture = "Profil", Command = () => Navigation.PushAsync(new ConditionsPage()) },
                new CommandButton() { Icon = "search.png", Capture = "Hledat", Command = () => Navigation.PushAsync(new ConditionsPage()) }
            };

            return commands;
        }
    }
}
