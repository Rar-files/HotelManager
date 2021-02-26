using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.Entity;
using System.Windows.Shapes;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy NewRoomClassWindow.xaml
    /// </summary>
    public partial class NewRoomClassWindow : Window
    {
        HotelDBEntities context = new HotelDBEntities();

        public NewRoomClassWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.RoomsClass.Load();
        }

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

        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
