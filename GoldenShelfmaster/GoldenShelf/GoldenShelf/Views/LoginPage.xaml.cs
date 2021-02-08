using GoldenShelf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoldenShelf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            backclick();
          
        }

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
        public void ShowPass(object sender, EventArgs args)
        {
            Password.IsPassword = Password.IsPassword ? false : true;
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            if (email.Text != null)
            {
                if (Password.Text != null)
                {


                    UserViewModel userView = new UserViewModel();
                    var app = Application.Current as App;
                    User user;
                    try
                    {

                        user = await userView.GetUserByEmail(email.Text);

                        if (email.Text.Equals(user.email) && Password.Text.Equals(user.password))
                        {
                            app.Email = email.Text;
                            app.LoggedIn = "true";
                            App.Current.MainPage = new HomePage();

                        }
                        else
                        {
                            PopUpTitle.Text = "LoginPage Failed!";
                            PopUpLabel.Text = "You entered incorrect email/password.Try Again.";
                            popUpImageView.IsVisible = true;
                            return;
                        }

                    }
                    catch (Exception)
                    {
                        PopUpTitle.Text = "User not found!";
                        PopUpLabel.Text = "User not found. Please register.";
                        popUpImageView.IsVisible = true;
                        return;
                    }
                }
                else
                {

                    passwordFrame.BackgroundColor = Color.LightGray;
                    Error.Text = "Please fill your password!";
                    Error.TextColor = Color.Red;
                    await Task.Delay(2000);
                    Error.TextColor = Color.Transparent;
                    passwordFrame.BackgroundColor = Color.White;
                }
            }
            else
            {
                emailFrame.BackgroundColor = Color.LightGray;
                passwordFrame.BackgroundColor = Color.LightGray;
                Error.Text = "Please fill your e-mail and password!";
                Error.TextColor = Color.Red;
                await Task.Delay(2000);
                Error.TextColor = Color.Transparent;
                emailFrame.BackgroundColor = Color.White;
                passwordFrame.BackgroundColor = Color.White;
            }
        }
        private void popUpButton(object sender, EventArgs e)
        {
            popUpImageView.IsVisible = false;
        }
    }
}