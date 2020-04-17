CREATE PROCEDURE [Security].[sp_User_Add]
	@Name NVARCHAR(100),
	@Created DATETIME2(7)
AS
	INSERT INTO [Security].[Users]
		([Name], [Created])
	VALUES
		(@Name, @Created)
RETURN 0