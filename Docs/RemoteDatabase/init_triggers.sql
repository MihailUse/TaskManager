USE DataTestC;
GO

/* Triggers to update the date in createdAt column */

/* For User table */
CREATE TRIGGER [USER_INSERT]
	ON [User]
	AFTER INSERT
AS
BEGIN
	UPDATE [User] 
    SET createdAt = GETUTCDATE()
    WHERE Id = (Select Id from Inserted)
END 
GO

/* For Project table */
CREATE TRIGGER [PROJECT_INSERT]
	ON [Project]
	AFTER INSERT
AS
BEGIN
	UPDATE [Project] 
    SET createdAt = GETUTCDATE()
    WHERE Id = (Select Id from Inserted)
END 
GO

/* For Task table */
CREATE TRIGGER [TASK_INSERT]
	ON [Task]
	AFTER INSERT
AS
BEGIN
	UPDATE [Task] 
    SET createdAt = GETUTCDATE()
    WHERE Id = (Select Id from Inserted)
END 
GO


/* Triggers to update the date in updatedAt column */

/* For User table */
CREATE TRIGGER [USER_UPDATE]
	ON [User]
	AFTER UPDATE
AS
BEGIN
	UPDATE [User] 
    SET updatedAt = GETUTCDATE()
    WHERE Id = (Select Id from Inserted)
END 
GO

/* For Project table */
CREATE TRIGGER [PROJECT_UPDATE]
	ON [Project]
	AFTER UPDATE
AS
BEGIN
	UPDATE [Project] 
    SET updatedAt = GETUTCDATE()
    WHERE Id = (Select Id from Inserted)
END 
GO

/* For Task table */
CREATE TRIGGER [TASK_UPDATE]
	ON [Task]
	AFTER UPDATE
AS
BEGIN
	UPDATE [Task] 
    SET updatedAt = GETUTCDATE()
    WHERE Id = (Select Id from Inserted)
END 
GO


/* Triggers to update the date in detetedAt column */

/* For User table */
CREATE TRIGGER [USER_DELETE]
	ON [User]
	INSTEAD OF DELETE
AS
BEGIN
	UPDATE [User] 
    SET detetedAt = GETUTCDATE()
    WHERE Id = (Select Id from deleted)
END 
GO

/* For Project table */
CREATE TRIGGER [PROJECT_DELETE]
	ON [Project]
	INSTEAD OF DELETE
AS
BEGIN
	UPDATE [Project] 
    SET detetedAt = GETUTCDATE()
    WHERE Id = (Select Id from deleted)
END 
GO

/* For Task table */
CREATE TRIGGER [TASK_DELETE]
	ON [Task]
	INSTEAD OF DELETE
AS
BEGIN
	UPDATE [Task] 
    SET detetedAt = GETUTCDATE()
    WHERE Id = (Select Id from deleted)
END 
GO