CREATE PROCEDURE [dbo].[Transaction_Insert]
	@AccountId int,
	@TransactionType tinyint,
	@Amount decimal (10,3),
	@Currency smallint

AS
BEGIN 

	INSERT INTO [dbo].[Transaction] 
	(
		[AccountId],
		[Date],
		[TransactionType],
		[Amount],
		[Currency]
	)

	VALUES 
	(@AccountId,getdate(),@TransactionType,@Amount,@Currency)

	SELECT SCOPE_IDENTITY() 
END
