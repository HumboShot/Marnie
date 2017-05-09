using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marnie.Layout;
using Xamarin.Forms;
using Marnie.Model;

namespace Marnie
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //Generate some Data, just for us
            //new GenerateData();

            //Standard Tasks
            if (Application.Current.Properties.ContainsKey("isLoggetIn") &&
                (bool) Application.Current.Properties["isLoggetIn"])
            {
                MainPage = new NavigationPage(new TrainSearch());
                
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
