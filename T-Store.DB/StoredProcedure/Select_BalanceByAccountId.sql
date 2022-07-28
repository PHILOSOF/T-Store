CREATE PROCEDURE [dbo].[Procedure1]
	@AccountId int
AS
BEGIN

	SELECT SUM(Amount) FROM [dbo].[Transaction]
	WHERE AccountId = @AccountId
END