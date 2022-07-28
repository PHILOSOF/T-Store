CREATE PROCEDURE [dbo].[SelectById_Transaction]
	@Id int
AS
	BEGIN
	SELECT * FROM [dbo].[Transaction]
	where Id = @Id
	END