using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marnie.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marnie.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateDetailPage : ContentPage
    {
        private Person _myDate;
        private Date _thisDate;

        public DateDetailPage(Date selectedDate)
        {
            InitializeComponent();
            _thisDate = selectedDate;
            SetMyDate();
            StartLocation.Text = _thisDate.DateStartLocation;
            //StartTime.Text = _thisDate.StartTime.ToString();
            Destination.Text = _thisDate.DateDestination;
            //StopTime.Text = _thisDate.EndTime.ToString();
            //todo Set PersonPicture (from _myDate object) to be loaded from a provided path
           // PersonPicture.Source = 
            PersonName.Text = _myDate.Name;



        }
        private void SetMyDate()
        {
            if (_thisDate.Person1Id == (int)Application.Current.Properties["PersonId"])
            {
                _myDate = _thisDate.Person2;
            }
            else
            {
                _myDate = _thisDate.Person1;
            }
        }
    }
}
