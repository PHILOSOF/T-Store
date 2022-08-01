create procedure [dbo].[Transaction_CheckExistenceAccountId]
	@AccountId bigint

as
begin

	select 
		case
		when exists 
		(
			select top (1)
			1
			from [dbo].[Transaction]
			where [AccountId] = @AccountId
		) 

		then 'True'
        else 'False'
		end
end
