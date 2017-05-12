﻿using System.Collections.Generic;
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

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainsFound : ContentPage
    {
        private ObservableCollection<Route> _obsList;
        private List<Route> _routeList = new List<Route>();
        private Route _selectedRoute;
        private Jorney myJorney = new Jorney();
        private string dateTimeFormat = "yyyy/MM/ddTHH:mm:ss.fff";

        public TrainsFound(List<Route> routeList, Jorney jorney)
        {
            _routeList = routeList;
            myJorney = jorney;
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
            var travelDate = myJorney.StartTime.Date.ToUniversalTime();
            
            myJorney.StartTime = travelDate + _selectedRoute.Stops.Single(stop => stop.Station.Name.Equals(myJorney.StartLocation)).DepartureTime;
            myJorney.EndTime = travelDate + _selectedRoute.Stops.Single(stop => stop.Station.Name.Equals(myJorney.Destination)).DepartureTime;
            myJorney.Route = _selectedRoute;
            myJorney.RouteId = _selectedRoute.Id;
            myJorney.Status = 0;
            myJorney.PersonId = (int) Application.Current.Properties["PersonId"];//person id comes as the result of login
            

            var journeyListByRoutId = GetJourneyListByRoutId();            
            var trainPeoplePage = new TrainPeople(journeyListByRoutId, myJorney);
            
            SaveMyJorneyToDb();
            await Navigation.PushAsync(trainPeoplePage);
        }

        private List<Jorney> GetJourneyListByRoutId()
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Jorney", Method.GET);
            //request.DateFormat = dateTimeFormat;            
            request.AddParameter("routeId", myJorney.RouteId);
            request.AddParameter("personId", myJorney.PersonId);
            request.AddParameter("myStart", myJorney.StartTime.ToString(dateTimeFormat));
            request.AddParameter("myStop", myJorney.EndTime.ToString(dateTimeFormat));

            IRestResponse response = marnieClient.Execute(request);            
            var journeyList = JsonConvert.DeserializeObject<List<Jorney>>(response.Content);
            return journeyList;
        }
        private void SaveMyJorneyToDb()
        {                       
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Jorney", Method.POST);            
            var json = request.JsonSerializer.Serialize(myJorney);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            IRestResponse response = marnieClient.Execute(request);
        }
    }
}
