using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfAppTest
{
    class Kos : INotifyPropertyChanged
    {
        private int id;
        private int guid;
        private DateTime cas_vnosa;

        public Kos()
        {
            cas_vnosa = DateTime.Now;
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
        public int Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
                OnPropertyChanged("Guid");
            }
        }
        public DateTime CasVnosa
        {
            get
            {
                return cas_vnosa;
            }
            set
            {
                cas_vnosa = value;
                OnPropertyChanged("CasVnosa");
            }
        }

        public string Print()
        {
            return String.Format("Kos -> id: {0}, guid: {1},  cas vnosa: {2}\n", id, guid, cas_vnosa);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
