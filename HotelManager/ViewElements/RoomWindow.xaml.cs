using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy RoomWindow.xaml
    /// </summary>
    public partial class RoomWindow : Window
    {
        /// <summary>
        /// Logika interakcji dla klasy EmployePage.xaml
        /// </summary>
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource roomViewSource;
        CollectionViewSource roomClassViewSource;

        public int getCurrentRoom;

        public RoomWindow()
        {
            InitializeComponent();
            roomViewSource = ((CollectionViewSource)(this.FindResource("roomsViewSource")));
            roomClassViewSource = ((CollectionViewSource)(this.FindResource("roomsClassViewSource")));
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
            context.Rooms.Load();
            context.RoomsClass.Load();
            roomViewSource.Source = context.Rooms.Local;
            roomClassViewSource.Source = context.RoomsClass.Local;
            roomClassViewSource.View.MoveCurrentTo(context.RoomsClass.Find((roomViewSource.View.CurrentItem as Rooms).Class));
        }


        /*Buttons*/

        /// <summary>
        /// Zamyka okno.
        /// </summary>
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            getCurrentRoom = (roomViewSource.View.CurrentItem as Rooms).RoomID;
            this.Close();
        }

        /// <summary>
        /// Zmiana aktualnego pokoju, na poprzedni.
        /// </summary>
        private void PrevCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (roomViewSource.View.MoveCurrentToPrevious())
                roomClassViewSource.View.MoveCurrentTo(context.RoomsClass.Find((roomViewSource.View.CurrentItem as Rooms).Class));
        }

        /// <summary>
        /// Zmiana aktualnego pokoju, na następny.
        /// </summary>
        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if(roomViewSource.View.MoveCurrentToNext())
                roomClassViewSource.View.MoveCurrentTo(context.RoomsClass.Find((roomViewSource.View.CurrentItem as Rooms).Class));
        }
    }
}
