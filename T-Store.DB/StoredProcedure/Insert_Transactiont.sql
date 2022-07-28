CREATE PROCEDURE [dbo].[Insert_Transactiont]
	@AccountId int,
	@Date datetime2,
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
	(@AccountId,@Date,@TransactionType,@Amount,@Currency)
	SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]
	END
