CREATE PROCEDURE [dbo].[Insert_Transactiont]
	@AccountId int,
	@DateTime datetime2,
	@TransactionType tinyint,
	@Amount decimal,
	@Currency smallint
AS
	BEGIN 
	INSERT INTO [dbo].[Transaction] 
	(
	[AccountId],
	[DateTime],
	[TransactionType],
	[Amount],
	[Currency]
	)
	VALUES 
	(@AccountId,@DateTime,@TransactionType,@Amount,@Currency)
	SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]
	END
