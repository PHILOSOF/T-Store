create procedure [dbo].[Transaction_Insert]
	@AccountId int,
	@TransactionType tinyint,
	@Amount decimal (11,4),
	@Currency smallint

as
begin 

	insert into [dbo].[Transaction]
	(
		[AccountId],
		[Date],
		[TransactionType],
		[Amount],
		[Currency]
	)

	values (@AccountId, sysdatetime(), @TransactionType, @Amount, @Currency)

	select scope_identity() 
end
