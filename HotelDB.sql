create database HotelDB;
GO

use HotelDB;
GO

CREATE TABLE "RoomsClass"(
	"ClassID" "int" IDENTITY Primary Key NOT NULL ,
	"ClassName" nvarchar (10),
	"StarsStandard" "int" NOT NULL,
	"Price" decimal NOT NULL,	
	"FlatArea" decimal NOT NULL,	
	"BedCount" "int" NOT NULL,
	"AdditionalInfo" nvarchar (80) NULL
)

CREATE TABLE "Rooms" (
	"RoomID" "int" IDENTITY  Primary Key NOT NULL,
	"Number" "int" NOT NULL,
	"Class" "int" Foreign Key REFERENCES RoomsClass (ClassID) NOT NULL,
	"forTheDisabled" bit DEFAULT 0 NOT NULL,
	"AdditionalInfo" nvarchar (80) NULL
)

CREATE TABLE "Employees" (
	"EmployeeID" "int" IDENTITY Primary Key NOT NULL ,
	"FirstName" nvarchar (14) NOT NULL ,
	"LastName" nvarchar (28) NOT NULL ,
	"BirthDate" "datetime" NULL ,
	"Address" nvarchar (80) NOT NULL ,
	"City" nvarchar (32) NOT NULL ,
	"PostalCode" nvarchar (6) NOT NULL ,
	"County" nvarchar (30) NOT NULL ,
	"Country" nvarchar (15) NOT NULL ,
	"Phone" nvarchar (9) NOT NULL,
	"Email" nvarchar (30) NOT NULL
)

CREATE TABLE "Customers" (
	"CustomerID" "int" IDENTITY Primary Key NOT NULL ,
	"FirstName" nvarchar (14) NOT NULL ,
	"LastName" nvarchar (28) NOT NULL,
	"PIN" nvarchar (4) NOT NULL,
	"IDCardSeries" char (9) NOT NULL ,
	"PESEL" char (11) NOT NULL ,
	"CarNumber" nvarchar (10) NULL ,
	"Address" nvarchar (80) NOT NULL ,
	"City" nvarchar (32) NOT NULL ,
	"PostalCode" nvarchar (6) NOT NULL ,
	"County" nvarchar (30) NOT NULL ,
	"Country" nvarchar (15) NOT NULL ,
	"Phone" char (9) NOT NULL,
	"Email" nvarchar (50) NULL,
	"AdditionalInfo" nvarchar (80) NULL
)

CREATE TABLE "Reservations" (
	"ReservID" "int" IDENTITY Primary Key NOT NULL ,
	"Room" "int" Foreign Key REFERENCES Rooms (RoomID) NOT NULL,
	"Customer" "int" Foreign Key REFERENCES Customers (CustomerID) NOT NULL,
	"BookingFrom" "datetime" NOT NULL ,
	"BookingTo" "datetime" NOT NULL ,
	"AddBy" "int" Foreign Key REFERENCES Employees (EmployeeID) NOT NULL,	
	"AdditionalInfo" nvarchar (80) NULL
)