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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageDetailPage : ContentPage
    {

        public ObservableCollection<Message> Messages { get; set; }
        public ObservableCollection<Message> MessagesOrdered { get; set; }
        public string Location;
        AdvertViewModel advertViewModel = new AdvertViewModel();
        public MessageDetailPage()
        {

            InitializeComponent();
            
            Messages = new ObservableCollection<Message>();

            BindingContext = advertViewModel;
            backclick();

        }

        public MessageDetailPage(string sender, string SpecialBookName)
        {
            InitializeComponent();
            
            Messages = new ObservableCollection<Message>();
           
            BindingContext = advertViewModel;
            backclick();
            this.SpecialBookName = SpecialBookName;
            this.BookName.Text = SpecialBookName;
            
            Sender = sender;
            this.SenderName.Text = Sender;


            OnAppearing();
            listview1.RefreshCommand = new Command(async () => {
                listview1.RefreshControlColor = Color.FromHex("#cba135");
                await HomePageAppears();
                listview1.ItemsSource = Messages.OrderBy(i => i.Date);
                listview1.IsRefreshing = false;
            });
            



        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await HomePageAppears();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                var v = listview1.ItemsSource.Cast<object>().LastOrDefault();
                listview1.ScrollTo(v, ScrollToPosition.End, true);
                return false;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }


        public string SpecialBookName { get; }
        public string Sender { get; }

        void backclick()
        {
            back.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await Navigation.PopAsync();

                })
            });
        }
        private async Task HomePageAppears()
        {
            var Mainapp = Application.Current as App;
            var messages = await advertViewModel.GetMessages();
            var myMessages = await advertViewModel.GetMyMessages();
            


            foreach (var item in messages)
            {
                if (!Messages.Any((arg) => arg.Id == item.Id))
                    if ((item.Receiver.ToString() == Mainapp.Email))
                    {
                        if (item.Sender.ToString() == SenderName.Text && item.SpecialBookName.ToString() == BookName.Text)
                        {
                            item.Location = "0";

                            Messages.Add(item);
                        }

                    }
               
            }
            foreach (var item in myMessages)
            {
                if (!Messages.Any((arg) => arg.Id == item.Id))
                    if ((item.Sender.ToString() == Mainapp.Email))
                    {
                        if (item.Receiver.ToString() == SenderName.Text && item.SpecialBookName.ToString() == BookName.Text)
                        {
                            item.Location = "1";

                            Messages.Add(item);
                        }

                    }
            }

            listview1.ItemsSource = Messages.OrderBy(i => i.Date);
            
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            var app = Application.Current as App;
            try
            {
                Message newMessage = new Message
                {
                    SpecialBookName = BookName.Text,
                    Sender = app.Email,
                    Receiver = SenderName.Text,
                    MessageText = senderMessage.Text,
                    Date = DateTime.Now
                };
                advertViewModel.InsertMessage(newMessage);   
            }
            catch
            {
                PopUpTitleAdvert.Text = "Error!";
                PopUpLabelAdvert.Text = "Please write a message.";
                popUpImageViewAdvert.IsVisible = true;
                return;
            }
            
            senderMessage.Text = "";
            OnAppearing();


        }
        private void popUpButton(object sender, EventArgs e)
        {
            popUpImageViewAdvert.IsVisible = false;
        }
    }
}