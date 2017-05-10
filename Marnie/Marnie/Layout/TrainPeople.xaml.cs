using System;
using System.Collections.Generic;
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
       // private List<Jorney> _jorneyList;

        public TrainPeople(int routeId, DateTime start, DateTime stop)
        {
          
            InitializeComponent();
            GetPersonsWithRouteIdAndTime(routeId, start, stop);


        }

        public List<Person> GetPersonsWithRouteIdAndTime(int routeId, DateTime start, DateTime stop)
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Person", Method.GET);
            request.AddParameter("routeId", routeId);
            request.AddParameter("start", start);
            request.AddParameter("stop", stop);

            IRestResponse response = marnieClient.Execute(request);
            var personList = JsonConvert.DeserializeObject<PersonList>(response.Content);
            return personList.persons;

        }

        private class PersonList
        {
            public List<Person> persons { get; set; }
        }

        private void OnPersonSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //todo get selected person's id, your id, routeId and create Date object 
            // set PersonId1 to your id and PersonId2 to selcted person's id;
            //set StatusP1 to 1;
            //set DateStatus to 0.
            

        }
    }
}
