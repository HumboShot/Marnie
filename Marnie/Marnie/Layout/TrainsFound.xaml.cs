using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marnie.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Newtonsoft.Json;
using RestSharp;
using System;
using Marnie.MultilingualResources;
using System.Text;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainsFound : ContentPage
    {
        private ObservableCollection<Route> _obsList;
        private List<Route> _routeList = new List<Route>();
        private Route _selectedRoute;
        private Journey myJourney = new Journey();
        private string dateTimeFormat = "yyyy/MM/ddTHH:mm:ss.fff";

        public TrainsFound(List<Route> routeList, Journey journey)
        {
            _routeList = routeList;
            
            myJourney = journey;
            InitializeComponent();
            SetObservableCollection();
        }

        private void SetObservableCollection()
        {
            _obsList = new ObservableCollection<Route>(_routeList);
            routesListView.ItemsSource = _obsList;
            Debug.WriteLine("Loaded " + _obsList.Count + " Routes");
        }

        private async void OnRouteSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (routesListView.SelectedItem == null)
                return;
            _selectedRoute = e.SelectedItem as Route;
            routesListView.SelectedItem = null;

            await UseSelected();
        }

        private async Task UseSelected()
        {
            var travelDate = myJourney.StartTime.Date.ToUniversalTime();
            
            myJourney.StartTime = travelDate + _selectedRoute.Stops.Single(stop => stop.Station.Name.Equals(myJourney.StartLocation)).DepartureTime;
            myJourney.EndTime = travelDate + _selectedRoute.Stops.Single(stop => stop.Station.Name.Equals(myJourney.Destination)).DepartureTime;
            myJourney.Route = _selectedRoute;
            myJourney.RouteId = _selectedRoute.Id;
            myJourney.Status = 0;
            myJourney.PersonId = (int) Application.Current.Properties["PersonId"];//person id comes as the result of login
            

            var journeyListByRoutId = GetJourneyListByRoutId();            
            var trainPeoplePage = new TrainPeople(journeyListByRoutId, myJourney);
            
            SaveMyJourneyToDb();
            await Navigation.PushAsync(trainPeoplePage);
        }

        private List<Journey> GetJourneyListByRoutId()
        {
            var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
            var request = new RestRequest("Journey", Method.GET);
            //request.DateFormat = dateTimeFormat;            
            request.AddParameter("routeId", myJourney.RouteId);
            request.AddParameter("personId", myJourney.PersonId);
            request.AddParameter("myStart", myJourney.StartTime.ToString(dateTimeFormat));
            request.AddParameter("myStop", myJourney.EndTime.ToString(dateTimeFormat));

            IRestResponse response = marnieClient.Execute(request);            
            var journeyList = JsonConvert.DeserializeObject<List<Journey>>(response.Content);
            return journeyList;
        }
        private void SaveMyJourneyToDb()
        {                       
            var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
            var request = new RestRequest("Journey", Method.POST);            
            var json = request.JsonSerializer.Serialize(myJourney);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            IRestResponse response = marnieClient.Execute(request);
        }
    }
}
