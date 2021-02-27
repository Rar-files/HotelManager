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
        /// <summary>
        /// Data Entity context.
        /// </summary>
        HotelDBEntities context = new HotelDBEntities();

        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Event ładuje elementy strony.
        /// </summary>
        /// <remarks>
        /// <para>Wczytuje zestawy danych.</para>
        /// <para>Ustawia widoczność elementów Admin</para>
        /// </remarks>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.Customers.Load();
            context.Reservations.Load();
            context.Employees.Load();
            if (App.adminFlag)
            {
                adminStatus.Visibility = Visibility.Visible;
                adminTools.Visibility = Visibility.Visible;
                adminButton.Visibility = Visibility.Visible;
                AdminFactoryReset.Visibility = Visibility.Visible;
            }
        }


        /*Admin Tools*/

        /// <summary>
        /// Przełącza tryb aplikacji z normalnego na tryb admina.
        /// </summary>
        /// <remarks>
        /// <para>Zmienia wartość zmiennej adminFlag.</para>
        /// <para>Zmiania Visibility property dla admin obiektów.</para>
        /// </remarks>
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

        /// <summary>
        /// Uruchamia HardRreset bazy danych.
        /// </summary>
        /// <remarks>
        /// Wyświetla okno z wyborem elementów do resetu.
        /// </remarks>
        private void HardResetHandler(object sender, MouseButtonEventArgs e)
        {
            ResetPopUp.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Usuwa rekordy z wszystkich tabel.
        /// </summary>
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

        /// <summary>
        /// Usuwa rekordy z tabeli rezerwacji i klientów.
        /// </summary>
        private void ResetHandler(object sender, RoutedEventArgs e)
        {
            context.Customers.RemoveRange(context.Set<Customers>());
            context.Reservations.RemoveRange(context.Set<Reservations>());
            context.SaveChanges();
            ResetPopUp.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Zamyka okno resetu.
        /// </summary>
        private void CloseResetPopUpHandler(object sender, RoutedEventArgs e)
        {
            ResetPopUp.Visibility = Visibility.Collapsed;
        }


        /*New Window Buttons*/

        /// <summary>
        /// Otwiera okno NewReservationWindow.
        /// </summary>
        private void AddReservationHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewReservationWindow().Show();
        }

        /// <summary>
        /// Otwiera okno NewCustomerWindow
        /// </summary>
        private void AddCustomerHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewCustomerWindow().Show();
        }

        /// <summary>
        /// Otwiera okno NewRoomWindow.
        /// </summary>
        private void AddRoomHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewRoomWindow().Show();
        }

        /// <summary>
        /// Otwiera okno NewRoomClassWindow.
        /// </summary>
        private void AddRoomClassHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewRoomClassWindow().Show();
        }

        /// <summary>
        /// Otwiera okno NewEmpWindow.
        /// </summary>
        private void AddEmpHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewEmpWindow().Show();
        }


        /*SearchBar*/

        /// <summary>
        /// Dodaje do customersList DataGrid, rekordy spełniające wpisany warunek w TextBox customerSearch.
        /// </summary>
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

        /// <summary>
        /// Event intepretuje wybrany rekord z SearchCustomersList DataGrid i otwiera wybranego klienta w stronie CustomerPage.
        /// </summary>
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

        /// <summary>
        /// Dodaje do reservationsList DataGrid, rekordy spełniające wpisany warunek w TextBox reservationSearch.
        /// </summary>
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

        /// <summary>
        /// Event intepretuje wybrany rekord z SearchReservList DataGrid i otwiera wybraną rezerwacje w stronie ReservationPage.
        /// </summary>
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

        /// <summary>
        /// Dodaje do SearchEmpList DataGrid, rekordy spełniające wpisany warunek w TextBox employerSearch.
        /// </summary>
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

        /// <summary>
        /// Event intepretuje wybrany rekord z SearchEmpList DataGrid i otwiera wybranego pracownika w stronie EmployePage.
        /// </summary>
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
