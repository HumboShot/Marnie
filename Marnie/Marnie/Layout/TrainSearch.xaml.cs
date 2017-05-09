using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RestSharp;
using Marnie.Model;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainSearch : ContentPage
    {
        private Position _position;

        public TrainSearch()
        {
            InitializeComponent();
            TimePicker.Time = DateTime.Now.TimeOfDay;
            //this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
            NavigationPage.SetHasBackButton(this, false);
            if (Application.Current.Properties.ContainsKey("isLoggetIn") &&
                (bool)Application.Current.Properties["isLoggetIn"])
            {
                LoginStatus.Text = "You are logget in";

            }
            else
            {
                LoginStatus.Text = "You are not logget in";

            }
        }


        private async void SearchForTrainBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainsFound());
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }

        private async void NearestStationBtn_OnClicked(object sender, EventArgs e)
        {
            await LocationCurrent();
            string latitude = _position.Latitude.ToString();
            latitude = latitude.Replace(",", ".");
            string longitude = _position.Longitude.ToString();
            longitude = longitude.Replace(",", ".");

            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Station", Method.GET);
           
            request.AddParameter("latitude", latitude);
            request.AddParameter("longitude", longitude);
            IRestResponse response = marnieClient.Execute(request);
            var num = (int)response.StatusCode;
            if (num >= 200 && num <= 299)
            {
                Debug.WriteLine(response.StatusCode);
                Station station = JsonConvert.DeserializeObject<Station>(response.Content);
                FromBox.Text = station.Name;
            }
            else
            {
                await DisplayAlert(response.StatusCode.ToString(), "Something went wrong", "OK");
            }
           
            
            
        }

        private async Task LocationCurrent()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                _position = await locator.GetPositionAsync(10000);
                if (_position == null)
                    return;

                Debug.WriteLine("Position Status: {0}", _position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", _position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", _position.Longitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
        }
    }
}
