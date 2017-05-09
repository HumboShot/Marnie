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
    public partial class TrainPeople : ContentPage
    {
        private List<Jorney> _jorneyList;

        public TrainPeople(List<Jorney> list)
        {
            _jorneyList = list;
            InitializeComponent();
        }
    }
}
