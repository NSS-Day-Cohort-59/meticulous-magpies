
SET IDENTITY_INSERT [UserProfile] ON
INSERT INTO [UserProfile] (
	[Id], [FirstName], [LastName], [DisplayName], [Email], [CreateDateTime], [ImageLocation], [UserTypeId])
VALUES (3, 'Seaborn', 'Mercer', 'seaborndan', 'surfermercer@gmail.com', SYSDATETIME(), NULL, 1);
SET IDENTITY_INSERT [UserProfile] OFF