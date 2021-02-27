using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;

namespace HotelManager
{
    /// <summary>
    /// Logika interakcji dla klasy NewRoomWindow.xaml
    /// </summary>
    public partial class NewRoomWindow : Window
    {
        /// <summary>
        /// Data Entity context.
        /// </summary>
        HotelDBEntities context = new HotelDBEntities();
        RoomsClass roomClass;
        bool classChecked = false;

        public NewRoomWindow()
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
            context.Rooms.Load();
        }


        /*Buttons*/

        /// <summary>
        /// Tworzy nowy pokój.
        /// </summary>
        /// <remarks>
        /// <para>Tworzy nowy pokój.</para>
        /// <para>Ustawia wartości pól nowego pokój, na wpisane w textbox oraz zapisane do zmiennych</para>
        /// <para>Zapisuje pokój w bazie danych</para>
        /// </remarks>
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (roomClass != null & numberTextBox.Text != null)
            {
                var room = new Rooms
                {
                    Number = int.Parse(numberTextBox.Text),
                    Class = roomClass.ClassID,
                    forTheDisabled = (bool)forTheDisabledCheckBox.IsChecked,
                    AdditionalInfo = additionalInfoTextBox.Text
                };

                context.Rooms.Add(room);
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


        /*SearchBox*/

        /// <summary>
        /// Dodaje do SearchClassList DataGrid, rekordy spełniające wpisany warunek w TextBox classTextBox.
        /// </summary>
        private void ClassSearchTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!classChecked)
            {
                var classes = (from c in context.RoomsClass
                               orderby c.ClassName
                               select new { ClassName = c.ClassName, ID = c.ClassID }); ;

                var classesList = classes.ToList();
                classesList.Clear();

                SearchClassList.Visibility = Visibility.Collapsed;
                SearchClassList.ItemsSource = null;

                string txt = (sender as TextBox).Text.ToLower();
                if (txt != "")
                {
                    SearchClassList.Visibility = Visibility.Visible;

                    foreach (var e in classes.ToList())
                    {
                        var name = e.ClassName.ToLower();
                        try
                        {
                            if (int.TryParse(txt, out int ID))
                            {
                                if (DataGridTools.CheckString(e.ID.ToString(), ID.ToString()))
                                {
                                    classesList.Add(e);
                                }
                            }

                            if (DataGridTools.CheckString(name, txt))
                            {
                                classesList.Add(e);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }
                    }

                    SearchClassList.ItemsSource = classesList;
                }
            }
            else
            {
                classChecked = false;
            }
        }

        /// <summary>
        /// Event intepretuje wybrany rekord z SearchClassList DataGrid i zapisuje wybraną klase do zmiennej.
        /// </summary>
        private void ClassDataGridSearchRowClick(object sender, MouseButtonEventArgs e)
        {
            var txt = (e.OriginalSource as TextBlock).Text.ToLower();
            if (txt != null)
            {
                int ID;
                if (!int.TryParse(txt, out ID))
                {
                    var row = sender as DataGridRow;

                    var cell = DataGridTools.GetCell(SearchClassList, row, 1);
                    cell.IsEnabled = false;

                    ID = int.Parse((cell.Content as TextBlock).Text);
                }

                SearchClassList.Visibility = Visibility.Collapsed;
                roomClass = context.RoomsClass.Find(ID);
                classChecked = true;
                classTextBox.Text = roomClass.ClassName;
            }
        }
    }
}
