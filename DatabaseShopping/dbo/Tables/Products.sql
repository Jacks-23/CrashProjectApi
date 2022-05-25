CREATE TABLE [dbo].[Products]
(
	[ProductId] INT NOT NULL IDENTITY (1, 1),
	[Name] Varchar(50) not null,
	[Price] dec(10,2) not null,
	[Picture] Varchar(50),
	[Description] Varchar(200),

	Constraint [PK_ProductId] PRIMARY KEY CLUSTERED ([ProductId] ASC)
)
