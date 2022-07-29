CREATE PROCEDURE [dbo].[Transaction_SelectByAccountId]
	@AccountId int

AS
BEGIN

	SELECT * FROM [dbo].[Transaction]

	WHERE AccountId = @AccountId

END