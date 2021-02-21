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

        private void AdminModeToggle(object sender, MouseButtonEventArgs e)
        {
            if(adminStatus.Visibility == Visibility.Visible)
            {
                adminStatus.Visibility = Visibility.Collapsed;
                adminTools.Visibility = Visibility.Collapsed;
            }
            else
            {
                adminStatus.Visibility = Visibility.Visible;
                adminTools.Visibility = Visibility.Visible;
            }
        }

        private void AddReservationHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewReservationWindow().Show();
        }

        private void AddCustomerHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewCustomerWindow().Show();
        }

        private void AddRoomHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewReservationWindow().Show();
        }

        private void AddEmpHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewCustomerWindow().Show();
        }


        /// <summary>
        /// SearchBar
        /// </summary>
        private void CustomerSearchTextChanged(object sender, TextChangedEventArgs args)
        {

            SearchCustomersList.Visibility = Visibility.Visible;
            var customers = (from c in context.Customers
                                orderby c.LastName
                                select new { Customer = (c.LastName + " " + c.FirstName), ID = c.CustomerID }); ;

            var customersList = customers.ToList();
            customersList.Clear();


            foreach (var e in customers.ToList())
            {
                var names = e.Customer.Split();
                try
                {
                    if (int.TryParse((sender as TextBox).Text, out int ID))
                    {
                        if (DataGridTools.CheckString(e.ID.ToString(), ID.ToString()))
                        {
                            customersList.Add(e);
                        }
                    }

                    if (DataGridTools.CheckString(names[0], customerSearch.Text))
                    {
                        customersList.Add(e);
                    }
                    if (DataGridTools.CheckString(names[1], customerSearch.Text))
                    {
                        customersList.Add(e);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                }
            }

            SearchCustomersList.ItemsSource = customersList;
        }

        private void CustomerDataGridSearchRowClick(object sender, MouseButtonEventArgs e)
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
                var custPage = new CustomerPage(ID);
                var window = (Window)this.Parent;
                window.Content = custPage;
            }
        }

        private void ReservSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            SearchReservList.Visibility = Visibility.Visible;
            var reservations = (from r in context.Reservations
                                orderby r.Customer
                                select new { Customer = context.Customers.Find(r.Customer).LastName + " " + context.Customers.Find(r.Customer).FirstName, ID = r.ReservID }); ;

            var reservationsList = reservations.ToList();
            reservationsList.Clear();


            foreach (var e in reservations.ToList())
            {
                var names = e.Customer.Split();
                try
                {
                    if (int.TryParse((sender as TextBox).Text, out int ID))
                    {
                        if (DataGridTools.CheckString(e.ID.ToString(), ID.ToString()))
                        {
                            reservationsList.Add(e);
                        }
                    }

                    if (DataGridTools.CheckString(names[0], customerSearch.Text))
                    {
                        reservationsList.Add(e);
                    }
                    if (DataGridTools.CheckString(names[1], customerSearch.Text))
                    {
                        reservationsList.Add(e);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                }
            }

            SearchCustomersList.ItemsSource = reservationsList;
        }

        private void ReservDataGridSearchRowClick(object sender, MouseButtonEventArgs e)
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
                var resrvPage = new ReservationPage(ID);
                var window = (Window)this.Parent;
                window.Content = resrvPage;
            }
        }

        private void EmpSearchTextChanged(object sender, TextChangedEventArgs args)
        {

            SearchCustomersList.Visibility = Visibility.Visible;
            var emps = (from e in context.Employees
                                orderby e.LastName
                                select new { Employee = (e.LastName + " " + e.FirstName), ID = e.EmployeeID }); ;

            var empsList = emps.ToList();
            empsList.Clear();


            foreach (var e in emps.ToList())
            {
                var names = e.Employee.Split();
                try
                {
                    if (int.TryParse((sender as TextBox).Text, out int ID))
                    {
                        if (DataGridTools.CheckString(e.ID.ToString(), ID.ToString()))
                        {
                            empsList.Add(e);
                        }
                    }

                    if (DataGridTools.CheckString(names[0], customerSearch.Text))
                    {
                        empsList.Add(e);
                    }
                    if (DataGridTools.CheckString(names[1], customerSearch.Text))
                    {
                        empsList.Add(e);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                }
            }

            SearchCustomersList.ItemsSource = empsList;
        }

        private void EmpDataGridSearchRowClick(object sender, MouseButtonEventArgs e)
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
                var empPage = new EmployePage(ID);
                var window = (Window)this.Parent;
                window.Content = empPage;
            }
        }

    }
}
