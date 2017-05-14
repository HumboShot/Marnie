using System.Collections.Generic;
using System.Collections.ObjectModel;
using Marnie.Model;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RestSharp;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyDatesPage : ContentPage
    {
        private ObservableCollection<Date> _obsList;
        private List<Date> _dateList = new List<Date>();
        private Date _selectedDate;

        public MyDatesPage()
        {
            GetDateList();
            if (_dateList != null) _obsList = new ObservableCollection<Date>(_dateList);
            InitializeComponent();
            DatesListView.ItemsSource = _obsList;
        }

        private void GetDateList()
        {
            var marnieClient = new RestClient("http://marnie-001-site1.atempurl.com/api");
            var request = new RestRequest("Date", Method.GET);
            request.AddParameter("personId", (int)Application.Current.Properties["PersonId"]);

            IRestResponse response = marnieClient.Execute(request);
            _dateList = JsonConvert.DeserializeObject<List<Date>>(response.Content);
        }

        private async void OnDateSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (DatesListView.SelectedItem == null)
                return;
            _selectedDate = e.SelectedItem as Date;
            DatesListView.SelectedItem = null;
            await Navigation.PushAsync(new DateDetailPage(_selectedDate));
        }
    }
}

