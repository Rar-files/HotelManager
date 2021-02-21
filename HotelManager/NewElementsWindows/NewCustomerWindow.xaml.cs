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
using System.Windows.Shapes;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy NewCustomerWindow.xaml
    /// </summary>
    public partial class NewCustomerWindow : Window
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource custViewSource;

        public NewCustomerWindow()
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            DataContext = this;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Customers.Load();
            custViewSource.Source = context.Customers.Local;
        }

        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Customers newCustomer = new Customers
            {
                FirstName = firstNameTextBox.Text,
                LastName = lastNameTextBox.Text,
                Email = emailTextBox.Text,
                Phone = phoneTextBox.Text,
                PESEL = peselTextBox.Text,
                PIN = pinTextBox.Text,
                IDCardSeries = idCardSeriesTextBox.Text,
                CarNumber = carNumberTextBox.Text,
                Address = addressTextBox.Text,
                City = cityTextBox.Text,
                PostalCode = postalCodeTextBox.Text,
                County = countyTextBox.Text,
                Country = countryTextBox.Text,
                AdditionalInfo = additionalInfoTextBox.Text
            };

            context.Customers.Add(newCustomer);
            context.SaveChanges();
            custViewSource.View.Refresh();
            this.Close();
        }

        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
