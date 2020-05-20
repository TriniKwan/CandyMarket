create table [User] (
	UserId int not null identity(1,1) primary key,
	FirstName varchar(250) not null,
	LastName varchar(250) not null
)

create table Category (
	CategoryId int not null identity(1,1) primary key,
	[Type] varchar(250) not null
)

create table Candy (
	CandyId int not null identity(1,1) primary key,
	[Name] varchar(250) not null,
	Manufacturer varchar(250) not null,
	CategoryId int foreign key references Category(CategoryId)
)

create table UserCandy (
	UserCandyId int not null identity(1,1) primary key,
	UserId int foreign key references [User](UserId),
	CandyId int foreign key references Candy(CandyId),
	DateAdded datetime
)

insert into [User] (FirstName, LastName)
values ('Laura', 'Collins'),
		('Randy', 'Tate'),
		('John', 'Thielman'),
		('Monica', 'Djunaidi')

insert into Category ([Type])
values ('Chocolate'),
		('Fruity'),
		('Gummy'),
		('Taffy'),
		('Sour')

insert into Candy ([Name], Manufacturer, CategoryId)
values ('Pez', 'Pez', 2),
		('Peeps', 'Just Born', 6),
		('Starburst', 'Mars', 2),
		('Laffy Taffy', 'Ferrero', 4),
		('Salt Water Taffy', 'A&A', 4),
		('Ferrero Rocher', 'Ferrero', 1),
		('Hershey Kisses', 'Hershey', 1),
		('Happy Cola', 'Haribo', 3),
		('Gummy Bears', 'Haribo', 3),
		('Gummy Worms', 'Haribo', 3),
		('Circus Peanuts', 'Brachs', 6),
		('Warheads', 'Impact Confections', 5),
		('Toxic Waste', 'Candy Dynamics', 5)

insert into UserCandy (UserId, CandyId, DateAdded)
values (1, 2, '2020-05-19'),
		(1, 1, '2020-05-18'),
		(1, 6, '2020-04-18'),
		(1, 4, '2020-01-01'),
		(2, 7, '2020-01-02'),
		(2, 6, '2020-02-14'),
		(2, 3, '2020-03-04'),
		(2, 9, '2020-02-02'),
		(3, 7, '2020-03-02'),
		(3, 6, '2020-04-03'),
		(3, 12, '2020-04-01'),
		(4, 4, '2020-05-10'),
		(4, 6, '2020-03-20'),
		(4, 10, '2020-02-23')

select *
from Candy

select *
from [User]

select *
from UserCandy

insert into Category ([Type])
values ('Marshmallow')

select Candy.[Name] as CandyType, [User].UserId, [User].FirstName + ' ' + [User].LastName as [Name]
from UserCandy
	join Candy
		on UserCandy.CandyId = Candy.CandyId
	join [User]
		on UserCandy.UserId = [User].UserId

select *
from Candy

select UserCandy.CandyId, [User].UserId, [User].FirstName + ' ' + [User].LastName as [Name]
from UserCandy
	join [User]
		on UserCandy.UserId = [User].UserId

select [User].FirstName + ' ' + [User].LastName as [Name]
from [User]