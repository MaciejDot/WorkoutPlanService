CREATE PROCEDURE [Workout].[sp_Exercise_Update]
	@Id INT,
	@Name NVARCHAR(400)
AS
	UPDATE [Workout].[Exercise]
	SET [Name] = @Name
	WHERE [Id] = @Id
RETURN 0