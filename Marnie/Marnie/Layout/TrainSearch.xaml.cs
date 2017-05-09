using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
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

        private  void NearestStationBtn_OnClicked(object sender, EventArgs e)
        {
            
        }

        private async Task LocationCurrent()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(10000);
                if (position == null)
                    return;

                Debug.WriteLine("Position Status: {0}", position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", position.Longitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
        }
    }
}
