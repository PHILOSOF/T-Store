CREATE PROCEDURE [dbo].[Select_BalanceByAccountId]
	@AccountId int
AS
BEGIN

	SELECT SUM(Amount) FROM [dbo].[Transaction]
	WHERE AccountId = @AccountId
END