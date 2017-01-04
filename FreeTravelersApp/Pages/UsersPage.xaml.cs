using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace FreeTravelersApp.Pages
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class UserResult
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsGuide { get; set; }
        public DateTime? GuideFrom { get; set; }
        public int IdentityId { get; set; }
    }

    public partial class UsersPage : ContentPage
    {
        public UsersPage()
        {
            var layout = new StackLayout();
            var entry = new Entry();
            var buttonGet = new Button() { Text = "Načti uživatele" };
            var buttonGetAll = new Button() { Text = "Načti všechny uživatele" };
            var buttonCreate = new Button() { Text = "Vytvoř uživatele" };
            var buttonDelete = new Button() { Text = "Smaž uživatele" };
            var buttonDeleteAll = new Button() { Text = "Smaž všechny uživatele" };
            var scrollView = new ScrollView();
            var listContent = new StackLayout();

            scrollView.Content = listContent;

            buttonGet.Clicked += async (sender, e) =>
            {
                var user = await WaitRequest(listContent, GetUser, entry.Text);
                if (user != null)
                {
                    listContent.Children.Add(new Frame() { Content = new Label() { Text = string.Format("{0} {1} {2}", user.Id, user.FirstName, user.LastName) } });
                }
            };

            buttonCreate.Clicked += (sender, e) =>
            {
                CreateUser(new UserViewModel() { FirstName = "Marek", LastName = "Novák" });
            };

            buttonDeleteAll.Clicked += (sender, e) =>
            {
                DeleteAllUsers();
            };

            buttonDelete.Clicked += (sender, e) =>
            {
                DeleteUser(new UserViewModel() { Id = entry.Text });
            };

            buttonGetAll.Clicked += async (sender, e) =>
            {
                var users = await WaitRequest(listContent, GetUsers);
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        listContent.Children.Add(new Frame() { Content = new Label() { Text = string.Format("{0} {1} {2}", user.Id, user.FirstName, user.LastName) } });
                    }
                }
            };

            layout.Children.Add(entry);
            var grid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition(), new ColumnDefinition() },
                RowDefinitions = new RowDefinitionCollection() { new RowDefinition(), new RowDefinition(), new RowDefinition() }
            };

            grid.Children.Add(buttonGet, 0, 0);
            grid.Children.Add(buttonGetAll, 0, 1);
            grid.Children.Add(buttonCreate, 1, 0);
            grid.Children.Add(buttonDelete, 1, 1);
            grid.Children.Add(buttonDeleteAll, 0, 2);

            layout.Children.Add(grid);
            layout.Children.Add(scrollView);
            Content = layout;
        }

        private async Task<List<UserResult>> GetUsers()
        {
            string url = "http://freetravelers.gear.host/User/GetAll";
            return await FetchResponseAsync<List<UserResult>>(url);
        }


        private async Task<UserResult> GetUser(string id)
        {
            string url = "http://freetravelers.gear.host/User/Get/" + id;
            return await FetchResponseAsync<UserResult>(url);
        }

        private async void CreateUser(UserViewModel user)
        {
            var url = string.Format("http://freetravelers.gear.host/User/Create");
            /*await*/
            PostRequestAsync(url, user, u => u.FirstName, u => u.LastName);
        }

        private async void DeleteUser(UserViewModel user)
        {
            var url = string.Format("http://freetravelers.gear.host/User/Delete");
            using (var client = new HttpClient())
            {
                try
                {
                    await client.DeleteAsync(url + "/" + user.Id);
                }
                catch (HttpRequestException ex)
                {
                    ;
                }
            }
        }

        private async void DeleteAllUsers()
        {
            var url = string.Format("http://freetravelers.gear.host/User/DeleteAll");
            /*await*/
            PostRequestAsync<object>(url, null);
        }

        private async Task<Type> FetchResponseAsync<Type>(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var responseText = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<Type>(responseText);
                }
                catch (HttpRequestException ex)
                {
                    return default(Type);
                }
            }
        }

        private async Task<HttpResponseMessage> PostRequestAsync<T>(string url, T entity, params Expression<Func<T, object>>[] properties)
        {
            var values = new List<KeyValuePair<string, string>>();
            foreach (var property in properties)
            {
                var value = property.Compile().Invoke(entity).ToString();
                var expression = property.Body as MemberExpression;
                values.Add(new KeyValuePair<string, string>(expression.Member.Name, value));
            }
            var content = new FormUrlEncodedContent(values);

            using (var client = new HttpClient())
            {
                try
                {
                    return await client.PostAsync(url, content);
                }
                catch (HttpRequestException ex)
                {
                    return null;
                }
            }
        }

        #region AsyncCalls
        private void BeforeWait(Layout<View> container)
        {
            container.Children.Clear();
            container.Children.Add(new Label { Text = "Načítám data..." });
            container.Children.Add(new ActivityIndicator { IsRunning = true });
        }
        private void AfterWait(Layout<View> container)
        {
            container.Children.Clear();
        }
        private async Task<TResult> WaitRequest<TResult>(Layout<View> container, Func<Task<TResult>> callMethod)
        {
            BeforeWait(container);
            var result = await callMethod();
            AfterWait(container);
            return result;
        }
        private async Task<TResult> WaitRequest<TResult, Arg1>(StackLayout container, Func<Arg1, Task<TResult>> callMethod, Arg1 arg1)
        {
            BeforeWait(container);
            var result = await callMethod(arg1);
            AfterWait(container);
            return result;
        }
        private async Task<TResult> WaitRequest<TResult, Arg1, Arg2>(StackLayout container, Func<Arg1, Arg2, Task<TResult>> callMethod, Arg1 arg1, Arg2 arg2)
        {
            BeforeWait(container);
            var result = await callMethod(arg1, arg2);
            AfterWait(container);
            return result;
        }
        private async Task<TResult> WaitRequest<TResult, Arg1, Arg2, Arg3>(StackLayout container, Func<Arg1, Arg2, Arg3, Task<TResult>> callMethod, Arg1 arg1, Arg2 arg2, Arg3 arg3)
        {
            BeforeWait(container);
            var result = await callMethod(arg1, arg2, arg3);
            AfterWait(container);
            return result;
        }
        #endregion
    }
}
