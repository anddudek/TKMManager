INSERT INTO [tkmmanagerdb].[dbo].[Products]
           ([ProductName]
           ,[CreatedBy]
           ,[Created]
           ,[LastOrder]
           ,[LastSuppy]
           ,[WarehouseAmount])
     VALUES
           ('Prod2'
           ,'Grzegorz'
           ,GETDATE()
           ,GETDATE()
           ,GETDATE()
           ,5)
GO


