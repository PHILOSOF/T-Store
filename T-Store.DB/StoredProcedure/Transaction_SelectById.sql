CREATE PROCEDURE [dbo].[Transaction_SelectById]
	@Id int

AS
BEGIN

	SELECT [Id],
		   [AccountId],
		   [Date],
		   [TransactionType],
		   [Amount],
		   [Currency]
	FROM [dbo].[Transaction]

	WHERE [Id] = @Id

END