using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace WpfAppTest
{
    class DelovniNalog : INotifyPropertyChanged
    {
        private int id;
        private string naslov;
        private int stKosov;
        private IList<Kos> _KosiList;
        private string info;

        public DelovniNalog()
        {
            _KosiList = new List<Kos>();
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Naslov
        {
            get
            {
                return naslov;
            }
            set
            {
                naslov = value;
                OnPropertyChanged("Naslov");
            }
        }
        public int StKosov
        {
            get
            {
                return stKosov;
            }
            set
            {
                stKosov = value;
                OnPropertyChanged("StKosov");
            }
        }

        public IList<Kos> KosiList
        {
            get
            {
                return _KosiList;
            }
            set
            {
                _KosiList = value;
                OnPropertyChanged("KosiList");
            }
        }

        public string Info
        {
            get { return info; }
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }

        public void addKos(Kos kos)
        {
            _KosiList.Add(kos);
            OnPropertyChanged("KosiList");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool SaveToDatabase()
        {
            Connection connection = (Connection)Application.Current.Resources["Connection"];

            int delovniNalogId = connection.InsertDelovniNalog(Naslov, StKosov);
            foreach (Kos kos in KosiList)
            {
                bool success = connection.InsertKos(kos.Guid, delovniNalogId, kos.CasVnosa);
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
