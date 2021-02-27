using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy NewEmpWindow.xaml
    /// </summary>
    public partial class NewEmpWindow : Window
    {
        /// <summary>
        /// Data Entity context.
        /// </summary>
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource empViewSource;

        public NewEmpWindow()
        {
            InitializeComponent();
            empViewSource = ((CollectionViewSource)(this.FindResource("employeesViewSource")));
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
            context.Employees.Load();
            empViewSource.Source = context.Employees.Local;
        }


        /*Buttons*/

        /// <summary>
        /// Tworzy nowego pracownika.
        /// </summary>
        /// <remarks>
        /// <para>Tworzy nowego pracownika.</para>
        /// <para>Ustawia wartości pól nowego pracownika, na wpisane w textbox oraz zapisane do zmiennych</para>
        /// <para>Zapisuje pracownika w bazie danych</para>
        /// </remarks>
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Employees newEmp = new Employees
            {
                FirstName = firstNameTextBox.Text,
                LastName = lastNameTextBox.Text,
                Address = addressTextBox.Text,
                City = cityTextBox.Text,
                PostalCode = postalCodeTextBox.Text,
                County = countyTextBox.Text,
                Country = countryTextBox.Text,
                BirthDate = birthDateDatePicker.DisplayDate,
                Email = emailTextBox.Text,
                Phone = phoneTextBox.Text
            };

            context.Employees.Add(newEmp);
            context.SaveChanges();
            empViewSource.View.Refresh();
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
