create procedure [dbo].[Transaction_SelectBalanceByAccountId]
	@AccountId bigint

as
begin

	select coalesce(sum([Amount]),0)
	from [dbo].[Transaction]

	where [AccountId] = @AccountId

end