create procedure [dbo].[Transaction_Insert]
	@AccountId bigint,
	@Date datetime,
	@TransactionType tinyint,
	@Amount decimal (11,4),
	@Currency smallint
as
begin
begin transaction -- ?


	declare @lastDate datetime
	set @lastDate = (select [Date] 
					from [dbo].[Transaction]
					with (tablock, holdlock) -- ????
					where [Date] = (select max([Date])
					from [dbo].[Transaction]
					where AccountId = @AccountId))
	
	if @lastDate != @Date
		raiserror ('Error Transaction duplicate', 16, 1)	

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
commit --?
end



	