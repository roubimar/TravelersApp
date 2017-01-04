using System;
using System.IO;
using System.Reflection;

using Xamarin.Forms;

namespace FreeTravelersApp.Pages
{
    public class ConditionsPage : ContentPage
    {
        public ConditionsPage()
        {
            Title = "Obchodní podmínky";
            Content = new ScrollView() { Content = CreateTextStack() };
        }

        private StackLayout CreateTextStack()
        {
            StackLayout textStack = new StackLayout
            {
                Padding = new Thickness(5),
                Spacing = 10
            };
            // Get access to the text resource. 
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string resource = "FreeTravelersApp.Conditions.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while (null != (line = reader.ReadLine()))
                    {
                        Label label = new Label
                        {
                            Text = line,
                            // Black text for ebooks! 
                            TextColor = Color.Black
                        };
                        // Add subsequent labels to textStack. 
                        textStack.Children.Add(label);

                    }
                }
            }

            return textStack;
        }
    }
}
