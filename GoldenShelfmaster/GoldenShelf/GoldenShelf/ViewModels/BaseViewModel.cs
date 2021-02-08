using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GoldenShelf { 
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName="null") 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingField,T value, [CallerMemberName] string propertyName = "null")
        {
            if(EqualityComparer<T>.Default.Equals(backingField, value)) // Compares if backingfield and values are same
                return;
            backingField = value;
                                                                     // If not calls OnProperty Changed
            OnPropertyChanged(propertyName);
        }
    }
}
