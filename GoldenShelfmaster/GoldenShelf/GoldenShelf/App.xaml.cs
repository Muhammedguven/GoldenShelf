using GoldenShelf.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoldenShelf
{
    public partial class App : Application
    {
        public const string emailKey = "email";
        public const string loggedInKey= "loggedIn";
       
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new intro1());

            
        }

        protected override void OnStart()

        {
            if (LoggedIn == "false")
                MainPage = new NavigationPage(new intro1());

            if (LoggedIn == "true")
                MainPage = new HomePage();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


        //Defines a key for email
        public string Email
        {
            get
            {
                if (Properties.ContainsKey(emailKey))
                {
                    SavePropertiesAsync();
                    return Properties[emailKey].ToString();
                }

                return "";
            }
            set
            {
                Properties[emailKey] = value;
            }
        }
      
        public string LoggedIn
        {
            get
            {
                if (Properties.ContainsKey(loggedInKey))
                {
                    SavePropertiesAsync();
                    return Properties[loggedInKey].ToString();
                }

                return "";
            }
            set
            {
                Properties[loggedInKey] = value;
            }
        }





    }
}
