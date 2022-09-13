create procedure [dbo].[Transaction_Insert]
	@AccountId bigint,
	@TransactionType tinyint,
	@Amount decimal (11,4),
	@Currency smallint,
	@Date datetime
as
begin 

	declare @lastDate datetime2(7) 
	set @lastDate = (select [date] 
					from [dbo].[Transaction]
					where [Date] = (select max([Date])
					from [dbo].[Transaction]
					where AccountId = @AccountId))

	if @lastDate <> @Date
		RAISERROR ('Error transactions duplicate', 16, 1)	
	else 
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
			@AccountId, 
			sysdatetime(), 
			@TransactionType, 
			@Amount, 
			@Currency
		)
		select scope_identity() 					
end
