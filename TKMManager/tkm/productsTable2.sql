CREATE TABLE Products
(
ProductID int IDENTITY(1,1) primary key,
ProductName varchar(255) not null,
CreatedBy varchar(255) null,
Created datetime null,
LastOrder datetime null,
LastSuppy datetime null,
WarehouseAmount int not null
); 