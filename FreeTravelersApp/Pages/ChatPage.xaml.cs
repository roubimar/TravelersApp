using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using System.Linq;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace FreeTravelersApp.Pages
{
    public class Message
    {
        public string Name { get; set; }
        public string MessageText { get; set; }
    }

    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            var button = new Button { Text = "AddMessage" };

            var stacklayout = new StackLayout
            {
                Children = {
                    new Label { Text = "Messages:" },

                }
            };

            var scrollView = new ScrollView();
            var listContent = new ListView();
            var messages = new ObservableCollection<Message>();
            listContent.ItemsSource = messages;

            listContent.ItemTemplate = new DataTemplate(() =>
            {
                var frame = new Frame();

                var nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, "Name");

                var messageLabel = new Label();
                messageLabel.SetBinding(Label.TextProperty, "MessageText");

                frame.Content = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { nameLabel, messageLabel } };

                return new ViewCell { View = messageLabel };
            });

            scrollView.Content = listContent;
            // Connect to the server
            var hubConnection = new HubConnection("http://freetravelers.gear.host/");

            // Create a proxy to the 'ChatHub' SignalR Hub
            var chatHubProxy = hubConnection.CreateHubProxy("ChatHub");

            // Wire up a handler for the 'UpdateChatMessage' for the server
            // to be called on our client
            chatHubProxy.On<string, string>("addNewMessageToPage", (name, message) =>
            {
                var item = new Message() { Name = name, MessageText = message };
                messages.Add(item);
                listContent.ScrollTo(messages.LastOrDefault(), ScrollToPosition.End, true);
            }
            );

            // Start the connection
            hubConnection.Start();

            // Invoke the 'UpdateNick' method on the server
            button.Clicked += async (sender, e) =>
            {
                await chatHubProxy.Invoke("Send", "UpdateNick", "JohnDoe");
            };

            stacklayout.Children.Add(button);
            stacklayout.Children.Add(scrollView);
            Content = stacklayout;
        }
    }
}
