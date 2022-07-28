CREATE PROCEDURE [dbo].[SelectById_Transaction]
	@Id int

AS
BEGIN

	SELECT * FROM [dbo].[Transaction]

	WHERE Id = @Id

END