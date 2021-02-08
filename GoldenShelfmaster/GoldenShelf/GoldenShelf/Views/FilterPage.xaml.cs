using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoldenShelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPage : ContentPage
    {
        public FilterPage()
        {
            InitializeComponent();
            var back_tap = new TapGestureRecognizer();
            back_tap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            back.GestureRecognizers.Add(back_tap);
        }
        string shareType = "";
        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (ExchangeCheck.IsChecked == false && DonationCheck.IsChecked == false)
            {
                ErrorLabel.IsVisible = true;
                await Task.Delay(2000);
                ErrorLabel.IsVisible = false;
            }
            else
            {

                await Navigation.PushAsync(new SpecialListPage(shareType));
            }

        }

        private void Donation_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ExchangeCheck.IsChecked == true)
            {
                ExchangeCheck.IsChecked = false;
                DonationCheck.IsChecked = true;
            }
            var result = e.Value;

            if (result == true)
            {
                shareType = "Donate";
            }

        }
        private void Exchange_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (DonationCheck.IsChecked == true)
            {
                DonationCheck.IsChecked = false;
                ExchangeCheck.IsChecked = true;
            }
            var result = e.Value;

            if (result == true)
            {
                shareType = "Exchange";
            }
        }
    }
}