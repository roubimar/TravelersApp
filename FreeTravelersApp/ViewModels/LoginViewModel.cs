using FreeTravelersApp.DataClass;
using Microsoft.AspNet.SignalR.Client;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreeTravelersApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private INavigation navigation;
        private HubConnection hubConnection;
        private bool connected;

        private HubConnection HubConnection
        {
            set
            {
                hubConnection = value;
            }
            get
            {
                if(hubConnection == null)
                {
                    hubConnection = new HubConnection("http://freetravelers.gear.host/");
                }
                return hubConnection;
            }
        }
        private IHubProxy chatHubProxy;
        private IHubProxy ChatHubProxy
        {
            set
            {
                chatHubProxy = value;
            }
            get
            {
                if (chatHubProxy == null)
                {
                    chatHubProxy = HubConnection.CreateHubProxy("ConnectionHub");
                }
                return chatHubProxy;
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            InitializeConnection();
        }

        private async void InitializeConnection()
        {
            ChatHubProxy.On<ConnectionInfo>("ConnectionSucceced", (connectionInfo) =>
            {
                navigation.PopModalAsync();
            });

            ChatHubProxy.On("ConnectionFailed", () =>
            {
                Error = "Nepodařilo se přihlásit, zkontrolujte si údaje.";

            });
            await HubConnection.Start();
            connected = true;
        }

        private string username = string.Empty;
        public const string UsernamePropertyName = "Username";
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        private string password = string.Empty;
        public const string PasswordPropertyName = "Password";
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        private string error = string.Empty;
        public const string ErrorPropertyName = "Error";
        public string Error
        {
            get { return error; }
            set
            {
                if (error != value)
                {
                    error = value;
                    OnPropertyChanged("Error");
                }
            }
        }


        private Command loginCommand;
        public const string LoginCommandPropertyName = "LoginCommand";

        public event PropertyChangedEventHandler PropertyChanged;

        public Command LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new Command(async () => await ExecuteLoginCommand()));
            }
        }        

        protected async Task ExecuteLoginCommand()
        {
            Error = string.Empty;

            if (!connected)
            {
                Error = "Problém s připojením, pravděpodobně nejste připojení k internetu.";
                return;
            }

            // Start the connection
            await ChatHubProxy.Invoke("Login", username, password);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
