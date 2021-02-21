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
    /// Logika interakcji dla klasy ReservationPage.xaml
    /// </summary>
    public partial class ReservationPage : Page
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource reservViewSource;
        Reservations initial = null;

        public ReservationPage()
        {
            InitializeComponent();
            reservViewSource = ((CollectionViewSource)(this.FindResource("reservationsViewSource")));
            DataContext = this;
        }
        public ReservationPage(int resrvID)
        {
            InitializeComponent();
            reservViewSource = ((CollectionViewSource)(this.FindResource("reservationsViewSource")));
            DataContext = this;
            initial = context.Reservations.Find(resrvID);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.Customers.Load();
            reservViewSource.Source = context.Customers.Local;
        }


        //Buttons
        private void DeleteCustomerCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var reservations = reservViewSource.View.CurrentItem as Reservations;

            var reserv = (from r in context.Reservations
                            where r.ReservID == reservations.ReservID
                            select r).FirstOrDefault();

            int currentID = reserv.ReservID;

            context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT('Reservations', RESEED, {currentID - 1})");
            context.SaveChanges();
            reservViewSource.View.Refresh();
        }


        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

            var currentReservation = reservViewSource.View.CurrentItem as Reservations;

            //if (currentReservation != null)
            //{
            //    currentReservation.Room = 
            //}


            context.SaveChanges();
            reservViewSource.View.Refresh();
        }

        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewReservationWindow().Show();
        }

        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var mainPage = new MainPage();
            var window = (Window)this.Parent;
            window.Content = mainPage;
        }


        //Search bar
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
                    if (!CheckID(e.ID, ID))
                    {
                        customersList.Remove(e);
                    }
                }

                SearchCustomersList.ItemsSource = customersList;
            }
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
                customerSearch.Text = "Customer";
                var custPage = new CustomerPage(ID);
                var window = (Window)this.Parent;
                window.Content = custPage;
            }
        }

        private void ReservSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            if (int.TryParse(customerSearch.Text, out int ID))
            {
                SearchReservList.Visibility = Visibility.Visible;
                var reservations = (from r in context.Reservations
                                 orderby r.Customer
                                 select new { Customer = r.Customer, ID = r.ReservID }); ;

                var reservationsList = reservations.ToList();
                foreach (var e in reservations.ToList())
                {
                    if (!CheckID(e.ID, ID))
                    {
                        reservationsList.Remove(e);
                    }
                }

                SearchReservList.ItemsSource = reservationsList;
            }
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

                    var cell = DataGridTools.GetCell(SearchReservList, row, 1);
                    cell.IsEnabled = false;

                    ID = int.Parse((cell.Content as TextBlock).Text);
                }

                SearchReservList.Visibility = Visibility.Collapsed;
                customerSearch.Text = "Reservation";
                var resrvPage = new ReservationPage(ID);
                var window = (Window)this.Parent;
                window.Content = resrvPage;
            }
        }

        private void EmpSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            if (int.TryParse(customerSearch.Text, out int ID))
            {
                SearchEmpList.Visibility = Visibility.Visible;
                var reservations = (from r in context.Reservations
                                    orderby r.Customer
                                    select new { Customer = r.Customer, ID = r.ReservID }); ;

                var reservationsList = reservations.ToList();
                foreach (var e in reservations.ToList())
                {
                    if (!CheckID(e.ID, ID))
                    {
                        reservationsList.Remove(e);
                    }
                }

                SearchEmpList.ItemsSource = reservationsList;
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
                customerSearch.Text = "Reservation";
                var empPage = new ReservationPage(ID);
                var window = (Window)this.Parent;
                window.Content = empPage;
            }
        }

        private bool CheckID(int test, int target)
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
