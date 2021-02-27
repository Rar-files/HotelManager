using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy NewCustomerWindow.xaml
    /// </summary>
    public partial class NewCustomerWindow : Window
    {
        /// <summary>
        /// Data Entity context.
        /// </summary>
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource custViewSource;

        public NewCustomerWindow()
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            DataContext = this;
        }

        /// <summary>
        /// Event ładuje elementy strony.
        /// </summary>
        /// <remarks>
        /// Wczytuje zestawy danych.
        /// </remarks>
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Customers.Load();
            custViewSource.Source = context.Customers.Local;
        }


        /*Buttons*/

        /// <summary>
        /// Tworzy nowego klienta.
        /// </summary>
        /// <remarks>
        /// <para>Tworzy nowego klienta.</para>
        /// <para>Ustawia wartości pól nowego klienta, na wpisane w textbox.</para>
        /// <para>Zapisuje klienta w bazie danych</para>
        /// </remarks>
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
            this.Close();
        }

        /// <summary>
        /// Zamyka okno.
        /// </summary>
        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
