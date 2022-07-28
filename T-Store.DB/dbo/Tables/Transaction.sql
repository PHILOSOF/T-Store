CREATE TABLE [dbo].[Transaction] (
    [Id]              INT           NOT NULL IDENTITY,
    [AccountId]       INT           NOT NULL,
    [DateTime]        DATETIME2 (7) NOT NULL,
    [TransactionType] TINYINT       NOT NULL,
    [Amount]          DECIMAL (18)  NOT NULL,
    [Currency]        SMALLINT      NOT NULL, 
    CONSTRAINT [PK_Transaction] PRIMARY KEY ([Id])
);

