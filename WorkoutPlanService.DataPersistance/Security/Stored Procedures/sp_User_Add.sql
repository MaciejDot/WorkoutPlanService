CREATE PROCEDURE [Security].[sp_User_Add]
	@Id UNIQUEIDENTIFIER,
	@Name NVARCHAR(100)
AS
	INSERT INTO [Security].[Users]
		([Id], [Name])
	VALUES
		(@Id, @Name)
RETURN 0