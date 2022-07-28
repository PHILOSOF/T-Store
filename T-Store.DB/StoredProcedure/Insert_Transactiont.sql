CREATE PROCEDURE [dbo].[Insert_Transactiont]
	@AccountId int,
	@TransactionType tinyint,
	@Amount decimal,
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
	(@AccountId,DateTime.Now,@TransactionType,@Amount,@Currency)
	SELECT SCOPE_IDENTITY() 
	END
