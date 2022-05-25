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
INSERT [dbo].[Users] ([FirstName], [LastName], [Email], [Username], [SaltedAndHashedPassword], [Admin]) 
VALUES ('Jean', 'Jean', 'jean.jean@gmail.com', 'JEAN', '5974B1B8CE509055BABA59DC2A73057C0F3E98FCB0A2C0ED827F49C88F77034925129148838120093169236510721417894', 'False');

INSERT [dbo].[Users] ([FirstName], [LastName], [Email], [Username], [SaltedAndHashedPassword], [Admin]) 
VALUES ('Pierre', 'Pierre', 'pierre.pierre@gmail.com', 'PIERRE', '0EAA14AC9D13814A186A5F4CC885108562D24CEFAB14AED9BD941A9F1AA7FE6412975608071861201629230249227914205', 'False');

INSERT [dbo].[Users] ([FirstName], [LastName], [Email], [Username], [SaltedAndHashedPassword], [Admin]) 
VALUES ('Guy', 'Guy', 'guy.guy@gmail.com', 'GUY', '8A1C4AEF40803FC791FAC5BF36C2CB31056801E26F6BB648CA2B1536698C055E247712112322321995718812252110215126215', 'False');

INSERT [dbo].[Users] ([FirstName], [LastName], [Email], [Username], [SaltedAndHashedPassword], [Admin]) 
VALUES ('Marie', 'Marie', 'marie.marie@gmail.com', 'MARIE', '3CF729BCA0B26C8FCB8A7EB1ED84D48AFD3849B5792E0AB9E194FB4CB6979F1172431401947842161209191421631737242181', 'False');

INSERT [dbo].[Users] ([FirstName], [LastName], [Email], [Username], [SaltedAndHashedPassword], [Admin]) 
VALUES ('Camille', 'Camille', 'camille.camille@gmail.com', 'CAMILLE', '4E9FF5625918782CD3E1402F736E1E35C81B138AAB1F60D0F6CFC7D252C1ABB6173201329610945194138564922724910439216', 'True');
