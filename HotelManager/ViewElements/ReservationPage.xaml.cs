﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.Entity;
using System.Collections.Generic;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy ReservationPage.xaml
    /// </summary>
    public partial class ReservationPage : Page
    {
        /// <summary>
        /// Data Entity context.
        /// </summary>
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource reservViewSource;
        Reservations initial = null;

        public ReservationPage()
        {
            InitializeComponent();
            reservViewSource = ((CollectionViewSource)(this.FindResource("reservationsViewSource")));
            DataContext = this;
        }

        /// <summary>
        /// Konstruktor, który również ustawia widok na podaną rezerwacje.
        /// </summary>
        /// <remarks>
        /// Ustawia zmienną initial jako rezerwacje o podanym id jako parametr.
        /// </remarks>
        /// <param name="resrvID">Konstruktor przyjmuje ID rezerwacji i ustawia aktualny widok jako podaną rezerwacje</param>
        public ReservationPage(int resrvID)
        {
            InitializeComponent();
            reservViewSource = ((CollectionViewSource)(this.FindResource("reservationsViewSource")));
            DataContext = this;
            initial = context.Reservations.Find(resrvID);
        }

        /// <summary>
        /// Event ładuje elementy strony.
        /// </summary>
        /// <remarks>
        /// <para>Wczytuje zestawy danych.</para>
        /// <para>Ustawia widok na element initial</para>
        /// <para> Wpisuje do customersDataGrid powiązanego klienta</para>
        /// <para> Wpisuje do roomsDataGrid powiązany pokój</para>
        /// <para> Wpisuje do employeesDataGrid powiązanego pracownika</para>
        /// </remarks>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.Reservations.Load();
            context.Customers.Load();
            context.Rooms.Load();
            context.Employees.Load();
            reservViewSource.Source = context.Reservations.Local;

            if (initial != null)
                reservViewSource.View.MoveCurrentTo(initial);
            else
                initial = reservViewSource.View.CurrentItem as Reservations;

            customersDataGrid.ItemsSource = new List<Customers> { initial.Customers };
            roomsDataGrid.ItemsSource = new List<Rooms> { initial.Rooms };
            employeesDataGrid.ItemsSource = new List<Employees> { initial.Employees };
        }


        /*Buttons*/

        /// <summary>
        /// Usuwa rezerwacje.
        /// </summary>
        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var reservations = reservViewSource.View.CurrentItem as Reservations;

            context.Reservations.Remove(reservations);

            context.SaveChanges();
            var mainPage = new MainPage();
            var window = (Window)this.Parent;
            window.Content = mainPage;
        }

        /// <summary>
        /// Zapisuje zmiany w aktualnie otwartej rezerwacji.
        /// </summary>
        /// <remarks>
        /// <para>Pobiera aktualneą rezerwacje.</para>
        /// <para>Zmienia wartości pól aktualnej rezerwacji, na wpisane w textbox.</para>
        /// <para>Nadpisuje rezerwacje w bazie danych</para>
        /// </remarks>
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var currentReservation = reservViewSource.View.CurrentItem as Reservations;

            if (currentReservation != null)
            {
                currentReservation.AdditionalInfo = additionalInfoTextBox.Text;
                currentReservation.BookingFrom = bookingFromDatePicker.DisplayDate;
                currentReservation.BookingTo = bookingToDatePicker.DisplayDate;
            }


            context.SaveChanges();
            reservViewSource.View.Refresh();
        }

        /// <summary>
        /// Otwiera okno NewReservationWindow.
        /// </summary>
        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewReservationWindow().Show();
        }

        /// <summary>
        /// Zamyka stonę rezeracji.
        /// </summary>
        /// <remarks>
        /// Ładuje stronę główną do okna aplikacji.
        /// </remarks>
        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var mainPage = new MainPage();
            var window = (Window)this.Parent;
            window.Content = mainPage;
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
