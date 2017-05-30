using System;
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
            //Email.Text = "mm@mm.com";
            //Password.Text = "123";
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
            LogInBtn.IsEnabled = false;
            LogInBtn.Text = AppResources.LoginProgress;
            if (service.Login(Email.Text.Trim(), Password.Text.Trim()))
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
