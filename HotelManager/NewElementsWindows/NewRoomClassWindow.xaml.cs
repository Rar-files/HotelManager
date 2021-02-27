using System.Windows;
using System.Windows.Input;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy NewRoomClassWindow.xaml
    /// </summary>
    public partial class NewRoomClassWindow : Window
    {
        /// <summary>
        /// Data Entity context.
        /// </summary>
        HotelDBEntities context = new HotelDBEntities();

        public NewRoomClassWindow()
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
            context.RoomsClass.Load();
        }


        /*Buttons*/

        /// <summary>
        /// Tworzy nową klasę pokoju.
        /// </summary>
        /// <remarks>
        /// <para>Tworzy nową klasę pokoju.</para>
        /// <para>Ustawia wartości pól nowej klasy, na wpisane w textbox oraz zapisane do zmiennych</para>
        /// <para>Zapisuje klasę w bazie danych</para>
        /// </remarks>
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var roomClass = new RoomsClass
            {
                ClassName = classNameTextBox.Text,
                StarsStandard = int.Parse(starsStandardTextBox.Text),
                Price = int.Parse(priceTextBox.Text),
                FlatArea = int.Parse(flatAreaTextBox.Text),
                BedCount = int.Parse(bedCountTextBox.Text),
                AdditionalInfo = additionalInfoTextBox.Text
            };

            context.RoomsClass.Add(roomClass);
            context.SaveChanges();
            this.Close();
        }

        /// <summary>
        /// Zamyka okno.
        /// </summary>
        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
