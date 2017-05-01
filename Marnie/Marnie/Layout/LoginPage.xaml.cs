using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void SignUpBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUpPage());
        }

        private void LogInBtn_OnClicked(object sender, EventArgs e)
        {
            if (Login(Email.Text, Password.Text))
            {
                Navigation.PushModalAsync(new TrainSearch());
            }
            else
            {
                DisplayAlert("Login failed", "Please try again or Sign Up", "OK");
                Navigation.PushModalAsync(new LoginPage());
            }
        }

        public bool Login(string username, string password)
        {
            var client = new RestClient("https://olek.eu.auth0.com");
            var request = new RestRequest("oauth/ro", Method.POST);

            request.AddParameter("client_id", "WU0DxEKuQXDpgSJ8lGmLNr1Nux2ejl1P");
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            request.AddParameter("connection", "Username-Password-Authentication");
            request.AddParameter("grant_type", "password");
            request.AddParameter("scope", "openid");

            IRestResponse response = client.Execute(request);

            // Using the Newtonsoft.Json library we deserialaize the string into an object,
            // we have created a LoginToken class that will capture the keys we need
            LoginToken token = JsonConvert.DeserializeObject<LoginToken>(response.Content);

            if (token.id_token != null)
            {
                Application.Current.Properties["id_token"] = token.id_token;
                Application.Current.Properties["access_token"] = token.access_token;
                GetUserData(token.id_token);

                DisplayAlert("Login Succsessfull", "", "OK");
                return true;
            }
            else
            {
                DisplayAlert("Oh No!", "It's seems that you have entered an incorrect email or password. Please try again.", "OK");
                return false;
            }
        }


        public void GetUserData(string token)
        {
            var client = new RestClient("https://https://olek.eu.auth0.com");
            var request = new RestRequest("tokeninfo", Method.GET);
            request.AddParameter("id_token", token);


            IRestResponse response = client.Execute(request);

            User user = JsonConvert.DeserializeObject<User>(response.Content);

            // Once the call executes, we capture the user data in the
            // `Application.Current` namespace which is globally available in Xamarin
            // Application.Current.Properties["email"] = user.email;
            //Application.Current.Properties["picture"] = user.picture;

            // Finally, we navigate the user the the Orders page
            //Navigation.PushModalAsync(new OrdersPage());
        }

        public class LoginToken
        {
            public string id_token { get; set; }
            public string access_token { get; set; }
            public string token_type { get; set; }
        }

        public class User
        {
            public string name { get; set; }
            public string picture { get; set; }
            public string email { get; set; }
        }

    }
}
