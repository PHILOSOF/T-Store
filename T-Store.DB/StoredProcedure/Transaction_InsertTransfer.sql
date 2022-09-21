create procedure [dbo].[Transaction_InsertTransfer]
	@AccountIdSender bigint,
	@AccountIdRecipient bigint,
	@Amount decimal (11,4),
	@AmountConverted decimal (11,4),
	@CurrencySender smallint,
	@CurrencyRecipient smallint
as
begin 	
begin transaction

	declare @Date datetime2(7) = sysdatetime()
	declare @TransactionTransfer int = 3

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
		@TransactionTransfer,
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
		@TransactionTransfer, 
		@AmountConverted, 
		@CurrencyRecipient
	)


	declare @actualBalance decimal (11,4)
	set @actualBalance = (select coalesce(sum([Amount]),0)
						  from [dbo].[Transaction] with (tablock, holdlock) 
						  where [AccountId] = @AccountIdRecipient)

	if @actualBalance<0
	rollback

	else		
		declare @RecipientId int= scope_identity()

	select @SenderId union all select  @RecipientId

commit transaction
end