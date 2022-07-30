create procedure [dbo].[Transaction_GetAllTransfersByAccountId]
	@AccountId int

as
begin

	with T as
	(
		select [Id],
			   [AccountId],
			   [Date],
			   [TransactionType],
			   [Amount],
			   [Currency]
		from [dbo].[Transaction]
		where (AccountId =@AccountId )
	)

	select [Id],
		   [AccountId],
		   [Date],
		   [TransactionType],
		   [Amount],
		   [Currency]
	from [dbo].[Transaction]
	where [Date] in (select [Date] from T)
		
end