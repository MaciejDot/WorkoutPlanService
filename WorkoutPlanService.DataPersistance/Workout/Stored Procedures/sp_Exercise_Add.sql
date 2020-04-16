CREATE PROCEDURE [Workout].[sp_Exercise_Add]
	@Id INT,
	@Name NVARCHAR(400)
AS
	INSERT INTO [Workout].[Exercise]
		([Id], [Name])
	VALUES
		(@Id, @Name)
RETURN 0