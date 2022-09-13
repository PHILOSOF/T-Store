create procedure [dbo].[Transaction_GetLastTransactionByAccountId]
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

	where [Date] = (select max([Date]) from [dbo].[Transaction] where AccountId = @AccountId )

end
