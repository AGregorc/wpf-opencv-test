using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private object currentView;
        public object CurrentView
        {
            get
            {
                return currentView;
            }
            set
            {
                currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CurrentView = new UserControlDelNalog();
        }

        private void Button_Click_Sql(object sender, RoutedEventArgs e)
        {
            if (!(CurrentView is UserControlDelNalog))
            {
                CurrentView = new UserControlDelNalog();
            }
        }

        private void Button_Click_OpenCV(object sender, RoutedEventArgs e)
        {
            if (!(CurrentView is UserControlOpenCV))
            {
                CurrentView = new UserControlOpenCV();
            }
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
