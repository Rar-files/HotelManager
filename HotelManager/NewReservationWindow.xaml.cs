using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy NewReservationWindow.xaml
    /// </summary>
    public partial class NewReservationWindow : Window
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource custViewSource;
        Customers currentCustomer;
        Employees currentEmployees;
        Rooms currentRoom;

        public NewReservationWindow()
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.Customers.Load();
            custViewSource.Source = context.Customers.Local;
        }

        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Reservations newReservation = new Reservations
            {
                Customer = currentCustomer.CustomerID,
                AddBy = currentEmployees.EmployeeID,
                Room = currentRoom.RoomID,
                BookingFrom = bookingFromDatePicker.DisplayDate,
                BookingTo = bookingToDatePicker.DisplayDate,
                AdditionalInfo = additionalInfoTextBox.Text
            };

            context.Reservations.Add(newReservation);
            context.SaveChanges();
            custViewSource.View.Refresh();
            this.Close();
        }

        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void CustomerSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            if (int.TryParse(customerSearch.Text, out int ID))
            {
                SearchCustomersList.Visibility = Visibility.Visible;
                var customers = (from c in context.Customers
                                 orderby c.LastName
                                 select new { Customer = (c.LastName + " " + c.FirstName), ID = c.CustomerID }); ;

                var customersList = customers.ToList();
                foreach (var e in customers.ToList())
                {
                    if (!CheckCustomerID(e.ID, ID))
                    {
                        customersList.Remove(e);
                    }
                }

                SearchCustomersList.ItemsSource = customersList;
            }
        }

        private void DataGridSearchRowClick(object sender, MouseButtonEventArgs e)
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

                currentCustomer = context.Customers.Find(ID);
                SearchCustomersList.Visibility = Visibility.Collapsed;
                customerSearch.Text = "FindCustomer";
                customerLabelName.Content = currentCustomer.LastName + " " + currentCustomer.FirstName;
            }
        }

        private bool CheckCustomerID(int test, int target)
        {
            char[] tests = test.ToString().ToCharArray();
            char[] targets = target.ToString().ToCharArray();
            bool checkFlag = true;
            for (int i = 0; i < targets.Length & checkFlag; i++)
            {
                if (tests[i] != targets[i]) checkFlag = false;
            }
            return checkFlag;
        }
    }
}
