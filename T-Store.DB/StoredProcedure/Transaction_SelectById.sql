create procedure [dbo].[Transaction_SelectById]
	@Id bigint

as
begin

	select [Id],
		   [AccountId],
		   [Date],
		   [TransactionType],
		   [Amount],
		   [Currency]
	from [dbo].[Transaction]

	where [Id] = @Id

end