CREATE PROCEDURE [Security].[sp_User_Add]
	@Id UNIQUEIDENTIFIER,
	@Name NVARCHAR(100),
	@Created DATETIME2(7)
AS
	INSERT INTO [Security].[Users]
		([Id], [Name], [Created])
	VALUES
		(@Id, @Name, @Created)
RETURN 0