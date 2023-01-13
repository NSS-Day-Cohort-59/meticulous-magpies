USE [TabloidMVC]

DROP TABLE IF EXISTS AdminRequest
DROP TABLE IF EXISTS AdminRequestType

GO

CREATE TABLE dbo.AdminRequestType (
	Id int PRIMARY KEY IDENTITY NOT NULL,
	[Name] varchar(30) NOT NULL
)

CREATE TABLE dbo.AdminRequest (
	Id int PRIMARY KEY IDENTITY,
	RequesterUserProfileId int NOT NULL,
	CreateDateTime DateTime NOT NULL,
	CloseDateTime DateTime NULL,
	TargetUserProfileId int NOT NULL,
	AdminRequestTypeId int NOT NULL,
	IsCancelled bit NOT NULL DEFAULT(0),

	CONSTRAINT FK_AdminRequest_UserProfile_Requester FOREIGN KEY (RequesterUserProfileId) REFERENCES UserProfile (Id),
	CONSTRAINT FK_AdminRequest_UserProfile_Target FOREIGN KEY (TargetUserProfileId) REFERENCES UserProfile (Id),
	CONSTRAINT FK_AdminRequest_AdminRequestType FOREIGN KEY (AdminRequestTypeId) REFERENCES AdminRequestType (Id)
)

GO

INSERT INTO dbo.AdminRequestType ([Name])
VALUES ('Demote'), ('Deactivate')