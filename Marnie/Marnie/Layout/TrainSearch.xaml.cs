using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RestSharp;
using Marnie.Model;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Marnie.MultilingualResources;

namespace Marnie.Layout
{
    //Make dropdown of stations to select stations
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainSearch : ContentPage
    {
        private Position _position;
        private List<Station> stationList = new List<Station>();
        public TrainSearch()
        {
            InitializeComponent();
            //GetStationListFromApi();
            //StationPicker.ItemsSource = stationList;
            //StationPicker.ItemDisplayBinding = new Binding("Name");
                        
            TimePicker.Time = DateTime.Now.TimeOfDay;
            //this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
            NavigationPage.SetHasBackButton(this, false);
            if (Application.Current.Properties.ContainsKey("isLoggetIn") &&
                (bool)Application.Current.Properties["isLoggetIn"] && 
                Application.Current.Properties.ContainsKey("UserName") && 
                !Application.Current.Properties["UserName"].Equals(""))
            {
                LoginStatus.Text = AppResources.LogInStatus + Application.Current.Properties["UserName"];

            }
            else
            {
                LoginStatus.Text = AppResources.LogInStatusErr;

            }
        }

        private void GetStationListFromApi()
        {            
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Station", Method.GET);
            
            IRestResponse response = marnieClient.Execute(request);
            stationList = JsonConvert.DeserializeObject<List<Station>>(response.Content);
        }

        private async void SearchForTrainBtn_OnClicked(object sender, EventArgs e)
        {
            //Station st1 = StationPicker.SelectedItem as Station;
            //string from = st1.Name;
            string from = FromBox.Text.Trim();
            string destination = Destination.Text.Trim();            
            var startTime = DateTime.SpecifyKind(DatePicker.Date + TimePicker.Time, DateTimeKind.Utc);

            var jorney = new Jorney();
            jorney.StartLocation = from;
            jorney.Destination = destination;
            //startTime set to transfer the date to create real time when route is selected.
            jorney.StartTime = startTime;

            List<Route> routeList = new List<Route>();
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Route", Method.GET);
            request.AddParameter("from", from);
            request.AddParameter("to", destination);
            request.AddParameter("startTime", startTime.TimeOfDay);

            IRestResponse response = marnieClient.Execute(request);
            var num = (int)response.StatusCode;
            if (num >= 200 && num <= 299)
            {
                Debug.WriteLine(response.StatusCode);
                routeList = JsonConvert.DeserializeObject<List<Route>>(response.Content);
            }
            else
            {
                await DisplayAlert(response.StatusCode.ToString(), AppResources.Error, "OK");
                return;
            }
            
            await Navigation.PushAsync(new TrainsFound(routeList, jorney));
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
                //StationPicker.SelectedItem = station;
            }
            else
            {
                await DisplayAlert(response.StatusCode.ToString(), AppResources.Error, "OK");
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
