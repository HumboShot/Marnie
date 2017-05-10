using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Marnie.MultilingualResources;


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
            var gender = new Picker();
            gender.Title = AppResources.Gender;

            if (AppResources.ConfirmPassword.Equals(AppResources.Password))
            {
                if (service.Signup(AppResources.Name, Birthdate.Date, AppResources.Picture, gender.SelectedItem.ToString(), AppResources.Email, AppResources.Password))
                {
                    
                    await DisplayAlert("Account created successfuly", "You are logget in", "OK");
                    await Navigation.PushModalAsync(new TrainSearch());
                }
                else
                {
                    await DisplayAlert("An Error has occured", "Please try again", "OK");
                }
            }
            else
            {
                await DisplayAlert("ConfirmPasword don't match Password ", "Please try again", "Ok");
            }

        }
    }
}

