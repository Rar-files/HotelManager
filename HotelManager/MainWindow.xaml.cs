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
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource custViewSource;
        CollectionViewSource reservViewSource;


        public MainWindow()
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            reservViewSource = ((CollectionViewSource)(this.FindResource("customersReservationsViewSource")));
            DataContext = this;


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Customers.Load();

            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // customersViewSource.Źródło = [ogólne źródło danych]
            custViewSource.Source = context.Customers.Local;


        }

        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            custViewSource.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            custViewSource.View.MoveCurrentToNext();
        }

        private void DeleteCustomerCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // If existing window is visible, delete the customer and all their orders.  
            // In a real application, you should add warnings and allow the user to cancel the operation.  
            var customers = custViewSource.View.CurrentItem as Customers;

            var customer = (from c in context.Customers
                        where c.CustomerID == customers.CustomerID
                        select c).FirstOrDefault();

            if (customer != null)
            {
                foreach (var reserv in customer.Reservations.ToList())
                {
                    customer.Reservations.Remove(reserv);
                }
                context.Customers.Remove(customer);
            }
            context.SaveChanges();
            custViewSource.View.Refresh();
        }

        // Commit changes from the new customer form, the new order form,  
        // or edits made to the existing customer form.  
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newCustomerGrid.IsVisible)
            {
                // Create a new object because the old one  
                // is being tracked by EF now.  
                Customers newCustomer = new Customers
                {
                    FirstName = newFirstNameTextBox.Text,
                    LastName = newLastNameTextBox.Text,
                    Email = newEmailTextBox.Text,
                    Phone = newPhoneTextBox.Text,
                    PESEL = newPeselTextBox.Text,
                    PIN = newPinTextBox.Text,
                    IDCardSeries = newIdCardSeriesTextBox.Text,
                    CarNumber = newCarNumberTextBox.Text,
                    Address = newAddressTextBox.Text,
                    City = newCityTextBox.Text,
                    PostalCode = newPostalCodeTextBox.Text,
                    County = newCountyTextBox.Text,
                    Country = newCountryTextBox.Text,
                    AdditionalInfo = newAdditionalInfoTextBox.Text
                };

                newCustomerGrid.Visibility = Visibility.Collapsed;
                customerGrid.Visibility = Visibility.Visible;
            }
            else if (customerGrid.IsVisible)
            {
                // Order ID is auto-generated so we don't set it here.  
                // For CustomerID, address, etc we use the values from current customer.  
                // User can modify these in the datagrid after the order is entered.  

                Customers currentCustomer = (Customers)custViewSource.View.CurrentItem;

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
            customerGrid.Visibility = Visibility.Collapsed;
            //newOrderGrid.Visibility = Visibility.Collapsed;
            newCustomerGrid.Visibility = Visibility.Visible;

            // Clear all the text boxes before adding a new customer. 
            customerIDLabel.Content = 0;
            foreach (var child in newCustomerGrid.Children)
            {
                var tb = child as TextBox;
                if (tb != null)
                {
                    tb.Text = "";
                }
            }
        }
    }
}
