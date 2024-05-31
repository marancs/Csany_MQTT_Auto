namespace NaviOkt.Model
{
    public class Auto : ObservableObject
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
    }
}
