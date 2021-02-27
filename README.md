# HotelManager

HotelManager jest prostą aplikacją WPF typu CRUD do obsługi hotelu. Do swojej pracy wykorzystuje bazę danych MS SQL Server jako LocalDB w VisualStudio (SQL Server Express) oraz ORM Entity Framework.

## Pasek wyszukiwania
Każda strona oraz ekran główny posiadają pasek wyszukiwania. Pasek Pozwala wyszukać Klienta, Rezerwacje oraz Pracownika.
Pasek wyszukuje elementów na podstawie ID, Imienia i Nazwiska.

## Ekran główny

Ekran w domyślnym trybie oferuje możliwość dodania nowego klienta oraz rezerwacji.![enter image description here](https://imagizer.imageshack.com/img923/7016/CWgko5.png)

### Ekran główny - tryb administratora

W trybie administratora, dostępna jest opcja Hard resetu. Dostępna również jest opcja dodania nowej klasy pokoju, nowego pokoju oraz nowego pracownika.
Aby przełączyć między trybami, należy wcisnąć ikonę użytkownika z kluczem, znajdującą się w prawym dolnym rogu.
![enter image description here](https://imagizer.imageshack.com/img924/3325/NGyCZq.png)

### Hard reset
Hard reset pozwala wyczyścić dane z tabeli rezerwacji i klientów, bądź z wszystkich tabel. Polecenie hard reset resetuje również licznik ID i pozwala przydzielić id od nowa.
Hard reset wywołuje się z trybu administracyjnego, wciskając ikonę trybki z fabryką. (Ikonka znajduje się w lewym dolnym rogu)![enter image description here](https://imagizer.imageshack.com/img923/8683/xyXwB7.png)

## Strony danych

Aplikacja posiada 3 strony, przedstawiające klientów, pracowników oraz rezerwacje.
Na danej stronie, korzystając z odpowiednich przycisków - można dokonać zmian w danym elemencie.

 - Delete - usuwa element,
 - Commit - zapisuje wprowadzone zmiany,
 - New - uruchamia okno tworzenia danego elementu,
 - Cancel - Wraca do Menu.
 
 ### Customer Page![enter image description here](https://imagizer.imageshack.com/img924/3797/AlTIZr.png)
 ### Reservation Page![enter image description here](https://imagizer.imageshack.com/img922/2167/8aKtV9.png)
 ### EmployerPage![enter image description here](https://imagizer.imageshack.com/img923/2610/1AfKZO.png)

## Okna nowych elementów
Aplikacja posiada 5 okien, służących tworzeniu elementów.

### New Customer
Tworzy nowego klienta z podanych danych.
![enter image description here](https://imagizer.imageshack.com/img922/7963/LPr5Of.png)
### New Employer
Tworzy nowego pracownika z podanych danych.![enter image description here](https://imagizer.imageshack.com/img924/6286/jXCAMp.png)
### New Room Class
Tworzy nową klasę pokoju z podanych danych.
![enter image description here](https://imagizer.imageshack.com/img922/8862/iEHUvY.png)
### New Room
Tworzy nowy pokój z wybranej klasy i podanych danych.
Aby wybrać klasę, wystarczy w polu "Class" wpisać id bądź nazwę danej klasy.
![enter image description here](https://imagizer.imageshack.com/img923/3539/YNLCWH.png)
### New Reservation
Tworzy nową rezerwacje zna dany pokój, przypisywując ją do klienta i przypisywując jej pracownika który ją dodał oraz czas i inne dane.
Aby wybrać Klienta i Pracownika, wystarczy w polu "Customers" oraz "AddBy" wpisać id bądź nazwę danej klasy.
![enter image description here](https://imagizer.imageshack.com/img923/3716/3zfvP0.png)
Wybór pokoju dokonuje się poprzez przycisk "choose". Otwiera się wtedy okno które pozwala przeglądać dostępne pokoje.
![enter image description here](https://imagizer.imageshack.com/img923/9074/my7F3M.png)
