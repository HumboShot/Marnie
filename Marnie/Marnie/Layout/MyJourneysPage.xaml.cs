using System.Collections.Generic;
using System.Collections.ObjectModel;
using Marnie.Model;
using Marnie.MultilingualResources;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RestSharp;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyJourneysPage : ContentPage
    {
        private ObservableCollection<Journey> _obsList;
        private List<Journey> _journeyList = new List<Journey>();
        private Journey _selectedJourney;

        public MyJourneysPage()
        {
            GetJourneyList();
            if (_journeyList != null) _obsList = new ObservableCollection<Journey>(_journeyList);
            InitializeComponent();
            JourneysListView.ItemsSource = _obsList;
        }

        private void GetJourneyList()
        {
            var marnieClient = new RestClient(AppResources.OwnApiEndpoint);
            var request = new RestRequest("Journey", Method.GET);
            request.AddParameter("personId", (int)Application.Current.Properties["PersonId"]);
            
            IRestResponse response = marnieClient.Execute(request);
            _journeyList = JsonConvert.DeserializeObject<List<Journey>>(response.Content);
        }

        private async void OnJourneySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (JourneysListView.SelectedItem == null)
                return;
            _selectedJourney = e.SelectedItem as Journey;
            JourneysListView.SelectedItem = null;
            await Navigation.PushAsync(new JourneyDetailPage(_selectedJourney));
        }
    }
}
