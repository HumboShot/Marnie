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
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void SignUpBtn_OnClicked(object sender, EventArgs e)
        {
            if (ConfirmPassword.Text.Equals(Password.Text))
            {
                Signup(Email.Text, Password.Text);
            }
            else
            {
                DisplayAlert("ConfirmPasword don't match Password ", "Please try again", "Ok");
            }

        }

        public void Signup(string username, string password)
        {
            var client = new RestClient("https://olek.eu.auth0.com");
            var request = new RestRequest("dbconnections/signup", Method.POST);

            request.AddParameter("client_id", "WU0DxEKuQXDpgSJ8lGmLNr1Nux2ejl1P");
            request.AddParameter("email", username);
            request.AddParameter("password", password);
            request.AddParameter("connection", "Username-Password-Authentication");

            IRestResponse response = client.Execute(request);
            // Once the request is executed we capture the response.
            // If we get a `user_id`, we know that the account has been created
            // and display an appropriate message. If we do not get a `user_id`
            // we know something went wrong, so we ask the user if they already have
            // an account and if not to try again.
            UserSignup user = JsonConvert.DeserializeObject<UserSignup>(response.Content);
            if (user.user_id != null)
            {
                DisplayAlert("Account Created", "Head back to the hompage and login with your new account", "Ok");
            }
            else
            {
                DisplayAlert("Oh No!", "Account could not be created. Do you already have an account? Please try again.", "Ok");
            }
        }

        public class UserSignup
        {
            [JsonProperty("_id")]
            public string user_id { get; set; }
            public string email { get; set; }
        }
    }
}

