CREATE TABLE [dbo].[Transaction] (
    [Id]              BIGINT           NOT NULL IDENTITY,
    [AccountId]       BIGINT           NOT NULL,
    [Date]        DATETIME2 (7) NOT NULL,
    [TransactionType] TINYINT       NOT NULL,
    [Amount]          DECIMAL (10, 3)  NOT NULL,
    [Currency]        SMALLINT      NOT NULL, 
    CONSTRAINT [PK_Transaction] PRIMARY KEY ([Id])
);

