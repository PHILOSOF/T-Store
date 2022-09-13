CREATE TABLE [dbo].[Transaction] (
    [Id]              BIGINT           NOT NULL IDENTITY,
    [AccountId]       BIGINT           NOT NULL,
    [Date]        DATETIME2 (7) NOT NULL,
    [TransactionType] TINYINT       NOT NULL,
    [Amount]          DECIMAL (13, 4)  NOT NULL,
    [Currency]        SMALLINT      NOT NULL, 
    CONSTRAINT [PK_Transaction] PRIMARY KEY ([Id])
);
GO
CREATE INDEX idx_AccountId ON [dbo].[Transaction] ([AccountId]) INCLUDE ([Amount],[Date])
GO

