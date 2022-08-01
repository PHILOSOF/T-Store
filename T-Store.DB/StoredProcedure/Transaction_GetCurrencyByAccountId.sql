create procedure [dbo].[Transaction_GetCurrencyByAccountId]
	@AccountId bigint

as
begin 
	with T as 
	( 
		select [AccountId],
			   [Currency]
		from [dbo].[Transaction]
		where [AccountId] = @AccountId
	)
	select distinct Currency
	from T

end
