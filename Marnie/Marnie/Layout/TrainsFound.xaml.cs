using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marnie.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Newtonsoft.Json;
using RestSharp;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainsFound : ContentPage
    {
        private ObservableCollection<Route> _obsList;
        private List<Route> _routeList = new List<Route>();
        private Route _selectedRoute;
        private Jorney _jorney = new Jorney();        

        public TrainsFound(List<Route> routeList, Jorney jorney)
        {
            _routeList = routeList;
            _jorney = jorney;
            InitializeComponent();
            SetObservableCollection();
        }

        private void SetObservableCollection()
        {
            _obsList = new ObservableCollection<Route>(_routeList);
            routesListView.ItemsSource = _obsList;
            Debug.WriteLine("Loaded " + _obsList.Count + " Routes");
        }

        private void OnRouteSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (routesListView.SelectedItem == null)
                return;
            _selectedRoute = e.SelectedItem as Route;
            routesListView.SelectedItem = null;
        }

        private async void Button_OnRouteSelected(object sender, EventArgs eventArgs)
        {
            _jorney.StartTime = DateTime.Today + _selectedRoute.Stops.Single(stop => stop.Station.Name.Equals(_jorney.StartLocation)).DepartureTime;

            _jorney.EndTime = DateTime.Today + _selectedRoute.Stops.Single(stop => stop.Station.Name.Equals(_jorney.Destination)).ArrivalTime;
            _jorney.Route = _selectedRoute;
            _jorney.RouteId = _selectedRoute.Id;
            _jorney.Status = 0;
            _jorney.PersonId = (int) Application.Current.Properties["PersonId"];//person id comes as the result of login

            //Get jorneyList from Api and if succesfull push next Page with it.
            //we send route id , statrt time and end time as parameters to TrainPeople
            
            //var trainPeoplePage = new TrainPeople(_jorney.RouteId, _jorney.StartTime, _jorney.EndTime);
            var journeyListByRoutId = GetJourneyListDyRoutId(_jorney.RouteId, _jorney.StartTime, _jorney.EndTime);
            var trainPeoplePage = new TrainPeople(journeyListByRoutId, _jorney);
            await Navigation.PushAsync(trainPeoplePage);
        }
    }
}
