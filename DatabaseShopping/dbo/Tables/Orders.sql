CREATE TABLE [dbo].[Orders] (
    [OrderId]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50)    NOT NULL,
    [DateCrea]         NVARCHAR (50)   DEFAULT (CONVERT([nvarchar],getdate())) NOT NULL,
    [Total]            DECIMAL (10, 2) NOT NULL,
    [NumberOfProducts] INT            NOT NULL,
    
    Constraint [PK_OrderId] PRIMARY KEY CLUSTERED ([OrderId] ASC)
    
);


