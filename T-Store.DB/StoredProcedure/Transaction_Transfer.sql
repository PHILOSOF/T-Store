CREATE PROCEDURE [dbo].[Transaction_Transfer]
	@AccountIdSender int,
	@AccountIdRecipient int,
	@Amount decimal (10,3),
	@AmountConverted decimal (10,3),
	@CurrencySender smallint,
	@CurrencyRecipient smallint,
	@TransactionType tinyint
AS
BEGIN 	


	DECLARE @Date datetime2(7) = getdate()

		
		INSERT INTO [dbo].[Transaction] 
			(
				[AccountId],
				[Date],
				[TransactionType],
				[Amount],
				[Currency]
			)

			VALUES 

			(@AccountIdSender,@Date,@TransactionType,@Amount,@CurrencySender)

			DECLARE @senderId int= SCOPE_IDENTITY() 




			INSERT INTO [dbo].[Transaction] 
				(
					[AccountId],
					[Date],
					[TransactionType],
					[Amount],
					[Currency]
				)

				VALUES 
				(@AccountIdRecipient,@Date,@TransactionType,@AmountConverted,@CurrencyRecipient)

				SELECT @senderId, SCOPE_IDENTITY()
END