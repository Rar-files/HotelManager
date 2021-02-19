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
using System.Data.Entity;


namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource custViewSource;
        CollectionViewSource reservViewSource;
        Customers initial = null;

        public CustomerPage()
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            reservViewSource = ((CollectionViewSource)(this.FindResource("customersReservationsViewSource")));
            DataContext = this;
        }

        public CustomerPage(Customers customer)
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            reservViewSource = ((CollectionViewSource)(this.FindResource("customersReservationsViewSource")));
            DataContext = this;
            initial = customer;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // customersViewSource.Źródło = [ogólne źródło danych]
            context.Customers.Load();
            custViewSource.Source = context.Customers.Local;
            if(initial != null)
                custViewSource.View.MoveCurrentTo(initial);
        }

        private void DeleteCustomerCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // If existing window is visible, delete the customer and all their orders.  
            // In a real application, you should add warnings and allow the user to cancel the operation.  
            var customers = custViewSource.View.CurrentItem as Customers;

            var customer = (from c in context.Customers
                            where c.CustomerID == customers.CustomerID
                            select c).FirstOrDefault();

            int currentID = customer.CustomerID;

            if (customer != null)
            {
                foreach (var reserv in customer.Reservations.ToList())
                {
                    customer.Reservations.Remove(reserv);
                }
                context.Customers.Remove(customer);
            }

            context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT('Customers', RESEED, {currentID-1})");
            context.SaveChanges();
            custViewSource.View.Refresh();
        }

        // Commit changes from the new customer form, the new order form,  
        // or edits made to the existing customer form.  
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

            // Order ID is auto-generated so we don't set it here.  
            // For CustomerID, address, etc we use the values from current customer.  
            // User can modify these in the datagrid after the order is entered.  
            var currentCustomer = custViewSource.View.CurrentItem as Customers;

            if (currentCustomer != null)
            {
                currentCustomer.FirstName = firstNameTextBox.Text;
                currentCustomer.LastName = lastNameTextBox.Text;
                currentCustomer.Email = emailTextBox.Text;
                currentCustomer.Phone = phoneTextBox.Text;
                currentCustomer.PESEL = peselTextBox.Text;
                currentCustomer.PIN = pinTextBox.Text;
                currentCustomer.IDCardSeries = idCardSeriesTextBox.Text;
                currentCustomer.CarNumber = carNumberTextBox.Text;
                currentCustomer.Address = addressTextBox.Text;
                currentCustomer.City = cityTextBox.Text;
                currentCustomer.PostalCode = postalCodeTextBox.Text;
                currentCustomer.County = countyTextBox.Text;
                currentCustomer.Country = countryTextBox.Text;
                currentCustomer.AdditionalInfo = additionalInfoTextBox.Text;
            }


            // Save the changes, either for a new customer, a new order  
            // or an edit to an existing customer or order.
            context.SaveChanges();
            custViewSource.View.Refresh();
        }

        // Sets up the form so that user can enter data. Data is later  
        // saved when user clicks Commit.  
        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewCustomerWindow().Show();
        }

        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var main = new MainPage();
            var window = (Window)this.Parent;
            window.Content = main;
        }

        private void CustomerSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            if(int.TryParse(customerSearch.Text,out int ID))
            {
                SearchCustomersList.Visibility = Visibility.Visible;
                var customers = (from c in context.Customers
                                 orderby c.LastName
                                 select new { Customer = (c.LastName + " " + c.FirstName), ID = c.CustomerID }); ;

                var customersList = customers.ToList();
                foreach (var e in customers.ToList())
                {
                    if(!CheckCustomerID(e.ID,ID))
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
                Customers customer;
                int ID;
                if (!int.TryParse(txt, out ID))
                {
                    var row = sender as DataGridRow;

                    var cell = DataGridTools.GetCell(SearchCustomersList, row, 1);
                    cell.IsEnabled = false;

                    ID = int.Parse((cell.Content as TextBlock).Text);
                }

                customer = context.Customers.Find(ID);

                SearchCustomersList.Visibility = Visibility.Collapsed;
                customerSearch.Text = "Customer";
                new CustomerPage(customer);
            }
        }

        private bool CheckCustomerID(int test, int target)
        {
            char[] tests = test.ToString().ToCharArray();
            char[] targets = target.ToString().ToCharArray();
            bool checkFlag = true;
            for(int i = 0; i<targets.Length & checkFlag; i++)
            {
                if (tests[i] != targets[i]) checkFlag = false;
            }
            return checkFlag;
        }


    }

    
}
