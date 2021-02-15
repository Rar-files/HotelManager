using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // customersViewSource.Źródło = [ogólne źródło danych]
        }

        private void AddReservationHandler(object sender, ExecutedRoutedEventArgs e)
        {
            //var reservation = new ReservationWindow();
            //reservation.Show();
        }

        private void AddCustomerHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var customer = new CustomerPage();
            var window = (Window)this.Parent;
            window.Content = customer;
        }
    }
}
