using GoldenShelf.ViewModels;
using GoldenShelf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoldenShelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : Shell
    {
        public byte[] imagebyte;
        public ObservableCollection<Advert> DonationAdverts { get; set; }
        public ObservableCollection<Advert> ExchangeAdverts { get; set; }

        public ObservableCollection<Message> Messages { get; set; }


        AdvertViewModel advertViewModel = new AdvertViewModel();


        public HomePage()
        {
            InitializeComponent();
            var myadverts_tap = new TapGestureRecognizer();
            myadverts_tap.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new MyAdverts()); ;
            };
            myAdverts.GestureRecognizers.Add(myadverts_tap);

            Messages = new ObservableCollection<Message>();
            DonationAdverts = new ObservableCollection<Advert>();
            ExchangeAdverts = new ObservableCollection<Advert>();

            BindingContext = advertViewModel;

            var Categories = new List<Category>
            {
                new Category {CategoryName="TYT AYT",CategoryImage="Tyt.png" },
                new Category {CategoryName="LGS",CategoryImage="Lgs.jpg" },
                new Category {CategoryName="YDS TOEFL",CategoryImage="Toefl.jpg" },
                new Category {CategoryName="Academic Book",CategoryImage="Academic.png" },
                new Category {CategoryName="Children's Book",CategoryImage="Children.png" },
                new Category {CategoryName="Novel",CategoryImage="Novel.jpg" },
                new Category {CategoryName="Poem",CategoryImage="Poem.png" },
                new Category {CategoryName="Comic Book",CategoryImage="comicbook.jpg" },
                new Category {CategoryName="Others",CategoryImage="Others.png" }

            };




            CategoryListView.ItemsSource = Categories;
            DonationsListView.ItemsSource = DonationAdverts;
            ExchangesListView.ItemsSource = ExchangeAdverts;
            MessageListView.ItemsSource = Messages;
            OnAppearing();

            DonationsListView.RefreshCommand = new Command(async () =>
            {
                DonationsListView.RefreshControlColor = Color.FromHex("#1B9101");
                DonationsListView.IsRefreshing = true;
                var donationAdverts = await advertViewModel.GetDonationAdverts();

                foreach (var item in donationAdverts)
                {
                    if (!DonationAdverts.Any((arg) => arg.AdvertID == item.AdvertID))
                        DonationAdverts.Add(item);
                }
                DonationsListView.ItemsSource = DonationAdverts;
                DonationsListView.IsRefreshing = false;
            });

            ExchangesListView.RefreshCommand = new Command(async () =>
            {
                ExchangesListView.RefreshControlColor = Color.FromHex("#00B5B9");
                ExchangesListView.IsRefreshing = true;
                var exchangeAdverts = await advertViewModel.GetExchangeAdverts();

                foreach (var item in exchangeAdverts)
                {
                    if (!ExchangeAdverts.Any((arg) => arg.AdvertID == item.AdvertID))
                        ExchangeAdverts.Add(item);
                }
                ExchangesListView.ItemsSource = ExchangeAdverts;
                ExchangesListView.IsRefreshing = false;
            });
            ExchangesListView.RefreshCommand = new Command(async () =>
            {
                var Mainapp = Application.Current as App;
                var messages = await advertViewModel.GetMessages();

                messages.Reverse();
                foreach (var item in messages)
                {
                    if (!Messages.Any((arg) => arg.Id == item.Id))
                        if (!Messages.Any((arg) => item.Sender == arg.Sender && item.SpecialBookName == arg.SpecialBookName))

                            if ((item.Receiver.ToString() == Mainapp.Email))
                            {
                                Messages.Add(item);

                            }

                }
            });
           
        }

        private void BookSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchDonationResult = DonationAdverts.Where(c => c.BookName.ToLower().Contains(BookSearchBar.Text.ToLower()));
            var searchExchangeResult = ExchangeAdverts.Where(c => c.BookName.ToLower().Contains(BookSearchBar.Text.ToLower()));
            DonationsListView.ItemsSource = searchDonationResult;
            ExchangesListView.ItemsSource = searchExchangeResult;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await HomePageAppears();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }

        public async void CategoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedInstructor = e.Item as Category;
            await Navigation.PushAsync(new CategoryList(selectedInstructor.CategoryName));

        }

        public async void MessageListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedInstructor = e.Item as Message;
            await Navigation.PushAsync(new MessageDetailPage(selectedInstructor.Sender, selectedInstructor.SpecialBookName));

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
        private async void AddPhotoFromGallery(object sender, EventArgs e)
        {
            try
            {

                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsTakePhotoSupported)
                {

                    PopUpTitleShare.Text = "Not Supported!";
                    PopUpLabelShare.Text = "Your device does not currently support this functionality.";
                    popUpImageViewShare.IsVisible = true;
                    return;
                }

                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                if (selectedImage == null)
                {
                    PopUpTitleShare.Text = "Error!";
                    PopUpLabelShare.Text = "Could not get the image , please try again.";
                    popUpImageViewShare.IsVisible = true;
                }
                selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
                imagebyte = GetImageStreamAsBytes(selectedImageFile.GetStream());

                //TODO :Add selection of multichocice

            }
            catch (Exception)
            {
                
                PopUpTitleShare.Text = "Error!";
                PopUpLabelShare.Text = "Could not get the image , please try again.";
                popUpImageViewShare.IsVisible = true;
            }
        }
        private async void TakePhoto(object sender, EventArgs e)
        {
            try
            {

                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    PopUpTitleShare.Text = "Not Supported!";
                    PopUpLabelShare.Text = "Your device does not currently support this functionality.";
                    popUpImageViewShare.IsVisible = true;
                    return;
                }

                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                var selectedImageFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { AllowCropping = true, SaveToAlbum = true });


                if (selectedImage == null)
                {
                    PopUpTitleShare.Text = "Error!";
                    PopUpLabelShare.Text = "Could not get the image , please try again.";
                    popUpImageViewShare.IsVisible = true;
                }
                selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
                imagebyte = GetImageStreamAsBytes(selectedImageFile.GetStream());

                //TODO :Add selection of multichocice

            }
            catch (Exception)
            {
                PopUpTitleShare.Text = "Error!";
                PopUpLabelShare.Text = "Could not get the image , please try again.";
                popUpImageViewShare.IsVisible = true;
            }
        }
        // Image to Byte Converter
        public byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async void Share_Clicked(object sender, EventArgs e)
        {
            var app = Application.Current as App;
            try
            {
                Advert newAdvert = new Advert
                {
                    BookAuthor = bookAuthor.Text,
                    BookCategory = bookCategoryPicker.SelectedItem.ToString(),
                    BookName = bookName.Text,
                    Condition = bookConditionPicker.SelectedItem.ToString(),
                    Description = description.Text,
                    ShareType = shareTypePicker.SelectedItem.ToString(),
                    PublisherEmail = app.Email,
                    Image = imagebyte

                };
                advertViewModel.InsertAdvert(newAdvert);

            }
            catch
            {
                PopUpTitleShare.Text = "Error!";
                PopUpLabelShare.Text = "Please fill required spaces and try again.";
                popUpImageViewShare.IsVisible = true;
                return;
            }

            PopUpTitleShare.Text = "Successful!";
            PopUpLabelShare.Text = "You published a new advert succesfully.";
            popUpImageViewShare.IsVisible = true;
            bookAuthor.Text = "";
            selectedImage.Source = "imgclear.png";
            bookCategoryPicker.SelectedItem = null;
            bookName.Text = "";
            bookConditionPicker.SelectedItem = null;
            description.Text = "";
            shareTypePicker.SelectedItem = null;
            imagebyte = null;
            await HomePageAppears();



        }



        private async void EditProfile_Clicked(object sender, EventArgs e)
        {
            UserViewModel userView = new UserViewModel();
            var app = Application.Current as App;
            User user;

            try
            {
                user = await userView.GetUserByEmail(app.Email);
                try
                {

                    user = await userView.GetUserByEmail(app.Email);
                }
                catch (Exception ex)
                {
                    PopUpTitle.Text = "Kullanıcı Bulunamadı!";
                    PopUpLabel.Text = ex.Message;
                    popUpImageView.IsVisible = true;
                }

            }
            catch (Exception)
            {
                PopUpTitle.Text = "User not found!";
                PopUpLabel.Text = "User not found. Please register.";
                popUpImageView.IsVisible = true;
                return;
            }
            await Navigation.PushAsync(new EditProfilePage(user));
        }


        private async void MyAdvertsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyAdverts());
        }
        private async Task HomePageAppears()
        {
            var Mainapp = Application.Current as App;
            var messages = await advertViewModel.GetMessages();

            messages.Reverse();
            foreach (var item in messages)
            {
                if (!Messages.Any((arg) => arg.Id == item.Id))
                    if (!Messages.Any((arg) => item.Sender == arg.Sender && item.SpecialBookName == arg.SpecialBookName))

                        if ((item.Receiver.ToString() == Mainapp.Email))
                        {
                            Messages.Add(item);

                        }

            }
            //-------------- To show Donation Adverts on the main page
            var donationAdverts = await advertViewModel.GetDonationAdverts();

            //List reversed to show people last adverts.
            donationAdverts.Reverse();

            foreach (var item in donationAdverts)
            {
                if (!DonationAdverts.Any((arg) => arg.AdvertID == item.AdvertID))
                    DonationAdverts.Add(item);
            }
            //------------------------------------------------------------------------
            //-------------- To show Donation Adverts on the main page
            var exchangeAdverts = await advertViewModel.GetExchangeAdverts();

            //List reversed to show people last adverts.
            exchangeAdverts.Reverse();
            foreach (var item in exchangeAdverts)
            {
                if (!ExchangeAdverts.Any((arg) => arg.AdvertID == item.AdvertID))
                    ExchangeAdverts.Add(item);
            }

            //------------------------------------------------------------------------





            //To show profile information to user using saved email 

            UserViewModel userView = new UserViewModel();
            var app = Application.Current as App;
            User user;
            try
            {

                user = await userView.GetUserByEmail(app.Email);
                profileTabName.Text = user.name;
                profileTabLocation.Text = user.city + "/" + user.district;
                profileTabEmail.Text = user.email;
                changeName.Text = user.name;
                changeEmail.Text = user.email;
                changeLocation.Text = user.city + "/" + user.district;
                userImage.Source = ImageSource.FromStream(() => new MemoryStream(user.image));

            }
            catch (Exception ex)
            {
                PopUpTitle.Text = "User not found!";
                PopUpLabel.Text = ex.Message;
                popUpImageView.IsVisible = true;
            }

        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            // Alert Yes No
            var app = Application.Current as App;
            app.Email = "";
            app.LoggedIn = "false";
            App.Current.MainPage = new NavigationPage(new MainPage());

        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            UserViewModel userView = new UserViewModel();
            var app = Application.Current as App;
            User user;

            try
            {
                user = await userView.GetUserByEmail(app.Email);
                try
                {

                    user = await userView.GetUserByEmail(app.Email);
                }
                catch (Exception ex)
                {
                    PopUpTitle.Text = "User not found!";
                    PopUpLabel.Text = ex.Message;
                    popUpImageView.IsVisible = true;
                }

            }
            catch (Exception)
            {
                PopUpTitle.Text = "User not found!";
                PopUpLabel.Text = "User not found. Please register.";
                popUpImageView.IsVisible = true;
                return;
            }
            await Navigation.PushAsync(new EditProfilePage(user));
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FilterPage());
        }
        private void popUpButton(object sender, EventArgs e)
        {
            popUpImageView.IsVisible = false;
        }
        private void popUpButtonShare(object sender, EventArgs e)
        {
            if (PopUpTitleShare.Text.Equals("Successful!"))
            {
                popUpImageViewShare.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }
            else
            {
                popUpImageViewShare.IsVisible = false;
            }

        }
        private void popUpButtonMessage(object sender, EventArgs e)
        {
            if (PopUpTitleMessage.Text.Equals("Successful!"))
            {
                popUpImageViewMessage.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }
            else
            {
                popUpImageViewMessage.IsVisible = false;
            }

        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            var ob = btn.CommandParameter as Message;
            advertViewModel.DeleteMessages(ob);
            PopUpTitleMessage.Text = "Successful!";
            PopUpLabelMessage.Text = "These messages deleted succesfully.";
            popUpImageViewMessage.IsVisible = true;

        }
    }
}