using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy NewReservationWindow.xaml
    /// </summary>
    public partial class NewReservationWindow : Window
    {
        /// <summary>
        /// Data Entity context.
        /// </summary>
        HotelDBEntities context = new HotelDBEntities();
        Customers currentCustomer;
        Employees currentEmployer;
        Rooms currentRoom;
        bool customerChecked = false;
        bool employerChecked = false;

        public NewReservationWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Event ładuje elementy strony.
        /// </summary>
        /// <remarks>
        /// Wczytuje zestawy danych.
        /// </remarks>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Reservations.Load();
            context.Rooms.Load();
        }


        /*Buttons*/

        /// <summary>
        /// Tworzy nową rezerwacje.
        /// </summary>
        /// <remarks>
        /// <para>Tworzy nową rezerwacje.</para>
        /// <para>Ustawia wartości pól nowej rezerwacji, na wpisane w textbox oraz zapisane do zmiennych</para>
        /// <para>Zapisuje klasę w bazie danych</para>
        /// </remarks>
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (currentCustomer != null & currentRoom != null & currentEmployer != null & bookingFromDatePicker.DisplayDate != null & bookingToDatePicker.DisplayDate != null)
            {
                Reservations newReservation = new Reservations
                {
                    Customer = currentCustomer.CustomerID,
                    AddBy = currentEmployer.EmployeeID,
                    Room = currentRoom.RoomID,
                    BookingFrom = bookingFromDatePicker.DisplayDate,
                    BookingTo = bookingToDatePicker.DisplayDate,
                    AdditionalInfo = additionalInfoTextBox.Text
                };

                context.Reservations.Add(newReservation);
                context.SaveChanges();
                this.Close();
            }
        }

        /// <summary>
        /// Zamyka okno.
        /// </summary>
        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Wyświetla okno pokoju, oraz wczytuje wybraną wartość w otwartym oknie.
        /// </summary>
        /// <remarks>
        /// <para>Wyświetla okno pooju.</para>
        /// <para>Czeka na wybranie i zamknięcie okna</para>
        /// <para>Wpisuje wybrany pokój do zmiennej</para>
        /// </remarks>
        private void ChooseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var roomWin = new RoomWindow();
            if(roomWin.ShowDialog() == false)
            {
                currentRoom = context.Rooms.Find(roomWin.getCurrentRoom);
                roomLabel.Content = currentRoom.Number;
            }
        }


        /*SearchBoxs*/

        /// <summary>
        /// Dodaje do SearchCustomersList DataGrid, rekordy spełniające wpisany warunek w customerSearch TextBox.
        /// </summary>
        private void CustomerSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!customerChecked)
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
            else
            {
                customerChecked = false;
            }
        }

        /// <summary>
        /// Event intepretuje wybrany rekord z SearchCustomersList DataGrid i zapisuje wybraną klase do zmiennej.
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
                currentCustomer = context.Customers.Find(ID);
                customerChecked = true;
                customerSearch.Text = currentCustomer.FirstName + " " + currentCustomer.LastName;
            }
        }

        /// <summary>
        /// Dodaje do SearchEmpList DataGrid, rekordy spełniające wpisany warunek w empTextBlock TextBox.
        /// </summary>
        private void EmpSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!employerChecked)
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
            else
            {
                employerChecked = false;
            }
        }

        /// <summary>
        /// Event intepretuje wybrany rekord z SearchEmpList DataGrid i zapisuje wybraną klase do zmiennej.
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
                currentEmployer = context.Employees.Find(ID);
                employerChecked = true;
                empTextBlock.Text = currentEmployer.FirstName + " " + currentEmployer.LastName;
            }
        }
    }
}
