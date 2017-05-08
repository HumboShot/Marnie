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
        private AuthService service = new AuthService();

        public SignUpPage()
        {
            InitializeComponent();

            
        }

        private async void SignUpBtn_OnClicked(object sender, EventArgs e)
        {
            if (ConfirmPassword.Text.Equals(Password.Text))
            {
                if (service.Signup(Name.Text, Birthdate.Date, Picture.Text, Gender.SelectedItem.ToString(), Email.Text, Password.Text))
                {
                    
                    DisplayAlert("Account created successfuly", "You are logget in", "OK");
                    await Navigation.PushModalAsync(new TrainSearch());
                }
                else
                {
                    DisplayAlert("An Error has occured", "Please try again", "OK");
                }
            }
            else
            {
                DisplayAlert("ConfirmPasword don't match Password ", "Please try again", "Ok");
            }

        }
    }
}

