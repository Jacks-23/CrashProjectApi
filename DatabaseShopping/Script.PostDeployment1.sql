/*
Modèle de script de post-déploiement							
--------------------------------------------------------------------------------------
 Ce fichier contient des instructions SQL qui seront ajoutées au script de compilation.		
 Utilisez la syntaxe SQLCMD pour inclure un fichier dans le script de post-déploiement.			
 Exemple :      :r .\monfichier.sql								
 Utilisez la syntaxe SQLCMD pour référencer une variable dans le script de post-déploiement.		
 Exemple :      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/***** SEED DATA FOR PRODUCTS TABLE *****/
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Fraise', 1.99)
INSERT [dbo].[Products] ( [Name], [Price]) VALUES ('Orange', 2.99)
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Poire', 3.99)
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Pomme', 4.99)
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Raisin', 5.99)
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Pastèque', 6.99)
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Pamplemousse', 7.99)
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Clémentine', 8.99)
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Abricot', 9.99)
INSERT [dbo].[Products] ([Name], [Price]) VALUES ('Noix de coco', 10.99)


/***** SEED DATA FOR ORDERS TABLE *****/
/*SET Identity_Insert [dbo].[Orders] On;**/
INSERT INTO [dbo].[Orders] ([Name], [Total], [NumberOfProducts]) VALUES ('Liste1', 1.99, 1);
INSERT INTO [dbo].[Orders] ([Name], [Total], [NumberOfProducts]) VALUES ('Liste2', 1.99, 1)
INSERT INTO [dbo].[Orders] ([Name], [Total], [NumberOfProducts]) VALUES ('Liste3', 2.99, 1)
INSERT INTO [dbo].[Orders] ([Name], [Total], [NumberOfProducts]) VALUES ('Liste4', 3.99, 1)
INSERT INTO [dbo].[Orders] ([Name], [Total], [NumberOfProducts]) VALUES ('Liste5', 55.93, 7)


/***** SEED DATA FOR PRODUCTS_ORDERS TABLE *****/
INSERT INTO [dbo].[Products_Orders] VALUES (1, 1, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (1, 2, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (2, 3, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (3, 4, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (4, 5, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (5, 5, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (6, 5, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (7, 5, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (8, 5, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (9, 5, 1);
INSERT INTO [dbo].[Products_Orders] VALUES (10, 5, 1);
