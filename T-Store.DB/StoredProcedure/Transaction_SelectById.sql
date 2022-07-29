CREATE PROCEDURE [dbo].[Transaction_SelectById]
	@Id int

AS
BEGIN

	SELECT * FROM [dbo].[Transaction]

	WHERE Id = @Id

END