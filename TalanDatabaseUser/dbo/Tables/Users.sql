CREATE TABLE [dbo].[Users]
(
	[UserId] INT NOT NULL IDENTITY (1, 1),
	[FirstName] Varchar(50) not null,
	[LastName] Varchar(50) not null,
	[Email] Varchar(50) not null,
	[Username] Varchar(50) not null,
	[SaltedAndHashedPassword] Varchar(200) not null,
	[Admin] bit not null,

	Constraint [PK_UserId] PRIMARY KEY CLUSTERED ([UserId] ASC),
	Constraint [UN_Email] Unique ([Email] ASC),
	Constraint [UN_Username] Unique ([Username] ASC)
)
