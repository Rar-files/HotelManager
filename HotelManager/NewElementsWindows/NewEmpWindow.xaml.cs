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
    /// Logika interakcji dla klasy NewEmpWindow.xaml
    /// </summary>
    public partial class NewEmpWindow : Window
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource empViewSource;

        public NewEmpWindow()
        {
            InitializeComponent();
            empViewSource = ((CollectionViewSource)(this.FindResource("employeesViewSource")));
            DataContext = this;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Employees.Load();
            empViewSource.Source = context.Employees.Local;
        }

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

        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

    }
}
