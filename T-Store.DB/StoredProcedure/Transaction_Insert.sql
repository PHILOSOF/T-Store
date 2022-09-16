create procedure [dbo].[Transaction_Insert]
	@AccountId bigint,
	@Date datetime2(7),
	@TransactionType tinyint,
	@Amount decimal (11,4),
	@Currency smallint
as
begin
begin transaction
	--declare @lastDate datetime2(7)
	--set @lastDate = (select top 1 [Date] 
	--				from [dbo].[Transaction]
	--				where AccountId = @AccountId
	--				order by [Date] desc)

	--if @lastDate != @Date
	--	raiserror ('Error Transaction duplicate', 16, 1)	

	--else 
		insert into [dbo].[Transaction] with (tablock, holdlock) 
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
		declare @actualBalance decimal
		set @actualBalance = (select coalesce(sum([Amount]),0)
							  from [dbo].[Transaction] with (tablock, holdlock) 
							  where [AccountId] = @AccountId)
		if @actualBalance<0
		rollback

		else		
		select scope_identity()
commit
end



	