
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoldenShelf
{
    class UserViewModel : BaseViewModel
    {
        static IMongoCollection<User> usersCollection;
        readonly static string dbName = "GoldenShelf";
        readonly static string collectionName = "Users";
        static MongoClient client;

        private string _name;
        private string _password;
        private string _email;
        private string _city;
        private string _district;
        private byte[] _image;

        public UserViewModel()
        {
            UpdateUserCommand = new Command(UpdateUser);
            SaveUserCommand = new Command(InsertUser);
            DeleteUserCommand = new Command(DeleteUser);
        }


        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetValue(ref _password, value); }

        }
        public string Email
        {
            get { return _email; }
            set { SetValue(ref _email, value); }
        }

        public string City
        {
            get { return _city; }
            set { SetValue(ref _city, value); }
        }
        public string District
        {
            get { return _district; }
            set { SetValue(ref _district, value); }
        }

        public byte[] Image
        {
            get { return _image; }
            set { SetValue(ref _image, value); }
        }


        #region Get Functions
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var users = await MongoConnection
                    .Find(new BsonDocument())
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;

        }
        public async Task<User> GetUserByEmail(String email)
        {
            var user = await MongoConnection
                .Find(f => f.email.Equals(email))
                .FirstOrDefaultAsync();

            return user;
        }


        #endregion
        public async void UpdateUser()
        {
            var User = new User
            {
                name = Name,
                password = Password,
                city = City,
                district = District,
                image = Image
            };
            await MongoConnection.InsertOneAsync(User);
            
        }
        public async Task UpdateUser(User user)
        {
            await MongoConnection.ReplaceOneAsync(t => t.email.Equals(user.email), user);
        }
    

        #region Search Functions 

        #endregion

        #region Save/Delete Functions
        public async void InsertUser()
        {
            var newUser = new User
            {
                name = Name,
                password = Password,
                email = Email,
                city = City,
                district = District,
                image = Image
            };
            try
            {

                await MongoConnection.InsertOneAsync(newUser);

            }
            catch (Exception)
            {

            }
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void InsertUser(User newUser)
        {
            try
            {
                await MongoConnection.InsertOneAsync(newUser);
            }
            catch
            {

            }
        }


        public async void DeleteUser(object obj)
        {
            var items = (User)obj;
            var result = await MongoConnection.DeleteOneAsync(tdi => tdi.email == items.email);


        }

        #endregion

        #region Command Functions

        public ICommand AddUserCommand { get; set; }

        public ICommand SaveUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand UpdateUserCommand { get; set; }

        #endregion

        #region Connection
        public IMongoCollection<User> MongoConnection
        {
            get
            {
                if (client == null || usersCollection == null)
                {

                    var connectionString = "mongodb://User:9MKh9Sdt4X5Xkt5z@goldenshelf-shard-00-00.j5cx5.mongodb.net:27017,goldenshelf-shard-00-01.j5cx5.mongodb.net:27017,goldenshelf-shard-00-02.j5cx5.mongodb.net:27017/<dbname>?ssl=true&replicaSet=atlas-9097aj-shard-0&authSource=admin&retryWrites=true&w=majority";
                    MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
                    client = new MongoClient(settings);
                    var db = client.GetDatabase(dbName);


                    var collectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                    usersCollection = db.GetCollection<User>(collectionName, collectionSettings);

                }
                return usersCollection;
            }
        }
        #endregion


    }
}