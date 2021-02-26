﻿using System;
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
    /// Logika interakcji dla klasy CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource custViewSource;
        CollectionViewSource reservViewSource;
        Customers initial = null;

        public CustomerPage()
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            reservViewSource = ((CollectionViewSource)(this.FindResource("customersReservationsViewSource")));
            DataContext = this;
        }

        public CustomerPage(int customerID)
        {
            InitializeComponent();
            custViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            reservViewSource = ((CollectionViewSource)(this.FindResource("customersReservationsViewSource")));
            DataContext = this;
            initial = context.Customers.Find(customerID);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // customersViewSource.Źródło = [ogólne źródło danych]
            context.Customers.Load();
            custViewSource.Source = context.Customers.Local;
            if(initial != null)
                custViewSource.View.MoveCurrentTo(initial);
        }

        private void DeleteCustomerCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // If existing window is visible, delete the customer and all their orders.  
            // In a real application, you should add warnings and allow the user to cancel the operation.  
            var customers = custViewSource.View.CurrentItem as Customers;

            var customer = (from c in context.Customers
                            where c.CustomerID == customers.CustomerID
                            select c).FirstOrDefault();

            int currentID = customer.CustomerID;

            if (customer != null)
            {
                foreach (var reserv in customer.Reservations.ToList())
                {
                    customer.Reservations.Remove(reserv);
                }
                context.Customers.Remove(customer);
            }

            context.SaveChanges();
            custViewSource.View.Refresh();
        }

        // Commit changes from the new customer form, the new order form,  
        // or edits made to the existing customer form.  
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

            // Order ID is auto-generated so we don't set it here.  
            // For CustomerID, address, etc we use the values from current customer.  
            // User can modify these in the datagrid after the order is entered.  
            var currentCustomer = custViewSource.View.CurrentItem as Customers;

            if (currentCustomer != null)
            {
                currentCustomer.FirstName = firstNameTextBox.Text;
                currentCustomer.LastName = lastNameTextBox.Text;
                currentCustomer.Email = emailTextBox.Text;
                currentCustomer.Phone = phoneTextBox.Text;
                currentCustomer.PESEL = peselTextBox.Text;
                currentCustomer.PIN = pinTextBox.Text;
                currentCustomer.IDCardSeries = idCardSeriesTextBox.Text;
                currentCustomer.CarNumber = carNumberTextBox.Text;
                currentCustomer.Address = addressTextBox.Text;
                currentCustomer.City = cityTextBox.Text;
                currentCustomer.PostalCode = postalCodeTextBox.Text;
                currentCustomer.County = countyTextBox.Text;
                currentCustomer.Country = countryTextBox.Text;
                currentCustomer.AdditionalInfo = additionalInfoTextBox.Text;
            }


            // Save the changes, either for a new customer, a new order  
            // or an edit to an existing customer or order.
            context.SaveChanges();
            custViewSource.View.Refresh();
        }

        // Sets up the form so that user can enter data. Data is later  
        // saved when user clicks Commit.  
        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            new NewCustomerWindow().Show();
        }

        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var mainPage = new MainPage();
            var window = (Window)this.Parent;
            window.Content = mainPage;
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
                                select new { Customer = context.Customers.Find(r.Customer).LastName + " " + context.Customers.Find(r.Customer).FirstName, ID = r.ReservID }); ;

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
