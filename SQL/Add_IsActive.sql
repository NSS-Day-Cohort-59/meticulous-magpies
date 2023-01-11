USE [TabloidMVC]
BEGIN

ALTER TABLE UserProfile
ADD IsActive bit NOT NULL DEFAULT(1)

END
