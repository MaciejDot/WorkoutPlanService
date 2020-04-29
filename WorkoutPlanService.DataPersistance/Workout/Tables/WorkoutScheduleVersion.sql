CREATE TABLE [Workout].[WorkoutScheduleVersion](
	[Id] [uniqueidentifier] NOT NULL,
	[ExternalId] [uniqueidentifier] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[WorkoutPlanExternalId] [uniqueidentifier] NOT NULL,
	[FirstDate] [datetime2](7) NOT NULL,
	[Recurrence] [int] NULL,
	[RecurringTimes] [int] NULL,
 CONSTRAINT [PK_WorkoutScheduleVersion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Workout].[WorkoutScheduleVersion] ADD  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [Workout].[WorkoutScheduleVersion]  WITH CHECK ADD  CONSTRAINT [CHECK_WorkoutScheduleVersion_WorkoutPlanVersion] CHECK  ([Workout].[CheckWorkoutExternalIdConstraint]([WorkoutPlanExternalId]) = 1)
GO

ALTER TABLE [Workout].[WorkoutScheduleVersion] CHECK CONSTRAINT [CHECK_WorkoutScheduleVersion_WorkoutPlanVersion]
GO
