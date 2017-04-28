using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marnie.Layout;
using Xamarin.Forms;

namespace Marnie
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void FacebookLogin_OnClicked(object sender, EventArgs e)
        {
            //Todo Make the facebook login
            //Todo Make sure that the login page is skipped when the user is already logged in
            await Navigation.PushAsync(new TrainSearch());
        }
    }
}
