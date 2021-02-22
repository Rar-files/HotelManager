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
    /// Logika interakcji dla klasy RoomWindow.xaml
    /// </summary>
    public partial class RoomWindow : Window
    {
        HotelDBEntities context = new HotelDBEntities();
        CollectionViewSource roomViewSource;
        CollectionViewSource roomClassViewSource;

        public Rooms GetCurrentRoom { get => roomViewSource.View.CurrentItem as Rooms; }

        public RoomWindow()
        {
            InitializeComponent();
            roomViewSource = ((CollectionViewSource)(this.FindResource("roomsViewSource")));
            roomClassViewSource = ((CollectionViewSource)(this.FindResource("roomsClassViewSource")));
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // customersViewSource.Źródło = [ogólne źródło danych]
            context.Rooms.Load();
            roomViewSource.Source = context.Rooms.Local;
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void PrevCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            roomViewSource.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            roomViewSource.View.MoveCurrentToNext();
        }
    }
}