using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GoldenShelf.ViewModels;
using System.IO;
using GoldenShelf.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace GoldenShelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookPage : ContentPage
    {
        string publisherEmail;
        AdvertViewModel advertViewModel = new AdvertViewModel();
        public BookPage(String BookName,String BookAuthor,String BookCategory,String Condition ,String ShareType, String Description ,String PublisherEmail,byte[]image)
        {
            InitializeComponent();

            BindingContext = new AdvertViewModel();
            var back_tap = new TapGestureRecognizer();
            back_tap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            back.GestureRecognizers.Add(back_tap);
            

            if (ShareType == "Donate")
            {
                imageBG.BackgroundColor = Color.FromHex("#1B9101");
            }
            if (ShareType == "Exchange")
            {
               imageBG.BackgroundColor = Color.FromHex("#00B5B9");
            }
            if (ShareType == "Donate")
            {
                BgMessage.BackgroundColor = Color.FromHex("#1B9101");
            }
            if (ShareType == "Exchange")
            {
                BgMessage.BackgroundColor = Color.FromHex("#00B5B9");
            }


            bookName.Text = BookName;
            bookAuthor.Text = BookAuthor;
            bookCategory.Text = BookCategory;
            condition.Text = Condition;
            shareType.Text = ShareType;
            description.Text = Description;

            adImage.Source = ImageSource.FromStream(() => new MemoryStream(image));
            getPublisherInfo(PublisherEmail);
            
        }
        
        private async void getPublisherInfo(String email)
        {
            UserViewModel userView = new UserViewModel();
            User user = await userView.GetUserByEmail(email);
            userEmail.Text = user.email;
            userName.Text = user.name;
            location.Text = user.city + "/" + user.district;
            publisherEmail = user.email;
            var app = Application.Current as App;
            if (userEmail.Text.Equals(app.Email))
            {
                BgMessage.IsVisible = false;
            }
        }


        private  void SendButton_Clicked(object sender, EventArgs e)
        {
            if (messageText.Text == null || messageText.Text == "")
            {
                PopUpTitleAdvert.Text = "Error!";
                PopUpLabelAdvert.Text = "Please write a message.";
                popUpImageViewAdvert.IsVisible = true;
            }
            else
            {
                var app = Application.Current as App;
                try
                {
                    Message newMessage = new Message
                    {
                        SpecialBookName = bookName.Text,
                        Sender = app.Email,
                        Receiver = publisherEmail,
                        MessageText = messageText.Text,
                        Date = DateTime.Now
                    };
                    advertViewModel.InsertMessage(newMessage);
                    PopUpTitleAdvert.Text = "Successful!";
                    PopUpLabelAdvert.Text = "You send a message succesfully.";
                    popUpImageViewAdvert.IsVisible = true;
                    messageText.Text = "";

                }
                catch
                {

                    return;
                }
            }

           

        }
        private void popUpButton(object sender, EventArgs e)
        {
            popUpImageViewAdvert.IsVisible = false;
        }

    }
}