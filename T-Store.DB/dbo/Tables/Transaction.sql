CREATE TABLE [dbo].[Transaction] (
    [Id]              BIGINT           NOT NULL IDENTITY,
    [AccountId]       BIGINT           NOT NULL,
    [Date]        DATETIME2 (7) NOT NULL,
    [TransactionType] TINYINT       NOT NULL,
    [Amount]          DECIMAL (11, 4)  NOT NULL,
    [Currency]        SMALLINT      NOT NULL, 
    CONSTRAINT [PK_Transaction] PRIMARY KEY ([Id])
);

