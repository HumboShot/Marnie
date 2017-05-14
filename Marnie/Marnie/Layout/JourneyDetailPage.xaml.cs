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
    public partial class JourneyDetailPage : ContentPage
    {

        public JourneyDetailPage(Journey selectedJourney)
        {
            InitializeComponent();
        }
    }
}
