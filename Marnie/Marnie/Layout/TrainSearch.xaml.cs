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
using Marnie.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace Marnie.Layout
{
    //Make dropdown of stations to select stations
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainSearch : ContentPage
    {
        private Position _position;
        private List<Station> stationList = new List<Station>();
        private MediaFile _mediaFile;
        public TrainSearch()
        {
            InitializeComponent();

          
            //Search data for testing
            //FromBox.Text = "Aalborg";
            //Destination.Text = "Vejle";
            //DatePicker.Date = DateTime.Parse("11-05-2017");
            //TimePicker.Time = TimeSpan.Parse("09:55:00");
            
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SearchForTrainBtn.IsEnabled = true;
            NearestStationBtn.IsEnabled = true;
        }

        private void GetStationListFromApi()
        {            
            var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
            var request = new RestRequest("Station", Method.GET);
            
            IRestResponse response = marnieClient.Execute(request);
            stationList = JsonConvert.DeserializeObject<List<Station>>(response.Content);
        }

        private async void SearchForTrainBtn_OnClicked(object sender, EventArgs e)
        {
            SearchForTrainBtn.IsEnabled = false;
            string from = FromBox.Text.Trim();
            string destination = Destination.Text.Trim();            
            var startTime = DateTime.SpecifyKind(DatePicker.Date + TimePicker.Time, DateTimeKind.Utc);

            var journey = new Journey();
            journey.StartLocation = from;
            journey.Destination = destination;
            //startTime set to transfer the date to create real time when route is selected.
            journey.StartTime = startTime;

            List<Route> routeList = new List<Route>();
            var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
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
                SearchForTrainBtn.IsEnabled = true;
                return;
            }
            
            await Navigation.PushAsync(new TrainsFound(routeList, journey));
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }

        private async void NearestStationBtn_OnClicked(object sender, EventArgs e)
        {
            NearestStationBtn.IsEnabled = false;
            await LocationCurrent();
            string latitude = _position.Latitude.ToString();
            latitude = latitude.Replace(",", ".");
            string longitude = _position.Longitude.ToString();
            longitude = longitude.Replace(",", ".");

            var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
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
                await DisplayAlert(response.StatusCode.ToString(), AppResources.Error, "OK");
            }
            NearestStationBtn.IsEnabled = true;
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

        private async void MyJourneys_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyJourneysPage());
        }

        private async void MyDates_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyDatesPage());
        }
        private async void PickPictureBtn_OnClicked(object sender, EventArgs e)
        {
            PickPictureBtn.IsEnabled = false;
            try
            {
                await PickPictureAsync();
                ImageService service = new ImageService();
                if (service.SavePicture(_mediaFile))
                {
                    await DisplayAlert("picture saved", AppResources.PictureSaved, "OK");
                }
                else
                {
                    await DisplayAlert("picture saved", AppResources.Error, "OK");
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Error", AppResources.NoPicture, "OK");
                
            }
            PickPictureBtn.IsEnabled = true;
        }
        private async Task PickPictureAsync()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Pick Photo", ":( Pick Photo  not supported", "OK");
                return;
            }
            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
        }
    }
}
