CREATE TABLE [dbo].[Products_Orders]
(
	[ProductId] Int not null,
	[OrderId] Int not null,
	[Quantity] Int not null,

	Constraint [FK_Products_Orders_Orders] Foreign Key ([OrderId]) References [dbo].[Orders] ([OrderId]) on delete cascade,
	Constraint [FK_Products_Orders_Products] Foreign Key ([ProductId]) References [dbo].[Products] ([ProductId])

)
