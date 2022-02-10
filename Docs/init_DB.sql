/*
   4 декабря 2021 г.20:45:23
   Пользователь: MihailUse_SQLLogin_1
   Сервер: DataTestC.mssql.somee.com
   База данных: DataTestC
   Приложение: 
*/

/* Чтобы предотвратить возможность потери данных, необходимо внимательно просмотреть этот скрипт, прежде чем запускать его вне контекста конструктора баз данных.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_User
	(
	id int NOT NULL IDENTITY (1, 1),
	login nvarchar(256) NOT NULL,
	password nvarchar(256) NOT NULL,
	about nvarchar(MAX) NULL,
	avatar varbinary(MAX) NULL,
	createdAt datetime2(7) NOT NULL,
	updatedAt datetime2(7) NULL,
	detetedAt datetime2(7) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_User SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_User ON
GO
IF EXISTS(SELECT * FROM dbo.[User])
	 EXEC('INSERT INTO dbo.Tmp_User (id, login, password, about, avatar, createdAt, updatedAt, detetedAt)
		SELECT id, login, password, about, avatar, createdAt, updatedAt, detetedAt FROM dbo.[User] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_User OFF
GO
ALTER TABLE dbo.Membership
	DROP CONSTRAINT FK_Membership_User
GO
DROP TABLE dbo.[User]
GO
EXECUTE sp_rename N'dbo.Tmp_User', N'User', 'OBJECT' 
GO
ALTER TABLE dbo.[User] ADD CONSTRAINT
	PK_User PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Role
	(
	id int NOT NULL IDENTITY (1, 1),
	name nvarchar(256) NOT NULL,
	description nvarchar(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Role SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Role ON
GO
IF EXISTS(SELECT * FROM dbo.Role)
	 EXEC('INSERT INTO dbo.Tmp_Role (id, name, description)
		SELECT id, name, description FROM dbo.Role WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Role OFF
GO
ALTER TABLE dbo.Membership
	DROP CONSTRAINT FK_Membership_Role
GO
DROP TABLE dbo.Role
GO
EXECUTE sp_rename N'dbo.Tmp_Role', N'Role', 'OBJECT' 
GO
ALTER TABLE dbo.Role ADD CONSTRAINT
	PK_Role PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Project
	(
	id int NOT NULL IDENTITY (1, 1),
	name nvarchar(256) NOT NULL,
	description nvarchar(MAX) NULL,
	logo varbinary(MAX) NULL,
	createdAt datetime2(7) NOT NULL,
	updatedAt datetime2(7) NULL,
	detetedAt datetime2(7) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Project SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Project ON
GO
IF EXISTS(SELECT * FROM dbo.Project)
	 EXEC('INSERT INTO dbo.Tmp_Project (id, name, description, logo, createdAt, detetedAt)
		SELECT id, name, description, logo, createdAt, detetedAt FROM dbo.Project WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Project OFF
GO
ALTER TABLE dbo.Task
	DROP CONSTRAINT FK_Task_Project
GO
ALTER TABLE dbo.Membership
	DROP CONSTRAINT FK_Membership_Project
GO
DROP TABLE dbo.Project
GO
EXECUTE sp_rename N'dbo.Tmp_Project', N'Project', 'OBJECT' 
GO
ALTER TABLE dbo.Project ADD CONSTRAINT
	PK_Project PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Membership
	(
	id int NOT NULL IDENTITY (1, 1),
	user_id int NOT NULL,
	project_id int NOT NULL,
	role_id int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Membership SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Membership ON
GO
IF EXISTS(SELECT * FROM dbo.Membership)
	 EXEC('INSERT INTO dbo.Tmp_Membership (id, user_id, project_id, role_id)
		SELECT id, user_id, project_id, role_id FROM dbo.Membership WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Membership OFF
GO
ALTER TABLE dbo.Assignment
	DROP CONSTRAINT FK_Assignment_Membership
GO
DROP TABLE dbo.Membership
GO
EXECUTE sp_rename N'dbo.Tmp_Membership', N'Membership', 'OBJECT' 
GO
ALTER TABLE dbo.Membership ADD CONSTRAINT
	PK_Membership PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Membership ADD CONSTRAINT
	FK_Membership_Role FOREIGN KEY
	(
	role_id
	) REFERENCES dbo.Role
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Membership ADD CONSTRAINT
	FK_Membership_User FOREIGN KEY
	(
	user_id
	) REFERENCES dbo.[User]
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Membership ADD CONSTRAINT
	FK_Membership_Project FOREIGN KEY
	(
	project_id
	) REFERENCES dbo.Project
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Status
	(
	id int NOT NULL IDENTITY (1, 1),
	name nvarchar(256) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Status SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Status ON
GO
IF EXISTS(SELECT * FROM dbo.Status)
	 EXEC('INSERT INTO dbo.Tmp_Status (id, name)
		SELECT id, name FROM dbo.Status WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Status OFF
GO
ALTER TABLE dbo.Task
	DROP CONSTRAINT FK_Task_Status
GO
DROP TABLE dbo.Status
GO
EXECUTE sp_rename N'dbo.Tmp_Status', N'Status', 'OBJECT' 
GO
ALTER TABLE dbo.Status ADD CONSTRAINT
	PK_Status PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Task
	(
	id int NOT NULL IDENTITY (1, 1),
	title nvarchar(256) NOT NULL,
	description nvarchar(MAX) NULL,
	project_id int NOT NULL,
	status_id int NOT NULL,
	createdAt datetime2(7) NOT NULL,
	updatedAt datetime2(7) NULL,
	detetedAt datetime2(7) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Task SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Task ON
GO
IF EXISTS(SELECT * FROM dbo.Task)
	 EXEC('INSERT INTO dbo.Tmp_Task (id, title, description, project_id, status_id, createdAt, updatedAt, detetedAt)
		SELECT id, title, description, project_id, status_id, createdAt, updatedAt, detetedAt FROM dbo.Task WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Task OFF
GO
ALTER TABLE dbo.Assignment
	DROP CONSTRAINT FK_Assignment_Task
GO
DROP TABLE dbo.Task
GO
EXECUTE sp_rename N'dbo.Tmp_Task', N'Task', 'OBJECT' 
GO
ALTER TABLE dbo.Task ADD CONSTRAINT
	PK_Task PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Task ADD CONSTRAINT
	FK_Task_Status FOREIGN KEY
	(
	status_id
	) REFERENCES dbo.Status
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Task ADD CONSTRAINT
	FK_Task_Project FOREIGN KEY
	(
	project_id
	) REFERENCES dbo.Project
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Assignment
	(
	id int NOT NULL IDENTITY (1, 1),
	membership_id int NOT NULL,
	task_id int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Assignment SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Assignment ON
GO
IF EXISTS(SELECT * FROM dbo.Assignment)
	 EXEC('INSERT INTO dbo.Tmp_Assignment (id, membership_id, task_id)
		SELECT id, membership_id, task_id FROM dbo.Assignment WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Assignment OFF
GO
DROP TABLE dbo.Assignment
GO
EXECUTE sp_rename N'dbo.Tmp_Assignment', N'Assignment', 'OBJECT' 
GO
ALTER TABLE dbo.Assignment ADD CONSTRAINT
	PK_Assignment PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Assignment ADD CONSTRAINT
	FK_Assignment_Membership FOREIGN KEY
	(
	membership_id
	) REFERENCES dbo.Membership
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Assignment ADD CONSTRAINT
	FK_Assignment_Task FOREIGN KEY
	(
	task_id
	) REFERENCES dbo.Task
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
