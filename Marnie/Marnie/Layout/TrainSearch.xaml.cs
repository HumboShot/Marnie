using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainSearch : ContentPage
    {
        public TrainSearch()
        {
            InitializeComponent();
            TimePicker.Time = DateTime.Now.TimeOfDay;
            //this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
            NavigationPage.SetHasBackButton(this, false);
        }


        private async void SearchForTrainBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainsFound());
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }
    }
}
