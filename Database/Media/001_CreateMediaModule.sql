/*
    Morabi Iran - Media module
    Safe to run more than once. Review the target database before execution.
*/
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF COL_LENGTH('dbo.AccountOrder', 'IsManualByAdmin') IS NULL
BEGIN
    ALTER TABLE dbo.AccountOrder
        ADD IsManualByAdmin bit NOT NULL
            CONSTRAINT DF_AccountOrder_IsManualByAdmin DEFAULT (0);
END;

IF OBJECT_ID('dbo.CourseVideoSession', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CourseVideoSession
    (
        ID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_CourseVideoSession PRIMARY KEY,
        ProductId int NOT NULL,
        Title nvarchar(250) NOT NULL,
        Description nvarchar(max) NULL,
        VideoUrl nvarchar(2000) NOT NULL,
        SortOrder int NOT NULL CONSTRAINT DF_CourseVideoSession_SortOrder DEFAULT (0),
        Active bit NOT NULL CONSTRAINT DF_CourseVideoSession_Active DEFAULT (1),
        CreateDateTime datetime NOT NULL CONSTRAINT DF_CourseVideoSession_CreateDateTime DEFAULT (GETDATE()),
        UpdateDateTime datetime NULL,
        CONSTRAINT FK_CourseVideoSession_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product(ID),
        CONSTRAINT FK_CourseVideoSession_Picture FOREIGN KEY (PictureId) REFERENCES dbo.Picture(ID)
    );
    CREATE INDEX IX_CourseVideoSession_Product_Sort
        ON dbo.CourseVideoSession(ProductId, Active, SortOrder);
END;

IF OBJECT_ID('dbo.VideoCollection', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.VideoCollection
    (
        ID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_VideoCollection PRIMARY KEY,
        Title nvarchar(250) NOT NULL,
        Description nvarchar(max) NULL,
        PictureId int NULL,
        Active bit NOT NULL CONSTRAINT DF_VideoCollection_Active DEFAULT (1),
        CreateDateTime datetime NOT NULL CONSTRAINT DF_VideoCollection_CreateDateTime DEFAULT (GETDATE()),
        UpdateDateTime datetime NULL
    );
    CREATE INDEX IX_VideoCollection_Active ON dbo.VideoCollection(Active, ID);
END;

IF OBJECT_ID('dbo.VideoCollectionSession', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.VideoCollectionSession
    (
        ID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_VideoCollectionSession PRIMARY KEY,
        VideoCollectionId int NOT NULL,
        Title nvarchar(250) NOT NULL,
        Description nvarchar(max) NULL,
        VideoUrl nvarchar(2000) NOT NULL,
        SortOrder int NOT NULL CONSTRAINT DF_VideoCollectionSession_SortOrder DEFAULT (0),
        Active bit NOT NULL CONSTRAINT DF_VideoCollectionSession_Active DEFAULT (1),
        CreateDateTime datetime NOT NULL CONSTRAINT DF_VideoCollectionSession_CreateDateTime DEFAULT (GETDATE()),
        UpdateDateTime datetime NULL,
        CONSTRAINT FK_VideoCollectionSession_Collection
            FOREIGN KEY (VideoCollectionId) REFERENCES dbo.VideoCollection(ID)
    );
    CREATE INDEX IX_VideoCollectionSession_Collection_Sort
        ON dbo.VideoCollectionSession(VideoCollectionId, Active, SortOrder);
END;

IF OBJECT_ID('dbo.VideoCollectionAccount', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.VideoCollectionAccount
    (
        ID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_VideoCollectionAccount PRIMARY KEY,
        VideoCollectionId int NOT NULL,
        AccountId bigint NOT NULL,
        AssignedBySiteUserId int NULL,
        AssignedDateTime datetime NOT NULL
            CONSTRAINT DF_VideoCollectionAccount_AssignedDateTime DEFAULT (GETDATE()),
        Active bit NOT NULL CONSTRAINT DF_VideoCollectionAccount_Active DEFAULT (1),
        CONSTRAINT FK_VideoCollectionAccount_Collection
            FOREIGN KEY (VideoCollectionId) REFERENCES dbo.VideoCollection(ID),
        CONSTRAINT FK_VideoCollectionAccount_Account
            FOREIGN KEY (AccountId) REFERENCES dbo.Account(ID),
        CONSTRAINT FK_VideoCollectionAccount_SiteUser
            FOREIGN KEY (AssignedBySiteUserId) REFERENCES dbo.SiteUser(ID),
        CONSTRAINT UQ_VideoCollectionAccount UNIQUE (VideoCollectionId, AccountId)
    );
    CREATE INDEX IX_VideoCollectionAccount_Account
        ON dbo.VideoCollectionAccount(AccountId, Active);
END;

IF OBJECT_ID('dbo.CourseVideoVisit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CourseVideoVisit
    (
        ID bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_CourseVideoVisit PRIMARY KEY,
        CourseVideoSessionId int NOT NULL,
        AccountId bigint NOT NULL,
        FirstVisitDateTime datetime NOT NULL,
        LastVisitDateTime datetime NOT NULL,
        WatchedSeconds int NOT NULL CONSTRAINT DF_CourseVideoVisit_WatchedSeconds DEFAULT (0),
        DurationSeconds int NULL,
        IsCompleted bit NOT NULL CONSTRAINT DF_CourseVideoVisit_IsCompleted DEFAULT (0),
        CONSTRAINT FK_CourseVideoVisit_Session
            FOREIGN KEY (CourseVideoSessionId) REFERENCES dbo.CourseVideoSession(ID),
        CONSTRAINT FK_CourseVideoVisit_Account
            FOREIGN KEY (AccountId) REFERENCES dbo.Account(ID),
        CONSTRAINT UQ_CourseVideoVisit UNIQUE (CourseVideoSessionId, AccountId)
    );
    CREATE INDEX IX_CourseVideoVisit_Account ON dbo.CourseVideoVisit(AccountId, LastVisitDateTime);
END;

IF OBJECT_ID('dbo.VideoCollectionVisit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.VideoCollectionVisit
    (
        ID bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_VideoCollectionVisit PRIMARY KEY,
        VideoCollectionSessionId int NOT NULL,
        AccountId bigint NOT NULL,
        FirstVisitDateTime datetime NOT NULL,
        LastVisitDateTime datetime NOT NULL,
        WatchedSeconds int NOT NULL CONSTRAINT DF_VideoCollectionVisit_WatchedSeconds DEFAULT (0),
        DurationSeconds int NULL,
        IsCompleted bit NOT NULL CONSTRAINT DF_VideoCollectionVisit_IsCompleted DEFAULT (0),
        CONSTRAINT FK_VideoCollectionVisit_Session
            FOREIGN KEY (VideoCollectionSessionId) REFERENCES dbo.VideoCollectionSession(ID),
        CONSTRAINT FK_VideoCollectionVisit_Account
            FOREIGN KEY (AccountId) REFERENCES dbo.Account(ID),
        CONSTRAINT UQ_VideoCollectionVisit UNIQUE (VideoCollectionSessionId, AccountId)
    );
    CREATE INDEX IX_VideoCollectionVisit_Account
        ON dbo.VideoCollectionVisit(AccountId, LastVisitDateTime);
END;

COMMIT TRANSACTION;
