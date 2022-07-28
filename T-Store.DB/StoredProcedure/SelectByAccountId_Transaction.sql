CREATE PROCEDURE [dbo].[SelectByAccountId_Transaction]
	@AccountId int

AS
BEGIN

	SELECT * FROM [dbo].[Transaction]

	WHERE AccountId = @AccountId

END