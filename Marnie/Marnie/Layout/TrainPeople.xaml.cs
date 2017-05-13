using System.Collections.Generic;
using System.Collections.ObjectModel;
using Marnie.Model;
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
            myJourney = MyJourney;
            // GetPersonsWithRouteIdAndTime(routeId, start, stop);
            SetObservableCollection();

        }
        private void SetObservableCollection()
        {
            personList = new ObservableCollection<Journey>(_journeyList);
            PersonListByRoute.ItemsSource = personList;
           }

        //public List<Person> GetPersonsWithRouteIdAndTime(int routeId, DateTime start, DateTime stop)
        //{
        //    var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
        //    var request = new RestRequest("Person", Method.GET);
        //    request.AddParameter("routeId", routeId);
        //    request.AddParameter("start", start);
        //    request.AddParameter("stop", stop);

        //    IRestResponse response = marnieClient.Execute(request);
        //    var personList = JsonConvert.DeserializeObject<PersonList>(response.Content);
        //    return personList.persons;

        //}

        //private class PersonList
        //{
        //    public List<Person> persons { get; set; }
        //}

        private void OnPersonSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //todo get selected person's id, your id, routeId and create Date object 
            // set PersonId1 to your id and PersonId2 to selcted person's id;
            //set StatusP1 to 1;
            //set DateStatus to 0.
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
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
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

    }
}
