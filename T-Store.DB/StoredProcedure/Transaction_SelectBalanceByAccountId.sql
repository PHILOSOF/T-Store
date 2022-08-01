create procedure [dbo].[Transaction_SelectBalanceByAccountId]
	@AccountId bigint

as
begin

	select sum([Amount])
	from [dbo].[Transaction]

	where [AccountId] = @AccountId

end