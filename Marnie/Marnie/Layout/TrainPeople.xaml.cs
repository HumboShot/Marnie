using System.Collections.Generic;
using System.Collections.ObjectModel;
using Java.Lang;
using Marnie.Model;
using Marnie.MultilingualResources;
using RestSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainPeople : ContentPage
    {
        private List<Journey> _journeyList;
        private Journey myJourney;
        private ObservableCollection<Journey> personList;

        public TrainPeople(List<Journey> journeys, Journey MyJourney)
        {

            InitializeComponent();
            _journeyList = journeys;
            if (_journeyList.Count == 0)
            {
                NoMatch.IsVisible = true;
            }
            
            myJourney = MyJourney;
            SetObservableCollection();

        }
        private void SetObservableCollection()
        {
            personList = new ObservableCollection<Journey>(_journeyList);
            PersonListByRoute.ItemsSource = personList;
           }


        private void OnPersonSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var date = new Date();

            if (PersonListByRoute.SelectedItem == null)
                return;

            var chosenJourney = e.SelectedItem as Journey;

            if (chosenJourney != null)
            {

                date.Person1Id = myJourney.PersonId;
                date.Person2Id = chosenJourney.PersonId;
                date.RouteId = chosenJourney.RouteId;
                date.StatusP1 = 1;
                if (myJourney.StartTime < chosenJourney.StartTime)
                {
                    date.StartTime = chosenJourney.StartTime;
                    date.DateStartLocation = chosenJourney.StartLocation;
                }
                else
                {
                    date.StartTime = myJourney.StartTime;
                    date.DateStartLocation = myJourney.StartLocation;
                }

                if (myJourney.EndTime > chosenJourney.EndTime)
                {
                    date.EndTime = chosenJourney.EndTime;
                    date.DateDestination = chosenJourney.Destination;
                }
                else
                {
                    date.EndTime = myJourney.EndTime;
                    date.DateDestination = myJourney.Destination;
                }
            }
            if (!SaveDateToDb(date))
            {
                DisplayAlert("Date has not been created", "", "OK");
            }
            else
            {
                if (chosenJourney != null)
                    DisplayAlert("Date has been created succsessfully",
                        "You could chat now with " + chosenJourney.Person.Name + " if chat was implemented", "OK");
            }
            
            PersonListByRoute.SelectedItem = null;


        }

        private bool SaveDateToDb(Date date)
        {
            try
            {
                var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
                var request = new RestRequest("Date", Method.POST);
                var json = request.JsonSerializer.Serialize(date);

                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);


                IRestResponse response = marnieClient.Execute(request);
                //get responce status code, then return true if succsessfull (between 200 and 299) else return false
                var num = (int)response.StatusCode;
                if (num >= 200 && num <= 299)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
