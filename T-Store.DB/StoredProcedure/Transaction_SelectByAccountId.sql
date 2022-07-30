create procedure [dbo].[Transaction_SelectByAccountId]
	@AccountId int

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