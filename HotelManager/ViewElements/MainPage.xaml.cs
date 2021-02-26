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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // customersViewSource.Źródło = [ogólne źródło danych]
            context.Customers.Load();
            context.Reservations.Load();
            custViewSource.Source = context.Customers.Local;
            if (App.adminFlag)
            {
                adminStatus.Visibility = Visibility.Visible;
                adminTools.Visibility = Visibility.Visible;
                adminButton.Visibility = Visibility.Visible;
                AdminFactoryReset.Visibility = Visibility.Visible;
            }
        }

        private void AdminModeToggle(object sender, MouseButtonEventArgs e)
        {
            if(adminStatus.Visibility == Visibility.Visible)
            {
                App.adminFlag = false;
                adminStatus.Visibility = Visibility.Collapsed;
                adminTools.Visibility = Visibility.Collapsed;
                adminButton.Visibility = Visibility.Collapsed;
                AdminFactoryReset.Visibility = Visibility.Collapsed;
            }
            else
            {
                App.adminFlag = true;
                adminStatus.Visibility = Visibility.Visible;
                adminTools.Visibility = Visibility.Visible;
                adminButton.Visibility = Visibility.Visible;
                AdminFactoryReset.Visibility = Visibility.Visible;
            }
        }

        private void HardResetHandler(object sender, MouseButtonEventArgs e)
        {
            ResetPopUp.Visibility = Visibility.Visible;
        }

        private void HardResetHandler(object sender, RoutedEventArgs e)
        {
            context.Customers.RemoveRange(context.Set<Customers>());
            context.Employees.RemoveRange(context.Set<Employees>());
            context.Rooms.RemoveRange(context.Set<Rooms>());
            context.RoomsClass.RemoveRange(context.Set<RoomsClass>());
            context.Reservations.RemoveRange(context.Set<Reservations>());
            context.SaveChanges();
            ResetPopUp.Visibility = Visibility.Collapsed;
        }

        private void ResetHandler(object sender, RoutedEventArgs e)
        {
            context.Customers.RemoveRange(context.Set<Customers>());
            context.Reservations.RemoveRange(context.Set<Reservations>());
            context.SaveChanges();
            ResetPopUp.Visibility = Visibility.Collapsed;
        }
        private void CloseResetPopUpHandler(object sender, RoutedEventArgs e)
        {
            ResetPopUp.Visibility = Visibility.Collapsed;
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
            new NewRoomWindow().Show();
        }

        private void AddRoomClassHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewRoomClassWindow().Show();
        }

        private void AddEmpHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewEmpWindow().Show();
        }


        //SearchBar
        private void CustomerSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            var customers = (from c in context.Customers
                             orderby c.LastName
                             select new { Customer = (c.LastName + " " + c.FirstName), ID = c.CustomerID }); ;

            var customersList = customers.ToList();
            customersList.Clear();

            SearchCustomersList.Visibility = Visibility.Collapsed;
            SearchCustomersList.ItemsSource = null;

            string txt = (sender as TextBox).Text.ToLower();
            if (txt != "")
            {
                SearchCustomersList.Visibility = Visibility.Visible;

                foreach (var e in customers.ToList())
                {
                    var names = e.Customer.ToLower().Split();
                    try
                    {
                        if (int.TryParse(txt, out int ID))
                        {
                            if (DataGridTools.CheckString(e.ID.ToString(), ID.ToString()))
                            {
                                customersList.Add(e);
                            }
                        }

                        if (DataGridTools.CheckString(names[0], txt))
                        {
                            customersList.Add(e);
                        }
                        if (DataGridTools.CheckString(names[1], txt))
                        {
                            if (!customersList.Exists(elem => elem == e))
                                customersList.Add(e);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }

                SearchCustomersList.ItemsSource = customersList;
            }
        }

        private void CustomerDataGridSearchRowClick(object sender, MouseButtonEventArgs e)
        {
            var txt = (e.OriginalSource as TextBlock).Text.ToLower();
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
                    var reservations = (from r in context.Reservations
                                        orderby r.Customer
                                        select new { Customer = r.Customers.LastName + " " + r.Customers.FirstName, ID = r.ReservID }); ;
            
                var reservationsList = reservations.ToList();
                reservationsList.Clear();

                SearchReservList.Visibility = Visibility.Collapsed;
                SearchReservList.ItemsSource = null;

                string txt = (sender as TextBox).Text.ToLower();
                if (txt != "")
                {
                    SearchReservList.Visibility = Visibility.Visible;
                    foreach (var e in reservations.ToList())
                    {
                        var names = e.Customer.ToLower().Split();
                        try
                        {
                            if (int.TryParse(txt, out int ID))
                            {
                                if (DataGridTools.CheckString(e.ID.ToString(), ID.ToString()))
                                {
                                    reservationsList.Add(e);
                                }
                            }

                            if (DataGridTools.CheckString(names[0], txt))
                            {
                                reservationsList.Add(e);
                            }
                            if (DataGridTools.CheckString(names[1], txt))
                            {
                                if (!reservationsList.Exists(elem => elem == e))
                                    reservationsList.Add(e);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }
                    }

                    SearchReservList.ItemsSource = reservationsList;
                }
        }

        private void ReservDataGridSearchRowClick(object sender, MouseButtonEventArgs e)
        {
            var txt = (e.OriginalSource as TextBlock).Text.ToLower();
            if (txt != null)
            {
                int ID;
                if (!int.TryParse(txt, out ID))
                {
                    var row = sender as DataGridRow;

                    var cell = DataGridTools.GetCell(SearchReservList, row, 1);
                    cell.IsEnabled = false;

                    ID = int.Parse((cell.Content as TextBlock).Text);
                }

                SearchReservList.Visibility = Visibility.Collapsed;
                var resrvPage = new ReservationPage(ID);
                var window = (Window)this.Parent;
                window.Content = resrvPage;
            }
        }

        private void EmpSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            var emps = (from e in context.Employees
                        orderby e.LastName
                        select new { Employee = (e.LastName + " " + e.FirstName), ID = e.EmployeeID }); ;

            var empsList = emps.ToList();
            empsList.Clear();

            SearchEmpList.Visibility = Visibility.Collapsed;
            SearchEmpList.ItemsSource = null;

            string txt = (sender as TextBox).Text.ToLower();
            if (txt != "")
            {
                SearchEmpList.Visibility = Visibility.Visible;
                foreach (var e in emps.ToList())
                {
                    var names = e.Employee.ToLower().Split();
                    try
                    {
                        if (int.TryParse(txt, out int ID))
                        {
                            if (DataGridTools.CheckString(e.ID.ToString(), ID.ToString()))
                            {
                                empsList.Add(e);
                            }
                        }

                        if (DataGridTools.CheckString(names[0], txt))
                        {
                            empsList.Add(e);
                        }
                        if (DataGridTools.CheckString(names[1], txt))
                        {
                            if (!empsList.Exists(elem => elem == e))
                                empsList.Add(e);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }


                SearchEmpList.ItemsSource = empsList;
            }
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

                    var cell = DataGridTools.GetCell(SearchEmpList, row, 1);
                    cell.IsEnabled = false;

                    ID = int.Parse((cell.Content as TextBlock).Text);
                }

                SearchEmpList.Visibility = Visibility.Collapsed;
                var empPage = new EmployePage(ID);
                var window = (Window)this.Parent;
                window.Content = empPage;
            }
        }
    }
}
