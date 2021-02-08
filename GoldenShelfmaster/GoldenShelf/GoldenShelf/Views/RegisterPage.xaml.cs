using GoldenShelf.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GoldenShelf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public ObservableCollection<string> GetUsers { get; set; }
        byte[] imagebyte;
        public RegisterPage()
        {
            InitializeComponent();
            GetUsers = new ObservableCollection<string>();
            BindingContext = new LocationViewModel();

            UserViewModel user = new UserViewModel();


            var back_tap = new TapGestureRecognizer();
            back_tap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };
            back.GestureRecognizers.Add(back_tap);


        }

        async void Button_Clicked(object sender, EventArgs e)
        {

            UserViewModel viewModel = new UserViewModel();
            foreach (var item in await viewModel.GetAllUsers())
            {
                GetUsers.Add(item.email);
            }
            
            var SelectedCity = "";
            var SelectedDistrict = "";
            if (cityPicker.SelectedItem != null ||districtPicker.SelectedItem != null)
            {
                var selectedItem = cityPicker.SelectedItem as City;
                SelectedCity = selectedItem.Value;
                var selectedItem2 = districtPicker.SelectedItem as City;
                SelectedDistrict = selectedItem2.Value;
            }
          
            if (name.Text != null && email.Text != null && SelectedCity.ToString() != "" && SelectedDistrict.ToString() != "")
            {
                if (password.Text == verifyPassword.Text)
                {
                   
                    var user = new User
                    {
                        name = name.Text,
                        email = email.Text,
                        city = SelectedCity.ToString(),
                        district = SelectedDistrict.ToString(),
                        password = password.Text,
                        image = imagebyte
                    };
                    if (GetUsers.Contains(user.email))
                    {
                        PopUpTitle.Text = "Error!";
                        PopUpLabel.Text = "You have registered already!";
                        popUpImageView.IsVisible = true;
                    }
                    else
                    {
                        try
                        {
                            viewModel.InsertUser(user);

                        }
                        catch (Exception)
                        {
                            throw;

                        }
                        PopUpTitle.Text = "Registration!";
                        PopUpLabel.Text = "You have registered successfully.";
                        popUpImageView.IsVisible = true;
                    }
                   

                }
                else
                {
                    Error.Text = "Passwords do not match!";
                    Error.IsVisible = true;
                    await Task.Delay(2000);
                    Error.IsVisible = false;
                }
            }
            else
            {
                Error.Text = "Please fill all of them!";
                Error.IsVisible = true;
                await Task.Delay(2000);
                Error.IsVisible = false;
            }






        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }

        private async void AddPhotoFromGallery(object sender, EventArgs e)
        {
            try
            {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsTakePhotoSupported)
            {
                PopUpTitle.Text = "Not Supported!";
                PopUpLabel.Text = "Your device does not currently support this functionality.";
                popUpImageView.IsVisible = true;
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };
            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImage == null)
            {
                PopUpTitle.Text = "Error!";
                PopUpLabel.Text = "Could not get the image , please try again.";
                popUpImageView.IsVisible = true;
            }
            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            imagebyte = GetImageStreamAsBytes(selectedImageFile.GetStream());

                //TODO :Add selection of multichocice

            }
            catch (Exception)
            {
                PopUpTitle.Text = "Error!";
                PopUpLabel.Text = "Could not get the image , please try again.";
                popUpImageView.IsVisible = true;
            }
        }
        private async void TakePhoto(object sender, EventArgs e)
        {
            try
            {


            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsTakePhotoSupported)
            {
                PopUpTitle.Text = "Not Supported!";
                PopUpLabel.Text = "Your device does not currently support this functionality.";
                popUpImageView.IsVisible = true;
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };
            var selectedImageFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { AllowCropping = true, SaveToAlbum = true });


            if (selectedImage == null)
            {
                PopUpTitle.Text = "Error!";
                PopUpLabel.Text = "Could not get the image , please try again.";
                popUpImageView.IsVisible = true;
            }
            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            imagebyte = GetImageStreamAsBytes(selectedImageFile.GetStream());

                //TODO :Add selection of multichocice

            }
            catch
            {
                PopUpTitle.Text = "Error!";
                PopUpLabel.Text = "Could not get the image , please try again.";
                popUpImageView.IsVisible = true;
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
        private void popUpButton(object sender, EventArgs e)
        {
            popUpImageView.IsVisible = false;
        }


    }
}