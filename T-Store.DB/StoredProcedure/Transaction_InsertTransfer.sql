create procedure [dbo].[Transaction_InsertTransfer]
	@AccountIdSender int,
	@AccountIdRecipient int,
	@Amount decimal (11,4),
	@AmountConverted decimal (11,4),
	@CurrencySender smallint,
	@CurrencyRecipient smallint
as
begin 	

	declare @Date datetime2(7) = sysdatetime()
	declare @TransactionType int = 3

	insert into [dbo].[Transaction]
	(
		[AccountId], 
		[Date], 
		[TransactionType], 
		[Amount], 
		[Currency]
	)
	values 
	(
		@AccountIdSender,
		@Date,
		@TransactionType,
		@Amount,
		@CurrencySender
	)

	declare @SenderId int= scope_identity() 

	insert into [dbo].[Transaction]
	( 
		[AccountId], 
		[Date], 
		[TransactionType], 
		[Amount], 
		[Currency]
	)
	values
	(
		@AccountIdRecipient, 
		@Date, 
		@TransactionType, 
		@AmountConverted, 
		@CurrencyRecipient
	)

	select @SenderId, scope_identity()

end