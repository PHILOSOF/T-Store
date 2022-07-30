CREATE PROCEDURE [dbo].[Transaction_SelectBalanceByAccountId]
	@AccountId int

AS
BEGIN

	SELECT sum(Amount)
	FROM [dbo].[Transaction]

	WHERE AccountId = @AccountId

END