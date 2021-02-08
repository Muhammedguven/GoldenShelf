using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace GoldenShelf.ViewModels
{
    public class LocationViewModel : INotifyPropertyChanged
    {
        public List<City> CitiesList { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<City> _myCity;
        public ObservableCollection<City> MyCity
        {
            get { return _myCity; }
            set
            {
                if (_myCity != value)
                {
                    _myCity = value;
                    OnPropertyChanged();
                }
            }
        }

        private City _selectedCity { get; set; }
        public City SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    if (_selectedCity.Value.Equals("Eskişehir"))
                    {
                        MyCity = new ObservableCollection<City>()
                        {
                            new City() { Key = 1, Value = "Tepebaşı" },
                            new City() { Key = 2, Value = "Odunpazarı" }
                        };
                    }
                    if (_selectedCity.Value.Equals("Ankara"))
                    {
                        MyCity = new ObservableCollection<City>()
                        {
                            new City() { Key = 1, Value = "Kızılay" },
                            new City() { Key = 2, Value = "Etimesgut" }
                        };
                    }
                    if (_selectedCity.Value.Equals("İstanbul"))
                    {
                        MyCity = new ObservableCollection<City>()
                        {
                            new City() { Key = 1, Value = "Kadıköy" },
                            new City() { Key = 2, Value = "Beşiktaş" }
                        };
                        

                    }
                }
            }
        }
        public LocationViewModel()
        {
            CitiesList = GetCities().OrderBy(t => t.Value).ToList();
            MyCity = new ObservableCollection<City>();
        }
        public List<City> GetCities()
        {
            var cities = new List<City>()
            {
                new City(){Key =  1, Value= "Eskişehir"},
                new City(){Key =  2, Value= "Ankara"},
                new City(){Key =  3, Value= "İstanbul"}
            };

            return cities;
        }
    }


    public class City
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
