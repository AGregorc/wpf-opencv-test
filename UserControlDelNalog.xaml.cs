using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UserControlDelNalog.xaml
    /// </summary>
    public partial class UserControlDelNalog : UserControl
    {
        private DelovniNalog delovniNalog;

        public UserControlDelNalog()
        {
            InitializeComponent();
            delovniNalog = new DelovniNalog();
            this.DataContext = delovniNalog;
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click_Next(object sender, RoutedEventArgs e)
        {
            int stKosov = Int32.Parse(StKosovTB.Text);
            PrintKosiText.Text = "";
            KosiViewer.Children.Clear();
            for (int i = 0; i < stKosov; i++)
            {
                Kos kos = new Kos();
                delovniNalog.addKos(kos);
                UserControlKos UCKos = new UserControlKos();
                UCKos.DataContext = kos;
                KosiViewer.Children.Add(UCKos);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            string result = "";
            //Task.Factory.StartNew(() => delovniNalog.Info = "Shranjujem v bazo ...");
            //foreach (Kos kos in delovniNalog.KosiList) {
            //    result += kos.Print();
            //}
            if (delovniNalog.SaveToDatabase())
            {
                result = "Končano!";
            } else
            {
                result = "Prišlo je do težave!";
            }
            PrintKosiText.Text = result;
            CleanCurrentDN();
        }

        private void CleanCurrentDN()
        {
            delovniNalog = new DelovniNalog();
            this.DataContext = delovniNalog;
            KosiViewer.Children.Clear();
        }

        private void StKosovTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
