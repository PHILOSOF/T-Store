create procedure [dbo].[Transaction_SelectByAccountId]
	@AccountId bigint

as
begin

	select [Id],
		   [AccountId],
		   [Date],
		   [TransactionType],
		   [Amount],
		   [Currency]
	from [dbo].[Transaction]

	where [AccountId] = @AccountId

end