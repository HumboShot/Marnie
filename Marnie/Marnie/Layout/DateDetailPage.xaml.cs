using System;
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
            SetLabels();
            
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

        private void SetLabels()
        {
            Name.Text = _myDate.Name;
            Age.Text = ""+(DateTime.Now.Year - _myDate.Birthday.Year);
            Gender.Text = _myDate.Gender;
            DateStartLocation.Text = _thisDate.DateStartLocation;
            DateDestination.Text = _thisDate.DateDestination;
            Image.Source = _myDate.ProfilePicture;
        }
    }
}
