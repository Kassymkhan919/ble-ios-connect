using AwesomeApp.Services;
using AwesomeApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            var navigationPage = new NavigationPage(new MainPage());
            MainPage = navigationPage;

            //DependencyService.Register<MockDataStore>();
            //MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
