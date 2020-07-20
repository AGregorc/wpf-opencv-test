using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfAppTest
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public int SwitchView { get; set; }

        public int NumKosov { get; set; }

        public MainWindowViewModel()
        {
            SwitchView = 0;
            NumKosov = 0;
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
