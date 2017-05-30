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
            if (!String.IsNullOrEmpty(Name.Text) ||! String.IsNullOrEmpty(Birthdate.Date.ToString())
                ||! String.IsNullOrEmpty(Picture.Text) || !String.IsNullOrEmpty(Gender.SelectedItem.ToString())||
               ! String.IsNullOrEmpty(Email.Text) || !String.IsNullOrEmpty(Password.Text))
            {
                if (ConfirmPassword.Text.Equals(Password.Text))
                {
                    if (service.Signup(Name.Text, Birthdate.Date, Picture.Text, Gender.SelectedItem.ToString(),
                        Email.Text, Password.Text))
                    {

                        await DisplayAlert(AppResources.SignUpSuc, AppResources.LoggedIn, "OK");
                        await Navigation.PushModalAsync(new TrainSearch());
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SignUpFailTitle, AppResources.SignUpFailText, "OK");
                    }
                }
                else
                {
                    await DisplayAlert(AppResources.ConfirmPassFailTitle, AppResources.ConfirmPassFailText, "Ok");
                }
            }
            else
            {
                await DisplayAlert(AppResources.Attension, AppResources.FillUpFields, "Ok");
            }

        }
    }
}

