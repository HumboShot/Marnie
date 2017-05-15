using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marnie.Model;
using RestSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Marnie.MultilingualResources;
using Newtonsoft.Json;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JourneyDetailPage : ContentPage
    {
        private Journey _journey;
        private string dateTimeFormat = "yyyy/MM/ddTHH:mm:ss.fff";

        public JourneyDetailPage(Journey selectedJourney)
        {
            InitializeComponent();
            _journey = selectedJourney;
            this.BindingContext = _journey;
        }

        private async void Button_CancelJourneyOnClicked(object sender, EventArgs e)
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Journey", Method.DELETE);
            request.AddParameter("Id", _journey.Id);

            IRestResponse response = marnieClient.Execute(request);
            var num = (int)response.StatusCode;
            if (num >= 200 && num <= 299)
            {
                Debug.WriteLine(response.StatusCode);
                await DisplayAlert(response.StatusCode.ToString(), AppResources.JourneyCancelled, "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert(response.StatusCode.ToString(), AppResources.Error, "OK");
                return;
            }
        }

        //dublicate method from TrainsFound
        private List<Journey> GetJourneyListByRoutId()
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Journey", Method.GET);
            //request.DateFormat = dateTimeFormat;            
            request.AddParameter("routeId", _journey.RouteId);
            request.AddParameter("personId", _journey.PersonId);
            request.AddParameter("myStart", _journey.StartTime.ToString(dateTimeFormat));
            request.AddParameter("myStop", _journey.EndTime.ToString(dateTimeFormat));

            IRestResponse response = marnieClient.Execute(request);
            var journeyList = JsonConvert.DeserializeObject<List<Journey>>(response.Content);
            return journeyList;
        }

        private async void Button_PlanDatesOnClicked(object sender, EventArgs e)
        {
            var journeyList = GetJourneyListByRoutId();
            if (journeyList != null) await Navigation.PushAsync(new TrainPeople(journeyList, _journey));
        }
    }
}
