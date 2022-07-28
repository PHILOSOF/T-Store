CREATE PROCEDURE [dbo].[SelectByAccountId_Transaction]
	@AccountId int
AS
	BEGIN
	SELECT * FROM [dbo].[Transaction]
	where AccountId = @AccountId
	END