using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marnie.Model;
using Newtonsoft.Json;
using RestSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainPeople : ContentPage
    {
        private List<Jorney> _jorneyList;
        private Jorney myJorney;
        private ObservableCollection<Jorney> personList;

        public TrainPeople(List<Jorney> journeys, Jorney MyJorney)
        {

            InitializeComponent();
            _jorneyList = journeys;
            myJorney = MyJorney;
            // GetPersonsWithRouteIdAndTime(routeId, start, stop);
            SetObservableCollection();

        }
        private void SetObservableCollection()
        {
            personList = new ObservableCollection<Jorney>(_jorneyList);
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

            var chosenJourney = e.SelectedItem as Jorney;

            if (chosenJourney != null)
            {

                date.Person1Id = myJorney.PersonId;
                date.Person2Id = chosenJourney.PersonId;
                date.RouteId = chosenJourney.RouteId;
                date.StatusP1 = 1;
                if (myJorney.StartTime < chosenJourney.StartTime)
                {
                    date.StartTime = chosenJourney.StartTime;
                    date.DateStartLocation = chosenJourney.StartLocation;
                }
                else
                {
                    date.StartTime = myJorney.StartTime;
                    date.DateStartLocation = myJorney.StartLocation;
                }

                if (myJorney.EndTime > chosenJourney.EndTime)
                {
                    date.EndTime = chosenJourney.EndTime;
                    date.DateDestination = chosenJourney.Destination;
                }
                else
                {
                    date.EndTime = myJorney.EndTime;
                    date.DateDestination = myJorney.Destination;
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
                        "You could chat now with" + chosenJourney.Person.Name + "if you have a chat implemented", "OK");
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
