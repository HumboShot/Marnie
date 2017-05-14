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
