create procedure [dbo].Transaction_GetAllTransactionsByAccountId
	@AccountId bigint

as
begin

	with T as
	(
		select [Date]
		from   [dbo].[Transaction]
		where  (AccountId =@AccountId )
	)

	select [Id],
		   [AccountId],
		   [Date],
		   [TransactionType],
		   [Amount],
		   [Currency]
	from   [dbo].[Transaction]
	where  [Date] in (select [Date] from T)
		
end