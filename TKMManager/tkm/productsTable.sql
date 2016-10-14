CREATE TABLE Products
(
ProductID int IDENTITY(1,1) primary key,
ProductName varchar(255) not null,
CreatedBy varchar(255) null,
Created date,
LastOrder date,
LastSuppy date,
WarehouseAmount int not null
); 