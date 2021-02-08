using GoldenShelf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GoldenShelf.ViewModels;

namespace GoldenShelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyAdverts : ContentPage
    {
        public ObservableCollection<Advert> userAdverts { get; set; }

        AdvertViewModel advertViewModel = new AdvertViewModel();
        public MyAdverts()
        {
            InitializeComponent();
            BindingContext = advertViewModel;
            userAdverts = new ObservableCollection<Advert>();

            var back_tap = new TapGestureRecognizer();
            back_tap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            back.GestureRecognizers.Add(back_tap);
            MyAdvertsListView.ItemsSource = userAdverts;
          

            MyAdvertsListView.RefreshCommand = new Command(async () => {

                MyAdvertsListView.IsRefreshing = true;
                await getUserAdverts();
                MyAdvertsListView.IsRefreshing = false;
            });
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await getUserAdverts();
        }

        public async Task getUserAdverts()
        {
            var app = Application.Current as App;
            var myAdverts = await advertViewModel.getUsersAdverts(app.Email);

            //List reversed to show people last adverts.
            myAdverts.Reverse();

            foreach (var item in myAdverts)
            {
                if (!userAdverts.Any((arg) => arg.AdvertID == item.AdvertID))
                    userAdverts.Add(item);

            }

        }


        private void MyAdvertsListview_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void Deleted_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            
            var ob = btn.CommandParameter as Advert;
            advertViewModel.DeleteUser(ob);
            PopUpTitleAdvert.Text = "Deleted!";
            PopUpLabelAdvert.Text = "You have deleted the Advert successfully.";
            popUpImageViewAdvert.IsVisible = true;


        }
        private void popUpButton(object sender, EventArgs e)
        {
            popUpImageViewAdvert.IsVisible = false;
            App.Current.MainPage = new HomePage();
        }



    }
}