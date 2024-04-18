using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NaviOkt.Model
{
    public class Auto : INotifyPropertyChanged
    {

        private string? _rendszam;
        private string? _tipus;
        private string? _marka;
        private int _kor;

        public string Rendszam
        {
            get { return _rendszam; }
            set { SetProperty(ref _rendszam, value); }
        }
        public string Tipus
        {
            get { return _tipus; }
            set { SetProperty(ref _tipus, value); }
        }
        public string Marka
        {
            get { return _marka; }
            set { SetProperty(ref _marka, value); }
        }
        public int Kor
        {
            get { return _kor; }
            set { SetProperty(ref _kor, value); }
        }

        public Auto()
        {

        }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
