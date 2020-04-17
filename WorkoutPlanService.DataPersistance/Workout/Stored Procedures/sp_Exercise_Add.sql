CREATE PROCEDURE [Workout].[sp_Exercise_Add]
	@ExerciseId INT,
	@Name NVARCHAR(400),
	@Created DATETIME2(7),
	@IsActive BIT
AS
	INSERT INTO [Workout].[Exercise]
		([ExerciseId], [Name], [Created], [IsActive])
	VALUES
		(@ExerciseId, @Name, @Created, @IsActive)
RETURN 0