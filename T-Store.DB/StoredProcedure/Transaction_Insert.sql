create procedure [dbo].[Transaction_Insert]
	@AccountId bigint,
	@Date datetime,
	@TransactionType tinyint,
	@Amount decimal (11,4),
	@Currency smallint
as
begin 

	declare @lastDate datetime
	set @lastDate = (select [Date] 
					from [dbo].[Transaction]
					where [Date] = (select max([Date])
					from [dbo].[Transaction]
					where AccountId = @AccountId))
					
	if @Date <> (select convert (varchar, @lastDate, 113))
		raiserror ('Eror transaction duplicate', 16, 1)	
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
