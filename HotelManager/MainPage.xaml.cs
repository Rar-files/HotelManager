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
        HotelDBEntities context = new HotelDBEntities();

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
            var custPage = new CustomerPage();
            var window = (Window)this.Parent;
            window.Content = custPage;
        }

        private void CustomerSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            if (int.TryParse(customerSearch.Text, out int ID))
            {
                SearchCustomersList.Visibility = Visibility.Visible;
                var customers = (from c in context.Customers
                                 orderby c.LastName
                                 select new { Customer = (c.LastName + " " + c.FirstName), ID = c.CustomerID }); ;

                var customersList = customers.ToList();
                foreach (var e in customers.ToList())
                {
                    if (!CheckCustomerID(e.ID, ID))
                    {
                        customersList.Remove(e);
                    }
                }

                SearchCustomersList.ItemsSource = customersList;
            }
        }

        private void DataGridSearchRowClick(object sender, MouseButtonEventArgs e)
        {
            Customers customer;
            int ID;
            if (!int.TryParse((e.OriginalSource as TextBlock).Text, out ID))
            {
                var row = sender as DataGridRow;

                var cell = DataGridTools.GetCell(SearchCustomersList, row, 1);
                cell.IsEnabled = false;

                ID = int.Parse((cell.Content as TextBlock).Text);
            }

            customer = context.Customers.Find(ID);

            SearchCustomersList.Visibility = Visibility.Collapsed;
            customerSearch.Text = "Customer";
            var custPage = new CustomerPage(customer);
            var window = (Window)this.Parent;
            window.Content = custPage;
        }

        private bool CheckCustomerID(int test, int target)
        {
            char[] tests = test.ToString().ToCharArray();
            char[] targets = target.ToString().ToCharArray();
            bool checkFlag = true;
            for (int i = 0; i < targets.Length & checkFlag; i++)
            {
                if (tests[i] != targets[i]) checkFlag = false;
            }
            return checkFlag;
        }
    }
}
