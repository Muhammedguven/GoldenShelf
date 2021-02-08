using GoldenShelf.Models;
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
    public partial class CategoryList : ContentPage
    {
        public byte[] imagebyte;
        public ObservableCollection<Advert> DonationAdverts { get; set; }
        public ObservableCollection<Advert> ExchangeAdverts { get; set; }
        AdvertViewModel advertViewModel = new AdvertViewModel();
        
        public CategoryList()
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

        public string CImage { get; }
    
        public CategoryList(string cname)
        {
            InitializeComponent();

            DonationAdverts = new ObservableCollection<Advert>();
            ExchangeAdverts = new ObservableCollection<Advert>();

            BindingContext = advertViewModel;

            CName = cname;
            this.CategoryName.Text = CName;
            var back_tap = new TapGestureRecognizer();
            back_tap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            back.GestureRecognizers.Add(back_tap);
            DonationsListView.ItemsSource = DonationAdverts;
            ExchangesListView.ItemsSource = ExchangeAdverts;
            OnAppearing();
            



            DonationsListView.RefreshCommand = new Command(async () => {
                DonationsListView.RefreshControlColor = Color.FromHex("#1B9101");
                DonationsListView.IsRefreshing = true;
                var donationAdverts = await advertViewModel.GetDonationAdverts();

                foreach (var item in donationAdverts)
                {
                    if ((item.BookCategory.ToString() == CategoryName.Text))
                    {
                        DonationsListView.ItemsSource = DonationAdverts;
                    }
                }
                
                DonationsListView.IsRefreshing = false;
            });

            ExchangesListView.RefreshCommand = new Command(async () => {
                ExchangesListView.RefreshControlColor = Color.FromHex("#00B5B9");
                ExchangesListView.IsRefreshing = true;
                var exchangeAdverts = await advertViewModel.GetExchangeAdverts();

                foreach (var item in exchangeAdverts)
                {
                    if ((item.BookCategory.ToString() == CategoryName.Text))
                    {
                        ExchangesListView.ItemsSource = ExchangeAdverts;
                    }
                }
                
                ExchangesListView.IsRefreshing = false;
            });
           
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await CategoryListAppears();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
        public async void DonationListview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Advert;

            await Navigation.PushAsync(new BookPage(myItem.BookName, myItem.BookAuthor, myItem.BookCategory, myItem.Condition, myItem.ShareType, myItem.Description, myItem.PublisherEmail, myItem.Image));
        }
        public async void ExchangeListview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Advert;

            await Navigation.PushAsync(new BookPage(myItem.BookName, myItem.BookAuthor, myItem.BookCategory, myItem.Condition, myItem.ShareType, myItem.Description, myItem.PublisherEmail, myItem.Image));
        }
        
        private async Task CategoryListAppears()
        {
            //-------------- To show Donation Adverts on the main page
            var donationAdverts = await advertViewModel.GetDonationAdverts();

            //List reversed to show people last adverts.
            donationAdverts.Reverse();

            foreach (var item in donationAdverts)
            {
                if (!DonationAdverts.Any((arg) => arg.AdvertID == item.AdvertID))
                {
                    if (( item.BookCategory.ToString() == CategoryName.Text))
                    {
                        DonationAdverts.Add(item);
                    }
                        
                    
                        
                   
                }
                    
                       
                    



            }
            //------------------------------------------------------------------------
            //-------------- To show Donation Adverts on the main page
            var exchangeAdverts = await advertViewModel.GetExchangeAdverts();

            //List reversed to show people last adverts.
            exchangeAdverts.Reverse();
            foreach (var item in exchangeAdverts)
            {
                if (!ExchangeAdverts.Any((arg) => arg.AdvertID == item.AdvertID ))
                {
                    if ((item.BookCategory.ToString() == CategoryName.Text))
                    {
                        ExchangeAdverts.Add(item);
                    }
                }
                    
                        
             
            }

            //------------------------------------------------------------------------




        }
    }
}