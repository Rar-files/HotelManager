using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource custViewSource;

        public MainPage()
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // customersViewSource.Źródło = [ogólne źródło danych]
            context.Customers.Load();
            custViewSource.Source = context.Customers.Local;
        }

        private void AddReservationHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewReservationWindow().Show();
        }

        private void AddCustomerHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewCustomerWindow().Show();
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
                    try
                    {
                        if (!CheckCustomerID(e.ID, ID))
                        {
                            customersList.Remove(e);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        customersList.Remove(e);
                    }
                }

                SearchCustomersList.ItemsSource = customersList;
            }
        }

        private void DataGridSearchRowClick(object sender, MouseButtonEventArgs e)
        {
            var txt = (e.OriginalSource as TextBlock).Text;
            if (txt != null)
            {
                int ID;
                if (!int.TryParse(txt, out ID))
                {
                    var row = sender as DataGridRow;

                    var cell = DataGridTools.GetCell(SearchCustomersList, row, 1);
                    cell.IsEnabled = false;

                    ID = int.Parse((cell.Content as TextBlock).Text);
                }

                SearchCustomersList.Visibility = Visibility.Collapsed;
                customerSearch.Text = "Customer";
                var custPage = new CustomerPage(ID);
                var window = (Window)this.Parent;
                window.Content = custPage;
            }
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
