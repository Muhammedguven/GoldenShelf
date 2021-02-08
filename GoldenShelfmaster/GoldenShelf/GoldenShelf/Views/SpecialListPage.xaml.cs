using GoldenShelf.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoldenShelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecialListPage : ContentPage
    {
        public ObservableCollection<Advert> userAdverts { get; set; }
        AdvertViewModel advertViewModel = new AdvertViewModel();
       
            public SpecialListPage()
        {
            InitializeComponent();
            var back_tap = new TapGestureRecognizer();
            back_tap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            back.GestureRecognizers.Add(back_tap);
            
        }
        
        public string CName { get; }

        public SpecialListPage(string cname)
        {
            InitializeComponent();

            userAdverts = new ObservableCollection<Advert>();

            var back_tap = new TapGestureRecognizer();
            back_tap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            back.GestureRecognizers.Add(back_tap);
            SpecialListView.ItemsSource = userAdverts;
            OnAppearing();

            SpecialListView.RefreshCommand = new Command(async () => {

                SpecialListView.IsRefreshing = true;
                await getItems();
                SpecialListView.IsRefreshing = false;
            });

            CName = cname;
            this.shareName.Text = CName;

            
            if (CName.ToString() == "Donate")
            {
                shareName.TextColor = Color.FromHex("#1B9101");
                howMany.TextColor = Color.FromHex("#1B9101");
            }
            if (CName.ToString() == "Exchange")
            {
                shareName.TextColor = Color.FromHex("#00B5B9");
                howMany.TextColor = Color.FromHex("#00B5B9");
            }
            
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await getItems();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
        public async void SpecialListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Advert;

            await Navigation.PushAsync(new BookPage(myItem.BookName, myItem.BookAuthor, myItem.BookCategory, myItem.Condition, myItem.ShareType, myItem.Description, myItem.PublisherEmail, myItem.Image));
        }
        public async Task getItems()
        {
            var donationAdverts = await advertViewModel.GetDonationAdverts();

            //List reversed to show people last adverts.
            donationAdverts.Reverse();

            foreach (var item in donationAdverts)
            {
                if (!userAdverts.Any((arg) => arg.AdvertID == item.AdvertID))
                    if ((item.ShareType.ToString() == shareName.Text))
                    {
                        userAdverts.Add(item);
                    }
                    
            }
            //------------------------------------------------------------------------
            //-------------- To show Donation Adverts on the main page
            var exchangeAdverts = await advertViewModel.GetExchangeAdverts();

            //List reversed to show people last adverts.
            exchangeAdverts.Reverse();
            foreach (var item in exchangeAdverts)
            {
                if (!userAdverts.Any((arg) => arg.AdvertID == item.AdvertID))
                    if ((item.ShareType.ToString() == shareName.Text))
                    {
                        userAdverts.Add(item);
                    }
            }
            howMany.Text = userAdverts.Count().ToString() + " adverts found.";

        }
    }
}