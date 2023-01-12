USE [TabloidMVC]

CREATE TABLE [AdminRequest] (
	[Id] int PRIMARY KEY IDENTITY,
	[RequesterUserProfileId] int NOT NULL,
	[CreateDateTime] DateTime NOT NULL,
	[CloseDateTime] DateTime NULL,
	[TargetUserProfileId] int NOT NULL,
	[Demote] bit NOT NULL DEFAULT(0),
	[Deactivate] bit NOT NULL DEFAULT(0),
	[IsCancelled] bit NOT NULL DEFAULT(0),

	CONSTRAINT [FK_AdminRequest_UserProfile_Requester] FOREIGN KEY ([RequesterUserProfileId]) REFERENCES [UserProfile] ([Id]),
	CONSTRAINT [FK_AdminRequest_UserProfile_Target] FOREIGN KEY ([TargetUserProfileId]) REFERENCES [UserProfile] ([Id])
)