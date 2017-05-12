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
    public partial class LoginPage : ContentPage
    {
        private AuthService service = new AuthService();
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
            if (service.Login(Email.Text, Password.Text))
            {
                DisplayAlert(AppResources.LogInSuc, "", "OK");
                Navigation.PushModalAsync(new TrainSearch());
            }
            else
            {
                DisplayAlert(AppResources.LogInFailTitle, AppResources.LogInFailText, "OK");
                Navigation.PushModalAsync(new LoginPage());
            }
        }
        
        
       
    }
}
