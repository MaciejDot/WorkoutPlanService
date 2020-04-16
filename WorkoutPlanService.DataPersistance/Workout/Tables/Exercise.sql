CREATE TABLE [Workout].[Exercise] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (400) NULL,
    CONSTRAINT [PK_Exercise] PRIMARY KEY CLUSTERED ([Id] ASC)
);

