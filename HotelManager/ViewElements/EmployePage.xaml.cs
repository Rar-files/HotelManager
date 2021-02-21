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
    /// Logika interakcji dla klasy EmployePage.xaml
    /// </summary>
    public partial class EmployePage : Page
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource empViewSource;
        CollectionViewSource reservViewSource;
        Employees initial = null;

        public EmployePage()
        {
            InitializeComponent();
            empViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            DataContext = this;
        }

        public EmployePage(int EmpID)
        {
            InitializeComponent();
            empViewSource = ((CollectionViewSource)(this.FindResource("customersViewSource")));
            DataContext = this;
            initial = context.Employees.Find(EmpID);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // customersViewSource.Źródło = [ogólne źródło danych]
            context.Employees.Load();
            empViewSource.Source = context.Customers.Local;
            if (initial != null)
                empViewSource.View.MoveCurrentTo(initial);
        }
    }
}
