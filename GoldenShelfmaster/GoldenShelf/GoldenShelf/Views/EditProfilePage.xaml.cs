using GoldenShelf.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoldenShelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        byte[] imagebyte;

        public object SelectedCity { get; set; }
        public object SelectedDistrict { get; set; }

        public EditProfilePage(User user)
        {
            InitializeComponent();
            BindingContext = new LocationViewModel();
                name.Text = user.name;
                oldPassword.Text = user.password;
                locCity.Text = user.city.ToString();
                locDist.Text = user.district.ToString();
            
            //SelectedCity = new City { Key = 1, Value = user.city.ToString() };
            //SelectedDistrict = new City { Key = 1, Value = user.district.ToString() };
            var back_tap = new TapGestureRecognizer();
            back_tap.Tapped += async (s, e) =>
            {
                await Navigation.PopAsync();
            };

            back.GestureRecognizers.Add(back_tap);

            /*var selectedItem = cityPicker.SelectedItem as City;
            var selectedItem2 = districtPicker.SelectedItem as City;
            cityPicker.SelectedItem = SelectedCity;
            districtPicker.SelectedItem = SelectedDistrict;*/
            





        }

        async void Edit_Clicked(object sender, EventArgs e)
        {
            var app = Application.Current as App;
            UserViewModel viewModel = new UserViewModel();
            var SelectedCity = "";
            var SelectedDistrict = "";
            string userEmail = app.Email;
            if (cityPicker.SelectedItem != null || districtPicker.SelectedItem != null)
            {
                var selectedItem = cityPicker.SelectedItem as City;
                SelectedCity = selectedItem.Value;
                var selectedItem2 = districtPicker.SelectedItem as City;
                SelectedDistrict = selectedItem2.Value;
            }
            if (name.Text != null && SelectedCity.ToString() != "" && SelectedDistrict.ToString() != "" && newPassword.Text == null && verifyPassword.Text == null)
            {
                if (newPassword.Text == verifyPassword.Text)
                {
                    if (oldPassword.Text != newPassword.Text)
                    {
                        var user = new User
                        {
                            name = name.Text,
                            city = SelectedCity.ToString(),
                            email = userEmail,
                            district = SelectedDistrict.ToString(),
                            password = oldPassword.Text,
                            image = imagebyte
                        };
                        await viewModel.UpdateUser(user);
                        PopUpTitle.Text = "Edit Profile!";
                        PopUpLabel.Text = "You have updated successfully. You have to login again.";
                        popUpImageView.IsVisible = true;
                        app.Email = "";
                        app.LoggedIn = "false";
                    }
                    else
                    {
                        Error.Text = "Your password cannot be the same as before!";
                        Error.IsVisible = true;
                        await Task.Delay(2000);
                        Error.IsVisible = false;
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
                if (name.Text != null  && newPassword.Text != null && verifyPassword.Text != null)
                {

                    if (newPassword.Text == verifyPassword.Text)
                    {
                        if (oldPassword.Text != newPassword.Text)
                        {
                            var user = new User
                            {
                                name = name.Text,
                                city = locCity.Text,
                                email = userEmail,
                                district = locDist.Text,
                                password = newPassword.Text,
                                image = imagebyte
                            };
                            await viewModel.UpdateUser(user);
                            PopUpTitle.Text = "Edit Profile!";
                            PopUpLabel.Text = "You have updated successfully. You have to login again.";
                            popUpImageView.IsVisible = true;
                            app.Email = "";
                            app.LoggedIn = "false";
                        }
                        else
                        {
                            Error.Text = "Your password cannot be the same as before!";
                            Error.IsVisible = true;
                            await Task.Delay(2000);
                            Error.IsVisible = false;
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
                    PopUpTitle.Text = "Not supported!";
                    PopUpLabel.Text = "our device does not currently support this functionality.";
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
            catch (Exception)
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

            if (PopUpTitle.Text.Equals("Edit Profile!"))
            {
                popUpImageView.IsVisible = false;
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                popUpImageView.IsVisible = false;
            }
        }
    }
}